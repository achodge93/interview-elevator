using ElevatorApp.Elevator.ElevatorStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorApp.Elevator.ElevatorStates.ElevatorState;

namespace ElevatorApp.Elevator
{
    public class Sensor
    {
        public int CurrentFloor { get; set; }
        public int NextFloor  => CurrentQueue.FirstOrDefault();
        public bool StopOnNextFloor => NextFloor == CurrentFloor + 1;
        public IElevatorState ElevatorState;
        public CurrentElevatorBehavior CurrentBehavior { get; set; } = CurrentElevatorBehavior.Stopped;
        public ElevatorDirection Direction { get; set; }
        /// <summary>
        /// Represents the current queue of floors the elevator will stop on. 
        /// When this completes and does not get a valid input before the waiting period expires it will be replaced by NextAscendingQueue or NextDescendingQueue
        /// </summary>
        public LinkedList<int> CurrentQueue { get; set; } = new LinkedList<int>();
        public LinkedList<int> NextAscendingQueue { get; set; } = new LinkedList<int>();
        public LinkedList<int> NextDescendingQueue { get; set; } = new LinkedList<int>();


    }
}
