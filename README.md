# NHMonitor
NHMonitor remote NHibernate interceptor using gRPC streaming

This project aims monitor NHibernate activity without interfer with the performance of the application under test, using gRPC streams and asynchronous writes.

Here a screenshot showing the status of the project as it is today:
![screenshot](https://github.com/FelicePollano/NHMonitor/blob/main/screenshots/screenshot-%202023-08-01%20084402.png)

## How to use
(preliminary version )
Notice: the tool is not supposed to be used while application we want to monitor is in production, the tool is a debug utility and steps to test are supposed to be removed when app is deployed.
In order to monitor an application, the package NHMonitor.Probe has to be installed

 `Install-Package NHmonitor.Probe -v 1.0.5-alpha`

 After that, in the entry point of your application create an instance of:
 
 `var interceptor=new NHMonitor.Probe.Interceptor("MyAppName");`

 and dispose it in in feasible dispose point of your app.
 Build and run NHMonitor, starts your application. Queries must be intecepeted and reported.
