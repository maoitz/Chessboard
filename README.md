This is a simple console program that first displays a main menu, in which the user can choose to play or quit.
If play-option is chosen the console displays a standardized 8x8 chessboard with added tags (ex. B4).
Under the chessboard is an options-menu displayed in which the user can choose to customize
the size of the chessboard by a custom area.
The user can also choose to change the black and white squares to custom characters, supported by UTF-8.
After each prompt the user has a choice to return to the main menu which exits the game loop and goes to the main loop.

The code has a fail-safe for each user input which makes sure the program doesn't crash if wrong type of input is made.
The logic for the chessboard is made up of methods aswell as the menus and fail-safes.

I tried implementing a way to replace a square with a chesspiece by user choice, using the attached tags to each row and column
but to no avail.
