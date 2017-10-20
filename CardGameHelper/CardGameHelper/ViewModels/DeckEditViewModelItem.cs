using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.ViewModels
{
    public class DeckEditViewModelItem : ObservableObject
    {
        public Deck Deck { get; set; }

        public DeckCard DeckCard { get; set; }

        public bool IsOnDeck => Deck.Cards.Contains(DeckCard);

        public void NotifyModelChanged()
        {
            OnPropertyChanged(nameof(IsOnDeck));
        }

    }
}
