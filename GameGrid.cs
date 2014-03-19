using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleSolitaire
{
    //a class used to store x and y coordinates in the GameGrid so that it can be used as a list
    class Coordinates
    {
        //internal limits access to current assembly
        protected internal int x;
        protected internal int y;

        public Coordinates()
        {
            x = 0;
            y = 0;
        }

        public Coordinates(int setx, int sety)
        {
            x = setx;
            y = sety;
        }

        protected internal bool isEqual(Coordinates check)
        {
            if (this.x == check.x && this.y == check.y)
                return true;
            else
                return false;
        }

        protected internal void reset()
        {
            x = 0;
            y = 0;
        }
    }

    class GameGrid
    {
        private char[,] grid; //The actual grid, a 2d array of characters
        private int size; //The size of each dimension of the array (length and width)
        private Coordinates origin; //used to ensure the game will begin searching at 0,0
        private List<Coordinates> coords; //keeps a list of the coordinates used to find the word
        private List<string> found; //The array of words that have been found by the user, used to make sure double credit is not given
        private string word; //The word being searched for in the grid
        private bool wordFound; //a bool flag used to see if the word has been found in the grid
        private int totalPoints; //the total points the user has earned so far

        //Constructor for the game grid.  sets some default values and does the initial fill on the board.
        public GameGrid(int gridSize)
        {
            size = gridSize;
            grid = new char[size,size];
            coords = new List<Coordinates>();
            word = "";
            wordFound = false;
            totalPoints = 0;

            //fill the game grid with letters
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //adds 65 so that the int will map to an ascii character
                    grid[i,j] = Convert.ToChar(random.Next(26) + 65);
                }
            }

            //initalize current location
            origin = new Coordinates();
            coords.Add(origin);

            found = new List<string>();
        }
        //resets important values for the search back to their starting values, so search will correctly
        //start from the beginning
        protected internal void resetGameCalcs()
        {
            wordFound = false;
            coords.Clear();
            origin.reset();
            coords.Add(origin);
        }
        //a getter to find if the word has been found in the game grid
        protected internal bool isWordFound()
        {
            return wordFound;
        }
        //Prints out the game grid to the console for the player to view
        protected internal void showGrid()
        {
            Console.WriteLine();

            //Prints out the array so that the array coordinates will correspond with x,y rather than
            //being reversed.  This means traversing the grid left to right will mean moving the x
            //position, and traversing top to bottom will mean moving the y position.
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(grid[j,i] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //A setter to save the word the player is looking for in the game grid
        protected internal void setWord(string Word)
        {
            word = Word.ToUpper();
        }

        //Searches the list of words the player has already found to make sure they are not
        //able to search for the same word multiple times
        protected internal bool foundWordSearch()
        {
            int index;

            found.Sort();
            index = found.BinarySearch(word);
            if (index < 0)
                return false;
            else
                return true;

        }

        //checks to see if a location has already been used in creating a word on the grid
        protected internal bool coordCheck()
        {
            foreach (Coordinates coord in coords)
            {
                if (coord.isEqual(coords[coords.Count - 1]))
                {
                    return true;
                }
            }
            return false;
        }
        //allows creation of a new point to check
        protected internal bool coordCheck(int x, int y)
        {
            Coordinates check = new Coordinates(x, y);
            foreach (Coordinates coord in coords)
            {
                if (coord.isEqual(check))
                {
                    return true;
                }
            }
            return false;
        }

        //this search function recursively travels through a 2d array, starting at [0,0] and attempts to build
        //the word given by the player.  It will check all 8 adjacent spaces to the starting location (while
        //making sure spaces that do not exist are not attempted) and will also ensure that each letter is
        //used only once.
        protected internal void search()
        {
            //Check if word has been found on this move
            if (coords.Count == word.Length && word[coords.Count - 1] == grid[coords.Last().x, coords.Last().y])
            {
                //word has been found, reset information and end recursion
                coords.Clear();
                coords.Add(origin);
                wordFound = true; //flips to true so the stack will resolve
            }
            //Check to see if still inside grid and not finished searching, as well as that the word has still not been found
            else if (coords.Last().x < size && coords.Last().y < size && !wordFound)
            {
                //Check if the current position contains the current letter in the word being searched for
                //If true, it will then go to the 8 adjacent spaces (where possible) and see if it can
                //continue attempting to make the word with any of the letters in the spaces.  If not,
                //the search will move on to the next point and attempt from there.
                //
                //If one were to draw out how the grid around the current space to be checked would appear logically, it would look
                //like the following:
                //
                //  [x-1, y-1][x, y-1 ][x+1, y-1]
                //  [x-1, y  ][current][x+1, y  ]
                //  [x-1, y+1][x, y+1 ][x+1, y+1]
                if (grid[coords.Last().x, coords.Last().y] == word[coords.Count - 1])
                {
                    //if not currently in the top row
                    if (coords.Last().y > 0)
                    {
                        //check above current position
                        if (grid[coords.Last().x, coords.Last().y - 1] == word[coords.Count] && !coordCheck(coords.Last().x, coords.Last().y - 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x, coords.Last().y - 1);
                            coords.Add(next);
                            search();
                        }

                        //check above and left of current position, if possible
                        if (coords.Last().x > 0 && grid[coords.Last().x - 1, coords.Last().y - 1] == word[coords.Count] && !coordCheck(coords.Last().x - 1, coords.Last().y - 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x - 1, coords.Last().y - 1);
                            coords.Add(next);
                            search();
                        }

                        //check above and right of current position, if possible
                        if (coords.Last().x < size - 1 && grid[coords.Last().x + 1, coords.Last().y - 1] == word[coords.Count] && !coordCheck(coords.Last().x + 1, coords.Last().y - 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x + 1, coords.Last().y - 1);
                            coords.Add(next);
                            search();
                        }
                    }
                    //if not currently in the bottom row
                    if (coords.Last().y < size - 1)
                    {
                        //check below current position
                        if (grid[coords.Last().x, coords.Last().y + 1] == word[coords.Count] && !coordCheck(coords.Last().x, coords.Last().y + 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x, coords.Last().y + 1);
                            coords.Add(next);
                            search();
                        }

                        //check below and left of current position, if possible
                        if (coords.Last().x > 0 && grid[coords.Last().x - 1, coords.Last().y + 1] == word[coords.Count] && !coordCheck(coords.Last().x - 1, coords.Last().y + 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x - 1, coords.Last().y + 1);
                            coords.Add(next);
                            search();
                        }

                        //check below and right of current position, if possible
                        if (coords.Last().x < size - 1 && grid[coords.Last().x + 1, coords.Last().y + 1] == word[coords.Count] && !coordCheck(coords.Last().x + 1, coords.Last().y + 1))
                        {
                            Coordinates next = new Coordinates(coords.Last().x + 1, coords.Last().y + 1);
                            coords.Add(next);
                            search();
                        }
                    }
                    //check left if not currently in left column
                    if (coords.Last().x > 0 && grid[coords.Last().x - 1, coords.Last().y] == word[coords.Count] && !coordCheck(coords.Last().x - 1, coords.Last().y))
                    {
                        Coordinates next = new Coordinates(coords.Last().x - 1, coords.Last().y);
                        coords.Add(next);
                        search();
                    }
                    //check right if not currently in right column
                    if (coords.Last().x < size - 1 && grid[coords.Last().x + 1, coords.Last().y] == word[coords.Count] && !coordCheck(coords.Last().x + 1, coords.Last().y))
                    {
                        Coordinates next = new Coordinates(coords.Last().x + 1, coords.Last().y);
                        coords.Add(next);
                        search();
                    }
                    //At this point, there is no adjacent space with a letter that will help completed the word.  We now need to
                    //fall back down the stack to see if we can solve it in another direction, or decide we are unable to find
                    //the word in the grid.
                    //
                    //This will check if we are in the middle of finding a word at the moment, since the coords list will only
                    //have the origin point in it if we are not.  Otherwise, it will remove the last coordinate to be checked
                    //from the list and drop down a level on the stack.
                    if (coords.Count > 1)
                    {
                        coords.RemoveAt(coords.Count - 1);
                    }
                    else if (coords.Count == 1)
                    {
                        //Check to see if you are at the end of the row, and go to the next row if needed
                        if (coords.Last().x > size - 1)
                        {
                            coords.Last().x = 0;
                            coords.Last().y++;
                        }
                        else
                        {
                            coords.Last().x++;
                        }

                        search();
                    }
                }
                //This position did not contain a letter we need, so move on to the next one
                else
                {
                    //Check to see if you are at the end of the row, and go to the next row if needed
                    if (coords.Last().x >= size - 1)
                    {
                        coords.Last().x = 0;
                        coords.Last().y++;
                        Console.WriteLine();
                    }
                    else
                    {
                        coords.Last().x++;
                    }
                    search();
                }
            }
            //Check if the current coordinate to be checked is now off the board, meaning the entire board has been checked.
            else if(coords.Last().x > size - 1 && coords.Last().y >= size - 1)
            {
                //stops the search, as the entire grid has been checked
                coords.Clear();
                Console.WriteLine("not found");
            }
        }

        //Adds a word found by the player to the found word list
        protected internal void addWord()
        {
            found.Add(word);
        }

        //rewards the player points after finding a word.  Point values are equal to the length of the word minus 3, so that only 4 letter
        //words and longer are rewarded
        protected internal void addPoints()
        {
            int points = word.Length - 3;

            Console.WriteLine("Congratulations!  You found a word!");
            if (points > 0)
            {
                totalPoints += points;
                Console.WriteLine("You earned {0} points!", points);
            }
            else
                Console.WriteLine("I'm sorry.  You only earn points if the word is 4 letters more.");

            Console.WriteLine("You have {0} points so far.", totalPoints);
            Console.WriteLine();
        }

        //Runs after the game ends, displaying total points and number of words found
        protected internal void endGame()
        {
            Console.WriteLine("Thanks for playing!  You had a score of {0}.", totalPoints);
            Console.WriteLine("You found a total of {0} words.", found.Count);
        }
    }
}
