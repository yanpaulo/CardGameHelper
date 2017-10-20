using CardGameHelper.DependencyServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CardGameHelper.Models
{
    public class CardGameDb
    {
        #region Singleton stuff
        private static CardGameDb instance;
        public static CardGameDb Instance
        {
            get { return instance ?? (instance = new CardGameDb()); }
            set { instance = value; }
        } 
        #endregion

        private SQLiteAsyncConnection connection;

        private CardGameDb()
        {
            connection = new SQLiteAsyncConnection(DependencyService.Get<IPathService>().GetLocalPath("CardGame.db"));
            connection.CreateTablesAsync<Card, Deck, DeckCard>().Wait();
            if(connection.Table<Deck>().CountAsync().Result == 0)
            {
                connection.InsertAsync(new Deck());
            }
        }

        
        public async Task<int> AddCardAsync(Card card)
        {
            return await connection.InsertAsync(card);
        }

        public async Task<IEnumerable<Card>> GetCardsAsync()
        {
            return await connection.Table<Card>().ToListAsync();
        }

        public async Task<IEnumerable<Deck>> GetDecksAsync()
        {
            var decks = await connection.Table<Deck>().ToListAsync();
            var cards = await GetCardsAsync();
            var deckCards = await connection.Table<DeckCard>().ToListAsync();
            foreach (var dc in deckCards)
            {
                dc.Card = cards.Single(c => c.Id == dc.CardId);
            }
            foreach (var d in decks)
            {
                d.Cards = new System.Collections.ObjectModel.ObservableCollection<DeckCard>(deckCards.Where(c => c.DeckId == d.Id));
            }

            return decks;

        }

        public async Task SaveDeckAsync(Deck deck)
        {
            await connection.InsertAsync(deck);
        }

        public async Task UpdateDeckAsync(Deck deck)
        {
            await connection.ExecuteAsync($"DELETE FROM DeckCard WHERE DeckId={deck.Id}");
            foreach (var deckCard in deck.Cards)
            {
                if (deckCard.Card.Id == 0)
                {
                    await connection.InsertAsync(deckCard);
                }
                deckCard.DeckId = deck.Id;
                deckCard.CardId = deckCard.Card.Id;
            }
            await connection.InsertAllAsync(deck.Cards);
        }


        public async Task DeleteDeckAsync(Deck deck)
        {
            await connection.ExecuteAsync($"DELETE FROM DeckCard WHERE DeckId={deck.Id}");
            await connection.ExecuteAsync($"DELETE FROM Deck WHERE Id={deck.Id}");
        }
    }
}
