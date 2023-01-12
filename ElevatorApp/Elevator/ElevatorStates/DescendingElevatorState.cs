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
        public DescendingElevatorState(Sensor sensor) : base(sensor)
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
                Sensor.FloorList[floor.FloorNumber].IsAscending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
        }

        private void AddUserInputToQueue(Floor floor)
        {
            if (floor.FloorNumber == Sensor.CurrentFloor - 1)
            {
                Sensor.FloorList[floor.FloorNumber].IsDescending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                // Only add to stop list if we are not in between floors
                if (Sensor.CurrentBehavior == CurrentElevatorBehavior.Stopped)
                {
                    Sensor.CurrentQueue.Add(floor.FloorNumber);
                }
            }
            else if (floor.FloorNumber > Sensor.CurrentFloor - 1)
            {
                Sensor.FloorList[floor.FloorNumber].IsAscending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
            else
            {
                Sensor.FloorList[floor.FloorNumber].IsDescending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                Sensor.CurrentQueue.Add(floor.FloorNumber);
            }
        }

        public override void ArriveOnFloor()
        {
            if (Sensor.StopOnNextFloor(false))
            {
                Sensor.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                Sensor.CurrentFloor--;
                Sensor.CurrentQueue.Remove(Sensor.CurrentFloor);
                Sensor.FloorList[Sensor.CurrentFloor].IsDescending = false;
            }
            else
            {
                Sensor.CurrentFloor--;
            }
        }

        public override void MoveToNextFloor()
        {
            if (!Sensor.CurrentQueue.Any())
            {
                var nextQueue = Sensor.FloorList.Where(x => x.IsAscending);
                Sensor.CurrentQueue = new SortedSet<int>(nextQueue.Select(x => x.FloorNumber));
                Sensor.ElevatorState = new AscendingElevatorState(Sensor);
                Sensor.ElevatorState.MoveToNextFloor();
            }

            Sensor.Direction = ElevatorDirection.Descending;
            Sensor.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }
    }
}
