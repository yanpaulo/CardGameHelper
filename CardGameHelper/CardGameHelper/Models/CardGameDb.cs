using CardGameHelper.DependencyServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CardGameHelper.Models
{
    public class CardGameDb
    {
        #region Singleton stuff
        private static CardGameDb instance;
        public static CardGameDb Instance
        {
            get { return instance ?? (instance = new CardGameDb()); }
            set { instance = value; }
        }
        #endregion

        private SQLiteConnection connection;

        private CardGameDb()
        {
            connection = new SQLiteConnection(DependencyService.Get<IPathService>().GetLocalPath("CardGame.db"));
            connection.CreateTables<Card, Deck, DeckCard>();
            if (connection.Table<Deck>().Count() == 0)
            {
                var deck = new Deck();
                connection.Insert(deck);
                deck = deck.AsCopy();
                deck.IsSelected = true;
                connection.Insert(deck);
            }
        }


        public int AddCard(Card card)
        {
            return connection.Insert(card);
        }

        public IEnumerable<Card> GetCards()
        {
            return connection.Table<Card>().ToList();
        }

        public  IEnumerable<Deck> GetDecks()
        {
            var decks = connection.Table<Deck>().ToList();
            var cards = GetCards();
            var deckCards = connection.Table<DeckCard>().ToList();
            foreach (var dc in deckCards)
            {
                dc.Card = cards.Single(c => c.Id == dc.CardId);
            }
            foreach (var d in decks)
            {
                d.Cards = new System.Collections.ObjectModel.ObservableCollection<DeckCard>(deckCards.Where(c => c.DeckId == d.Id));
            }

            return decks;

        }
        
        public void SaveDeck(Deck deck)
        {
            connection.Insert(deck);
        }

        public void SaveSelectedDeck(Deck deck)
        {
            var selected = connection.Table<Deck>().Where(d => d.IsSelected);
            foreach (var item in selected)
            {
                connection.Delete(item);
            }

            deck.IsSelected = true;
            connection.Insert(deck);
        }

        public  void UpdateDeck(Deck deck)
        {
            connection.Execute($"DELETE FROM DeckCard WHERE DeckId={deck.Id}");
            foreach (var deckCard in deck.Cards)
            {
                deckCard.DeckId = deck.Id;
                deckCard.CardId = deckCard.Card.Id;
            }
            connection.InsertAll(deck.Cards);
        }


        public  void DeleteDeck(Deck deck)
        {
            connection.Execute($"DELETE FROM DeckCard WHERE DeckId={deck.Id}");
            connection.Execute($"DELETE FROM Deck WHERE Id={deck.Id}");
        }
    }
}
