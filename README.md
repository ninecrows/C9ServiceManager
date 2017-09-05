# C9ServiceManager
Facilitate the timed management of windows service activity. Sandbox service project with web UI.

This is a small sandbox project built using visual studio 2017.

The code implements (or will implement) a windows service in C# that
monitors other services and based on the time enables or disables each
service.

Expecting to persist configuration to the registry under the service
key for this service. Logging will be to the application log.

I expect that in the final implementation it will expose a web UI for
checking and modifying configuration and a RESTful interface that
allows external programmatic control.

<hr>

<h3>2017-Sep-05</h3>

Pushing in updates from over the weekend. The new code compiles but is
incomplete at this point. I'll finish factoring the configuration code
into the new assembly shortly and then add the rest of the
configuration information needed. Likely I'll switch all of the time
related bits over to <code>TimeSpan</code> and store these as strings
rather than integers. 

I expect to have the factored objects store 'events' internally and
then have the main service push them into the even log later. This
keeps some of the service specific code out of the helpers and
generalizes things a bit. 

I am trying to figure out how best to handle unit test and dependency
injection in some cases. The configuration object could have registry
code injected to facilitate but that would seem to add a lot of work
to enable control of the back end. Still chewing on that one.

<h3>2017-Sep-04</h3>

Pulling out the Win32 specific items into a separate assembly to
facilitate testing and perhaps some isolation between the main service
and the registry and SCM interactions.

Figuring this might help when the web UI and RESTful control
interfaces get added. Still not sure how to properly secure the web
interfaces...really want TLS without self signed certificate
hassles. Once I get there I guess I'll see.

Once this is done I should be in good shape to build the main body of
the service that checks periodically for service state changes and
make necessary adjustments to service state.

<h3>2017-Aug-27</h3>

Added registry key enumeration for list of services to be probed.

Full features are evolving a bit as I work on this. Logging of service
state changes seems like another useful feature. Stopping and starting
on a time schedule on top of that makes for a tidy package.

Still need to factor the service control aspects and the registry
aspects and layer interface(s) on top to clean this up. May pull them
out into a separate assembly before I'm done.

Once the main functions are in place I'll be looking to add the
external RESTful interfaces to control and monitor the service and
(hopefully) a web UI to make human control easier.

I expect to put the main operational guts in place today.

- Run and monitor service status changes logging them.
- Read suppression time ranges and stop selected services during their 'off' times.
- Probably include an 'always off' option to suppress given services until told to stop.
- If we've stopped a service, restore it to its previous state (stored in memory) when its passes outside of its suppress interval.

After that code is in place and working I'll move on to trying to
build an installer using Wix to simplify the process of getting this
on a system.

I'm realizing that the service should have more persistence of its
memory for pre-suppression state of a service...likely to be a JSON
serialzed object stored in the service's control key. To be done
later...for now if you reboot your system, the service will remember
what it saw on restart.

I don't expect that I'll get to the RESTful interface addition
today. I suspect that there will be issues with runtime versions there
as I've been reading that .NET core and Win32 don't necessarily
coexist peacefully...though OWIN and NancyFX based versions may be
easier to slot in than .NET core in this case.

<hr>

<h3>2017-Aug-26</h3>

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

Looking at the possibility of splitting off the helper code into a
separate assembly.

Need to figure out some option for packing the service parts and
registration/deregistration into an MSI for ease of use.

<hr>

<h3>2017-Aug-25</h3>

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

