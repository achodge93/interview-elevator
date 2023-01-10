using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator
{
    public class Sensor
    {
        public enum ElevatorDirection
        {
            Ascending,
            Descending
        };

        public enum ElevatorState
        {
            Moving,
            Stopped,
            Quitting
        }

        public int CurrentFloor { get; set; }
        public ElevatorDirection Direction { get; set; }
        public ElevatorState State { get; set; }

        /// <summary>
        /// Represents the current queue of floors the elevator will stop on. 
        /// When this completes and does not get a valid input before the waiting period expires it will be replaced by NextAscendingQueue or NextDescendingQueue
        /// </summary>
        private SortedSet<int> CurrentQueue { get; set; }
        private SortedSet<int> NextAscendingQueue { get; set; }
        private SortedSet<int> NextDescendingQueue { get; set; }

        public void AddFloor(int floor)
        {
            // base case - inside the elevator
            if (Direction == ElevatorDirection.Ascending)
            {
                if (CurrentFloor > floor)
                {
                    NextDescendingQueue.Add(floor);
                }
                else if (CurrentFloor < floor)
                {
                    CurrentQueue.Add(floor);
                }
            }
            else
            {
                if (CurrentFloor > floor)
                {
                    CurrentQueue.Add(floor);
                }
                else if (CurrentFloor < floor)
                {
                    NextDescendingQueue.Add(floor);
                }
            }
        }
    }
}
