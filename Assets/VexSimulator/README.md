TODO: IMPROVE THIS

# Design comments

- Threads only pass data! Don't make thread queues to call functions on the main thread. Simply set
data inside a thread which the code can then react to in order to process unity main thread functions.
Example: MotorizedWheel.cs + Robot.cs

# Common Crash Problems
- Inside c# unsafe api, trying to reference a non-existent cpp api function