using Elevators.Interfaces;

namespace Elevators
{
    public enum Status {
        Stationary,
        GoingUp,
        GoingDown
    }

    public class Elevator : IElevator
    {
        
        public int Id { get; private set; }
        public int MaximumCapacity { get; set; }
        public int Floors { get; set; }
        public int CurrentFloor { get; set; }
        public int Capacity { get; set; }
        private double MovingBonus { get; set; }
        private double WrongDirectionPenalty { get; set; }
        public Status Status { get; set; } = Status.Stationary;
        public Dictionary<int, bool> DestinationFloors { get; set; } = new Dictionary<int, bool>();

        private readonly Dictionary<string, string> _config;
        

        public Elevator(int id, Dictionary<string, string> config)
        {
            Id = id;
            _config = config;

            var gen = new Random();
            
            Floors = int.Parse(_config["NumberOfFloors"]);
            CurrentFloor = gen.Next(0, Floors + 1);
            Status = (Status)gen.Next(0, 3);
            MaximumCapacity = int.Parse(_config["ElevatorCapacity"]);
            Capacity = id == 2 ? 0 : gen.Next(0, MaximumCapacity + 1); //Guarantees that at least one elevator is at capacity.
            MovingBonus = double.Parse(config["MovingBonus"]) * -1;
            WrongDirectionPenalty = double.Parse(config["WrongDirectionPenalty"]) * -1;

            for (int i = 0; i <= Floors; i++)
                DestinationFloors.Add(i, false);
        }

        public (int, int) SendElevator(int passengerFloor)
        {
            var gen = new Random();
            DestinationFloors[passengerFloor] = true;

            if (gen.Next(0, 101) <= int.Parse(_config["FailurePercent"])) // Simulating a random chance that the elevator fails to send.
                throw new Exception($"Door won't close");

            Status = CurrentFloor.CompareTo(passengerFloor) switch {
                > 0 => Status.GoingDown,
                < 0 => Status.GoingUp,
                _ => Status.Stationary,
            };

            if (Status == Status.Stationary)
                DestinationFloors[passengerFloor] = false;

            return (Id, Capacity);
        }

        public double GetTravelDistance(int passangerFloor)
        {
            if (!IsValid(passangerFloor))
                return 999;

            double travelDistance = Math.Abs(CurrentFloor - passangerFloor);

            travelDistance += Status switch
            {
                Status.Stationary => 0,
                Status.GoingDown => (CurrentFloor - passangerFloor) > 0 ? MovingBonus : WrongDirectionPenalty,
                Status.GoingUp => (CurrentFloor - passangerFloor) < 0 ? MovingBonus : WrongDirectionPenalty
            };

            return travelDistance;
        }

        #region Private Methods
        private bool IsValid(int passangerFloor)
        {
            //Capacity doesn't matter if the elevator is already going to let out passengers on that floor
            if (Capacity == 0 && !DestinationFloors[passangerFloor])
                return false;

            return Status switch {
                Status.Stationary => true,
                Status.GoingUp => passangerFloor > CurrentFloor,
                Status.GoingDown => passangerFloor < CurrentFloor,
                _ => false
            };
        }
        #endregion
    }
}
