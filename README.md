# Simple Notepad Automation with WinAppDriver

This simple console application uses [WinAppDriver](https://github.com/microsoft/WinAppDriver) to automate the following actions:

* Opening Notepad
* Typing random text
* Closing Notepad

This process repeats in an infinite loop until a random key is pressed.

## Prerequisites

* .NET 8 Runtime

## Usage

To run the application, execute `WinAppDriver_UserMouseKeystrokesSimulation.exe` with the following arguments:

* `--appid {path_to_notepad}`: Specifies the path to the Notepad executable.
* `--appdriverurl {WinAppDriver server url}`: Specifies the URL of the WinAppDriver server.

### Default Values

* `--appid="C:\Windows\System32\notepad.exe"`
* `--appdriverurl=http://127.0.0.1:4723`

### Example

```bash
WinAppDriver_UserMouseKeystrokesSimulation.exe --appid="C:\Windows\System32\notepad.exe" --appdriverurl=[http://127.0.0.1:4723](http://127.0.0.1:4723)