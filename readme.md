# Overview
This is a simple elevator app that does simple elevator things. I time boxed this to a few hours of real work. Work I did not get to because of time is explained below.
It functions using a simple implementation of the state machine pattern. Additional behaviors can be added by creating new IElevatorStates and updating the state transitions on new floors.
Async functionality is encapsulated within the ElevatorEngine. Elevator is the context class for the state machine.

#  Usage

- Enter a number between 0 and 200 to simulate pressing the button in the elevator.
- Enter a number between 0 and 200 followed by U or D to simulating going up or down respectively. Ex: 10U simulates pressing the outside up button on the 10th floor
- Type 'Q' or 'q' to exit. The elevator will continue its actions untill all remaining floors are visited. No user input will be accepted.
- Elevator progress will be printed to the console as well as a file log.

# Known Issues
- If the current floor is the 5th floor and a user enters 10D the elevator will correctly ascend to the 10th floor. However, if the user enters 14U or another ascending button, the elevator will still stop on the 10th floor and then ascend to the 14th floor before descending again. This could be resolved by adding a new ElevatorState or a little logic, but I don't have time to do so.
- Elevator input is not validated. You could put in 250U and it would crash the application. Due to time I assumed that users would not be able to press buttons that do not exist. In a real program the input would need to be validated (even if it's "impossible").
- Elevator floors are not configurable. Ideally the max floor, time per floor, etc would configurable. For the purposes of this exercise I didn't think it would matter a lot.
- General need for more tests/some general code cleanup. Tests need to be cleaned up and code de-deduplicated.
- Weight isn't really implemented