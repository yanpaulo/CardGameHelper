using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.ViewModels
{
    public class DeckListViewModel : ObservableObject
    {
        public DeckListViewModel()
        {
            Decks = new ObservableCollection<DeckListViewModelItem>();
            var items = CardAppContext.Instance.Decks.Select(d => new DeckListViewModelItem { Deck = d });
            foreach (var item in items)
            {
                Decks.Add(item);
            }
        }

        public ObservableCollection<DeckListViewModelItem> Decks { get; set; }

        public void SelectDeck(DeckListViewModelItem deck)
        {
            CardAppContext.Instance.SelectedDeck = deck.Deck.AsCopy();
            NotifyListChanges();
        }


        public void RemoveDeck(DeckListViewModelItem deck)
        {
            Decks.Remove(deck);
            
            NotifyListChanges();
            OnPropertyChanged(nameof(CanRemoveDeck));
        }

        public bool CanRemoveDeck => Decks.Count > 1;

        private void NotifyListChanges()
        {
            foreach (var item in Decks)
            {
                item.NotifyModelChanged();
            }
        }
    }
}
