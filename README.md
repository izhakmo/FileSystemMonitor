# FileSystemMonitor

The project includes include 2 Web APIs

1. FileSystemRegistrator
	* Responsible for folder monitoring registration and unregistration
	* Has a Structure for the monitored Folders
	
2. FileSystemEventsHandler
	* Has a controller that is responsible for PrintEvents operations
	* Save events operations in a data Structures


When a monitored folder is receiving an event - FileSystemRegistrator (the monitored Folder) sends a PostEventLog request to FileSystemEventsHandler which save the event log in the data Structures


TODO:
* UT (there are some UTs right now, but need to add more)
* Improve DI - inject maxNumberOfFoldersToMonitor, create FileSystemMonitorFactory and inject to FileSystemWatcherMonitor, FileSystemMonitor - inject the URL, inject log filePath
* Improve PrintEventLogs data stractures