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

        public abstract void SetStopForCurrentDirection(Floor floorNumber);
        public abstract void ResetStopForCurrentDirection(Floor floorNumber);
        public abstract void SetStopForOppositeDirection(Floor floorNumber);
        public abstract void ResetStopForOppositeDirection(Floor floorNumber);
        public abstract void UpdateCurrentFloor();
        public abstract int GetUpcomingFloor();
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
        /**
        * The elevator does the following steps in the following order:
        * 1) Moves to the next floor
        * 2) If the floor is a stop, set the sensor state and stop. Otherwise, continue to the next floor
        * 3) Update current floor and next floor
        */
        public abstract void AddFloor(Floor floor);
        public abstract void MoveToNextFloor();
        public abstract bool StopOnNextFloor();
        public virtual void ArriveOnFloor()
        {
            if (StopOnNextFloor())
            {
                Elevator.CurrentBehavior = CurrentElevatorBehavior.Stopped;
                UpdateCurrentFloor();
                Elevator.CurrentQueue.Remove(Elevator.CurrentFloor);
                ResetStopForCurrentDirection(Elevator.FloorList[Elevator.CurrentFloor]);
                Console.WriteLine($"Stopping on floor {Elevator.CurrentFloor}");
            }
            else
            {
                UpdateCurrentFloor();
                Console.WriteLine($"Skipping floor {Elevator.CurrentFloor}");
            }
        }

        public virtual void UpdateState()
        {
        }
    }
}
