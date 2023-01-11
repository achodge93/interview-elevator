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
            throw new NotImplementedException();
        }
    }
}
