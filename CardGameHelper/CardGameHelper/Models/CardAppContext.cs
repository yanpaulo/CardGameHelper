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
            Task.Run(() =>
            {
                decks = db.GetDecksAsync().Result;
                Cards = new ObservableCollection<Card>(db.GetCardsAsync().Result);
            }).Wait();

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
                Task.Run(() =>
                {
                    var old = selectedDeck;
                    
                    selectedDeck = value;
                    selectedDeck.IsSelected = true;
                    db.DeleteDeckAsync(old).Wait();
                    db.SaveDeckAsync(selectedDeck).Wait();

                }).Wait();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Deck> Decks { get; set; }

        public ObservableCollection<Card> Cards { get; set; }
        
        public async Task AddCardAsync(Card c)
        {
            Cards.Add(c);
            await db.AddCardAsync(c);
        }

        public async Task AddDeckAsync(Deck deck)
        {
            await db.SaveDeckAsync(deck);
            Decks.Add(deck);
        }

        public async Task UpdateDeckAsync(Deck deck)
        {
            var old = Decks.Single(d => d.Id == deck.Id);
            Decks[Decks.IndexOf(old)] = deck;

            if (old.Id == SelectedDeck.OriginalId)
            {
                SelectedDeck = deck.AsCopy();
            }

            await db.UpdateDeckAsync(deck);
        }

        public async Task RemoveDeckAsync(Deck deck)
        {
            Decks.Remove(deck);
            if (SelectedDeck.Id == deck.Id)
            {
                SelectedDeck = Decks[0];
            }

            await db.DeleteDeckAsync(deck);
        }
    }
}
