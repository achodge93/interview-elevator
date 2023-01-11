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
    public class AscendingElevatorStateTests
    {
        Sensor sensor;
        ElevatorState elevatorState;

        public AscendingElevatorStateTests()
        {
            this.sensor = new Sensor();
            this.elevatorState = new AscendingElevatorState(sensor);
        }

        [Fact]
        public void WhenUserPushesUpOnFloorAboveCurrentFloor_ThenElevatorAddsFloorToQueue()
        {
            var newFloor = new Floor() { Direction = ElevatorApp.ElevatorDirection.Ascending, FloorNumber = 10 };
            elevatorState.AddFloor(newFloor);
            Assert.Single(sensor.CurrentQueue);
            Assert.Equal(10, sensor.CurrentQueue.First.Value);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        public void WhenUserPushesInsideButtonForFloorGreaterThanCurrent_ThenFloorIsAddedToQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { Direction = ElevatorApp.ElevatorDirection.None, FloorNumber = desiredFloor };
            elevatorState.AddFloor(newFloor);
            sensor.CurrentFloor = currentFloor;
            Assert.Single(sensor.CurrentQueue);
            Assert.Equal(desiredFloor, sensor.CurrentQueue.First.Value);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        [InlineData(10, 5)]
        public void WhenButtonIsDescending_ThenFloorIsAddedToNextDescendingQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { Direction = ElevatorApp.ElevatorDirection.Descending, FloorNumber = desiredFloor };
            sensor.CurrentFloor = currentFloor;
            elevatorState.AddFloor(newFloor);
            Assert.Empty(sensor.CurrentQueue);
            Assert.Single(sensor.NextDescendingQueue);
            Assert.Equal(desiredFloor, sensor.NextDescendingQueue.First.Value);
        }
    }
}
