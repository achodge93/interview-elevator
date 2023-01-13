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
        Elevator Elevator;
        AscendingElevatorState elevatorState;
        public AscendingElevatorStateTests()
        {
            this.Elevator = new Elevator();
            this.elevatorState = new AscendingElevatorState(Elevator);
        }

        [Fact]
        public void WhenUserPushesUpOnFloorAboveCurrentFloor_ThenElevatorAddsFloorToQueue()
        {
            var newFloor = new Floor() { FloorNumber = 10 };
            newFloor.AscendingCommand.ShouldStop = true;
            elevatorState.AddFloor(newFloor);
            Assert.Single(Elevator.CurrentQueue);
            Assert.Equal(10, Elevator.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        public void WhenUserPushesInsideButtonForFloorGreaterThanCurrent_ThenFloorIsAddedToQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { FloorNumber = desiredFloor };
            newFloor.AscendingCommand.IsInElevator = true;
            elevatorState.AddFloor(newFloor);
            Elevator.CurrentFloor = currentFloor;
            Assert.Single(Elevator.CurrentQueue);
            Assert.Equal(desiredFloor, Elevator.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        [InlineData(10, 5)]
        public void WhenButtonIsDescending_ThenFloorIsAddedToNextDescendingQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { FloorNumber = desiredFloor };
            newFloor.DescendingCommand.ShouldStop = true;
            Elevator.CurrentFloor = currentFloor;
            elevatorState.AddFloor(newFloor);
            Assert.Empty(Elevator.CurrentQueue);
            Assert.Equal(desiredFloor, Elevator.FloorList.FirstOrDefault(x => x.DescendingCommand.ShouldStop).FloorNumber);
        }

        [Fact]
        public void WhenElevatorMovesToNextFloor_ThenElevatorBehaviorSetToMoving()
        {
            var newFloor = new Floor() { FloorNumber = 2 };
            newFloor.AscendingCommand.IsInElevator = true;
            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();

            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Elevator.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloor_ThenElevatorContinuesMoving()
        {
            var newFloor = new Floor() { FloorNumber = 2 };
            newFloor.AscendingCommand.IsInElevator = true;
            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Elevator.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloorPriorToExpectedFloor_ThenStopOnNextFloorIsTrue()
        {
            var newFloor = new Floor() {  FloorNumber = 2 };
            newFloor.AscendingCommand.IsInElevator = true;
            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.True(Elevator.StopOnNextFloor(true));
        }

        [Fact]
        public void WhenElevatorArrivesOnExpectedFloor_ThenElevatorBehaviorIsStoppedAndFloorIsRemovedFromQueue()
        {
            var newFloor = new Floor() { FloorNumber = 2 };
            newFloor.AscendingCommand.IsInElevator = true;

            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();

            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Stopped, Elevator.CurrentBehavior);
            Assert.Empty(Elevator.CurrentQueue);
        }
    }
}
