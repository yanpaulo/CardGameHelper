using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.ViewModels
{
    public class DeckEditViewModel : ObservableObject
    {
        private string searchText;
        private IList<DeckCard> deckCards;
        private CardAppContext context = CardAppContext.Instance;

        public DeckEditViewModel() : this(CardAppContext.Instance.SelectedDeck, false)
        { }

        public DeckEditViewModel(Deck deck, bool canPersist)
        {
            Deck = deck;
            DeckCards = Deck.Cards.ToList();
            CanPersist = canPersist;
        }

        public Deck Deck { get; set; }

        public bool CanPersist { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged(); }
        }

        public IList<DeckCard> DeckCards
        {
            get { return deckCards; }
            set
            {
                deckCards = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CardsFound));
            }
        }

        public bool CardsFound => DeckCards.Count > 0;

        public void DoSearch()
        {
            var searchText = this.searchText ?? "";
            List<DeckCard> cards = new List<DeckCard>();
            var existing =
                Deck.Cards
                    .Where(c => c.Card.Name.ToLower().Contains(searchText.ToLower()));

            cards.AddRange(existing);

            var existingNames = cards.Select(c => c.Card.Name);

            var others =
                context.Cards
                    .Where(c => !existingNames.Contains(c.Name) && c.Name.ToLower().Contains(searchText.ToLower()))
                    .Select(c => new DeckCard { Card = c });

            cards.AddRange(others);

            DeckCards = cards;
        }

        public void CreateCard()
        {
            var card = new Card { Name = SearchText };
            var deckCard = new DeckCard { Card = card, Count = 1 };

            Deck.Cards.Add(deckCard);
            context.Cards.Add(card);

            DoSearch();
        }

        public void SaveDeck()
        {
            var old = context.Decks.Single(d => d.Id == Deck.Id);
            context.Decks[context.Decks.IndexOf(old)] = Deck;

            if (old.Id == context.SelectedDeck.Id)
            {
                context.SelectedDeck = Deck.AsCopy();
            }
        }
    }
}
