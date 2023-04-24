using Xunit;
using Elevators;
using System.Collections.Generic;

namespace Elevators.Tests
{
    public class ControllerTests
    {
        private Dictionary<string, string> _config = new Dictionary<string, string>
        {
            { "NumberOfFloors", "10" },
            { "NumberOfElevators", "2" },
            { "ElevatorCapacity", "6" },
            { "MovingBonus", "0,5" },
            { "WrongDirectionPenalty", "1,5" },
            { "FailurePercent", "10" }
        };

        [Fact]
        public void ControllerCreation()
        {
            var controller = new Controller(_config);
            Assert.NotNull(controller);
        }

        [Fact]
        public void ControllerRequestElevator_Success()
        {
            var controller = new Controller(_config);
            controller.Elevators[0].CurrentFloor = 3;
            controller.Elevators[0].Capacity = 6;

            controller.Elevators[1].CurrentFloor = 7;
            controller.Elevators[1].Capacity = 6;
            
            var elevators = controller.RequestElevator(6, 10);
            Assert.True(elevators[0] == 1);
            Assert.True(elevators[1] == 0);
        }

        [Fact]
        public void ControllerRequestElevator_Failure()
        {
            // Setting a 100% failure rate
            _config["FailurePercent"] = "100";

            var controller = new Controller(_config);
            var elevators = controller.RequestElevator(5, 5);
            Assert.Null(elevators);
        }

        [Fact]
        public void ControllerRequestElevator_InvalidFloor()
        {
            var controller = new Controller(_config);
            Assert.Throws<System.Exception>(() => controller.RequestElevator(-1, 5));
            Assert.Throws<System.Exception>(() => controller.RequestElevator(11, 5));
        }
    }
}