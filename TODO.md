## New Features

These features have to be implemented because they are not part of old VB.net project

* Performance numbers should highlight when they change, ideally with +/- sign and colour, or draw a small box on the right edge.
* DarkMode - Has to be custom to set colors properly.
* perfChart should be able to split to show 1 graph per value.
* Parent PID should be displayed, and possible grouped by and/or hide similars.
* Processes tab column selection should also allow select deltas.
* Implement shared ImageList for Process Icons that is accessible by any tab.
* Release Locked files was never implemented.
* Reveal process windows was never implemented.
* Processes CommandLine was never implemented.

## Future Implementations

* GPU Monitoring.
* WSL Instances lists and monitoring.

## Known bugs

* DONE: Idle process is not showing when not showing Processes for all users, it should always display.
* DONE: Process PageFault is freaking out, most likely some signature // It was a STRUCT issue with the value.
* ListView Sorted values are not re-sorted, check with cpu usage, shall we call sort again after all refreshers?.
* When sorting a ListView column for the second time it makes the control to scroll to origin.
* Highlighted items are not being cleared properly when property doesnt change.

## Pending migration list

* File / New Tasks (Run As)
* Test out User Connnect/logoff/disconnect
* Summary View for Processes (not sure i want it anymore)
* Set Default Task Manager
* System Tray CPU Usage
* Process Locked Files 
* Shrink Timer (is it still needed?)
