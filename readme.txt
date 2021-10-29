All of the code for actually copying the directory can be found within the CopyDirectory.Functions assembly

There is a CopyDirectory.ConsoleApp which demonstrates a basic way for the CopyDirectory.Functions to be used within a UI

If I was building this normally I would release the CopyDirectory.Functions assembly into a nuget library, I have normally used azure devops for this so that we can have a private nuget library for projects that you wouldn't want to make publicly available. This nuget package could then be installed into various front end projects to be used as and where desired.