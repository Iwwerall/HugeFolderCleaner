# HugeFolderCleaner
Tool to delete folders with millions of files

If you ever needed to delete a folder with millions of files in them and wasted hours waiting for the counting 
to finish in Windows Explorer then this is for you.

I ran into a problem on a customer's PC that the C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys had almost 
10 million single use certificates in it, god only knows why (still have to find that out) 
and the command prompt, powershell nor the explorer was able to start deleting in a timely fashion.

This program can delete all files with a specific pattern in all subfolders without touching the folders
and thus leaving the rights intact and will only delete files with a specific age.
If a file cannot be deleted it will simply be skipped. Best to start with administrator rights.

Use cases:

HugeFolderCleaner.exe "C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys" *. 120

--> this will delete all files without an extension in the folder C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys
    that have not been written to in the last 120 hours (5 days to help you count)

HugeFolderCleaner.exe C:\inetpub\logs\LogFiles *.log 48

--> This will delete all log files from the IIS log folder that are at least 48 hours old. 
    Especially handy for exchange servers that have a lot of webmail access :)
    
Disclaimer:
You can use this freely, if you would like to use the code somewhere please mention where found it.
Any loss of data or any other damage you might suffer or have someone else suffer will be your sole responsability, 
this software and the source is provided as is.

Any comments or enhancements are welcome!

Bart Vanseer
