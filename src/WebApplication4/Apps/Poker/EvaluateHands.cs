using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public enum Hand
    {
        Nothing,
        HighCard,
        OnePair,
        TwoPairs,
        ThreeKind,
        Straight,
        Flush,
        FullHouse,
        FourKind,
        StraightFlush
    }

    public struct HandValue
    {
        public int Total { get; set; }
    }

    class HandEvaluator : Card
    {
        private int heartsSum;
        private int diamondSum;
        private int clubSum;
        private int spadesSum;
        private Card[] cards;
        private HandValue handValue;
        public int HighCard;
        public int SecondHighCard;

        public HandEvaluator(Card[] sortedHand)
        {
            heartsSum = 0;
            diamondSum = 0;
            clubSum = 0;
            spadesSum = 0;
            if ((int)sortedHand[0].myValue > (int)sortedHand[1].myValue)
            {
                HighCard = (int)sortedHand[0].myValue;
                SecondHighCard = (int)sortedHand[1].myValue;
            }
            else if ((int)sortedHand[1].myValue > (int)sortedHand[0].myValue)
            {
                HighCard = (int)sortedHand[1].myValue;
                SecondHighCard = (int)sortedHand[0].myValue;
            }
            cards = new Card[sortedHand.Length];
            Cards = sortedHand;
            handValue = new HandValue();

        }

        public HandValue HandValues
        {
            get { return handValue; }
            set { handValue = value; }
        }

        public Card[] Cards
        {
            get { return cards; }
            set
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    cards[i] = value[i];
                }
            }
        }

        public Hand EvaluateHand()
        {
            //get the number of each suit on hand
            getNumberOfSuit();
            if (StraightFlush())
                return Hand.StraightFlush;
            else if (FourOfKind())
                return Hand.FourKind;
            else if (FullHouse())
                return Hand.FullHouse;
            else if (Flush())
                return Hand.Flush;
            else if (Straight())
                return Hand.Straight;
            else if (ThreeOfKind())
                return Hand.ThreeKind;
            else if (TwoPairs())
                return Hand.TwoPairs;
            else if (OnePair())
                return Hand.OnePair;

            //if the hand is nothing, then the player with highest card wins
            
            return Hand.HighCard;

            

        
    }


    private void getNumberOfSuit()
        {
            foreach (var element in Cards)
            {
                if (element.mySuit == Card.Suit.Hearts)
                    heartsSum++;
                else if (element.mySuit == Card.Suit.Diamonds)
                    diamondSum++;
                else if (element.mySuit == Card.Suit.Clubs)
                    clubSum++;
                else if (element.mySuit == Card.Suit.Spades)
                    spadesSum++;
            }
        }

        private bool StraightFlush()
        {
            //if 5 consecutive values and the 5 cards share the same suit
            if (cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                cards[4].myValue + 1 == cards[5].myValue &&
                cards[5].myValue + 1 == cards[6].myValue &&
                ((cards[2].mySuit == cards[3].mySuit) && (cards[3].mySuit == cards[4].mySuit)
                && (cards[4].mySuit == cards[5].mySuit) && (cards[5].mySuit == cards[6].mySuit)))
            {
                //player with the highest value of the last card wins
                handValue.Total = (int)cards[6].myValue;
                return true;
            }

            else if (
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                cards[4].myValue + 1 == cards[5].myValue &&
                ((cards[1].mySuit == cards[2].mySuit) && (cards[2].mySuit == cards[3].mySuit)
                && (cards[3].mySuit == cards[4].mySuit) && (cards[4].mySuit == cards[5].mySuit)))
            {
                handValue.Total = (int)cards[5].myValue;
                return true;
            }
            else if (
                cards[0].myValue + 1 == cards[1].myValue &&
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue&&
                ((cards[0].mySuit == cards[1].mySuit) && (cards[1].mySuit == cards[2].mySuit)
                && (cards[2].mySuit == cards[3].mySuit) && (cards[3].mySuit == cards[4].mySuit)))
            {
                handValue.Total = (int)cards[4].myValue;
                return true;
            }


            return false;
        }


        private bool FourOfKind()
        {
            //if the first 4 cards, add values of the four cards and last card is the highest
            if (cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue && cards[3].myValue == cards[6].myValue)
            {
                handValue.Total = (int)cards[1].myValue * 4;
                return true;
            }
            else if (cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue && cards[2].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[2].myValue * 4;
                return true;
            }
            else if (cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue && cards[1].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[1].myValue * 4;
                return true;
            }
            else if (cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue && cards[0].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[0].myValue * 4;
                return true;
            }

            return false;
        }

        private bool FullHouse()
        {
            //the first three cards and last two cards are of the same value
            //first two cards, and last three cards are of the same value
            if
                ((cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue && cards[5].myValue == cards[6].myValue) ||
                (cards[2].myValue == cards[3].myValue && cards[4].myValue == cards[5].myValue && cards[4].myValue == cards[6].myValue))
            {
                handValue.Total = (int)(cards[2].myValue) + (int)(cards[3].myValue) + (int)(cards[4].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[6].myValue);
                return true;
            }
            else if
                ((cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue && cards[4].myValue == cards[5].myValue) ||
                (cards[1].myValue == cards[2].myValue && cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[2].myValue) + (int)(cards[3].myValue) +
                    (int)(cards[4].myValue) + (int)(cards[5].myValue);
                return true;
            }
            else if ((cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue && cards[5].myValue == cards[6].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[2].myValue) + (int)(cards[3].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[6].myValue);
                return true;
            }
            else if ((cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue && cards[3].myValue == cards[4].myValue) ||
                (cards[0].myValue == cards[1].myValue && cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue))
            {
                handValue.Total = (int)(cards[0].myValue) + (int)(cards[1].myValue) + (int)(cards[2].myValue) +
                    (int)(cards[3].myValue) + (int)(cards[4].myValue);
                return true;
            }
            else if ((cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue && cards[4].myValue == cards[5].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[2].myValue) + (int)(cards[3].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[0].myValue);
                return true;
            }
            else if ((cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue && cards[5].myValue == cards[6].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[2].myValue) + (int)(cards[6].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[0].myValue);
                return true;
            }
            else if ((cards[0].myValue == cards[1].myValue && cards[4].myValue == cards[5].myValue && cards[4].myValue == cards[6].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[4].myValue) + (int)(cards[6].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[0].myValue);
                return true;
            }
            else if ((cards[2].myValue == cards[1].myValue && cards[4].myValue == cards[5].myValue && cards[4].myValue == cards[6].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[4].myValue) + (int)(cards[6].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[2].myValue);
                return true;
            }
            else if ((cards[0].myValue == cards[1].myValue && cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue))
            {
                handValue.Total = (int)(cards[1].myValue) + (int)(cards[4].myValue) + (int)(cards[6].myValue) +
                    (int)(cards[5].myValue) + (int)(cards[0].myValue);
                return true;
            }

            return false;
        }

        private bool Flush()
        {
            //if all suits are the same
            if (heartsSum == 5 || diamondSum == 5 || clubSum == 5 || spadesSum == 5)
            {
                //if flush, the player with higher cards win
                //whoever has the last card the highest, has automatically all the cards total higher
                handValue.Total = (int)cards[6].myValue;
                return true;
            }

            return false;
        }

        private bool Straight()
        {
            //if 5 consecutive values
            if (cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                cards[4].myValue + 1 == cards[5].myValue &&
                cards[5].myValue + 1 == cards[6].myValue)
            {
                //player with the highest value of the last card wins
                handValue.Total = (int)cards[6].myValue;
                return true;
            }

            else if (
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                cards[4].myValue + 1 == cards[5].myValue)
            {
                handValue.Total = (int)cards[5].myValue;
                return true;
            }
            else if (
                cards[0].myValue + 1 == cards[1].myValue &&
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue)
            {
                //player with the highest value of the last card wins
                handValue.Total = (int)cards[4].myValue;
                return true;
            }


            return false;
        }

        private bool ThreeOfKind()
        {
            //if 3 cards are the same
            if (cards[4].myValue == cards[5].myValue && cards[4].myValue == cards[6].myValue)
            {
                handValue.Total = (int)cards[6].myValue * 3;
                return true;
            }
            else if (cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[5].myValue * 3;
                return true;
            }

            else if (cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[4].myValue * 3;
                return true;
            }
            else if (cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[3].myValue * 3;
                return true;
            }
            else if (cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[2].myValue * 3;
                return true;
            }

            return false;
        }

        private bool TwoPairs()
        {
            //[6]+[5] 1st pair and check for second pair
            if (cards[6].myValue == cards[5].myValue && cards[4].myValue == cards[3].myValue)
            {
                handValue.Total = ((int)cards[4].myValue * 2) + ((int)cards[6].myValue * 2);
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = ((int)cards[3].myValue * 2) + ((int)cards[6].myValue * 2);
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = ((int)cards[2].myValue * 2) + ((int)cards[6].myValue * 2);
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = ((int)cards[1].myValue * 2) + ((int)cards[6].myValue * 2);
                return true;
            }
            //[5]+[4] first pair and check for second pair
            else if (cards[5].myValue == cards[4].myValue && cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = ((int)cards[3].myValue * 2) + ((int)cards[5].myValue * 2);
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = ((int)cards[2].myValue * 2) + ((int)cards[5].myValue * 2);
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = ((int)cards[1].myValue * 2) + ((int)cards[5].myValue * 2);
                return true;
            }
            //[4]+[3] pair check for second pair
            else if (cards[4].myValue == cards[3].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = ((int)cards[2].myValue * 2) + ((int)cards[4].myValue * 2);
                return true;
            }
            else if (cards[4].myValue == cards[3].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = ((int)cards[1].myValue * 2) + ((int)cards[4].myValue * 2);
                return true;
            }
            //[3]+[2] pair check for second pair
            else if (cards[3].myValue == cards[2].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = ((int)cards[1].myValue * 2) + ((int)cards[3].myValue * 2);
                return true;
            }
            return false;
        }

        private bool OnePair()
        {
            //if 1,2 -> 5th card has the highest value
            //2.3
            //3,4
            //4,5 -> card #3 has the highest value
            if (cards[6].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[6].myValue * 2;
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[5].myValue * 2;
                return true;
            }
            else if (cards[4].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[4].myValue * 2;
                return true;
            }
            else if (cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[3].myValue * 2;
                return true;
            }
            else if (cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = (int)cards[2].myValue * 2;
                return true;
            }
            else if (cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue * 2;
                return true;
            }
            return false;

        }
    }
}
