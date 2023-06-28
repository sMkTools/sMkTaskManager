## New Features

These features have to be implemented because they are not part of old VB.net project

* Performance numbers for IO should be deltas, not totals.
* Performance numbers should highlight when they change, ideally with +/- sign and colour, or draw a small box on the right edge.
* DarkMode - Has to be custom to set colors properly.
* perfChart should be able to split to show 1 graph per value
* Parent PID should be displayed, and possible grouped by and/or hide similars
* Processes tab column selection should also allow select deltas.

## Future Implementations

* GPU Monitoring
* WSL Instances lists and monitoring

## Known bugs
* Idle process is not showing when not showing Processes for all users, it should always display
* DONE: Process PageFault is freaking out, most likely some signature // It was a STRUCT issue with the value
* ListView Sorted values are not re-sorted, check with cpu usage, shall we call sort again after all refreshers?
