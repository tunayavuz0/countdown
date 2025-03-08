using System;

namespace proje
{
    class Program
    {
        static void Main(string[] args)
        {

            int long_wall, medium_wall, short_wall, wall_count, wall_type, a, b, length;
            int[] zero_array1 = new int[70];//For the coordinates of zero
            int[] zero_array2 = new int[70];
            int life = 5;
            int score = 0;
            int cursorx, cursory; // position of cursor
            int aDirX = -1; int aDirY = 1; // direction of 0:   1:right   -1:left

            // outer walls
            int rows = 23;
            int columns = 53;
            char[,] walls = new char[rows + 10, columns + 10]; // for creating the board

            // numbers of inner walls
            long_wall = 0;
            medium_wall = 0;
            short_wall = 0;
            wall_count = long_wall + medium_wall + short_wall;

            Console.CursorVisible = false; // hide the cursor
            DateTime startTime = DateTime.Now; // take the starting time
            DateTime lastMoveTime = DateTime.Now; // for 0 to move 1 unit in 1 second
            ConsoleKeyInfo cki; // required for readkey                        

            Random random = new Random();

            Console.SetCursorPosition(0, 24);
            Console.Write("Player controls: WASD and ARROW keys.\nPress ESC to quit.");
            Console.SetCursorPosition(0, 0);

            // creating outer walls
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    walls[i, j] = ' ';
                    if (i == 0 || i == rows - 1 || j == 0 || j == columns - 1) walls[i, j] = '#';
                }
            }

            // creating inner walls
            while (wall_count < 28) // The loop continues until the number of walls reaches 28.
            {
                bool canPlaceWall;
                length = 0;
                do
                {
                    wall_type = random.Next(1, 7);
                    a = random.Next(2, 21);
                    b = random.Next(2, 51);

                    if (wall_type == 1 || wall_type == 4) length = 3;
                    else if (wall_type == 2 || wall_type == 5) length = 7;
                    else if (wall_type == 3 || wall_type == 6) length = 11;

                    canPlaceWall = CanPlaceWall(walls, a, b, length, wall_type); // Function will return true or false and it will indicate the bool value                   

                } while (!canPlaceWall); // it will continue until the bool value returns false           

                if (wall_type == 1 && short_wall != 20) // short vertical
                {
                    for (int k = a; a < k + 3; a++)
                        walls[a, b] = '#';
                    short_wall++;
                }
                else if (wall_type == 2 && medium_wall != 5) // medium vertical
                {
                    for (int k = a; a < k + 7; a++)
                        walls[a, b] = '#';
                    medium_wall++;
                }
                else if (wall_type == 3 && long_wall != 3) // long vertical
                {
                    for (int k = a; a < k + 11; a++)
                        walls[a, b] = '#';
                    long_wall++;
                }
                else if (wall_type == 4 && short_wall != 20) // short horizontal
                {
                    for (int k = b; b < k + 3; b++)
                        walls[a, b] = '#';
                    short_wall++;
                }
                else if (wall_type == 5 && medium_wall != 5) // medium horizontal
                {
                    for (int k = b; b < k + 7; b++)
                        walls[a, b] = '#';
                    medium_wall++;
                }
                else if (wall_type == 6 && long_wall != 3) // long horizontal
                {
                    for (int k = b; b < k + 11; b++)
                        walls[a, b] = '#';
                    long_wall++;
                }

                wall_count = long_wall + medium_wall + short_wall;
            }

            // adding random numbers in the board
            for (int i = 0; i < 70; i++)
            {
                int x = random.Next(1, 23);
                int y = random.Next(1, 53);
                char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                if (walls[x, y] == ' ')
                {
                    int index = random.Next(numbers.Length);
                    char randomNum = numbers[index];
                    walls[x, y] = randomNum;
                }
                else
                {
                    bool flag = true;
                    while (flag == true)
                    {
                        x = random.Next(1, 22);
                        y = random.Next(1, 52);

                        int index = random.Next(numbers.Length);
                        char randomNum = numbers[index];
                        if (walls[x, y] == ' ')
                        {
                            walls[x, y] = randomNum;
                            flag = false;
                            continue;
                        }
                        else
                            flag = true;
                    }
                }
            }

