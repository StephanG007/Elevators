# Elevator Simulation

This project is an elevator simulation designed to demonstrate the logic behind a basic elevator system. The program simulates multiple elevators and finds the most suitable one to send to a requested floor.

## Conceptualization

The heart of the program lies in the decision making that occurs when passangers arrive on a floor and request an elevator. So to focus the program on the core functionalities, I have decided not to simulate the way that elevators will change over time as every decision will be made on a snapshot of the system's status at the moment the controller receives the call request.

To complement this approach, the program will randomize with every iteration (and on request), so that the decision making can be judged in various states. However, given the random loadouts, it would be fairly rare for an elevator to be at maxium capacity, so I have hardcoded elevator 2 to always have maximum occupancy to ensure that there is always an elevator available to test "at capacity" functionality.

In order to better assess program's decisions, I have decided to print out all the floors with the elevators. This makes it visually easy to determine the correct expected behaviour of the program should be. Although this will work with any number of floors and elevators configured in the program, it does get cumbersome with very large numbers of floors.

## Known Errors

There are several things that I know are less than ideal with the current program, but have decided to keep for various reasons. Here is my reason for keeping them:

1. **Failure to Parse**: I've simply written the local.settings JSON in the format expected by my environment. Depending on your Environment it may fail to parse the double values. I didn't consider it worth the trouble of accounting for. If you experience this error, just change the values to use a period instead of a comma.

2. **Non Injected Configuration**: Ideally there would be a config builder that could be injected into the various classes. But given the simplicity of this Console Application, I simply simulated having an injected configuration by reading the settings into a dictionary. This is not how I would do it in a more complicated solution, but wanted to keep my code readable and the plumbing to a minimum.

3. **Synchronous vs. Asynchronous**: The simulation uses asynchronous methods for sending elevator notifications in case of failure but doesn't fully utilize async/await throughout the codebase. This decision was made to keep the code simple and focused on the elevator logic.

4. **Requests Don't Have Direction**: A real elevator has buttons to indicate direction when summon them. However it seems to me that this would drastically increase the complexity of the program, while delivering very debatable results in a context where new requests can arrive at any time.

5. **Elevator Failure**: The simulation has a configurable percentage chance of elevator failure. When an elevator is sent to a floor, it may fail with the given probability. In the case of failure, the simulation will try to send the next best elevator and send an HTTP notification to an external party to notify them of the failure. This is purely for demonstration purposes, in practise different kinds of failures would be handled differently.

6. **Outgoing Call Error Handling** I have included the functionality for the elevator to make an outgoing call to an external party in the event of an elevator failure to communicate that it needs maintenance or intervention. This is only for demonstration purposes, in practise there would be a variety of potential errors that are each handled differently. Also there would be a much more thorough handling of an outgoing request that fails, however as that is outside of the scope of this exercise, I have left it to a bare minimum.

7. **Elevator Capacity**: Although the specification specifically asks for the elevators to show "how many people they are carrying", but I decided that it would make more sense for the elevators to show how many people they had room for instead. I felt this is more intuitive and simplifies calculations such as working out how many elevators are needed on a floor.

## Assumptions

While creating this simulation, several assumptions have been made to focus the program:

1. **Number of Floors and Elevators**: The number of floors and elevators in the building is configurable, but the simulation assumes a relatively small number of floors and elevators to keep the simulation and output easy to understand.

2. **Elevator Capacity**: Each elevator has a maximum capacity of passengers it can carry, which is also configurable. The simulation doesn't take into account the weight of passengers, assuming each passenger occupies one unit of capacity.

3. **Elevator Status**: The simulation assumes elevators can be in one of three states: stationary, going up, or going down. The simulation does not consider the time it takes for elevators to change direction or open/close doors. It does however give a slight advantage to an elevator that is already moving, so that between two elevators one that is already moving towards the target destination will be given priority.

4. **Elevator Travel Distance**: The simulation calculates the travel distance of an elevator based on its current floor and the requested floor. Elevators that are already moving towards the requested floor receive a slight bonus to their distance calculation, making them more likely to be selected.

5. **Randomization**: The simulation allows randomizing the elevators' positions and statuses. This feature is provided to quickly test the system with various elevator configurations.

## Usage

To run the simulation, first replace the **local.settings.json** file's location with it's full location on your own system. Then simply build and run the project. You will be presented with a visualization of the current state of the elevators and prompted to enter a floor number to request an elevator. You can also randomize the elevators' positions and statuses by entering 'R' or exit the program by entering 'E'.
