using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//This file contains all methods needed to run the main game program

namespace BoggleSolitaire
{
    partial class Program
    {
        //Fills up the starting dictionary for the game to operate, and ensures that each word is in
        //all capital letters so that it will be correctly searchable
        static void fillDict(List<string> dict)
        {
            using (StreamReader infile = new StreamReader("dictionary.txt"))
            {
                while (!infile.EndOfStream)
                {
                    dict.Add(infile.ReadLine().ToUpper());
                }
            }
            dict.Sort(); //sorts the dict so a binary search is possible later
        }

        //This method uses a binary search to improve search speed when looking through the dictionary for a word
        static bool searchDict(string word, List<string> dict)
        {
            int index = dict.BinarySearch(word);
            if (index < 0)
            {
                Console.WriteLine("That word was not found in the dictionary.  Please try again.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