            // printing the board
            int zerocount = 0;
            int count = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (walls[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(walls[i, j]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(walls[i, j]);
                    }
                    if (walls[i, j] == '0')
                    {
                        zero_array1[count] = j;
                        zero_array2[count] = i;
                        count++;
                        zerocount++;
                    }
                }
                Console.WriteLine();
            }

            // assigning the P to a random position
            do
            {
                cursorx = random.Next(2, 51);
                cursory = random.Next(2, 21);
            } while (walls[cursory, cursorx] != ' ');


            int counter1 = 0;
            bool numberDecrease = true; // for controlling the loop
            bool isThereNumber = false; // To check for consecutive numbers in the following locations

            // gameloop
            while (true)
            {
                // calculate the time
                Console.ForegroundColor = ConsoleColor.White;
                TimeSpan elapsedTime = DateTime.Now - startTime;
                Console.SetCursorPosition(62, 3);
                Console.WriteLine($"Time  : {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}");
                Console.SetCursorPosition(62, 4);
                Console.WriteLine("Life  : " + life);
                Console.SetCursorPosition(62, 5);
                Console.WriteLine("Score : " + score);

                // decreasing numbers every 15 seconds                
                if (elapsedTime.Seconds % 15 == 1)
                    numberDecrease = true;

                else if (numberDecrease == true)
                {
                    if (elapsedTime.Seconds % 15 == 0 && elapsedTime.Seconds != 0)
                    {
                        numberDecrease = false;
                        // to enter the while loop once and decrease the numbers one time in one second                                                                                                                        
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                // checking numbers except zero
                                if (walls[i, j] >= '1' && walls[i, j] <= '9')
                                {
                                    int currentValue = int.Parse(walls[i, j].ToString());

                                    if (currentValue > 1) walls[i, j] = (currentValue - 1).ToString()[0]; // decrease if the number is greater than one

                                    if (currentValue == 1) // if the number is one, it will be converted to zero with 3% probability
                                    {
                                        int oneOrZero = 3;
                                        walls[i, j] = random.Next(0, 100) >= oneOrZero ? '1' : '0'; // if the random number is greater than 3 it will remain 1, if it's not it will turn to 0

                                        if (walls[i, j] == '0')
                                        {
                                            zero_array1[count] = j;
                                            zero_array2[count] = i;
                                            count++;
                                            zerocount++;
                                        }
                                    }
                                }
                            }
                        }

                        // printing the board
                        Console.SetCursorPosition(0, 0);
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                if (walls[i, j] == '#')
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                                    Console.Write(walls[i, j]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write(walls[i, j]);
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }

                int smash_count = 0;

                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    while (Console.KeyAvailable) cki = Console.ReadKey(true);

                    if ((cki.Key == ConsoleKey.RightArrow || cki.Key == ConsoleKey.D) && cursorx < 51 && walls[cursory, cursorx + 1] != '#') //P can move to the right if the place is empty
                    {
                        char[] shift1 = new char[51];
                        if (walls[cursory, cursorx + 1] != ' ') isThereNumber = true;
                        else isThereNumber = false;

                        //pushing and smashing
                        while (isThereNumber)
                        {
                            int tempCursorX = cursorx;
                            int i = 0;
                            while (walls[cursory, tempCursorX + 1] != ' ' && walls[cursory, tempCursorX + 1] != '#')
                            {
                                shift1[i] = walls[cursory, tempCursorX + 1]; //If we encounter a number, we are placing these numbers sequentially into an array
                                i++;
                                tempCursorX++;
                            }
                            tempCursorX = cursorx;

                            char max = shift1[0];
                            for (int j = 1; j < i; j++)
                            {
                                if (shift1[j] > max) isThereNumber = false; //If the numbers are not in descending order or not equal, it cannot perform a push
                                else max = shift1[j];
                            }
                            if (walls[cursory, cursorx + 2] == '#') isThereNumber = false; // If there is a remaining  1 number, to prevent smashing
                            if (isThereNumber == false) break;

                            for (int j = 0; j < i; j++) // pushing and smashing the numbers
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.SetCursorPosition(tempCursorX + 2, cursory);

                                if (walls[cursory, tempCursorX + 3] == '#') // scoring
                                {
                                    if (walls[cursory, tempCursorX + 2] == '1' || walls[cursory, tempCursorX + 2] == '2' || walls[cursory, tempCursorX + 2] == '3' || walls[cursory, tempCursorX + 2] == '4') score += 2;
                                    if (walls[cursory, tempCursorX + 2] == '5' || walls[cursory, tempCursorX + 2] == '6' || walls[cursory, tempCursorX + 2] == '7' || walls[cursory, tempCursorX + 2] == '8' || walls[cursory, tempCursorX + 2] == '9') score += 1;
                                    if (walls[cursory, tempCursorX + 2] == '0') score += 20;
                                    if (walls[cursory, tempCursorX + 2] != ' ' && walls[cursory, tempCursorX + 2] != '#')
                                    {
                                        System.Media.SoundPlayer scoreGain = new System.Media.SoundPlayer(@"c:\score_gain.wav");
                                        scoreGain.Play();
                                        smash_count++;
                                    }
                                }
                                if (walls[cursory, tempCursorX + 2] != '#')
                                {
                                    walls[cursory, tempCursorX + 2] = shift1[j]; // Shift the numbers forward by 1 unit
                                    Console.Write(shift1[j]);// writing the numbers
                                }
                                Console.SetCursorPosition(tempCursorX + 1, cursory);
                                walls[cursory, cursorx + 1] = ' '; // Assign a blank space to the previous position of the number
                                tempCursorX++;

                                if (walls[cursory, tempCursorX] == ' ' || walls[cursory, tempCursorX] == '#') isThereNumber = false; // If there are no more numbers, it returns false.                                                              
                            }
                            findzero(walls, ref zero_array1, ref zero_array2, ref zerocount, ref count);// update the position of zero after doing push or smash 
                        }

                        if (walls[cursory, cursorx + 1] == ' ')
                        {
                            Console.SetCursorPosition(cursorx, cursory); // delete P (old position)
                            Console.WriteLine(" ");
                            cursorx++;
                        }
                    }

