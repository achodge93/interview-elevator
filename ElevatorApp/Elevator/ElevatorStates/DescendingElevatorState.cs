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
