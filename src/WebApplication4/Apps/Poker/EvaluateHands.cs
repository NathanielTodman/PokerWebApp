using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    // enum of winning hands from weakest to strongest
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

        //HandEvaluator constructor passing the hand to be evaluated
        public HandEvaluator(Card[] sortedHand)
        {
            heartsSum = 0;
            diamondSum = 0;
            clubSum = 0;
            spadesSum = 0;

            //determine the high cards for the hand of each player, important for if the hands match
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

            /* copy the sorted hand to an array called cards 
            (nicer naming convention than "sortedHand", I am aware arrays are passed by ref)*/
            cards = new Card[sortedHand.Length];
            Cards = sortedHand;
            handValue = new HandValue();

        }

        // property for handvalue
        public HandValue HandValues
        {
            get { return handValue; }
            set { handValue = value; }
        }

        //property for cards array
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

        //
        public Hand EvaluateHand()
        {
            //get the number of each suit on hand (for checking flush/5 cards of same suit)
            getNumberOfSuit();
            //Iterate checks through each possible hand from strongest to weakest to avoid false positive
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

            //if the hand is nothing, return HighCard as default
            
            return Hand.HighCard;

            

        
    }
    
    // Get total number of each suit in hand for flush check
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
                (cards[5].myValue + 1 == cards[6].myValue ||
                ((int)cards[2].myValue == 2 && (int)cards[6].myValue == 14)) &&
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
                ((cards[4].myValue + 1 == cards[5].myValue && cards[5].mySuit == cards[4].mySuit) ||
                ((int)cards[1].myValue == 2 && (((int)cards[5].myValue == 14 && cards[5].mySuit == cards[4].mySuit) ||
                ((int)cards[6].myValue == 14)&&cards[6].myValue == cards[1].myValue))) &&
                ((cards[1].mySuit == cards[2].mySuit) && (cards[2].mySuit == cards[3].mySuit)
                && (cards[3].mySuit == cards[4].mySuit)))
            {
                if ((int)cards[1].myValue == 2 && (int)cards[6].myValue == 14)
                {
                    handValue.Total = (int)cards[6].myValue;
                    return true;
                }
                handValue.Total = (int)cards[5].myValue;
                return true;
            }
            else if (
                cards[0].myValue + 1 == cards[1].myValue &&
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                ((cards[3].myValue + 1 == cards[4].myValue && cards[4].mySuit == cards[3].mySuit) ||
                ((int)cards[0].myValue == 2 && (((int)cards[4].myValue == 14
                && cards[4].mySuit == cards[3].mySuit) ||
                ((int)cards[5].myValue == 14 && cards[5].mySuit == cards[3].mySuit) || 
                ((int)cards[6].myValue == 14 && cards[6].mySuit == cards[3].mySuit)))) &&
                ((cards[0].mySuit == cards[1].mySuit) && (cards[1].mySuit == cards[2].mySuit)
                && (cards[2].mySuit == cards[3].mySuit)))
            {
                //if ace in the run ace value return
                if ((int)cards[0].myValue == 2 && (int)cards[5].myValue == 14)
                {
                    handValue.Total = (int)cards[5].myValue;
                    return true;
                }
                else if ((int)cards[0].myValue == 2 && (int)cards[6].myValue == 14)
                {
                    handValue.Total = (int)cards[6].myValue;
                    return true;
                }
                handValue.Total = (int)cards[4].myValue;
                return true;
            }


            return false;
        }


        private bool FourOfKind()
        {
            //if 4 cards have equal value return fourkind
            if (cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue && cards[3].myValue == cards[6].myValue)
            {
                handValue.Total = (int)cards[3].myValue;
                return true;
            }
            else if (cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue && cards[2].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[2].myValue;
                return true;
            }
            else if (cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue && cards[1].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[1].myValue;
                return true;
            }
            else if (cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue && cards[0].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[0].myValue;
                return true;
            }

            return false;
        }

        private bool FullHouse()
        {
            // check for a pair and three of a kind, wall of code! one day I will find a better way to calculate!
            // There are 12 variants for this to occur in a sorted hand! Sorry future self..
            if
                ((cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue && cards[5].myValue == cards[6].myValue) ||
                (cards[2].myValue == cards[3].myValue && cards[4].myValue == cards[5].myValue && cards[4].myValue == cards[6].myValue))
            {
                // the handValue calculation had to be more complex than purely the highest card,
                // as a three kind has precedence but if threekind is equal the pair value comes into play
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
            //Flush requires 5 cards of same suit, flush value being highest card of that suit
            /*  hearts = 0
             *  spades = 1
             *  diamonds =2 
             *  clubs = 3
             */
            int high = 0;
            if (heartsSum == 5)
            {
                // check for each card that is a heart and find the highest value of that suit
                foreach(var card in cards.Where(c=> (int)c.mySuit == 0)){
                    if ((int)card.myValue > high)
                        high = (int)card.myValue;
                }
                handValue.Total = high;
                return true;
            }
            else if(spadesSum == 5)
            {
                foreach (var card in cards.Where(c => (int)c.mySuit == 1))
                {
                    if ((int)card.myValue > high)
                        high = (int)card.myValue;
                }
                handValue.Total = high;
                return true;
            }
            else if (diamondSum == 5)
            {
                foreach (var card in cards.Where(c => (int)c.mySuit == 2))
                {
                    if ((int)card.myValue > high)
                        high = (int)card.myValue;
                }
                handValue.Total = high;
                return true;
            }
            else if (clubSum == 5)
            {
                foreach (var card in cards.Where(c => (int)c.mySuit == 3))
                {
                    if ((int)card.myValue > high)
                        high = (int)card.myValue;
                }
                handValue.Total = high;
                return true;
            }

            return false;
        }

        private bool Straight()
        {
            //if 5 consecutive values in a run (eg 2,3,4,5,6 of any suit)
            if (cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                cards[4].myValue + 1 == cards[5].myValue &&
                (cards[5].myValue + 1 == cards[6].myValue|| 
                (int)cards[2].myValue == 2 && (int)cards[6].myValue == 14))
                /*this last OR statement checks if the first card in the run is a 2 
                 * and if there is an ace at the end, since aces value is 14 it 
                 * would not be considered at the start of the run!  */
            {
                //player with the highest value of the last card wins
                handValue.Total = (int)cards[6].myValue;
                return true;
            }

            else if (
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                cards[3].myValue + 1 == cards[4].myValue &&
                (cards[4].myValue + 1 == cards[5].myValue ||
                ((int)cards[1].myValue == 2 && ((int)cards[5].myValue == 14||
                (int)cards[6].myValue == 14))))
            {
                // again the OR statements checking for aces
                if ((int)cards[1].myValue == 2 && (int)cards[6].myValue == 14)
                {
                    handValue.Total = (int)cards[6].myValue;
                    return true;
                }
                handValue.Total = (int)cards[5].myValue;
                return true;
            }
            else if (
                cards[0].myValue + 1 == cards[1].myValue &&
                cards[1].myValue + 1 == cards[2].myValue &&
                cards[2].myValue + 1 == cards[3].myValue &&
                (cards[3].myValue + 1 == cards[4].myValue ||
                ((int)cards[0].myValue == 2 && ((int)cards[5].myValue == 14 ||
                (int)cards[6].myValue == 14 || (int)cards[4].myValue == 14))))
            {
                //if ace in the run ace value return
                if ((int)cards[0].myValue == 2 && (int)cards[5].myValue == 14)
                {
                    handValue.Total = (int)cards[5].myValue;
                    return true;
                }
                else if ((int)cards[0].myValue == 2 && (int)cards[6].myValue == 14)
                {
                    handValue.Total = (int)cards[6].myValue;
                    return true;
                }

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
                handValue.Total = (int)cards[6].myValue;
                return true;
            }
            else if (cards[3].myValue == cards[4].myValue && cards[3].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[5].myValue;
                return true;
            }

            else if (cards[2].myValue == cards[3].myValue && cards[2].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[4].myValue;
                return true;
            }
            else if (cards[1].myValue == cards[2].myValue && cards[1].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[3].myValue;
                return true;
            }
            else if (cards[0].myValue == cards[1].myValue && cards[0].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[2].myValue;
                return true;
            }

            return false;
        }

        private bool TwoPairs()
        {
            //[6]+[5] 1st pair and check for second pair
            if (cards[6].myValue == cards[5].myValue && cards[4].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[4].myValue + (int)cards[6].myValue;
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[3].myValue + (int)cards[6].myValue;
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = (int)cards[2].myValue + (int)cards[6].myValue;
                return true;
            }
            else if (cards[6].myValue == cards[5].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue + (int)cards[6].myValue;
                return true;
            }
            //[5]+[4] first pair and check for second pair
            else if (cards[5].myValue == cards[4].myValue && cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[3].myValue + (int)cards[5].myValue;
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = (int)cards[2].myValue + (int)cards[5].myValue;
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue + (int)cards[5].myValue;
                return true;
            }
            //[4]+[3] pair check for second pair
            else if (cards[4].myValue == cards[3].myValue && cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = (int)cards[2].myValue + (int)cards[4].myValue;
                return true;
            }
            else if (cards[4].myValue == cards[3].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue + (int)cards[4].myValue;
                return true;
            }
            //[3]+[2] pair check for second pair
            else if (cards[3].myValue == cards[2].myValue && cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue + (int)cards[3].myValue;
                return true;
            }
            return false;
        }

        private bool OnePair()
        {
            //if pair of cards return single value, easiest to program!
            if (cards[6].myValue == cards[5].myValue)
            {
                handValue.Total = (int)cards[6].myValue;
                return true;
            }
            else if (cards[5].myValue == cards[4].myValue)
            {
                handValue.Total = (int)cards[5].myValue;
                return true;
            }
            else if (cards[4].myValue == cards[3].myValue)
            {
                handValue.Total = (int)cards[4].myValue;
                return true;
            }
            else if (cards[3].myValue == cards[2].myValue)
            {
                handValue.Total = (int)cards[3].myValue;
                return true;
            }
            else if (cards[2].myValue == cards[1].myValue)
            {
                handValue.Total = (int)cards[2].myValue;
                return true;
            }
            else if (cards[1].myValue == cards[0].myValue)
            {
                handValue.Total = (int)cards[1].myValue;
                return true;
            }
            return false;

        }
    }
}
