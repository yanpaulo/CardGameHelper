﻿using CardGameHelper.Models;
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
        private IList<DeckEditViewModelItem> deckCards;
        private CardAppContext context = CardAppContext.Instance;

        public DeckEditViewModel() : this(CardAppContext.Instance.SelectedDeck, false)
        { }

        public DeckEditViewModel(Deck deck, bool canPersist)
        {
            Deck = deck;
            CanPersist = canPersist;
            Search();

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

        public IList<DeckEditViewModelItem> DeckCards
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
            var cards = new List<DeckEditViewModelItem>();
            var existing =
                Deck.Cards
                    .Where(c => c.Card.Name.ToLower().Contains(searchText.ToLower()))
                    .Select(dc => new DeckEditViewModelItem { Deck = Deck, DeckCard = dc });

            cards.AddRange(existing);

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var existingNames = cards.Select(c => c.DeckCard.Card.Name).ToList();

                var others =
                    context.Cards
                        .Where(c => !existingNames.Contains(c.Name) && c.Name.ToLower().Contains(searchText.ToLower()))
                        .Select(dc =>
                            new DeckEditViewModelItem
                            {
                                Deck = Deck,
                                DeckCard = new DeckCard { Card = dc }
                            });

                cards.AddRange(others);
            }

            DeckCards = cards;
        }

        public void AddCard(DeckEditViewModelItem model)
        {
            var deckCard = model.DeckCard;
            deckCard.Count = 1;
            Deck.Cards.Add(deckCard);
            model.NotifyModelChanged();
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

        public void NotifyModelChange()
        {
            Deck.NotifyModelChanged();
        }
    }
}
