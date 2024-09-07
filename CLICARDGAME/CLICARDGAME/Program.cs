using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLICARDGAME
{

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the 3-2-5 Card Game!");

                var deck = InitializeDeck();
                var shuffledDeck = deck.OrderBy(x => Guid.NewGuid()).ToList();

                var players = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2"),
                new Player("Player 3")
            };

                var dealerIndex = DetermineDealer();
                DealCards(players, shuffledDeck, dealerIndex);

                int trumpPlayerIndex = (dealerIndex + 1) % 3;
                string trumpSuit = ChooseTrump(players[trumpPlayerIndex]);
                Console.WriteLine(string.Format("Trump Suit: {0}", trumpSuit));

                PlayGame(players, dealerIndex, trumpSuit);

                if (!PlayAgain())
                {
                    break;
                }
            }
        }

        static List<Card> InitializeDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "7", "8", "9", "10", "Q", "K", "A" };

            var deck = new List<Card>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }

            // Include 7 of Hearts and 7 of Spades
            deck.Add(new Card("Hearts", "7"));
            deck.Add(new Card("Spades", "7"));

            return deck;
        }

        static int DetermineDealer()
        {
            Random random = new Random();
            int dealerIndex = random.Next(0, 3);
            Console.WriteLine(string.Format("Dealer is Player {0}", dealerIndex + 1));
            return dealerIndex;
        }

        static void DealCards(List<Player> players, List<Card> shuffledDeck, int dealerIndex)
        {
            int[] trickGoals = { 2, 3, 5 };
            int playerIndex = dealerIndex;

            foreach (var player in players)
            {
                player.Hand = shuffledDeck.Skip(playerIndex * 10).Take(10).ToList();
                player.TrickGoal = trickGoals[playerIndex];
                player.TricksWon = 0;
                playerIndex = (playerIndex + 1) % 3;
            }
        }

        static string ChooseTrump(Player player)
        {
            Console.WriteLine(string.Format("{0}, here is your hand:\n", player.Name));

            // Ensure that the player has the cards
            if (player.Hand == null || !player.Hand.Any())
            {
                Console.WriteLine("Your hand is empty.");
                return null;
            }

            foreach (var card in player.Hand.Take(5))
            {
                Console.WriteLine(string.Format("{0} of {1}", card.Rank, ConvertToShortSuit(card.Suit)));
            }

            Console.WriteLine("\nChoose the trump suit from your first 5 cards (Enter H, D, C, S):");

            string trumpSuit;
            while (true)
            {
                Console.Write("Enter Trump Suit: ");
                trumpSuit = Console.ReadLine().ToUpper(); // Ensure input is uppercase
                if (IsValidSuit(trumpSuit))
                {
                    break;
                }
                Console.WriteLine("Invalid suit. Please enter a valid suit.");
            }

            return ConvertToFullSuit(trumpSuit);
        }

        static bool IsValidSuit(string suit)
        {
            string[] validSuits = { "H", "D", "C", "S" };
            return validSuits.Contains(suit);
        }

        static string ConvertToFullSuit(string shortSuit)
        {
            switch (shortSuit)
            {
                case "H": return "Hearts";
                case "D": return "Diamonds";
                case "C": return "Clubs";
                case "S": return "Spades";
                default: return null;
            }
        }

        static string ConvertToShortSuit(string fullSuit)
        {
            switch (fullSuit)
            {
                case "Hearts": return "H";
                case "Diamonds": return "D";
                case "Clubs": return "C";
                case "Spades": return "S";
                default: return null;
            }
        }

        static void PlayGame(List<Player> players, int dealerIndex, string trumpSuit)
        {
            int round = 1;

            while (round <= 10)
            {
                Console.WriteLine(string.Format("\n--- Round {0} ---", round));
                var trickCards = new List<Card>();

                foreach (var player in players)
                {
                    Console.WriteLine(string.Format("{0}, here is your hand:\n", player.Name));
                    foreach (var card in player.Hand)
                    {
                        Console.WriteLine(string.Format("{0} of {1}", card.Rank, ConvertToShortSuit(card.Suit)));
                    }

                    Card cardPlayed;
                    while (true)
                    {
                        Console.WriteLine("Play a card (format: Rank of Suit): ");
                        var input = Console.ReadLine();
                        cardPlayed = ParseCard(input, player.Hand);

                        // Debugging: Print the parsed card
                        Console.WriteLine(string.Format("Parsed Card: {0} of {1}",
                            cardPlayed != null ? cardPlayed.Rank : "Unknown",
                            cardPlayed != null ? cardPlayed.Suit : "Unknown"));

                        if (cardPlayed != null)
                        {
                            player.Hand.Remove(cardPlayed);
                            trickCards.Add(cardPlayed);
                            break;
                        }
                        Console.WriteLine("Invalid card. Please play a valid card from your hand.");
                    }
                }

                string leadSuit = trickCards[0].Suit;
                var winningCard = DetermineTrickWinner(trickCards, leadSuit, trumpSuit);
                var winningPlayer = players.First(p => p.Hand.Count == 10 - round);

                Console.WriteLine(string.Format("{0} wins the trick with {1} of {2}!",
                    winningPlayer.Name, winningCard.Rank, winningCard.Suit));

                winningPlayer.TricksWon++;
                round++;
            }

            foreach (var player in players)
            {
                Console.WriteLine(string.Format("{0} won {1} tricks.", player.Name, player.TricksWon));
            }

            Console.WriteLine("Game Over!");
        }

        static Card ParseCard(string input, List<Card> hand)
{
    var parts = input.Split(new string[] { " of " }, StringSplitOptions.None); // Correct split method
    if (parts.Length != 2) return null;

    string rank = parts[0].Trim();
    string suit = ConvertToFullSuit(parts[1].Trim());

    // Debugging: Print the expected suit and rank
    Console.WriteLine(string.Format("Expected Rank: {0}, Expected Suit: {1}", rank, suit));

    if (suit == null)
    {
        return null;
    }

    return hand.FirstOrDefault(card => card.Rank == rank && card.Suit == suit);
}


        static Card DetermineTrickWinner(List<Card> trickCards, string leadSuit, string trumpSuit)
        {
            Card winningCard = trickCards[0];
            foreach (var card in trickCards)
            {
                if (card.Suit == trumpSuit && winningCard.Suit != trumpSuit)
                {
                    winningCard = card;
                }
                else if (card.Suit == leadSuit && winningCard.Suit == leadSuit)
                {
                    if (CompareRanks(card.Rank, winningCard.Rank) > 0)
                    {
                        winningCard = card;
                    }
                }
            }
            return winningCard;
        }

        static int CompareRanks(string rank1, string rank2)
        {
            Dictionary<string, int> rankValues = new Dictionary<string, int>()
        {
            { "7", 1 },
            { "8", 2 },
            { "9", 3 },
            { "10", 4 },
            //{ "J", 5 },
            { "Q", 6 },
            { "K", 7 },
            { "A", 8 }
        };

            return rankValues[rank1].CompareTo(rankValues[rank2]);
        }

        static bool PlayAgain()
        {
            Console.WriteLine("Do you want to play again? (y/n): ");
            return Console.ReadLine().ToLower() == "y";
        }
    }

    class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }

    class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public int TrickGoal { get; set; }
        public int TricksWon { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            TricksWon = 0;
        }
    }
}
