using Elevators.External;
using Microsoft.Extensions.Configuration;

namespace Elevators
{
    public class Controller
    {
        public Elevator[] Elevators;
        public int NUM_OF_FLOORS;
        public int NUM_OF_ELEVATORS;
        public int ELEVATOR_CAPACITY;
        private DVT _dvt;
        
        private readonly Dictionary<string, string> _config;

        public Controller(Dictionary<string,string> config)
        {
            _dvt = new DVT(config);
            _config = config;

            NUM_OF_FLOORS = int.Parse(config["NumberOfFloors"]);
            NUM_OF_ELEVATORS = int.Parse(_config["NumberOfElevators"]);
            ELEVATOR_CAPACITY = int.Parse(_config["ElevatorCapacity"]);

            Elevators = new Elevator[NUM_OF_ELEVATORS];
            for (int i = 0; i < NUM_OF_ELEVATORS; i++)
                Elevators[i] = new Elevator(i, config);
        }

        public List<int>? RequestElevator(int passengerFloor, int passengerCount)
        {
            var bestElevators = Elevators.OrderBy(c => c.GetTravelDistance(passengerFloor)).ToList();
            var sendingElevators = new List<(int, int)>();

            if (!Elevators[0].DestinationFloors.Keys.Contains(passengerFloor))
                throw new Exception("Invalid Floor");

            foreach (var e in bestElevators)
            {
                try 
                { 
                    sendingElevators.Add(e.SendElevator(passengerFloor)); 
                }
                catch (Exception ex)
                {
                    HandleElevatorFailure(e.Id, ex.GetBaseException().Message).Wait();
                }

                if (sendingElevators.Sum(e => e.Item2) >= passengerCount)
                    return sendingElevators.Select(c => c.Item1).ToList();
            }    

            return null;
        }

        public void Randomize()
        {
            Elevators = new Elevator[NUM_OF_ELEVATORS];
            for (int i = 0; i < NUM_OF_ELEVATORS; i++)
                Elevators[i] = new Elevator(i, _config);
        }

        private async Task HandleElevatorFailure(int elevatorId, string message)
        {
            Console.WriteLine($"Failed to send elevator {elevatorId}.  Reason: {message}");
            await _dvt.ElevatorFailure(elevatorId, message);
        }
    }
}
