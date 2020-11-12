using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    class Player : IPlayer
    {
        public string Name => name;
        private string name;

        public int Wins => wins;
        private int wins;

        public Player(string playerName, int playerWins)
        {
            name = playerName;
            wins = playerWins;
            hand = new ICard[5];
        }

        public ICard[] Hand => hand;
        private ICard[] hand;

        public HandType HandType => handType;
        private HandType handType;

        public void BeforeShowHand()
        {
            SortCards();
            handType = GetHandType();
        }

        bool SortCards()
        {
            Suite suite = hand[0].Suite;
            for (int i = 1; i < hand.Length; i++)
            {
                if (suite != hand[i].Suite)
                {
                    return false;
                }
            }
            return true;
        }

        HandType GetHandType()
        {
            bool straight = IsStraight(hand);
            bool allSameSuit = IsAllSameSuit(hand);

            if (straight && allSameSuit)
            {
                if ((int)hand[0].Rank == 10)
                {
                    return HandType.RoyalStraightFlush;
                }
                else
                    return HandType.StraightFlush;
            }

            List<int> sameCardSet1, sameCardSet2;
            FindSetsOfCardsWithSameValue(hand, out sameCardSet1, out sameCardSet2);

            if (sameCardSet1.Count == 4)
                return HandType.FourOfAKind;

            if (sameCardSet1.Count + sameCardSet2.Count == 5)
                return HandType.FullHouse;

            if (allSameSuit)
                return HandType.Flush;

            if(straight)
                return HandType.Straight;

            if (sameCardSet1.Count == 3)
                return HandType.ThreeOfAKind;

            if (sameCardSet1.Count + sameCardSet2.Count == 4)
                return HandType.TwoPairs;

            if (sameCardSet1.Count == 2)
                return HandType.Pair;

            return HandType.HighCard;
        }

        bool IsStraight(ICard[] hand)
        {
            for (int i = 0; i < 4; i++)
            {
                if (hand[i].Rank == hand[i + 1].Rank)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsAllSameSuit(ICard[] hand)
        {
            Suite suite = hand[0].Suite;
            for (int i = 1; i < hand.Length; i++)
            {
                if (suite != hand[i].Suite)
                {
                    return false;
                }
            }
            return true;
        }

        void FindSetsOfCardsWithSameValue(ICard[] pokerHand, out List<int> sameValueSet1, out List<int> sameValueSet2)
        {
            //Find sets of cards with the same value.
            int index = 0;
            sameValueSet1 = FindSetsOfCardsWithSameValue_Helper(pokerHand, ref index);
            sameValueSet2 = FindSetsOfCardsWithSameValue_Helper(pokerHand, ref index);
        }

        List<int> FindSetsOfCardsWithSameValue_Helper(ICard[] pokerHand, ref int index)
        {
            List<int> sameCardSet = new List<int>();
            for (; index < 4; index++)
            {
                int intCard = (int)pokerHand[index].Rank;
                int intNextCard = (int)pokerHand[index + 1].Rank;
                if (intCard == intNextCard)
                {
                    if (sameCardSet.Count == 0)
                        sameCardSet.Add(intCard);
                    sameCardSet.Add(intCard);
                }
                else if (sameCardSet.Count > 0)
                {
                    index++;
                    break;
                }
            }
            return sameCardSet;
        }
        
        public ICard[] Discard { set => HandAfterDiscard(value); }
        private ICard[] HandAfterDiscard(ICard[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < hand.Length; j++)
                {
                    if (value[i] == hand[j])
                    {
                        hand[j] = null;
                    }
                }
            }
            return hand;
        }

    }
}