                    if ((cki.Key == ConsoleKey.LeftArrow || cki.Key == ConsoleKey.A) && cursorx > 1 && walls[cursory, cursorx - 1] != '#') //P can move to the left if the place is empty
                    {
                        char[] shift2 = new char[51];
                        if (walls[cursory, cursorx - 1] != ' ') isThereNumber = true;
                        else isThereNumber = false;

                        //pushing and smashing
                        while (isThereNumber)
                        {
                            int tempCursorX = cursorx;
                            int i = 0;
                            while (walls[cursory, tempCursorX - 1] != ' ' && walls[cursory, tempCursorX - 1] != '#')
                            {
                                shift2[i] = walls[cursory, tempCursorX - 1]; //If we encounter a number, we are placing these numbers sequentially into an array.
                                i++;
                                tempCursorX--;
                            }
                            tempCursorX = cursorx;

                            char max = shift2[0];
                            for (int j = 1; j < i; j++)
                            {
                                if (shift2[j] > max) isThereNumber = false; //If the numbers are not in descending order or not equal, it cannot perform a push.
                                else max = shift2[j];
                            }
                            if (walls[cursory, cursorx - 2] == '#') isThereNumber = false; // If there is a remaining number, to prevent smashing
                            if (isThereNumber == false) break;
                            ;
                            for (int j = 0; j < i; j++) // pushing and smashing the numbers
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.SetCursorPosition(tempCursorX - 2, cursory);

                                if (walls[cursory, Math.Abs(tempCursorX - 3)] == '#') // scoring   // We provided n to prevent exceeding the array limit
                                {
                                    if (walls[cursory, tempCursorX - 2] == '1' || walls[cursory, tempCursorX - 2] == '2' || walls[cursory, tempCursorX - 2] == '3' || walls[cursory, tempCursorX - 2] == '4') score += 2;
                                    if (walls[cursory, tempCursorX - 2] == '5' || walls[cursory, tempCursorX - 2] == '6' || walls[cursory, tempCursorX - 2] == '7' || walls[cursory, tempCursorX - 2] == '8' || walls[cursory, tempCursorX - 2] == '9') score += 1;
                                    if (walls[cursory, tempCursorX - 2] == '0') score += 20;
                                    if (walls[cursory, tempCursorX - 2] != ' ' && walls[cursory, tempCursorX - 2] != '#')
                                    {
                                        System.Media.SoundPlayer scoreGain = new System.Media.SoundPlayer(@"c:\score_gain.wav");
                                        scoreGain.Play();
                                        smash_count++;
                                    }
                                }
                                if (walls[cursory, tempCursorX - 2] != '#')
                                {
                                    walls[cursory, tempCursorX - 2] = shift2[j]; // Move the numbers back by 1 unit
                                    Console.Write(shift2[j]); // writing numbers
                                }

                                Console.SetCursorPosition(tempCursorX - 1, cursory);
                                walls[cursory, cursorx - 1] = ' '; // Assign a blank space to the previous position of the number
                                tempCursorX--;
                                // n--;

                                if (walls[cursory, tempCursorX] == ' ' || walls[cursory, tempCursorX] == '#') isThereNumber = false; // If there are no more numbers, it returns false                                                               
                            }
                            findzero(walls, ref zero_array1, ref zero_array2, ref zerocount, ref count);// update the position of zero after doing push or smash 
                        }

