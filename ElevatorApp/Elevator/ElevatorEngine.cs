using ElevatorApp.Elevator.ElevatorStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.Elevator
{
    /// <summary>
    /// Runs the Elevator on a separate thread using Tasks to allow asynchronous processing
    /// </summary>
    public class ElevatorEngine
    {
        Elevator Elevator = new Elevator();
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public bool IsExiting = false;
        private Task runningTask;
        public void Run()
        {
            runningTask = new Task(() =>
            {
                while (true)
                {
                    if (Elevator.ElevatorState is WaitingElevatorState)
                    {
                        if(IsExiting)
                        {
                            return;
                        }
                        else
                        {
                            resetEvent.WaitOne();
                        }
                    }
                    Elevator.MoveToNextFloor();
                    Task.Delay(3000).Wait();
                    Elevator.ArriveOnFloor();
                    if (Elevator.CurrentBehavior == ElevatorState.CurrentElevatorBehavior.Stopped)
                    {
                        Task.Delay(1000).Wait();
                    }
                    Elevator.UpdateState();
                }
            });
            runningTask.Start();
        }

        public void Exit()
        {
            IsExiting = true;
            resetEvent.Set();
            runningTask.Wait();
        }

        public void PushButton(string button)
        {
            if(!IsExiting)
            {
                Elevator.PushButton(button);
            }
            resetEvent.Set();
        }
    }
}
