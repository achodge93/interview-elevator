using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    /// <summary>
    /// Encapsulates logic to deal with the elevator as it ascends
    /// </summary>
    public class AscendingElevatorState : ElevatorState
    {
        public AscendingElevatorState(Elevator elevator) : base(elevator)
        {
            Direction = ElevatorDirection.Ascending;
        }

        public override void SetStopForCurrentDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].AscendingCommand.ShouldStop = true;
        }

        public override void ResetStopForCurrentDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].AscendingCommand.ShouldStop = false;
        }

        public override void SetStopForOppositeDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].DescendingCommand.ShouldStop = true;
        }

        public override void ResetStopForOppositeDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].DescendingCommand.ShouldStop = false;
        }

        public override void AddFloor(Floor floor)
        {
            if(floor.AscendingCommand.ShouldStop || floor.AscendingCommand.IsInElevator)
            {
                AddUserInputToQueue(floor);
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].DescendingCommand.ShouldStop = true;
                Elevator.FloorList[floor.FloorNumber].DescendingCommand.IsInElevator = floor.DescendingCommand.IsInElevator;
            }
        }

        public override void MoveToNextFloor()
        {
            if(!Elevator.CurrentQueue.Any())
            {
                var nextQueue = Elevator.FloorList.Where(x => x.DescendingCommand.ShouldStop).ToList();
                if(nextQueue.Any())
                {
                    Elevator.CurrentQueue = new SortedSet<int>(nextQueue.Select(x => x.FloorNumber), Comparer<int>.Create((x, y) => -x.CompareTo(y)));
                    Elevator.ElevatorState = new DescendingElevatorState(Elevator);
                    Elevator.ElevatorState.MoveToNextFloor();
                    return;
                }
                else
                {
                    Elevator.ElevatorState = new WaitingElevatorState(Elevator);
                    return;
                }

            }
            Console.WriteLine($"Moving to next floor: Current floor {Elevator.CurrentFloor } Next Stop: {Elevator.NextFloor}");
            Elevator.Direction = ElevatorDirection.Ascending;
            Elevator.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }

        public override bool StopOnNextFloor() => Elevator.StopOnNextFloor(true);

        public override void UpdateCurrentFloor() => Elevator.CurrentFloor++;

        public override int GetUpcomingFloor() => Elevator.CurrentFloor + 1;

        public override bool IsOppositeDirection(Floor floor)
        {
            return floor.FloorNumber < GetUpcomingFloor();
        }
    }
}
