using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Crawler
{
    /**
     * The main class of the Dungeon Crawler Application
     * 
     * You may add to your project other classes which are referenced.
     * Complete the templated methods and fill in your code where it says "Your code here".
     * Do not rename methods or variables which already exist or change the method parameters.
     * You can do some checks if your project still aligns with the spec by running the tests in UnitTest1
     * 
     * For Questions do contact us!
     */
    public class CMDCrawler
    {
        /**
         * use the following to store and control the next movement of the user
         */
        public enum PlayerActions { NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT, REPLAY };
        private PlayerActions action = PlayerActions.NOTHING;

        /**
         * tracks if the game is running
         */
        private bool active = true;

        /// <summary>
        /// Use this object member to store the loaded map.
        /// </summary>
        private char[][] originalMap = new char[0][];

        /// <summary>
        /// This stores the map that is updated when the user makes a move
        /// </summary>
        private char[][] dynamicMap = new char[0][];

        /// <summary>
        /// True if the user started the game by typing "Play"
        /// </summary>
        private bool gameStarted = false;

        /// <summary>
        /// other object variables used within the code
        /// </summary>
        private bool advanced = false; //game is in advanced mode
        private int coins = 0; //The number of coins accumulated by user
        public int monsterMovementCount = 0; //used to know when the monster moves
        public int playerDamagePoint = 3; //player damage point
        public bool onCoin = false; //spacifies if a player is standing on a coin

        /// <summary>
        /// Stores monster details
        /// killedMonster stores if any monster has been killed
        /// monster1Coins stores number of coins accumulated by monster1
        /// monster1DamagePoint stores the strength of monster1
        /// deadMonster1CoinX stores the x position of coin dropped by monster1 when killed
        /// deadMonster1CoinY stores the y position of coin dropped by monster1 when killed
        /// </summary>
        public Dictionary<string, int> monsterDetails = new Dictionary<string, int>() { {"killedMonster", 0},
            {"monster1Coins",0},{"monster2Coins",0},{"monster1DamagePoint",3},{"monster2DamagePoint",3},
            {"deadMonster1CoinX",0},{"deadMonster1CoinY",0},{"deadMonster2CoinX",0},{"deadMonster2CoinY",0} };



        /**
         * Reads user input from the Console
         * 
         * Please use and implement this method to read the user input.
         * 
         * Return the input as string to be further processed
         * 
         */
        private string ReadUserInput()
        {
            string inputRead;

            // Your code here
            if (advanced == true)
            {
                ConsoleKeyInfo input = Console.ReadKey();
                inputRead = input.KeyChar.ToString();
            }
            else
            {
                inputRead = Console.ReadLine();
            }
            return inputRead;
        }

        /**
         * Processed the user input string
         * 
         * takes apart the user input and does control the information flow
         *  * initializes the map ( you must call InitializeMap)
         *  * starts the game when user types in Play
         *  * sets the correct playeraction which you will use in the Update
         *  
         *  DO NOT read any information from command line in here but only act upon what the method receives.
         */
        public void ProcessUserInput(string input)
        {

            if (input.Trim().Equals("load Simple.map")) // Loading Simple map
            {
                if (this.gameStarted == false)
                {
                    InitializeMap("Simple.map");
                }
                else
                    Console.WriteLine("Game already started, kindly quit the game before loading another map");
            }

            else if (input.Trim().Equals("load Advanced.map")) // Loading Advanced map
            {
                if (this.gameStarted == false)
                {
                    InitializeMap("Advanced.map");
                }
                else
                    Console.WriteLine("Game already started, kindly quit the game before loading another map");
            }

            else if (input.Trim().Equals("load Simple2.map")) // Loading Simple2 map
            {
                if (this.gameStarted == false)
                {
                    InitializeMap("Simple2.map");
                }
                else
                    Console.WriteLine("Game already started, kindly quit the game before loading another map");
            }

            else if (input.Trim().Equals("play") || input.Trim().Equals("Play") || input.Trim().Equals("PLAY")) //Starting the game by typing play
            {
                if (dynamicMap.Length != 0)
                {
                    this.gameStarted = true;
                    this.active = true;
                    this.action = PlayerActions.NOTHING;
                    this.Update(this.active);
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("advanced play") || input.Trim().Equals("Advanced Play") || input.Trim().Equals("Advanced play") || input.Trim().Equals(" ADVANCED PLAY")) //Starting the game by typing advanced play
            {
                if (dynamicMap.Length != 0)
                {
                    this.advanced = true;
                    this.gameStarted = true;
                    this.active = true;
                    this.action = PlayerActions.NOTHING;
                    this.AdvancedUpdate(this.active);
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("a") || input.Trim().Equals("A")) //Moving west
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.WEST;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("s") || input.Trim().Equals("S")) //Moving east
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.SOUTH;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("w") || input.Trim().Equals("W")) //Moving north
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.NORTH;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("d") || input.Trim().Equals("D")) //Moving south
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.EAST;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Equals(" ")) //player attack
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.ATTACK;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("p") || input.Trim().Equals("P")) //Pickup gold
            {
                if (dynamicMap.Length != 0 && this.gameStarted == true)
                {
                    this.action = PlayerActions.PICKUP;
                }
                else
                    Console.WriteLine("Kindly load map first before starting to play");
            }

            else if (input.Trim().Equals("quit") || input.Trim().Equals("QUIT")) //Quitting the game
            {
                Console.WriteLine("Game has been quitted...");
                this.action = PlayerActions.QUIT;
            }

            else //Invalid input
            {
                Console.WriteLine("Invalid Input..!");
                this.action = PlayerActions.NOTHING;
            }
        }

        /// <summary>
        /// Updates the dynamic map according to the player move
        /// </summary>
        /// <param name="x"> player move X axis</param>
        /// <param name="y"> player move Y axis</param>
        /// <param name="initialX"> initial X</param>
        /// <param name="initiualY"> initial X</param>
        public void UpdateDynamicMap(int x, int y, int initialX, int initiualY)
        {
            //user moves in a valid path
            if (dynamicMap[x][y].Equals('-') || dynamicMap[x][y].Equals('C') || dynamicMap[x][y].Equals('X'))
            {
                if (this.onCoin == true)//if player was standing on coin initially
                {
                    this.dynamicMap[initialX][initiualY] = 'C';
                    this.onCoin = false;
                }
                else
                    this.dynamicMap[initialX][initiualY] = '-';

                if (dynamicMap[x][y].Equals('C')) //Player steps on coin
                {
                    if (advanced == true)
                        this.onCoin = true;
                    else
                    {
                        this.coins += 1;
                        this.playerDamagePoint += 1;
                    }

                    this.dynamicMap[x][y] = '@';
                }
                else if (dynamicMap[x][y].Equals('X')) //Player completes the game
                {
                    if (advanced)
                    {
                        this.dynamicMap[x][y] = '@';
                        this.dynamicMap[initialX][initiualY] = '-';
                        Console.WriteLine("SUCCESS!!!... Congrats! You successfully completed the game" + Environment.NewLine);
                        Console.WriteLine("COINS ACCUMULATED: " + this.coins + Environment.NewLine);
                        PrintMap();
                        Console.WriteLine("Enter \"Replay\" to play the map again, or \"Quit\" to quit the game " + this.coins + Environment.NewLine);
                        String input = Console.ReadLine();
                        if (input.Trim().Equals("Replay") || input.Trim().Equals("replay")) //replay
                        {
                            Reset(x, y);
                        }
                        else if (input.Trim().Equals("Quit") || input.Trim().Equals("quit")) //quit
                        {
                            this.active = false;
                        }
                    }
                    else
                    {
                        this.dynamicMap[x][y] = '@';
                        this.dynamicMap[initialX][initiualY] = '-';
                        Console.WriteLine("SUCCESS!!!... Congrats! You successfully completed the game" + Environment.NewLine);
                        Console.WriteLine("COINS ACCUMULATED: " + this.coins + Environment.NewLine);
                        this.active = false;

                    }
                }

                else
                    this.dynamicMap[x][y] = '@';

            }
            else //player hits wall
            {
                if (dynamicMap[x][y].Equals('#'))
                    Console.WriteLine("You hit the wall..." + Environment.NewLine);
                else
                    Console.WriteLine("MONSTER XXXX..." + Environment.NewLine);
            }

        }

        /// <summary>
        /// Resets the game
        /// Reloads the playing map and resets the necessary object variables
        /// </summary>
        /// <param name="positionX">x position of player</param>
        /// <param name="positionY">y position of player</param>
        public void Reset(int positionX, int positionY)
        {
            for (int x = 0; x < originalMap.Length; x++)
            {
                for (int y = 0; y < originalMap[x].Length; y++)
                {
                    this.dynamicMap[x][y] = originalMap[x][y];
                    if (x == positionX && y == positionY)
                        dynamicMap[x][y] = 'X';
                    if (originalMap[x][y].Equals('S'))
                        dynamicMap[x][y] = '@';
                }
            }
            this.coins = 0;
            this.monsterMovementCount = 0;
            this.playerDamagePoint = 3;
            this.onCoin = false;
            this.monsterDetails = new Dictionary<string, int>() { {"killedMonster", 0},
            {"monster1Coins",0},{"monster2Coins",0},{"monster1DamagePoint",3},{"monster2DamagePoint",3},
            {"deadMonster1CoinX",0},{"deadMonster1CoinY",0},{"deadMonster2CoinX",0},{"deadMonster2CoinY",0} };
        }

        /**
         * The Main Game Loop. 
         * It updates the game state.
         * 
         * This is the method where you implement your game logic and alter the state of the map/game
         * use playeraction to determine how the character should move/act
         * the input should tell the loop if the game is active and the state should advance
         * 
         * Returns true if the game could be updated and is ongoing
         */
        public bool Update(bool active)
        {
            bool working = false;

            // Your code here
            if (advanced == true)
                AdvancedUpdate(active);

            else
            {
                if (this.active == true && this.gameStarted == true)
                {
                    if (this.action == PlayerActions.NOTHING)
                    {
                        for (int x = 0; x < dynamicMap.Length; x++)
                        {
                            for (int y = 0; y < dynamicMap[x].Length; y++)
                            {
                                if (dynamicMap[x][y].Equals('S'))
                                {
                                    dynamicMap[x][y] = '@';
                                    break;
                                }
                            }
                        }
                        working = true;
                    }

                    else if (this.action == PlayerActions.WEST)
                    {
                        UpdateDynamicMap(GetPlayerPosition()[0], GetPlayerPosition()[1] - 1, GetPlayerPosition()[0], GetPlayerPosition()[1]);
                    }

                    else if (this.action == PlayerActions.EAST)
                    {
                        UpdateDynamicMap(GetPlayerPosition()[0], GetPlayerPosition()[1] + 1, GetPlayerPosition()[0], GetPlayerPosition()[1]);
                    }

                    else if (this.action == PlayerActions.NORTH)
                    {
                        UpdateDynamicMap(GetPlayerPosition()[0] - 1, GetPlayerPosition()[1], GetPlayerPosition()[0], GetPlayerPosition()[1]);
                    }

                    else if (this.action == PlayerActions.SOUTH)
                    {
                        UpdateDynamicMap(GetPlayerPosition()[0] + 1, GetPlayerPosition()[1], GetPlayerPosition()[0], GetPlayerPosition()[1]);
                    }
                }
            }

            return working;
        }

        /// <summary>
        /// Updates map in Advaced mode
        /// </summary>
        /// <param name="active"> Return true if map is updated</param>
        /// <returns></returns>
        public bool AdvancedUpdate(bool active)
        {
            MonsterMovement();
            bool working = false;
            // Your code here
            if (this.active == true && this.gameStarted == true)
            {
                if (this.action == PlayerActions.NOTHING)
                {
                    for (int x = 0; x < dynamicMap.Length; x++)
                    {
                        for (int y = 0; y < dynamicMap[x].Length; y++)
                        {
                            if (dynamicMap[x][y].Equals('S'))
                            {
                                dynamicMap[x][y] = '@';
                                break;
                            }
                        }
                    }
                    working = true;
                }

                else if (this.action == PlayerActions.WEST)
                {
                    UpdateDynamicMap(GetPlayerPosition()[0], GetPlayerPosition()[1] - 1, GetPlayerPosition()[0], GetPlayerPosition()[1]);
                }

                else if (this.action == PlayerActions.EAST)
                {
                    UpdateDynamicMap(GetPlayerPosition()[0], GetPlayerPosition()[1] + 1, GetPlayerPosition()[0], GetPlayerPosition()[1]);
                }

                else if (this.action == PlayerActions.NORTH)
                {
                    UpdateDynamicMap(GetPlayerPosition()[0] - 1, GetPlayerPosition()[1], GetPlayerPosition()[0], GetPlayerPosition()[1]);
                }

                else if (this.action == PlayerActions.SOUTH)
                {
                    UpdateDynamicMap(GetPlayerPosition()[0] + 1, GetPlayerPosition()[1], GetPlayerPosition()[0], GetPlayerPosition()[1]);
                }

                else if (this.action == PlayerActions.ATTACK)
                {
                    PlayerAttack();
                }

                else if (this.action == PlayerActions.PICKUP)
                {
                    PickupCoin();
                }
            }

            return working;
        }

        public void PickupCoin()
        {
            if (this.onCoin == true)
            {
                if (monsterDetails["killedMonster"] > 0) //checking the amount of coins to add to the accumulated coins
                {
                    if (GetPlayerPosition()[0] == monsterDetails["deadMonster1CoinX"] && GetPlayerPosition()[1] == monsterDetails["deadMonster1CoinY"])
                    {
                        this.coins += monsterDetails["monster1Coins"];
                    }
                    else if (GetPlayerPosition()[0] == monsterDetails["deadMonster2CoinX"] && GetPlayerPosition()[1] == monsterDetails["deadMonster2CoinY"])
                    {
                        this.coins += monsterDetails["monster2Coins"];
                    }
                    this.coins += 1;
                }
                else
                {
                    this.coins += 1;
                }

                this.onCoin = false;
                this.playerDamagePoint += 1;
            }
        }

        /// <summary>
        /// Player attacking the monster by hitting space key
        /// </summary>
        public void PlayerAttack()
        {
            int[] position = GetPlayerPosition();
            if (dynamicMap[position[0] - 1][position[1]].Equals('M') || dynamicMap[position[0] + 1][position[1]].Equals('M') || dynamicMap[position[0]][position[1] - 1].Equals('M') || dynamicMap[position[0]][position[1] + 1].Equals('M'))
            {
                if (dynamicMap[position[0] - 1][position[1]].Equals('M'))
                {
                    DoDamage(position[0] - 1, position[1]);
                }
                if (dynamicMap[position[0] + 1][position[1]].Equals('M'))
                {
                    DoDamage(position[0] + 1, position[1]);
                }
                if (dynamicMap[position[0]][position[1] - 1].Equals('M'))
                {
                    DoDamage(position[0], position[1] - 1);
                }
                if (dynamicMap[position[0]][position[1] + 1].Equals('M'))
                {
                    DoDamage(position[0], position[1] + 1);
                }
            }
        }

        /// <summary>
        /// Does damage to the monster
        /// </summary>
        /// <param name="positionX"> monster x position</param>
        /// <param name="positionY">monster y position</param>
        public void DoDamage(int positionX, int positionY)
        {
            if (GetMonsterPosition()[0] == positionX && GetMonsterPosition()[1] == positionY)
            {
                monsterDetails["monster1DamagePoint"] -= 1;
                Console.WriteLine("You attacked monster!... monster have " + monsterDetails["monster1DamagePoint"] + " lives left.");
                if (monsterDetails["monster1DamagePoint"] == 0)
                {
                    monsterDetails["killedMonster"] += 1;
                    monsterDetails["deadMonster1CoinX"] = positionX;
                    monsterDetails["deadMonster1CoinY"] = positionY;
                    dynamicMap[positionX][positionY] = 'C';
                }
            }
            else if (GetMonsterPosition()[2] == positionX && GetMonsterPosition()[3] == positionY)
            {
                monsterDetails["monster2DamagePoint"] -= 1;
                Console.WriteLine("You attacked monster!... monster have " + monsterDetails["monster2DamagePoint"] + " lives left.");
                if (monsterDetails["monster2DamagePoint"] == 0)
                {
                    monsterDetails["killedMonster"] += 2;
                    monsterDetails["deadMonster2CoinX"] = positionX;
                    monsterDetails["deadMonster2CoinY"] = positionY;
                    dynamicMap[positionX][positionY] = 'C';
                }
            }
        }

        /// <summary>
        /// Initiates the monsters movement within the map
        /// </summary>
        public void MonsterMovement()
        {
            monsterMovementCount += 1;
            int[] position = GetMonsterPosition();
            var random = new Random();
            int value = random.Next(4); //Random movement of monster
            if (monsterMovementCount % 2 == 0)
            {
                if (position[0] > 0) //if monster position !=0, indicating that monster exists
                {
                    if (value == 3) //if monster movement is west
                    {
                        MonsterMove(position[0], position[1] - 1, position[0], position[1], 1);
                    }

                    else if (value == 2) //if monster movement is south
                    {
                        MonsterMove(position[0] + 1, position[1], position[0], position[1], 1);
                    }

                    else if (value == 1) //if monster movement is east
                    {
                        MonsterMove(position[0], position[1] + 1, position[0], position[1], 1);
                    }

                    else if (value == 0) //if monster movement is north
                    {
                        MonsterMove(position[0] - 1, position[1], position[0], position[1], 1);
                    }
                }

                if (position[2] > 0) //if second monster position !=0, indicating that monster exists
                {
                    if (value == 3) //if monster movement is west
                    {
                        MonsterMove(position[2], position[3] - 1, position[2], position[3], 2);
                    }

                    else if (value == 2) //if monster movement is south
                    {
                        MonsterMove(position[2] + 1, position[3], position[2], position[3], 2);
                    }

                    else if (value == 1) //if monster movement is east
                    {
                        MonsterMove(position[2], position[3] + 1, position[2], position[3], 2);
                    }

                    else if (value == 0) //if monster movement is north
                    {
                        MonsterMove(position[2] - 1, position[3], position[2], position[3], 2);
                    }
                }
            }

        }

        /// <summary>
        /// Initiates a monster movement, accumulate gold or do damage to player
        /// </summary>
        /// <param name="positionX">monster's x movement</param>
        /// <param name="positionY"> monster's y movement</param>
        /// <param name="initialPositionX">initial monster x position</param>
        /// <param name="initialPositionY"> initial monster y position</param>
        /// <param name="coin">monster coin</param>
        /// <param name="damagePoint">monster damage point</param>
        public void MonsterMove(int positionX, int positionY, int initialPositionX, int initialPositionY, int monster)
        {
            if (dynamicMap[positionX][positionY].Equals('-') || dynamicMap[positionX][positionY].Equals('C') || dynamicMap[positionX][positionY].Equals('@'))
            {
                if (dynamicMap[positionX][positionY].Equals('@')) //monster hits player dealing one damage to player
                {
                    this.playerDamagePoint -= 1;
                    Console.WriteLine("Monster Attacked!... You have only " + playerDamagePoint + " lives left.");
                    if (playerDamagePoint == 0)
                    {
                        Console.WriteLine("GAME OVER!...You have been killed by the monster...");
                        this.active = false;
                    }
                }
                else
                {
                    if (dynamicMap[positionX][positionY].Equals('C')) //monster accumulates point and increases damage point
                    {
                        if (monster == 1)
                        {
                            monsterDetails["monster1Coins"] += 1;
                            monsterDetails["monster1DamagePoint"] += 1;
                        }
                        else
                        {
                            monsterDetails["monster2Coins"] += 1;
                            monsterDetails["monster2DamagePoint"] += 1;
                        }
                    }
                    this.dynamicMap[initialPositionX][initialPositionY] = '-';
                    this.dynamicMap[positionX][positionY] = 'M';
                }
            }
        }

        /**
         * The Main Visual Output element. 
         * It draws the new map after the player did something onto the screen.
         * 
         * This is the method where you implement your the code to draw the map ontop the screen
         * and show the move to the user. 
         * 
         * The method returns true if the game is running and it can draw something, false otherwise.
        */
        public bool PrintMap()
        {
            bool returnBool;
            // Your code here
            try
            {
                for (int x = 0; x < dynamicMap.Length; x++)
                {
                    Console.Write("     ");
                    for (int y = 0; y < dynamicMap[x].Length; y++)
                    {
                        Console.Write(dynamicMap[x][y]);
                    }
                    Console.Write(Environment.NewLine);
                }
                Console.Write(Environment.NewLine);
                returnBool = true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                returnBool = false;
            }

            return returnBool;
        }
        /**
         * Additional Visual Output element. 
         * It draws the flavour texts and additional information after the map has been printed.
         * 
         * This is the method does not need to be used unless you want to output somethign else after the map onto the screen.
         * 
         */
        public bool PrintExtraInfo()
        {
            // Your code here
            if (this.action != PlayerActions.QUIT && this.active != false)
            {
                if (dynamicMap.Length == 0)
                {
                    Console.WriteLine("HELP:" + Environment.NewLine);
                    Console.WriteLine("LOADING MAP:");
                    Console.WriteLine("     \"load Simple.map\" to load simple map.");
                    Console.WriteLine("     \"load Simple2.map\" to load simple2 map.");
                    Console.WriteLine("     \"load Advanced.map\" to load advance map." + Environment.NewLine);
                    Console.WriteLine("Type \"Play\" to start game after map is loaded. \"Quit\" to quit game" + Environment.NewLine);
                }
                else if (this.gameStarted == false)
                {
                    Console.WriteLine("Type \"Play\" to start game.  \"Advanced play\" to play in advanced mode. \"Quit\" to quit game." + Environment.NewLine);
                }

                if (advanced)
                    Console.WriteLine("COMMANDS: W (Move North)     S (Move South)     D (Move East)     A (Move West)     P (Pickup coin)     [Space](Attack monster)" + Environment.NewLine);
                else
                    Console.WriteLine("COMMANDS: W (Move North)     S (Move South)     D (Move East)     A (Move West)     Quit (Quit game)" + Environment.NewLine);
            }
            return true;
        }

        /**
        * Map and GameState get initialized
        * mapName references a file name 
        * Do not use abosolute paths but use the files which are relative to the executable.
        * 
        * Create a private object variable for storing the map in Crawler and using it in the game.
        */
        public bool InitializeMap(String mapName)
        {
            bool initSuccess = false;

            // Your code here
            //Reading the file
            List<string> mapList = new List<string>();

            var fileStream = new FileStream("maps/" + mapName, FileMode.Open, FileAccess.Read);

            try
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        mapList.Add(line);
                    }
                }
                //Loading the list to the map
                char[][] map = new char[mapList.Count][];
                for (int x = 0; x < mapList.Count; x++)
                {
                    map[x] = new char[mapList[x].Length];
                    for (int y = 0; y < mapList[x].Length; y++)
                    {
                        map[x][y] = mapList[x][y];
                    }
                }
                this.originalMap = map;
                this.dynamicMap = new char[map.Length][];
                for (int i = 0; i < map.Length; i++)
                {
                    this.dynamicMap[i] = new char[map[i].Length];
                    for (int j = 0; j < map[i].Length; j++)
                    {
                        this.dynamicMap[i][j] = map[i][j];
                    }
                }
                initSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading map!");
                Console.WriteLine(ex);

            }
            return initSuccess;
        }

        /**
         * Returns a representation of the currently loaded map
         * before any move was made.
         * This map should not change when the player moves
         */
        public char[][] GetOriginalMap()
        {
            char[][] map = new char[0][];

            // Your code here
            map = originalMap;

            return map;
        }

        /*
         * Returns the current map state and contains the player's move
         * without altering it 
         */
        public char[][] GetCurrentMapState()
        {
            // the map should be map[y][x]
            char[][] map = new char[0][];

            // Your code here
            map = dynamicMap;

            return map;
        }

        /**
         * Returns the current position of the player on the map
         * 
         * The first value is the y coordinate and the second is the x coordinate on the map
         */
        public int[] GetPlayerPosition()
        {
            int[] position = { -1, -1 };
            if (this.dynamicMap.Length != 0)
            {
                for (int x = 0; x < dynamicMap.Length; x++)
                {
                    for (int y = 0; y < dynamicMap[x].Length; y++)
                    {
                        if (dynamicMap[x][y].Equals('@') || dynamicMap[x][y].Equals('S'))
                        {
                            position[0] = x;
                            position[1] = y;
                            break;
                        }
                    }
                }
            }
            else
            {
                position[0] = 0;
                position[1] = 0;
            }

            return position;
        }

        /// <summary>
        /// Gets the monsters position within the map in an array
        /// </summary>
        /// <returns></returns>
        public int[] GetMonsterPosition()
        {
            int[] monsterPosition = { 0, 0, 0, 0 };
            if (this.dynamicMap.Length != 0)
            {
                for (int x = 0; x < dynamicMap.Length; x++)
                {
                    for (int y = 0; y < dynamicMap[x].Length; y++)
                    {
                        if (dynamicMap[x][y].Equals('M'))
                        {
                            if (monsterPosition[0] == 0 && monsterDetails["killedMonster"] != 1)
                            {
                                monsterPosition[0] = x;
                                monsterPosition[1] = y;
                            }
                            else
                            {
                                monsterPosition[2] = x;
                                monsterPosition[3] = y;
                                break;
                            }
                        }
                    }
                }
            }

            return monsterPosition;
        }

        /**
        * Returns the next player action
        * 
        * This method does not alter any internal state
        */
        public int GetPlayerAction()
        {
            int action = 0;

            // Your code here
            action = (int)this.action;

            return action;
        }


        public bool GameIsRunning()
        {
            bool running = false;
            // Your code here
            if (this.active == true && this.gameStarted == true)
                running = true;

            return running;
        }

        public void printInstruction()
        {
            Console.WriteLine("\"load Simple.map\" to load Simple map");
            Console.WriteLine("\"load Advanced.map\" to load Advanced map");
            Console.WriteLine("\"load Simple2.map\" to load Simple2 map" + Environment.NewLine);
        }

        /**
         * Main method and Entry point to the program
         * ####
         * Do not change! 
        */
        static void Main(string[] args)
        {
            CMDCrawler crawler = new CMDCrawler();

            string input = string.Empty;
            Console.WriteLine("Welcome to the Commandline Dungeon!" + Environment.NewLine +
                "May your Quest be filled with riches!" + Environment.NewLine);
            crawler.printInstruction();

            // Loops through the input and determines when the game should quit
            while (crawler.active && crawler.action != PlayerActions.QUIT)
            {
                Console.WriteLine("Your Command: ");
                input = crawler.ReadUserInput();
                Console.WriteLine(Environment.NewLine);

                crawler.ProcessUserInput(input);

                crawler.Update(crawler.active);
                crawler.PrintMap();
                crawler.PrintExtraInfo();
            }

            Console.WriteLine("See you again" + Environment.NewLine +
                "In the CMD Dungeon! ");

        }

    }
}
