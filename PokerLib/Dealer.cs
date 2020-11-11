using System;
using System.Collections.Generic;
namespace Poker.Lib
{
    class Dealer
    {
        Deck deck = new Deck();
        private IPlayer[] players;
        private Player player;
        Graveyard graveyard = new Graveyard();

        public Dealer(IPlayer[] players)
        {
            this.players = players;
        }

        public void OnNewDeal()
        {
            //returncards
            //shuffle
            deck.Shuffle();
            FirstDeal();
        }

        public void FirstDeal()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < players.Length; j++)
                {
                    players[j].Hand[i] = deck.DrawTopCard();
                }
            }
        }

        public ICard GiveNewCard()
        {
            return deck.DrawTopCard();
        }
    }
}