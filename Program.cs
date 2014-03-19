/* William Gallon
 * This program plays a game of Boggle Solitaire with the user.  It gets a size
 * of a grid that the player provides, and then creates a set of randomly generated
 * letters inside said grid.  It then takes words from the user and searches to
 * make sure the words given are a part of the game's dictionary file.  After they
 * are found in the dictionary, the word is searched for in the game grid itself.
 * If the word is found in the grid, the user scores points based on the length of
 * the word.  At the end of the game, the player is told how many points they have
 * earned as well as how many words they found.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoggleSolitaire
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //declare variables
            List<string> dict = new List<string>();
            string word;
            GameGrid game = new GameGrid(getSize());

            fillDict(dict); //Loads the dictionary into memory as a list
            do
            {
                game.resetGameCalcs(); //ensures important values are set correctly
                game.showGrid(); //displays the grid to the player
                word = getWord(); //gets the word the player wants to search for
                if (searchDict(word, dict)) //checks if the player's word is in the dictionary
                {
                    game.setWord(word); //saves the word in the GameGrid object
                    if (!game.foundWordSearch()) //checks that the word has not been found before
                    {
                        game.search(); //search for the word given on the grid
                        if (game.isWordFound()) //checks to see if the word was found on the grid
                        {
                            game.addPoints(); //reward the user points based on the word's length
                            game.addWord(); //adds the word to the list of found words
                        }
                        else
                        {
                            Console.WriteLine("That word is not on the game board.  Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Oops!  You have already found that word.  Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("That word was not found in the dictionary.  Please try again.");
                }
            }
            while (keepPlaying()); //asks user if they would like to continue playing
            game.endGame(); //gives the user their end of game statistics
        }
    }
}
