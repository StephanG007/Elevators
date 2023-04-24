namespace Elevators.Interfaces
{
    public interface IElevator
    {
        public int MaximumCapacity { get; set; }
        public int Floors { get; set; }
        public int CurrentFloor { get; set; }
        public int Capacity { get; set; }
        public Status Status { get; set; }
        public Dictionary<int, bool> DestinationFloors { get; set; }

        public (int, int) SendElevator(int passangerFloor);
        public double GetTravelDistance(int passangerFloor);
    }
}
