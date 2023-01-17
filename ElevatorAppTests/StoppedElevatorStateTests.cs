using ElevatorApp.Elevator;
using ElevatorApp.Elevator.ElevatorStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ElevatorAppTests
{
    public class WaitingElevatorStateTests
    {
        Elevator Elevator = new Elevator();
        WaitingElevatorState elevatorState;

        public WaitingElevatorStateTests()
        {
            elevatorState = new WaitingElevatorState(Elevator);
        }

        [Fact]
        public void WhenElevatorIsStoppedAndFloorIsAddedAboveCurrentFloor_ElevatorAscends()
        {
            Elevator.CurrentFloor = 5;
            elevatorState.AddFloor(new Floor { FloorNumber = 10 });
            
            Assert.IsType<AscendingElevatorState>(Elevator.ElevatorState);
            Assert.True(Elevator.FloorList[10].AscendingCommand.ShouldStop);
        }

        [Fact]
        public void WhenElevatorIsStoppedAndFloorIsAddedBelowCurrentFloor_ElevatorDescends()
        {
            Elevator.CurrentFloor = 5;
            elevatorState.AddFloor(new Floor { FloorNumber = 10 });

            Assert.IsType<AscendingElevatorState>(Elevator.ElevatorState);
            Assert.True(Elevator.FloorList[10].AscendingCommand.ShouldStop);
        }
    }
}
