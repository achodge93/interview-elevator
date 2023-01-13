﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator.ElevatorStates
{
    /// <summary>
    /// Encapsulates logic for when the elevator stops.
    /// If there are no more stops queued it will wait for user input before continuing
    /// </summary>
    public class WaitingElevatorState : ElevatorState
    {
        public WaitingElevatorState(Elevator elevator) : base(elevator)
        {

        }

        public override void AddFloor(Floor floor)
        {
            if(floor.FloorNumber > Elevator.CurrentFloor)
            {
                Elevator.ElevatorState = new AscendingElevatorState(Elevator);
                Elevator.ElevatorState.AddFloor(floor);
            }
            else if(floor.FloorNumber < Elevator.CurrentFloor)
            {
                Elevator.ElevatorState = new DescendingElevatorState(Elevator);
                Elevator.ElevatorState.AddFloor(floor);
            }

        }

        public override void ArriveOnFloor()
        {
        }

        public override int GetUpcomingFloor()
        {
            throw new NotImplementedException();
        }

        public override bool IsOppositeDirection(Floor floor)
        {
            throw new NotImplementedException();
        }

        public override void MoveToNextFloor()
        {
        }

        public override void ResetStopForCurrentDirection(Floor floor)
        {
        }

        public override void ResetStopForOppositeDirection(Floor floor)
        {
        }

        public override void SetStopForCurrentDirection(Floor floor)
        {
        }

        public override void SetStopForOppositeDirection(Floor floor)
        {
        }

        public override bool StopOnNextFloor() => true;

        public override void UpdateCurrentFloor()
        {
        }
    }
}
