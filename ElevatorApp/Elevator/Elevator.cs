using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator
{
    /// <summary>
    /// Controls and updates Elevator state based on input
    /// </summary>
    public class Elevator
    {
        public Sensor Sensor { get; }

        public void PushButton(string button)
        {
            if (button.EqualsIgnoreCase("Q"))
            {
                return;
            }
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

                floorUpdate.IsAscending = direction.EqualsIgnoreCase("U");
                floorUpdate.IsDescending = direction.EqualsIgnoreCase("D");
                floorUpdate.IsSetFromElevator = direction == null;

                Sensor.ElevatorState.AddFloor(floorUpdate);
            }
        }
    }
}
