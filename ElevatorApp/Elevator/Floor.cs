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
        public bool IsAscending { get; set; }
        public bool IsDescending { get; set; }
        public bool IsSetFromElevator { get; set; }
        public int FloorNumber { get; set; }
    }
}
