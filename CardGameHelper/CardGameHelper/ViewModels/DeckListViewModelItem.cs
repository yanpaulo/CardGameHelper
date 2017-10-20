using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.ViewModels
{
    public class DeckListViewModelItem : ObservableObject
    {
        public Deck Deck { get; set; }

        public bool IsSelected => CardAppContext.Instance.SelectedDeck?.Id == Deck.Id;

        public void NotifyModelChanged()
        {
            OnPropertyChanged(nameof(IsSelected));
        }
    }
}
