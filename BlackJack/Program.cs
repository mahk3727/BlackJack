using System;
using System.Collections.Generic;

namespace BlackjackGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Blackjack!");

            // Create and shuffle a deck of cards
            Deck deck = new Deck();
            deck.Shuffle();

            // Initialize players
            Player dealer = new Player("Dealer");
            Player player = new Player("Player");

            // Deal initial cards
            dealer.AddCard(deck.DealCard());
            dealer.AddCard(deck.DealCard());
            player.AddCard(deck.DealCard());
            player.AddCard(deck.DealCard());

            // Display initial hands
            Console.WriteLine("Dealer's Hand:");
            dealer.ShowHand();
            Console.WriteLine("\nPlayer's Hand:");
            player.ShowHand();

            // Player's turn
            while (true)
            {
                Console.WriteLine("\nDo you want to hit or stand? (h/s)");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "h")
                {
                    player.AddCard(deck.DealCard());
                    Console.WriteLine("\nPlayer's Hand:");
                    player.ShowHand();

                    if (player.CalculateHandValue() > 21)
                    {
                        Console.WriteLine("\nBusted! You lose.");
                        return;
                    }
                }
                else if (choice.ToLower() == "s")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'h' to hit or 's' to stand.");
                }
            }

            // Dealer's turn
            while (dealer.CalculateHandValue() < 17)
            {
                dealer.AddCard(deck.DealCard());
            }
            Console.WriteLine("\nDealer's Hand:");
            dealer.ShowHand();

            // Determine winner
            int playerScore = player.CalculateHandValue();
            int dealerScore = dealer.CalculateHandValue();

            if (playerScore > 21)
            {
                Console.WriteLine("\nPlayer busted! Dealer wins.");
            }
            else if (dealerScore > 21 || playerScore > dealerScore)
            {
                Console.WriteLine("\nPlayer wins!");
            }
            else if (playerScore < dealerScore)
            {
                Console.WriteLine("\nDealer wins.");
            }
            else
            {
                Console.WriteLine("\nIt's a tie!");
            }
        }
    }

    class Card
    {
        public string Suit { get; }
        public string FaceValue { get; }

        public Card(string suit, string faceValue)
        {
            Suit = suit;
            FaceValue = faceValue;
        }

        public int GetValue()
        {
            if (FaceValue == "Ace")
                return 11;
            if (FaceValue == "Jack" || FaceValue == "Queen" || FaceValue == "King")
                return 10;
            return int.Parse(FaceValue);
        }

        public override string ToString()
        {
            return $"{FaceValue} of {Suit}";
        }
    }

    class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] faceValues = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            foreach (string suit in suits)
            {
                foreach (string faceValue in faceValues)
                {
                    cards.Add(new Card(suit, faceValue));
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card DealCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }

    class Player
    {
        public string Name { get; }
        private List<Card> hand;

        public Player(string name)
        {
            Name = name;
            hand = new List<Card>();
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public void ShowHand()
        {
            foreach (Card card in hand)
            {
                Console.WriteLine(card);
            }
        }

        public int CalculateHandValue()
        {
            int value = 0;
            int aceCount = 0;

            foreach (Card card in hand)
            {
                value += card.GetValue();
                if (card.FaceValue == "Ace")
                    aceCount++;
            }

            // Adjust for aces
            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }
    }
}
