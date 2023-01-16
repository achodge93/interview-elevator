// See https://aka.ms/new-console-template for more information

using ElevatorApp;
using ElevatorApp.Elevator;
using Serilog;

bool loop = true;
var cancellationToken = new CancellationTokenSource();
var engine = new ElevatorEngine();
engine.Run(cancellationToken.Token);
Console.WriteLine(@"Please enter a floor to get started.");
Console.WriteLine("You can enter 4U to make a request to go up from the fourth floor or 4D to go down");
Console.WriteLine("If you enter 4 without a direction it will add this to a list of stops the elevator is making in the current direction or queue for the next direction");
Console.WriteLine("If you enter Q the elevator will complete its stops but will allow no more input");
while(true)
{
    Console.WriteLine("Please press a button: ");
    var nextAction = Console.ReadLine();

    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("elevator.log", rollingInterval: RollingInterval.Hour)
        .CreateLogger();

    if (nextAction.EqualsIgnoreCase("Q"))
    {
        engine.Exit();
        Environment.Exit(0);
    }
    else if(!cancellationToken.IsCancellationRequested)
    {
        engine.PushButton(nextAction);
    }
}