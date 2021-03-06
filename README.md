# SimpleEdit
SimpleEdit is a GUI application to quickly prototype code.

## Building from source
You will need the following requirements:

* .NET Framework 4.5 or later
* Visual Studio 2013 or later
* Windows Vista or later

To build from within Visual Studio, open the file "SimpleEdit.sln". In the Project menu, go to Build, and the results will be found in the "obj/" folder in the SimpleEdit folder if debugging is off, or it will build to "debug/" while debugging is on.

To build from command-line:

* CD to the folder the "SimpleEdit.sln" and "suo" are found.
* Type the following: `devenv /build Debug SimpleEdit.sln`
* If `devenv` cannot be found, it is likely this has not been set in your PATH.

## Using the Editor

You can navigate the file system just as you would using the command-line with some limited exceptions (using `cd`):

* You cannot cd out multiple folders like this: `../..` it can only be done once.

Pressing `CTRL+TAB` will shift between tabs.

The following commands are available:

* `cmd` allows you to execute any command-line argument, printing the results in the above box. This pauses execution of the client until the command is complete. It is best to use `cmds` for all other operations.
* `cmds` allows you to execute any command-line argument, redirecting the results asynchronously. Currently, this uses an async thread from the MainForm, so it still freezes. This known issue will be patched when a solution is found.
* `new` creates a new file in the current directory.
* `open` opens a file in the current directory.
* `save` saves the currently opened file in the currently selected tab.
* `opent` opens a file in the current directory in a new tab.
* `newt` creates a new file in a new tab.

All commands you previously used can be returned to using the `UP` and `DOWN` arrow keys, just like command prompt. These commands are saved in a file at the root of the application.
