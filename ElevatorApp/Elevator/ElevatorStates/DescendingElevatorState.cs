using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    /// <summary>
    /// Encapsulates logic to deal with the elevator as it descends
    /// </summary>
    public class DescendingElevatorState : ElevatorState
    {
        public DescendingElevatorState(Elevator Elevator) : base(Elevator)
        {
        }

        public override void SetStopForCurrentDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].DescendingCommand.ShouldStop = true;
        }

        public override void ResetStopForCurrentDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].DescendingCommand.ShouldStop = false;
        }

        public override void SetStopForOppositeDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].AscendingCommand.ShouldStop = true;
        }

        public override void ResetStopForOppositeDirection(Floor floor)
        {
            Elevator.FloorList[floor.FloorNumber].AscendingCommand.ShouldStop = false;
        }
        public override bool IsOppositeDirection(Floor floor)
        {
            return floor.FloorNumber > GetUpcomingFloor();
        }

        public override void AddFloor(Floor floor)
        {
            if (floor.DescendingCommand.ShouldStop || floor.DescendingCommand.IsInElevator)
            {
                AddUserInputToQueue(floor);
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].AscendingCommand.ShouldStop = true;
                Elevator.FloorList[floor.FloorNumber].AscendingCommand.IsInElevator = floor.AscendingCommand.IsInElevator;
            }
        }
        public override void MoveToNextFloor()
        {
            if (!Elevator.CurrentQueue.Any())
            {
                var nextQueue = Elevator.FloorList.Where(x => x.AscendingCommand.ShouldStop);
                var nextDescending = Elevator.FloorList.Where(x => x.DescendingCommand.ShouldStop);
                if(nextQueue.Any())
                {
                    Elevator.CurrentQueue = new SortedSet<int>(nextQueue.Select(x => x.FloorNumber));
                    Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                    Elevator.ElevatorState.MoveToNextFloor();
                    return;

                }
                else if (nextDescending.Any()) // Ascend to the highest and then move back down
                {
                    // This would ideally be a different state to properly handle adding new floors, but I don't have time to implement this.
                    Elevator.CurrentQueue = new SortedSet<int>(new[] { nextQueue.MaxBy(x => x.FloorNumber).FloorNumber });
                    Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                    Elevator.ElevatorState.MoveToNextFloor();
                    return;

                }
                else
                {
                    Elevator.ElevatorState = new WaitingElevatorState(Elevator);
                    Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                    Elevator.ElevatorState.MoveToNextFloor();
                    return;
                }
            }

            Log.Information($"Moving to next floor: Current floor {Elevator.CurrentFloor } Next Stop: {Elevator.NextFloor}");
            Elevator.Direction = ElevatorDirection.Descending;
            Elevator.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }

        public override bool ShouldStopOnNextFloor() => Elevator.StopOnNextFloor(false);
        public override void UpdateCurrentFloor() => Elevator.CurrentFloor--;
        public override int GetUpcomingFloor() => Elevator.CurrentFloor - 1;
    }
}
