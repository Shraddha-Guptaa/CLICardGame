# CLICardGame
This repository contains a 2-3-5 or5-3-2 CARD GAME. Created Asp.net Console Applicaion. And C# as a Backend Programing Language

Project Overview:
The 5-3-2 card game (also known as 2-3-5) is implemented as a Command Line Interface (CLI) application, following the rules provided. This implementation includes:

Three players.
30-card deck (cards 7 to Ace from each suit, no jokers).
Proper game flow: Shuffling, dealing, trump declaration, and trick play.
Validation for invalid moves and proper trick handling.
Game scoring: Based on the number of tricks each player wins.
Steps to Run the Project:
Download the Project Files:


Launch Visual Studio 2012 (or any compatible version).
Click on "File" > "Open" > "Project/Solution" and select the .sln file from the downloaded folder.
Build the Solution:

Once the project is open, go to the "Build" menu and select "Build Solution" or press Ctrl + Shift + B.
Ensure that the build completes successfully without any errors.
Run the Project:

Go to the "Debug" menu and select "Start Without Debugging" (Ctrl + F5).
This will launch the game in a console window.
Game Flow:

Player Setup: The game begins by shuffling and dealing the cards.
Trump Declaration: The player to the right of the dealer declares the trump suit after receiving their first batch of cards.
Playing Tricks: Players take turns playing cards, and the highest card of the lead suit or trump wins the trick.
Game Objective: The goal is to win the assigned number of tricks (5 for Player 1, 3 for Player 2, and 2 for the Dealer).
Scoring: Points are awarded based on the tricks won, with special rules for winning all 10 tricks.
Exit the Game:

After completing the game, the results are displayed, and the program exits automatically.
You can restart the game by re-running the application.

