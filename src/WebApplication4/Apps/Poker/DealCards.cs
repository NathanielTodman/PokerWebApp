using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class DealCards : DeckOfCards
    {
        private Card[] PlayerHand;
        private Card[] ComputerHand;
        private Card[] SortedPlayerHand;
        private Card[] SortedComputerHand;
        public Card[] FlopHand;
        public Card[] FirstPlayerHand;
        public Card[] FirstComputerHand;
        public Hand winningPlayerHand;
        public Hand winningCpuHand;
        public double playerWallet;
        public double cpuWallet;
        public int result;

        public DealCards()
        {
            FlopHand = new Card[5];
            PlayerHand = new Card[7];
            FirstPlayerHand = new Card[2];
            SortedPlayerHand = new Card[7];
            ComputerHand = new Card[7];
            FirstComputerHand = new Card[2];
            SortedComputerHand = new Card[7];
        }
        public void Deal(double pWallet, double cWallet)
        {
            playerWallet = pWallet;
            cpuWallet = cWallet;
            SetupDeck(); //Create deck and then shuffle them
            GetHand(); //Deal hand
            SortCards(); //Sort cards  
            EvaluateHands(); //Evaluate hand

        }
        public void GetHand()
        {
            //Deal 2 cards for the player
            for (int i = 0; i < 2; i++)
                PlayerHand[i] = GetDeck[i];
            //Deal 2 cards for the CPU
            for (int i = 2; i < 4; i++)
                ComputerHand[i - 2] = GetDeck[i];
            //Deal 5 cards for flop      
            for (int i = 4; i < 9; i++)
                FlopHand[i - 4] = GetDeck[i];
            //Remember first hands before merging with flop for evaluating personal high card

            FirstPlayerHand[0] = PlayerHand[0];
            FirstPlayerHand[1] = PlayerHand[1];
            FirstComputerHand[0] = ComputerHand[0];
            FirstComputerHand[1] = ComputerHand[1];
            //Add flop hand to both hands
            FlopHand.CopyTo(PlayerHand, 2);
            FlopHand.CopyTo(ComputerHand, 2);
        }

        //Sort cards by card value
        public void SortCards()
        {
            var QueryPlayer = from hand in PlayerHand
                              orderby hand.myValue
                              select hand;
            var QueryComputer = from hand in ComputerHand
                                orderby hand.myValue
                                select hand;
            var index = 0;

            foreach(var element in QueryPlayer.ToList())
            {
                SortedPlayerHand[index] = element;
                index++;
            }
            index = 0;
            foreach (var element in QueryComputer.ToList())
            {
                SortedComputerHand[index] = element;
                index++;
            }
        }

        // function to rename cards to match image names of card jpgs for displaying on page
        public string[] RenameHands(Card[] hand)
        {
            string[] newHand = new string[hand.Length];
            int i = 0;
            foreach (var card in hand)
            {
                if((int)card.myValue>10)
                    newHand[i] = (Convert.ToString(card.myValue)) + "_of_" + (Convert.ToString(card.mySuit)) + ".png";
                else
                    newHand[i] = (Convert.ToString((int)card.myValue)) + "_of_" + (Convert.ToString(card.mySuit)) + ".png";

                i++;
            }
            return newHand;
        }

        public void EvaluateHands()
        {
            //create each players hand evaluator object pre flop (to establish each players high cards)
            HandEvaluator firstPlayerHandEvaluator = new HandEvaluator(FirstPlayerHand);
            HandEvaluator firstComputerHandEvaluator = new HandEvaluator(FirstComputerHand);

            //create player and cpu evaluation objects (passing SORTED hand to constructor)
            HandEvaluator playerHandEvaluator = new HandEvaluator(SortedPlayerHand);
            HandEvaluator computerHandEvaluator = new HandEvaluator(SortedComputerHand);

            //evaluate the player's and computer's hand
            Hand playerHand = playerHandEvaluator.EvaluateHand();
            Hand computerHand = computerHandEvaluator.EvaluateHand();

            //assign to public variable to send to model
            winningPlayerHand = playerHand;
            winningCpuHand = computerHand;


            //evaluate hands
            if (playerHand > computerHand)
            {
                result = 1;
            }
            else if (playerHand < computerHand)
            {
                result = 0;
            }
            else


            //if the hands are the same, evaluate the values
            {
                //first evaluate who has higher value of poker hand
                if (playerHandEvaluator.HandValues.Total > computerHandEvaluator.HandValues.Total)
                {
                    result = 1;
                }

                else if (playerHandEvaluator.HandValues.Total < computerHandEvaluator.HandValues.Total)
                {
                    result = 0;
                }
                //if both have the same poker hand (for example, both have a pair of queens), 
                //then the player with the next higher card wins
                else if (firstPlayerHandEvaluator.HighCard > firstComputerHandEvaluator.HighCard)
                    result = 1;
                else if (firstPlayerHandEvaluator.HighCard < firstComputerHandEvaluator.HighCard)
                    result = 0;                //if high card is of same value check second card in players hand
                else if (firstPlayerHandEvaluator.SecondHighCard > firstComputerHandEvaluator.SecondHighCard)
                    result = 1;
                else if (firstPlayerHandEvaluator.SecondHighCard < firstComputerHandEvaluator.SecondHighCard)
                    result = 0;
                else
                {
                    result = 2;
                }
            }
        }
    }
}
