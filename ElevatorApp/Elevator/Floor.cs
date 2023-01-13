using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator
{
    /// <summary>
    /// Stores data related to the floor such as floor number and whether the floor request is for an ascending or descending elevator
    /// </summary>
    public class Floor
    {
        public ElevatorCommand AscendingCommand { get; set; } = new ElevatorCommand();
        public ElevatorCommand DescendingCommand { get; set; } = new ElevatorCommand();
        public int FloorNumber { get; set; }
    }
}
