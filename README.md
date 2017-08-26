# C9ServiceManager
Facilitate the timed management of windows service activity. Sandbox service project with web UI.

This is a small sandbox project.

The code implements (or will implement) a windows service in C# that
monitors other services and based on the time enables or disables each service.

I expect that in the final implementation it will expose a web UI for
checking and modifying configuration and a RESTful interface that
allows external programmatic control.

-----

2017-Aug-26

Added installer classes and renamed service class.

Need to move on to event logging and creating an MSI installer to put
the service in place on non-dev systems. Need to consider Wix or the
free installshield version.

-----

2017-Aug-25

Starting with the basic service definition and installer pieces.

Once those are together I'll probably look at creating a wix installer
to wrap the artifacts into an MSI to make this installable.

Added the blank service. Next I'll set names inside and start
configuring the guts.

Initially I expect it to read from the registry for targets and times
and provide essentially no user interface.

Once that is up and working I'll add in a set of web methods to
control it (likely using NancyFX as I have microservices examples
using that) and a small web UI.

