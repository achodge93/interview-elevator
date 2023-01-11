using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    public interface IElevatorState
    {
        void AddFloor(Floor floor);
    }
}
