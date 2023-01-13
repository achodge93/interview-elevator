﻿using ElevatorApp.Elevator;
using ElevatorApp.Elevator.ElevatorStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ElevatorAppTests
{
    public class DescendingElevatorStateTests
    {
        Elevator Elevator;
        DescendingElevatorState elevatorState;

        public DescendingElevatorStateTests()
        {
            this.Elevator = new Elevator();
            Elevator.CurrentQueue = new SortedSet<int>(Comparer<int>.Create((x, y) => -x.CompareTo(y)));
            this.elevatorState = new DescendingElevatorState(Elevator);
        }

        [Fact]
        public void WhenUserPushesUpOnFloorBelowCurrentFloor_ThenElevatorAddsFloorToQueue()
        {
            var newFloor = new Floor() { IsDescending = true, FloorNumber = 10 };
            Elevator.CurrentFloor = 20;
            elevatorState.AddFloor(newFloor);
            Assert.Single(Elevator.CurrentQueue);
            Assert.Equal(10, Elevator.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(12, 10)]
        [InlineData(3, 1)]
        public void WhenUserPushesInsideButtonForFloorGreaterThanCurrent_ThenFloorIsAddedToQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = desiredFloor };
            Elevator.CurrentFloor = currentFloor;
            elevatorState.AddFloor(newFloor);
            Assert.Single(Elevator.CurrentQueue);
            Assert.Equal(desiredFloor, Elevator.CurrentQueue.FirstOrDefault());
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 12)]
        [InlineData(1, 3)]
        [InlineData(10, 5)]
        public void WhenButtonIsAscending_ThenFloorIsAddedToNextAscendingQueue(int currentFloor, int desiredFloor)
        {
            var newFloor = new Floor() { IsAscending = true, FloorNumber = desiredFloor };
            Elevator.CurrentFloor = currentFloor;
            elevatorState.AddFloor(newFloor);
            Assert.Empty(Elevator.CurrentQueue);
            Assert.Equal(desiredFloor, Elevator.FloorList.FirstOrDefault(x => x.IsAscending).FloorNumber);
        }

        [Fact]
        public void WhenElevatorMovesToNextFloor_ThenElevatorBehaviorSetToMoving()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();

            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Elevator.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloor_ThenElevatorContinuesMoving()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            Elevator.CurrentFloor = 0;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.Equal(ElevatorState.CurrentElevatorBehavior.Moving, elevatorState.Elevator.CurrentBehavior);
        }

        [Fact]
        public void WhenElevatorArrivesOnFloorPriorToExpectedFloor_ThenStopOnNextFloorIsTrue()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            Elevator.CurrentFloor = 4;
            elevatorState.AddFloor(newFloor);

            elevatorState.MoveToNextFloor();
            elevatorState.ArriveOnFloor();
            Assert.True(Elevator.StopOnNextFloor(false));
        }

        [Fact]
        public void WhenElevatorArrivesOnExpectedFloor_ThenElevatorBehaviorIsStoppedAndFloorIsRemovedFromQueue()
        {
            var newFloor = new Floor() { IsSetFromElevator = true, FloorNumber = 2 };
            Elevator.CurrentFloor = 4;
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
