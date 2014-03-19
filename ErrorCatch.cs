using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

//This file contains all error checking methods to help ensure data integrity

namespace BoggleSolitaire
{
    partial class Program
    {
        //Gets the size of the game grid from the player, and checks that the input is valid
        static int getSize()
        {
            string temp;
            int size;

            Console.WriteLine("Please insert a positive number, 10 or less, to determine the size of the game grid.");
            temp = Console.ReadLine(); //reads in the input as a string, so that invalid characters will not affect the program
            size = Convert.ToInt32(temp);
            if (size > 0 && size < 11) //makes sure the number is in the allowed range
            {
                return size;
            }
            else
            {
                Console.WriteLine("The number given is not valid.");
                return getSize();
            }
        }

        //A method which asks the player if they would like to continue playing after running through the game once,
        //and then returns true or false depending on their answer.
        static bool keepPlaying()
        {
            string temp;
            char confirm;
            Console.WriteLine("Would you like to continue playing?  Type in Y for yes or N for no.");
            temp = Console.ReadLine(); //Reads answer as a string incase the player pushes too many buttons
            confirm = Char.ToUpper(temp[0]);

            if (confirm != 'N')
                return true;
            else //assumes that a character not putting in an N means they wish to continue
                return false;

        }

        //A method which gets the word the player is searching for, normalizes it to upper case to
        //match the words that are in the dictionary, checks that it only contains letters, and then
        //returns the word.
        static string getWord()
        {
            string word;
            Match charCheck;

            Console.WriteLine("Please type in the word you wish to search for in the grid.");
            word = (Console.ReadLine()).ToUpper();
            charCheck = Regex.Match(word, @"[A-Z]");

            if (!charCheck.Success)
            {
                Console.WriteLine("Please insert a word with letters only.");
                return getWord();
            }
            else
            {
                return word;
            }
        }
    }
}
