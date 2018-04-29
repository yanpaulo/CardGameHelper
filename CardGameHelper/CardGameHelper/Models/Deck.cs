using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.Models
{
    public class Deck : ObservableObject
    {
        public Deck()
        {
            Cards.CollectionChanged += Cards_CollectionChanged;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "Starter Deck";

        public bool IsSelected { get; set; }

        public int? OriginalId { get; set; }

        [Ignore]
        public int CardsCount => 
            Cards.Sum(c => c.Count);

        [Ignore]
        public ObservableCollection<DeckCard> Cards { get; set; } = new ObservableCollection<DeckCard>();

        public void NotifyModelChanged()
        {
            OnPropertyChanged(nameof(CardsCount));
        }

        public Deck AsCopy() =>
            new Deck
            {
                OriginalId = Id,
                Name = Name,
                Cards = new ObservableCollection<DeckCard>(Cards.Select(dc => new DeckCard { Card = dc.Card, Count = dc.Count }))
            };

        private void Card_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CardsCount));
        }

        private void Cards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        ((DeckCard)item).PropertyChanged += Card_PropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in e.OldItems)
                    {
                        ((DeckCard)item).PropertyChanged -= Card_PropertyChanged;
                    }
                    break;
                default:
                    break;
            }
            OnPropertyChanged(nameof(CardsCount));
        }
    }
}
