using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.Models
{
    public class CardAppContext : ObservableObject
    {
        private CardGameDb db = CardGameDb.Instance;
        private Deck selectedDeck;

        private CardAppContext()
        {
            var decks = db.GetDecks();

            Cards = new ObservableCollection<Card>(db.GetCards());
            SelectedDeck = decks.Single(d => d.IsSelected);
            Decks = new ObservableCollection<Deck>(decks.Except(new[] { SelectedDeck }));
        }

        public static CardAppContext Instance { get; private set; }
            = new CardAppContext();

        public Deck SelectedDeck
        {
            get { return selectedDeck; }
            set
            {
                selectedDeck = value;
                db.SaveSelectedDeck(selectedDeck);

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Deck> Decks { get; set; }

        public ObservableCollection<Card> Cards { get; set; }

        public void AddCard(Card c)
        {
            db.AddCard(c);
            Cards.Add(c);
        }

        public void AddDeck(Deck deck)
        {
            db.SaveDeck(deck);
            Decks.Add(deck);
        }

        public void UpdateDeck(Deck deck)
        {
            var old = Decks.Single(d => d.Id == deck.Id);

            if (old.Id == SelectedDeck.OriginalId)
            {
                SelectedDeck = deck.AsCopy();
            }

            Decks[Decks.IndexOf(old)] = deck;
            db.UpdateDeck(deck);
        }

        public void RemoveDeck(Deck deck)
        {
            if (SelectedDeck.OriginalId == deck.Id)
            {
                SelectedDeck = Decks.First().AsCopy();
            }

            db.DeleteDeck(deck);
            Decks.Remove(deck);
        }

        public Deck ResetSelectedDeck()
        {
            return SelectedDeck = Decks.Single(d => d.Id == SelectedDeck.OriginalId).AsCopy();
        }
    }
}
