using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    public abstract class ElevatorState : IElevatorState
    {
        public Elevator Elevator { get; }
        public enum CurrentElevatorBehavior
        {
            Moving,
            Stopped,
            Quitting
        }

        public ElevatorDirection Direction { get; protected set; }
        protected ElevatorState(Elevator elevator)
        {
            Elevator = elevator;
        }

        /// <summary>
        /// Sets the stop indicator and adds the stop to the queue for the current direction based on elevator state
        /// </summary>
        /// <param name="floorNumber">Data for the floor that will be stopped on</param>
        public abstract void SetStopForCurrentDirection(Floor floor);
        /// <summary>
        /// Resets the data for floor to indicate that it has stopped for the current direction based on elevator state
        /// </summary>
        /// <param name="floor"></param>
        public abstract void ResetStopForCurrentDirection(Floor floor);
        /// <summary>
        /// Sets the stop indicator for the opposite direction but does not add it to the current queue
        /// </summary>
        /// <param name="floor"></param>
        public abstract void SetStopForOppositeDirection(Floor floor);
        /// <summary>
        /// Removes the stop indicator in the opposite direction
        /// </summary>
        /// <param name="floor"></param>
        public abstract void ResetStopForOppositeDirection(Floor floor);
        /// <summary>
        /// Updates the current floor to have either incremented or decremented, based on elevator state
        /// </summary>
        public abstract void UpdateCurrentFloor();
        /// <summary>
        /// Gets the next upcoming floor the elevator will either stop or pass
        /// </summary>
        /// <returns></returns>
        public abstract int GetUpcomingFloor();
        /// <summary>
        /// Determines if the floor is within the current direction. IE, if the elevator is ascending and is on floor 6 and 4 UP is requested, this returns true.
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public abstract bool IsOppositeDirection(Floor floor);

        protected void AddUserInputToQueue(Floor floor)
        {
            if (floor.FloorNumber == GetUpcomingFloor())
            {
                // Only add to stop list if we are not in between floors
                if (Elevator.CurrentBehavior == CurrentElevatorBehavior.Stopped)
                {
                    Elevator.CurrentQueue.Add(floor.FloorNumber);
                    SetStopForCurrentDirection(floor);
                }
                else
                {
                    SetStopForOppositeDirection(floor);
                }
            }
            else if (IsOppositeDirection(floor))
            {
                SetStopForOppositeDirection(floor);
            }
            else
            {
                SetStopForCurrentDirection(floor);
                Elevator.CurrentQueue.Add(floor.FloorNumber);
            }
        }

        /// <inheritdoc />
        public abstract void AddFloor(Floor floor);
        /// <inheritdoc />
        public abstract void MoveToNextFloor();
        /// <inheritdoc />
        public abstract bool ShouldStopOnNextFloor();
        /// <inheritdoc />
        public virtual void ArriveOnFloor()
        {
            if (ShouldStopOnNextFloor())
            {
                Elevator.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                UpdateCurrentFloor();
                Elevator.CurrentQueue.Remove(Elevator.CurrentFloor);
                ResetStopForCurrentDirection(Elevator.FloorList[Elevator.CurrentFloor]);
                Log.Information($"Stopping on floor { Elevator.CurrentFloor }");
            }
            else
            {
                UpdateCurrentFloor();
                Log.Information($"Skipping floor { Elevator.CurrentFloor }");
            }
        }

        public virtual void UpdateState()
        {
        }
    }
}