                        if (walls[cursory, cursorx - 1] == ' ')
                        {
                            Console.SetCursorPosition(cursorx, cursory); // delete P (old position)
                            Console.WriteLine(" ");
                            cursorx--;
                        }

                    }
                    if ((cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.W) && cursory > 1 && walls[cursory - 1, cursorx] != '#')// P can move upwards if the place is empty
                    {
                        char[] shift3 = new char[21];//Vertical axis at 21
                        if (walls[cursory - 1, cursorx] != ' ') isThereNumber = true;
                        else isThereNumber = false;

                        //pushing and smashing

                        while (isThereNumber)
                        {
                            int tempCursorY = cursory;
                            int i = 0;
                            while (walls[tempCursorY - 1, cursorx] != ' ' && walls[tempCursorY - 1, cursorx] != '#')
                            {
                                shift3[i] = walls[tempCursorY - 1, cursorx]; //If we encounter a number, we are placing these numbers sequentially into an array
                                i++;
                                tempCursorY--;
                            }
                            tempCursorY = cursory;

                            char max = shift3[0];
                            for (int j = 1; j < i; j++)
                            {
                                if (shift3[j] > max) isThereNumber = false; //If the numbers are not in descending order or not equal, it cannot perform a push.
                                else max = shift3[j];
                            }
                            if (walls[cursory - 2, cursorx] == '#') isThereNumber = false; // If there is a remaining number, to prevent smashing
                            if (isThereNumber == false) break;

                            for (int j = 0; j < i; j++) // pushing and smashing the numbers
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.SetCursorPosition(cursorx, tempCursorY - 2);

                                if (walls[Math.Abs(tempCursorY - 3), cursorx] == '#') // scoring // We provided n to prevent exceeding the array limit
                                {
                                    if (walls[tempCursorY - 2, cursorx] == '1' || walls[tempCursorY - 2, cursorx] == '2' || walls[tempCursorY - 2, cursorx] == '3' || walls[tempCursorY - 2, cursorx] == '4') score += 2;
                                    if (walls[tempCursorY - 2, cursorx] == '5' || walls[tempCursorY - 2, cursorx] == '6' || walls[tempCursorY - 2, cursorx] == '7' || walls[tempCursorY - 2, cursorx] == '8' || walls[tempCursorY - 2, cursorx] == '9') score += 1;
                                    if (walls[tempCursorY - 2, cursorx] == '0') score += 20;
                                    if (walls[tempCursorY - 2, cursorx] != ' ' && walls[tempCursorY - 2, cursorx] != '#')
                                    {
                                        System.Media.SoundPlayer scoreGain = new System.Media.SoundPlayer(@"c:\score_gain.wav");
                                        scoreGain.Play();
                                        smash_count++;
                                    }
                                }
                                if (walls[tempCursorY - 2, cursorx] != '#')
                                {
                                    walls[tempCursorY - 2, cursorx] = shift3[j]; // Advance the numbers by 1 unit
                                    Console.Write(shift3[j]); // writing numbers
                                }
                                Console.SetCursorPosition(cursorx, tempCursorY - 1);
                                walls[cursory - 1, cursorx] = ' '; // Assign a blank space to the previous position of the number
                                tempCursorY--;


                                if (walls[tempCursorY, cursorx] == ' ' || walls[tempCursorY, cursorx] == '#') isThereNumber = false; // If there are no more numbers, it returns false                                                               
                            }
                            findzero(walls, ref zero_array1, ref zero_array2, ref zerocount, ref count);// update the position of zero after doing push or smash 
                        }

                        if (walls[cursory - 1, cursorx] == ' ')
                        {
                            Console.SetCursorPosition(cursorx, cursory); // delete P (old position)
                            Console.WriteLine(" ");
                            cursory--;
                        }
                    }
                    if ((cki.Key == ConsoleKey.DownArrow || cki.Key == ConsoleKey.S) && cursory < 21 && walls[cursory + 1, cursorx] != '#') // P can move downwards if the place is empty
                    {
                        char[] shift4 = new char[21];// Vertical axis of 21
                        if (walls[cursory + 1, cursorx] != ' ') isThereNumber = true;
                        else isThereNumber = false;

                        //pushing and smashing
                        while (isThereNumber)
                        {
                            int tempCursorY = cursory;
                            int i = 0;
                            while (walls[tempCursorY + 1, cursorx] != ' ' && walls[tempCursorY + 1, cursorx] != '#')
                            {
                                shift4[i] = walls[tempCursorY + 1, cursorx]; //If we encounter a number, we are placing these numbers sequentially into an array
                                i++;
                                tempCursorY++;
                            }
                            tempCursorY = cursory;

                            char max = shift4[0];
                            for (int j = 1; j < i; j++)
                            {
                                if (shift4[j] > max) isThereNumber = false; //If the numbers are not in descending order or not equal, it cannot perform a push
                                else max = shift4[j];
                            }
                            if (walls[cursory + 2, cursorx] == '#') isThereNumber = false; // If there is a remaining number, to prevent smashing
                            if (isThereNumber == false) break;

                            for (int j = 0; j < i; j++) // pushing and smashing the numbers
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.SetCursorPosition(cursorx, tempCursorY + 2);

                                if (walls[tempCursorY + 3, cursorx] == '#') // scoring
                                {
                                    if (walls[tempCursorY + 2, cursorx] == '1' || walls[tempCursorY + 2, cursorx] == '2' || walls[tempCursorY + 2, cursorx] == '3' || walls[tempCursorY + 2, cursorx] == '4') score += 2;
                                    if (walls[tempCursorY + 2, cursorx] == '5' || walls[tempCursorY + 2, cursorx] == '6' || walls[tempCursorY + 2, cursorx] == '7' || walls[tempCursorY + 2, cursorx] == '8' || walls[tempCursorY + 2, cursorx] == '9') score += 1;
                                    if (walls[tempCursorY + 2, cursorx] == '0') score += 20;
                                    if (walls[tempCursorY + 2, cursorx] != ' ' && walls[tempCursorY + 2, cursorx] != '#')
                                    {
                                        System.Media.SoundPlayer scoreGain = new System.Media.SoundPlayer(@"c:\score_gain.wav");
                                        scoreGain.Play();
                                        smash_count++;
                                    }
                                }
                                if (walls[tempCursorY + 2, cursorx] != '#')
                                {
                                    walls[tempCursorY + 2, cursorx] = shift4[j]; // Advance the numbers by 1 unit                          
                                    Console.Write(shift4[j]); // writing numbers
                                }
                                Console.SetCursorPosition(cursorx, tempCursorY + 1);
                                walls[cursory + 1, cursorx] = ' '; // Assign a blank space to the previous position of the number
                                tempCursorY++;

                                if (walls[tempCursorY, cursorx] == ' ' || walls[tempCursorY, cursorx] == '#') isThereNumber = false; // If there are no more numbers, it returns false                                                               
                            }
                            findzero(walls, ref zero_array1, ref zero_array2, ref zerocount, ref count);// update the position of zero after doing push or smash 
                        }

                        if (walls[cursory + 1, cursorx] == ' ')
                        {
                            Console.SetCursorPosition(cursorx, cursory); // delete P (old position)
                            Console.WriteLine(" ");
                            cursory++;
                        }
                    }
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        System.Media.SoundPlayer gameOver = new System.Media.SoundPlayer(@"c:\game_over.wav");
                        gameOver.Play();
                        Console.SetCursorPosition(62, 10);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("GAME ENDED");
                        break;
                    }
                }

                Console.SetCursorPosition(cursorx, cursory); // refresh p (current position)
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("P");

                // regeneration of smashed numbers
                for (int i = 0; i < smash_count; i++)
                {
                    int x = random.Next(1, 23);
                    int y = random.Next(1, 53);
                    char[] numbers = { '5', '6', '7', '8', '9' };
                    if (walls[x, y] == ' ')
                    {
                        int index = random.Next(numbers.Length);
                        char randomNum = numbers[index];
                        walls[x, y] = randomNum;
                    }
                    else
                    {
                        bool flag = true;
                        while (flag == true)
                        {
                            x = random.Next(1, 23);
                            y = random.Next(1, 53);

                            int index = random.Next(numbers.Length);
                            char randomNum = numbers[index];
                            if (walls[x, y] == ' ')
                            {
                                walls[x, y] = randomNum;
                                flag = false;
                                continue;
                            }
                            else
                                flag = true;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.SetCursorPosition(y, x);
                    Console.WriteLine(walls[x, y]);
                }


                // ZERO MOVEMENT
                // moves of '0's every second
                TimeSpan timeSinceLastMove = DateTime.Now - lastMoveTime;
                if (timeSinceLastMove.TotalMilliseconds >= 1000)
                {
                    for (int i = 0; i < zerocount; i++)
                    {
                        if (aDirX == 1 && zero_array1[counter1] >= 51) aDirX = -1; // change direction at boundaries for x-axis
                        if (aDirX == -1 && zero_array1[counter1] <= 1) aDirX = 1;

                        if (aDirY == 1 && zero_array2[counter1] >= 21) aDirY = -1; // change direction at boundaries for y-axis
                        if (aDirY == -1 && zero_array2[counter1] <= 1) aDirY = 1;


                        // clear the current position of zero
                        walls[zero_array2[counter1], zero_array1[counter1]] = ' ';
                        Console.SetCursorPosition(zero_array1[counter1], zero_array2[counter1]);
                        Console.Write(" ");
                        while (true)// so that zero does not remain motionless
                        {
                            int newAx1 = zero_array1[counter1] + 1;
                            int newAy1 = zero_array2[counter1];

                            int newAx2 = zero_array1[counter1] - 1;
                            int newAy2 = zero_array2[counter1];

                            int newAx3 = zero_array1[counter1];
                            int newAy3 = zero_array2[counter1] + 1;

                            int newAx4 = zero_array1[counter1];
                            int newAy4 = zero_array2[counter1] - 1;
                            //so that the game does not stop when there is no place for it to go around zero
                            if (walls[newAy1, newAx1] != ' ' && walls[newAy2, newAx2] != ' ' && walls[newAy3, newAx3] != ' ' && walls[newAy4, newAx4] != ' ')
                                break;


                            aDirX = random.Next(-1, 2);
                            if (aDirX == -1) aDirY = 0;
                            if (aDirX == 1) aDirY = 0;
                            if (aDirX == 0)
                            {
                                int move = random.Next(1, 3);
                                if (move == 1) aDirY = -1;
                                if (move == 2) aDirY = 1;
                            }
                            // calculate the new position
                            int newAx = zero_array1[counter1] + aDirX;
                            int newAy = zero_array2[counter1] + aDirY;

                            if (walls[newAy, newAx] == ' ')
                            {
                                zero_array1[counter1] = newAx;
                                zero_array2[counter1] = newAy;
                                break;
                            }
                        }
                        walls[zero_array2[counter1], zero_array1[counter1]] = '0';
                        Console.SetCursorPosition(zero_array1[counter1], zero_array2[counter1]);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("0");
                        counter1++;
                    }
                    // the new position of each '0' character is determined, the lastMoveTime variable is set to the current time, keeping track of when the next move occurs.
                    lastMoveTime = DateTime.Now;
                }
                if (counter1 >= zerocount) counter1 = 0;

                // check if the P and 0 touching each other
                for (int i = 0; i < zerocount; i++)
                {
                    if (cursorx == zero_array1[i] && cursory == zero_array2[i])
                    {
                        // if they touch decrease life by one and assign P to a random position
                        System.Media.SoundPlayer lifeLost = new System.Media.SoundPlayer(@"c:\life_lost.wav");

                        lifeLost.Play();
                        life--;
                        Console.SetCursorPosition(cursorx, cursory);
                        Console.WriteLine(" ");

                        do // assigning the P to a random position
                        {
                            cursorx = random.Next(2, 51);
                            cursory = random.Next(2, 21);
                        } while (walls[cursory, cursorx] != ' ');

                        Console.SetCursorPosition(cursorx, cursory);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("P");
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(62, 3);
                Console.WriteLine($"Time  : {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}");
                Console.SetCursorPosition(62, 4);
                Console.WriteLine("Life  : " + life);
                Console.SetCursorPosition(62, 5);
                Console.WriteLine("Score : " + score);

                if (life == 0)
                {
                    System.Media.SoundPlayer gameOver = new System.Media.SoundPlayer(@"c:\game_over.wav");
                    gameOver.Play();
                    Console.SetCursorPosition(0, 24);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ \r\n██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗\r\n██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝\r\n██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗\r\n╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║\r\n ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝");
                    break; // end game
                }
            }
            Console.ReadLine();
        }
        static bool CanPlaceWall(char[,] walls, int a, int b, int length, int type)
        {
            if (type == 1 || type == 2 || type == 3) // if the inner wall is vertical
            {
                if (a + length < 21) // check if the wall exceeds the borders
                {
                    for (int i = -1; i <= length; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (walls[a + i, b + j] == '#') return false; // it will return false if the place isn't empty
                        }
                    }
                }
                else return false; // if the wall exceeds the borders it will return false
            }

            else if (type == 4 || type == 5 || type == 6) // if the inner wall is horizontal
            {
                if (b + length < 51) // check if the wall exceeds the borders
                {
                    for (int i = -1; i <= length; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (walls[a + j, b + i] == '#') return false; // it will return false if the place isn't empty
                        }
                    }
                }
                else return false; // if the wall exceeds the borders it will return false                                             
            }
            return true; // if the place is empty it will return true
        }
        static void findzero(char[,] walls, ref int[] zero_array1, ref int[] zero_array2, ref int zerocount, ref int count)
        {
            int rows = 23;
            int columns = 53;
            // printing the board         
            count = 0;
            zerocount = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (walls[i, j] == '0')
                    {
                        zero_array1[count] = j;
                        zero_array2[count] = i;
                        count++;
                        zerocount++;
                    }
                }
            }
        }
    }
}


