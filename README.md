# ArduinoBoardRenamer
A very simple program to temporarily change the name of an Arduino board in the boards.txt file, and thereby changing the usb device name.

### Usage
Open the program. Admin rights might be required to write to the boards.txt file

Edit file path if boards.txt is located elsewhere and press 'Load'

Select the board you want to rename in the dropdown list

Change 'My Arduino Project' to the device name you want your board to have

Press 'Rename'

Flash your Arduino program to your board

Verify that the device name has been changed (e.g. in 'Devices & Printers')

Press 'Restore' to restore the name of the board to its original, so that next time you flash that kind of board, it will have the original name, (f.x. 'Arduino Micro').
