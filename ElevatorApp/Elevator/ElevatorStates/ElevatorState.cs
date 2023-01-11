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
        public ElevatorDirection Direction { get; protected set; }
        protected ElevatorState(Sensor sensor)
        {
            Sensor = sensor;
        }
        public abstract void AddFloor(Floor floor);
    }
}
