// See https://aka.ms/new-console-template for more information

using ElevatorApp.Elevator;

bool loop = true;
var engine = new ElevatorEngine();
engine.Run();
while(loop)
{
    Console.Write("Please press a button: ");
    var nextAction = Console.ReadLine();
    Console.WriteLine();
    if(nextAction == "Q")
    {
        loop = false;
    }
    else
    {
        engine.PushButton(nextAction);
    }
}