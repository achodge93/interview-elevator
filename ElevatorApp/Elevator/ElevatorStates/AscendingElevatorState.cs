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
        public AscendingElevatorState(Sensor sensor) : base(sensor)
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
                Sensor.FloorList[floor.FloorNumber].IsDescending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
        }

        private void AddUserInputToQueue(Floor floor)
        {
            if (floor.FloorNumber == Sensor.CurrentFloor + 1)
            {
                Sensor.FloorList[floor.FloorNumber].IsAscending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                // Only add to stop list if we are not in between floors
                if(Sensor.CurrentBehavior == CurrentElevatorBehavior.Stopped)
                {
                    Sensor.CurrentQueue.Add(floor.FloorNumber);
                }
            }
            else if (floor.FloorNumber < Sensor.CurrentFloor + 1)
            {
                Sensor.FloorList[floor.FloorNumber].IsDescending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
            }
            else
            {
                Sensor.FloorList[floor.FloorNumber].IsAscending = true;
                Sensor.FloorList[floor.FloorNumber].IsSetFromElevator = floor.IsSetFromElevator;
                Sensor.CurrentQueue.Add(floor.FloorNumber);
            }
        }

        public override void ArriveOnFloor()
        {
            if(Sensor.StopOnNextFloor(true))
            {
                Sensor.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                Sensor.CurrentFloor++;
                Sensor.CurrentQueue.Remove(Sensor.CurrentFloor);
                Sensor.FloorList[Sensor.CurrentFloor].IsAscending = false;
            }
            else
            {
                Sensor.CurrentFloor++;
            }

        }

        public override void MoveToNextFloor()
        {
            if(!Sensor.CurrentQueue.Any())
            {
                var nextQueue = Sensor.FloorList.Where(x => x.IsDescending);
                Sensor.CurrentQueue = new SortedSet<int>(nextQueue.Select(x => x.FloorNumber), Comparer<int>.Create((x, y) => -x.CompareTo(y)));
                Sensor.ElevatorState = new DescendingElevatorState(Sensor);
                Sensor.ElevatorState.MoveToNextFloor();
            }

            Sensor.Direction = ElevatorDirection.Ascending;
            Sensor.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }
    }
}
