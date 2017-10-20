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
        }

        
        public async Task<int> AddCardAsync(Card card)
        {
            return await connection.InsertAsync(card);
        }

        public async Task<IEnumerable<Card>> GetCardsAsync()
        {
            return await connection.Table<Card>().ToListAsync();
        }
        



    }
}
