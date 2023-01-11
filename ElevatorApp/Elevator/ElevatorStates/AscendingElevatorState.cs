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
            if(floor.Direction == ElevatorDirection.Ascending || floor.Direction == ElevatorDirection.None)
            {
                if(floor.FloorNumber <= Sensor.NextFloor)
                {
                    Sensor.NextDescendingQueue.AddLast(floor.FloorNumber);
                }
                else
                {
                    Sensor.CurrentQueue.AddLast(floor.FloorNumber);
                    Sensor.CurrentQueue = new LinkedList<int>(Sensor.CurrentQueue.OrderBy(x => x));
                }
            }
            else
            {
                Sensor.NextDescendingQueue.AddLast(floor.FloorNumber);
            }
        }

        public override void ArriveOnFloor()
        {
            if(Sensor.StopOnNextFloor)
            {
                Sensor.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                Sensor.CurrentQueue.RemoveFirst();
            }

            Sensor.CurrentFloor++;
        }

        public override void MoveToNextFloor()
        {
            if(!Sensor.CurrentQueue.Any())
            {
                Sensor.CurrentQueue = new LinkedList<int>(Sensor.NextDescendingQueue.OrderByDescending(x => x));
                Sensor.NextDescendingQueue = new LinkedList<int>();
                Sensor.ElevatorState = new DescendingElevatorState(Sensor);
            }
            Sensor.CurrentBehavior = CurrentElevatorBehavior.Moving;
        }
    }
}
