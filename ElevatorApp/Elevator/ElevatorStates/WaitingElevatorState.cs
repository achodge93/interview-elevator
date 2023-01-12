using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    /// <summary>
    /// Encapsulates logic for when the elevator stops.
    /// If there are no more stops queued it will wait for user input before continuing
    /// </summary>
    public class WaitingElevatorState : ElevatorState
    {
        public WaitingElevatorState(Sensor sensor) : base(sensor)
        {

        }

        public override void AddFloor(Floor floor)
        {
            throw new NotImplementedException();
        }

        public override void ArriveOnFloor()
        {
            throw new NotImplementedException();
        }

        public override void MoveToNextFloor()
        {
            throw new NotImplementedException();
        }
    }
}
