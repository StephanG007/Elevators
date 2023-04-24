using Xunit;
using Elevators;
using System.Collections.Generic;

namespace Elevators.Tests
{
    public class ElevatorTests
    {
        private Dictionary<string, string> _config = new Dictionary<string, string>
        {
            { "NumberOfFloors", "10" },
            { "ElevatorCapacity", "6" },
            { "MovingBonus", "0,1" },
            { "WrongDirectionPenalty", "3,5" },
            { "FailurePercent", "10" }
        };

        [Fact]
        public void ElevatorCreation()
        {
            var elevator = new Elevator(0, _config);
            Assert.NotNull(elevator);
        }

        [Fact]
        public void ElevatorValidTravelDistance()
        {
            var elevator = new Elevator(0, _config);
            elevator.CurrentFloor = 10;
            elevator.Status = Status.GoingDown;
            double distance = elevator.GetTravelDistance(5);
            Assert.Equal(4.9, distance);
        }

        [Fact]
        public void ElevatorInvalidTravelDistance()
        {
            var elevator = new Elevator(0, _config);
            elevator.Capacity = 0;
            double distance = elevator.GetTravelDistance(3);
            Assert.Equal(999, distance);
        }

        [Fact]
        public void ElevatorSuccessSend()
        {
            Elevator elevator = new Elevator(0, _config);
            elevator.Status = Status.Stationary;
            elevator.CurrentFloor = 0;
            elevator.Capacity = 2;

            (int id, int capacity) = elevator.SendElevator(5);
            Assert.Equal(0, id);
            Assert.Equal(2, capacity);
        }

        [Fact]
        public void ElevatorFailureSend()
        {
            // Setting a 100% failure rate
            _config["FailurePercent"] = "100";

            Elevator elevator = new Elevator(0, _config);
            elevator.Status = Status.Stationary;
            elevator.CurrentFloor = 0;
            elevator.Capacity = 2;

            // Expecting an exception to be thrown due to 100% failure rate
            Assert.Throws<System.Exception>(() => elevator.SendElevator(5));
        }
    }
}