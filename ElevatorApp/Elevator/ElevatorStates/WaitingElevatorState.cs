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
    public class WaitingElevatorState : IElevatorState
    {
        public Elevator Elevator {get;}
        public WaitingElevatorState(Elevator elevator)
        {
            Elevator = elevator;
        }

        public void AddFloor(Floor floor)
        {
            if(floor.FloorNumber > Elevator.CurrentFloor)
            {
                Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                floor.AscendingCommand.ShouldStop = true;
                Elevator.ElevatorState.AddFloor(floor);
            }
            else if(floor.FloorNumber < Elevator.CurrentFloor)
            {
                Elevator.ElevatorState = new DescendingElevatorState(Elevator);
                floor.DescendingCommand.ShouldStop = true;
                Elevator.ElevatorState.AddFloor(floor);
            }
        }

        public void MoveToNextFloor()
        {
        }

        public void ArriveOnFloor()
        {
        }

        public void UpdateState()
        {

        }
        public bool ShouldStopOnNextFloor()
        {
            return true;
        }
    }
}
