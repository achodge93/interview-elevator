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
        public void Run()
        {

            Task.Run(async () =>
            {
                while(true)
                {
                    if(Elevator.ElevatorState is WaitingElevatorState)
                    {
                        resetEvent.WaitOne();
                    }
                    Elevator.MoveToNextFloor();
                    await Task.Delay(3000);
                    Elevator.ArriveOnFloor();
                    if(Elevator.CurrentBehavior == ElevatorStates.ElevatorState.CurrentElevatorBehavior.Stopped)
                    {
                        await Task.Delay(1000);
                    }
                    Elevator.UpdateState();

                }
            });
        }
        public void PushButton(string button)
        {
            Elevator.PushButton(button);
            resetEvent.Set();
        }
    }
}
