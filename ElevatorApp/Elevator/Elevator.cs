using ElevatorApp.Elevator.ElevatorStates;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ElevatorApp.Elevator.ElevatorStates.ElevatorState;

namespace ElevatorApp.Elevator
{
    /// <summary>
    /// Controls and updates Elevator state based on input
    /// </summary>
    public class Elevator
    {
        /// <summary>
        /// Gets or sets the current floor the elevator is on.
        /// If the elevator is in-between floors this will display the previous floor.
        /// When it arrives or passes a floor it is updated.
        /// </summary>
        public int CurrentFloor { get; set; }
        /// <summary>
        /// Gets or sets the next floor that will be stopped on
        /// </summary>
        public int NextFloor => CurrentQueue.FirstOrDefault();
        public int CurrentWeight { get; set; }
        /// <summary>
        /// Determines if the elevator will stop on the next floor or continue.
        /// </summary>
        public bool StopOnNextFloor(bool isAscending) =>
            isAscending
                ? (NextFloor == CurrentFloor + 1)
                : (NextFloor == CurrentFloor - 1);

        public IElevatorState ElevatorState;
        public CurrentElevatorBehavior CurrentBehavior { get; set; } = CurrentElevatorBehavior.Stopped;
        public ElevatorDirection Direction { get; set; }
        private IElevatorState AscendingState;
        private IElevatorState DescendingState;

        /// <summary>
        /// Contains all of the floors and their current request state
        /// </summary>
        public IList<Floor> FloorList = new List<Floor>(Enumerable.Range(0, 200).Select(x => new Floor() { FloorNumber = x }));

        /// <summary>
        /// Contains and ordered set of the floor numbers that are in line to be stopped at. 
        /// New additions are automatically sorted in either ascending or descending order based on the current ElevatorState
        /// </summary>
        public SortedSet<int> CurrentQueue { get; set; } = new SortedSet<int>();

        public Elevator()
        {
            ElevatorState = new WaitingElevatorState(this);
            AscendingState = new AscendingElevatorState(this);
            DescendingState = new DescendingElevatorState(this);
        }
        public void MoveToNextFloor()
        {
            ElevatorState.MoveToNextFloor();
        }
        public void ArriveOnFloor()
        {
            ElevatorState.ArriveOnFloor();
        }

        public void UpdateState()
        {
            ElevatorState.UpdateState();
        }

        public void PushButton(string button)
        {
            /**
             * Matches any number of digits. Optionally matches an additional character at the end.
             * Match examples: 010U, 100000z, 0p, 1
             * Non-match: 10U11U, z100
             */ 
            var regex = new Regex(@"(?<floor>^[\d]+)(?<direction>[a-zA-Z])?$");
            var match = regex.Match(button);
            if(match.Success)
            {
                var floorMatch = int.TryParse(match.Groups["floor"].Value, out int floor);
                var direction = match.Groups["direction"]?.Value;
                var floorUpdate = new Floor();

                floorUpdate.FloorNumber = floor;

                var isFloorRequestAscending = direction?.EqualsIgnoreCase("U") ?? false;
                var isFloorRequestDescending = direction?.EqualsIgnoreCase("D") ?? false;

                floorUpdate.AscendingCommand.ShouldStop = isFloorRequestAscending;
                floorUpdate.DescendingCommand.ShouldStop = isFloorRequestDescending;
                floorUpdate.AscendingCommand.IsInElevator = string.IsNullOrEmpty(direction);
                floorUpdate.DescendingCommand.IsInElevator = string.IsNullOrEmpty(direction);
                ElevatorState.AddFloor(floorUpdate);
                Log.Information(
                                $"Adding new stop to the elevator." +
                                $"Is ascending stop: { FloorList[floor].AscendingCommand.ShouldStop }. " +
                                $"Is descending stop: { FloorList[floor].DescendingCommand.ShouldStop }. " +
                                $"Floor: { floor }"
                               );
            }
            else
            {
                Log.Warning($"Unable to parse input { button }. Ensure that the format matches {{number}}{{character}}");
            }
        }

        internal bool IsAtWeightCapacity() => CurrentWeight >= 2100;
    }
}
