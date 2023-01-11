using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    public abstract class ElevatorState : IElevatorState
    {
        public Sensor Sensor { get; }
        public enum CurrentElevatorBehavior
        {
            Moving,
            Stopped,
            Quitting
        }

        public ElevatorDirection Direction { get; protected set; }
        protected ElevatorState(Sensor sensor)
        {
            Sensor = sensor;
        }

        /**
        * The elevator does the following steps in the following order:
        * 1) Moves to the next floor
        * 2) If the floor is a stop, set the sensor state and stop. Otherwise, continue to the next floor
        * 3) Update current floor and next floor
        */
        public abstract void AddFloor(Floor floor);
        public abstract void MoveToNextFloor();
        public abstract void ArriveOnFloor();
    }
}
