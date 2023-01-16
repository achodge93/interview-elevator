using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    public interface IElevatorState
    {
        public void UpdateState();
        /// <summary>
        /// Add a new stop to the elevator queue based on the current direction the elevator is traveling in
        /// </summary>
        /// <param name="floor"></param>
        void AddFloor(Floor floor);
        /// <summary>
        /// Move the elevator from its current floor to the next floor
        /// </summary>
        public void MoveToNextFloor();
        /// <summary>
        /// Determines if the elevator should stop on the current floor
        /// </summary>
        /// <returns>True if the elevator should stop, false otherwise</returns>
        public bool ShouldStopOnNextFloor();
        /// <summary>
        /// Arrive on the floor, update any Sensor state as necessary, and make sure the current elevator state is still valid before continuing
        /// </summary>
        public void ArriveOnFloor();
    }
}
