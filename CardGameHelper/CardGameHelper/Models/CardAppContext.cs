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
        private CardAppContext() { }

        private Deck selectedDeck;

        public static CardAppContext Instance { get; private set; } 
            = new CardAppContext();
        
        public Deck SelectedDeck
        {
            get { return selectedDeck ?? Decks[0]; }
            set { selectedDeck = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Deck> Decks { get; set; }
            = new ObservableCollection<Deck>()
                { new Deck() };

        public ObservableCollection<Card> Cards { get; set; }
            = new ObservableCollection<Card>();

    }
}
