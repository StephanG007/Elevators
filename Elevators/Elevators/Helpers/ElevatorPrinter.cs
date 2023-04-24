namespace Elevators.Helpers
{
    public static class ElevatorPrinter
    {
        public static void PrintOverview(Elevator[] elevators)
        {
            var floors = elevators[0].Floors;
            for (int i = floors; i >= 0; i--)
            {
                Console.Write("{0,2}: |", i);
                for (int j = 0; j < elevators.Length; j++)
                    Console.Write(PrintElevatorStatus(i, elevators[j]));

                Console.WriteLine();
            }
            var xAxis = "   ";
            for (int i = 0; i < elevators.Length; i++)
                xAxis += $"    {i}";
            Console.WriteLine(xAxis);
        }

        private static string PrintElevatorStatus(int floor, Elevator elevator)
        {
            if (floor != elevator.CurrentFloor)
                return "    |";

            
            var capacity = string.Format("{0,2}", elevator.Capacity);

            if (elevator.Capacity == 0)
                capacity = $"\x1B[31m{capacity}\x1B[0m"; //Makes full elevators BOLD.
            else
                capacity = $"\x1B[32m{capacity}\x1B[0m";
            return elevator.Status switch
            {
                Status.Stationary => $"{capacity}X·|",
                Status.GoingUp => $"{capacity}X↑|",
                Status.GoingDown => $"{capacity}X↓|",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
