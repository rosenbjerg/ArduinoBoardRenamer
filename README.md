# ArduinoBoardRenamer
A very simple program to temporaily change the name of an Arduino board i the boards.txt file

### Usage
Open the program. Admin rights might be required to write to the boards.txt file
Edit file path is boards.txt is located elsewhere and press 'Load'
Change 'Arduino Micro' to the name of the board you are using
Change 'My Arduino Project' to the device name you want your board to have
Press 'Rename'
Flash your Arduino program to your board
Verify that the device name has been changed (e.g. in 'Devices & Printers')
Press 'Restore' to restore the name of the board to its original, so that next time you flash a similar board, it will have the original name, (f.x. 'Arduino Micro').
