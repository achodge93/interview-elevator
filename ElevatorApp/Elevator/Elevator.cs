using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator
{
    public class Elevator
    {
        public Sensor Sensor { get; }
        public Elevator()
        {
            Sensor = new Sensor();
        }
        public void PushButton(string button)
        {
            if (button.Equals("Q", StringComparison.OrdinalIgnoreCase))
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

                // base case - inside the elevator
                if(IsAscending)
                {
                    if(CurrentFloor > floor)
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
                    if(CurrentFloor > floor)
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
}
