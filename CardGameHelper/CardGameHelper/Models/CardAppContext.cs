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
            IEnumerable<Deck> decks = null;
            decks = db.GetDecks();
            Cards = new ObservableCollection<Card>(db.GetCards());

            //MUST be set directly for initialization. DO NOT use setter.
            selectedDeck = decks.Single(d => d.IsSelected);
            Decks = new ObservableCollection<Deck>(decks.Except(new[] { SelectedDeck }));
        }

        public static CardAppContext Instance { get; private set; }
            = new CardAppContext();

        public Deck SelectedDeck
        {
            get { return selectedDeck; }
            set
            {
                var old = selectedDeck;

                selectedDeck = value;
                selectedDeck.IsSelected = true;
                db.DeleteDeck(old);
                db.SaveDeck(selectedDeck);

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Deck> Decks { get; set; }

        public ObservableCollection<Card> Cards { get; set; }

        public void AddCard(Card c)
        {
            Cards.Add(c);
            db.AddCard(c);
        }

        public void AddDeck(Deck deck)
        {
            db.SaveDeck(deck);
            Decks.Add(deck);
        }

        public void UpdateDeck(Deck deck)
        {
            var old = Decks.Single(d => d.Id == deck.Id);
            Decks[Decks.IndexOf(old)] = deck;

            if (old.Id == SelectedDeck.OriginalId)
            {
                SelectedDeck = deck.AsCopy();
            }

            db.UpdateDeck(deck);
        }

        public void RemoveDeck(Deck deck)
        {
            Decks.Remove(deck);
            if (SelectedDeck.Id == deck.Id)
            {
                SelectedDeck = Decks[0];
            }

            db.DeleteDeck(deck);
        }
    }
}
