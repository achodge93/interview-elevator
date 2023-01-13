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

        public override void AddFloor(Floor floor)
        {
            if(floor.IsAscending || floor.IsSetFromElevator)
            {
                AddUserInputToQueue(floor);
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].IsDescending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
        }

        private void AddUserInputToQueue(Floor floor)
        {
            if (floor.FloorNumber == Elevator.CurrentFloor + 1)
            {
                // Only add to stop list if we are not in between floors
                if(Elevator.CurrentBehavior == CurrentElevatorBehavior.Stopped)
                {
                    Elevator.CurrentQueue.Add(floor.FloorNumber);
                    Elevator.FloorList[floor.FloorNumber].IsAscending = true;
                    Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                }
                else
                {
                    Elevator.FloorList[floor.FloorNumber].IsDescending = true;
                    Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                }
            }
            else if (floor.FloorNumber < Elevator.CurrentFloor + 1)
            {
                Elevator.FloorList[floor.FloorNumber].IsDescending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].IsAscending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                Elevator.CurrentQueue.Add(floor.FloorNumber);
            }
        }

        public override void ArriveOnFloor()
        {
            if(Elevator.StopOnNextFloor(true))
            {
                Elevator.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                Elevator.CurrentFloor++;
                Elevator.CurrentQueue.Remove(Elevator.CurrentFloor);
                Elevator.FloorList[Elevator.CurrentFloor].IsAscending = false;
                Console.WriteLine($"Stopping on floor {Elevator.CurrentFloor}");
            }
            else
            {
                Elevator.CurrentFloor++;
                Console.WriteLine($"Skipping floor {Elevator.CurrentFloor}");
            }

        }

        public override void MoveToNextFloor()
        {
            if(!Elevator.CurrentQueue.Any())
            {
                var nextQueue = Elevator.FloorList.Where(x => x.IsDescending).ToList();
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
    }
}
