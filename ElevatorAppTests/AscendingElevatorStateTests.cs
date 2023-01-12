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
        AscendingElevatorState elevatorState;

        public AscendingElevatorStateTests()
        {
            this.sensor = new Sensor();
            this.elevatorState = new AscendingElevatorState(sensor);
        }

        [Fact]
        public void WhenUserPushesUpOnFloorAboveCurrentFloor_ThenElevatorAddsFloorToQueue()
        {
            var newFloor = new Floor() { IsAscending = true, FloorNumber = 10 };
            elevatorState.AddFloor(newFloor);
            Assert.Single(sensor.CurrentQueue);
            Assert.Equal(10, sensor.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        public void WhenUserPushesInsideButtonForFloorGreaterThanCurrent_ThenFloorIsAddedToQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = desiredFloor };
            elevatorState.AddFloor(newFloor);
            sensor.CurrentFloor = currentFloor;
            Assert.Single(sensor.CurrentQueue);
            Assert.Equal(desiredFloor, sensor.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        [InlineData(10, 5)]
        public void WhenButtonIsDescending_ThenFloorIsAddedToNextDescendingQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { IsDescending = true, FloorNumber = desiredFloor };
            sensor.CurrentFloor = currentFloor;
            elevatorState.AddFloor(newFloor);
            Assert.Empty(sensor.CurrentQueue);
            Assert.Equal(desiredFloor, sensor.FloorList.FirstOrDefault(x => x.IsDescending).FloorNumber);
        }

        [Fact]
        public void WhenElevatorMovesToNextFloor_ThenElevatorBehaviorSetToMoving()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            sensor.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();

            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Sensor.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloor_ThenElevatorContinuesMoving()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            sensor.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Sensor.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloorPriorToExpectedFloor_ThenStopOnNextFloorIsTrue()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            sensor.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.True(sensor.StopOnNextFloor(true));
        }

        [Fact]
        public void WhenElevatorArrivesOnExpectedFloor_ThenElevatorBehaviorIsStoppedAndFloorIsRemovedFromQueue()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            sensor.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();

            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Stopped, sensor.CurrentBehavior);
            Assert.Empty(sensor.CurrentQueue);
        }
    }
}
