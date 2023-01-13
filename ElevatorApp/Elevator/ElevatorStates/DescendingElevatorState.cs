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

        public override void AddFloor(Floor floor)
        {
            if (floor.IsDescending || floor.IsSetFromElevator)
            {
                AddUserInputToQueue(floor);
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].IsAscending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
        }

        private void AddUserInputToQueue(Floor floor)
        {
            if (floor.FloorNumber == Elevator.CurrentFloor - 1)
            {
                // Only add to stop list if we are not in between floors
                if (Elevator.CurrentBehavior == CurrentElevatorBehavior.Stopped)
                {
                    Elevator.CurrentQueue.Add(floor.FloorNumber);
                    Elevator.FloorList[floor.FloorNumber].IsDescending = true;
                    Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                }
                else
                {
                    Elevator.FloorList[floor.FloorNumber].IsAscending = true;
                    Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                }
            }
            else if (floor.FloorNumber > Elevator.CurrentFloor - 1)
            {
                Elevator.FloorList[floor.FloorNumber].IsAscending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
            else
            {
                Elevator.FloorList[floor.FloorNumber].IsDescending = true;
                Elevator.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                Elevator.CurrentQueue.Add(floor.FloorNumber);
            }
        }

        public override void ArriveOnFloor()
        {
            if (Elevator.StopOnNextFloor(false))
            {
                Elevator.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                Elevator.CurrentFloor--;
                Console.WriteLine($"Stopping on floor {Elevator.CurrentFloor}");
                Elevator.CurrentQueue.Remove(Elevator.CurrentFloor);
                Elevator.FloorList[Elevator.CurrentFloor].IsDescending = false;
            }
            else
            {
                Elevator.CurrentFloor--;
                Console.WriteLine($"Skipping floor {Elevator.CurrentFloor}");
            }
            
        }

        public override void MoveToNextFloor()
        {
            if (!Elevator.CurrentQueue.Any())
            {
                var nextQueue = Elevator.FloorList.Where(x => x.IsAscending);

                if(!nextQueue.Any())
                {
                    Elevator.ElevatorState = new WaitingElevatorState(Elevator);
                }
                else
                {
                    Elevator.CurrentQueue = new SortedSet<int>(nextQueue.Select(x => x.FloorNumber));
                    Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                    Elevator.ElevatorState.MoveToNextFloor();
                    return;
                }

            }
            Console.WriteLine($"Moving to next floor: Current floor {Elevator.CurrentFloor } Next Stop: {Elevator.NextFloor}");
            Elevator.Direction = ElevatorDirection.Descending;
            Elevator.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }
    }
}
