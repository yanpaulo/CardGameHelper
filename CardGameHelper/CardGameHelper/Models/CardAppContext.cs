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

        private CardAppContext()
        {   
            Task.Run(() =>
            {
                Decks = new ObservableCollection<Deck>(db.GetDecksAsync().Result);
                Cards = new ObservableCollection<Card>(db.GetCardsAsync().Result);
            }).Wait();

            SelectedDeck = Decks[0].AsCopy();
        }

        private Deck selectedDeck;

        public static CardAppContext Instance { get; private set; }
            = new CardAppContext();

        public Deck SelectedDeck
        {
            get { return selectedDeck; }
            set { selectedDeck = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Deck> Decks { get; set; }

        public ObservableCollection<Card> Cards { get; set; }


        public async Task AddCardAsync(Card c)
        {
            Cards.Add(c);
            await db.AddCardAsync(c);
        }

        public async Task UpdateDeckAsync(Deck deck)
        {
            var old = Decks.Single(d => d.Id == deck.Id);
            Decks[Decks.IndexOf(old)] = deck;

            if (old.Id == SelectedDeck.Id)
            {
                SelectedDeck = deck.AsCopy();
            }

            await db.UpdateDeckAsync(deck);
        }
    }
}
