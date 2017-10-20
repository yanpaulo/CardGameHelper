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
        private CardGameDb db = CardGameDb.Instance;

        public DeckEditViewModel() : this(CardAppContext.Instance.SelectedDeck, false)
        { }

        public DeckEditViewModel(Deck deck, bool canPersist)
        {
            Deck = deck;
            DeckCards = Deck.Cards.ToList();
            CanPersist = canPersist;
        }
        
        public bool CanPersist { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged(); }
        }
        
        public bool CardsFound =>
            DeckCards.Count > 0;

        public Deck Deck { get; set; }

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

        public void Search()
        {
            var searchText = SearchText ?? "";
            List<DeckCard> cards = new List<DeckCard>();
            var existing =
                Deck.Cards
                    .Where(c => c.Card.Name.ToLower().Contains(searchText.ToLower()));

            cards.AddRange(existing);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var existingNames = cards.Select(c => c.Card.Name);

                var others =
                    context.Cards
                        .Where(c => !existingNames.Contains(c.Name) && c.Name.ToLower().Contains(searchText.ToLower()))
                        .Select(c => new DeckCard { Card = c });

                cards.AddRange(others); 
            }

            DeckCards = cards;
        }

        public void AddCard(DeckCard deckCard)
        {
            deckCard.Count = 1;
            Deck.Cards.Add(deckCard);
            OnPropertyChanged(nameof(CardsFound));
        }

        public async void CreateCard()
        {
            var card = new Card { Name = SearchText };
            var deckCard = new DeckCard { Card = card, Count = 1 };

            Deck.Cards.Add(deckCard);

            await context.AddCardAsync(card);
            
            Search();
        }

        public async Task SaveDeckAsync()
        {
            await context.UpdateDeckAsync(Deck);
        }
    }
}
