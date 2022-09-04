# A Command Line Dungeon Crawler
### _Welcome to Command Line Dungeon, may your quest be filled with riches!!!_

### Introduction
This project is the implementation of a commandline dungeon crawler in C#.
Using and modifying the given codebase in line with the tests as requested, i implemented the dungeon crawler with all required features, both in advanced mode, and in extra features. <br/>

### How to play
The map is loaded when the user enters the load command, and then displays on the screen. <br/>
By typing "play" or "Advanced play", the game starts and the player can crawl through the map through valid paths, to the X tile where the game ends. The player can pick up coins as extra gain. in the opposite sense, the player encounters obstacles on the way in the form of monsters. the player has to avoid these monsters, or attack and kill them to gain extra coins (in advanced mode). These monsters are dangerous and can attack and kill a player. the goal is to crawl through the map and exit through the X tile without being killed by the monster. <br/>
Sounds fun right? have a wonderful time playing the Dungeon Crawler! Cheers!!

### Game Characters
\# = walls (cannot be passed) <br/>
\- = valid path <br/>
C = coins (can be picked by player) <br/>
M = monsters <br/>
S = game start point <br/>
\@ = player <br/>
X = exit tile

### Controls
load Simple.map = loads simple map <br/>
load Advanced.map = loads advanced map <br/>
play = starts game <br/>
Advanced play = starts the game in advanced mode <br/>
w = move north <br/>
a = move west <br/>
s = move south <br/>
d = move east <br/>
quit = quit <br/>
p = pickup coin (Advanced) <br/>
[space] = attack monster (Advanced) <br/>
replay = replay the map (Advanced) <br/>

### Advanced functionalities
player moves without pressing enter <br/>
player can pick up coin or walk pass it <br/>
player can attack monster <br/>
player and monsters have more than one damage points (3) <br/>
monsters move <br/>
monsters can attack a player <br/>
monsters can pickup coins <br/>
monsters drops off coin when killed <br/>
Upon entering the X tile, can replay the map by typing "Replay" <br/>

### Algorithm and Resources
With the Console.writeLine(), an instruction is written to the screen and the user is required to enter a command to continue. The readLine() or [readKey()](https://stackoverflow.com/questions/3068168/how-to-read-a-key-pressed-by-the-user-and-display-it-on-the-console) class captures the user's command and the system processes this command.<br/>
To load a map, using Streamreader, the map is read from the map file to a List which holds every read line as a list element. A jagged array of characters is initialized with every character of the List elements. This jagged array holds the map throughout the gameplay. The jagged array is cloned, and the clone is updated each time the player makes a move to contain the player move. That is, one jagged array holds the originally read map, and another holds the map as updated each time the player makes a move.<br/>
By entering the "play" or "Advanced play" command, the starting point of the game (S) changes to the player symbol (@) to indicate the game started. using any of the commands, the player can move through a valid path, can pickup coins to increase its damagepoint, and can attack and do damage to a monster.<br/>
Anytime the player makes a move, the system checks if the position the player is moving to is a valid path. it then changes the player symbol to the position, if it is a valid path. if the position where the player moved to is occupied by a coin, the coin is added to the player's accumulated coins. For advanced mode, a boolean variable dictating if a player is standing on a coin is set to true, this boolean value is checked each time the player enters 'P' to pick up coin, as you know the player can only pick up coin when standing on it. if a player leaves the tile without picking up, the coin symbol is set back to the position.
In advanced mode, using [Random() class](https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0) the monsters move randomly through valid paths, and can pickup coins to increase their damagepoint. When they die, they drop off all the coins accumulated. Using the space key, the player can do damage to any monster placed beside it, reducing the damage point of the monster. when a monster moves in the direction of a player, hitting the player, the monster does damage to the player, reducing the player's damage point. if either a player's or a monster's damage point is zero (0), the player or the monster dies. the game ends when a player dies.<br/>
On entering the X tile, the game ends. For advanced mode, the player can replay the currently loaded map by typing 'Replay', or can quit the game by typing 'Quit'.

### References
How to read a key pressed by the user and display it on the console? - https://stackoverflow.com/questions/3068168/how-to-read-a-key-pressed-by-the-user-and-display-it-on-the-console <br/>
Random Class - https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0

### Video Link
Video walkthrough on the code and algorithm - [YouTube](https://youtu.be/cPrBwf5-yEI)

