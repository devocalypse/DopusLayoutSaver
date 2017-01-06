##Dopus Layout Saver
for [Directory Opus](http://www.gpsoft.com.au/)

##About
Detects if dopus is running, backs up a specific profile and then resaves it so changes will not be lost upon crashes or other unexpected scenarios. Keeps specified number of backups discarding oldest ones when the limit is reached.

##Usage
* Choose a profile **name**.
* Set up dopus to load the specified profile **name** during startup.
* Set up a scheduled task using windows task scheduler to run this program at regular intervals.
* Add profile **name** as argument to the scheduled task and choose interval.
* (Optional) add backup limit count as second argument (defaults to 10 if omitted)

##Example
DopusLayoutSaver.exe profile 24

##Binaries
https://github.com/devocalypse/DopusLayoutSaver/releases
