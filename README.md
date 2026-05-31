Project:  Countdown 

The aim of the project is to develop a maze game, 
in which treasures/numbers are collected while walking in it.


General Information

The game is played in a 23*53 game field including outer walls. Game characters are P and numbers. Human player is represented by P. Computer controls the numbers. The aim of the game is reaching the highest score.

Game Characters

P: Human player
   
•	Cursor keys: To move the human player (4 directions)

•	Human player can move in empty spaces, push number(s) or smash a number. 

•	At the beginning, human player has 5 lives.  


Numbers:  9, 8, 7, 6, 5, 4, 3, 2, 1, 0

•	These can be collected by the human player by smashing them. 
o	            0  : 20 points.
o	   1, 2, 3, 4  :  2 points.
o	5, 6, 7, 8, 9  :  1 point.

•	Only number 0 is alive and moves randomly in four directions. Other numbers are static.

Game Initialization

•	Walls
o	The inner walls in the game area are generated in random places.
o	There are 3 types of inner walls. The number of walls in the game area;
	 3 * Long wall (length:11). It can be horizontal or vertical.
	 5 * Medium wall (length:7). It can be horizontal or vertical.
	20 * Short wall (length:3). It can be horizontal or vertical.
o	Wall placement is random, but there must be at least 1 square (for 8 directions) among them. Walls cannot touch other walls.

•	Human player P is located randomly.

•	70 numbers (0, 1, 2, 3, 4, 5, 6, 7, 8 or 9 with equal probability) are placed at random positions.
