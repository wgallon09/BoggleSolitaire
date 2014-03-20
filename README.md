BoggleSolitaire
===============

This program plays a game of Boggle Solitaire with the player

The program was created in VS2010

This program was assigned as a school project in order to have practice in both object oriented programming and recursion.
The object of this program is to allow the user to play Boggle Solitaire, which is a variation of Boggle that is
for one player to play.

The game creates a grid, up to 10x10 depending on the player's input, and randomly generates characters in this grid.  The
player then inputs words they find in the grid, and the game will search the grid for the word (assuming it is in
the game's dictionary) and reward the player if the word is found.  The letters can be in any order, as long as there is
a clear path from the first letter to the last letter by going through adjacent spaces.  This is checked by iterating
through the array, then recursively searching for words when the first letter of the word is found.  This search continues
until it is unable to finish the word, or it is able to finish the word and reward the player for the word they searched
for.
