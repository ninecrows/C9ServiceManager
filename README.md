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

Now running the timer tick reliably. Manual install and uninstall with
events is all up and running.

Next moving on to reading settings from the registry...ideally from
the services key that the service owns. Looking in the ServiceBase
class to see what is there. If that isn't workable I'll look at using
the generic Registry class to read and enumerate settings as
appropriate.

The service now successfully reads the list of services in the system
and has the potential to also check and modify service status. Needs
to properly dispose of those ServiceController objects (should
probably encapsulate the function as I don't really need 'live'
objects here). Should also wrap the registry access in something that
provides a more 'civilized' view with back-end abstraction to
arbitrary persistence stores. Work to be done.

Need to add in the list of services that should be monitored and then
time interval support to allow services to be started and stopped at
appropriate times.

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

