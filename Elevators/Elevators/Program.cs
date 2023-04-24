using Elevators;
using Elevators.Helpers;
using Newtonsoft.Json;

var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"C:\Users\sggoo\Code\Elevators\local.settings.json"));

var controller = new Controller(config);

while (true)
{
    Console.WriteLine("Here is what the current elevator status looks like: ");
    ElevatorPrinter.PrintOverview(controller.Elevators);
    Console.WriteLine("Enter a floor number to send an elevator, 'R' to randomize the elevators again or 'E' to exit.");

    var input = Console.ReadLine() ?? "R";

    switch(input.ToUpper()[0])
    {
        case 'R': 
            controller.Randomize(); 
            break;
        case 'E': 
            ExitProgram(); 
            break;
        default:
            ProcessFloorInput(input); 
            break;
    };
    Console.Clear();
};

void ProcessFloorInput(string input)
{
    try
    {
        Console.WriteLine("How many passengers are waiting for a lift on this floor?");
        var passengerCount = int.Parse(Console.ReadLine() ?? "0");

        int.TryParse(input, out int floor);
        var best = controller.RequestElevator(floor, passengerCount);

        if (best == null)
            Console.WriteLine("No elevators available. Press Enter to continue");
        else
            Console.WriteLine($"Sending Elevators [{string.Join(", ", best)}]. Press Enter to continue.");

        Console.ReadLine();
    }
    catch
    {
        Console.WriteLine("That is not valid input.");
    }
}

void ExitProgram()
{
    Console.WriteLine("Thank you for using our simulator! Goobye.");
    Environment.Exit(0);
}