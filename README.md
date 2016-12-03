<p align="center"><img src="https://git.djdavid98.hu/SeinopSys/ServMon/raw/master/ServMon/Resources/logo.ico" alt="ServMon logo" height="150"></p>
<h1 align="center">ServMon</h1>
<p align="center">A simple, 2-side service monitor for Windows</p>

## What's the point?

I was previously using XAMPP's control panel to start/stop Apache and MySQL when I wanted to develop my projects. Ever since I switched to PostgreSQL I had to manage that service separately, as XAMPP's panel did not have a slot for it (obviously). For this reason, I wrote my own control panel that does what XAMPP's panel did, but for any 2 services of my choice.

In addition to custom service tracking/controlling, the tray icon of the application provides direct feedback on the state of the tracked services through 2 stipes which are colored green/yellow/red based on the service's state. The same autostart functionality also exists, in the form of a checkbox above the service name. After checking it and saving the settings, when the appplication is next run, it will automatically start the checked services.

### What if I only want to track 1 service?

Just select the first, empty value on either side. In this case, instead of 2 stripes the entire tray icon color will show the state of the tracked service.
