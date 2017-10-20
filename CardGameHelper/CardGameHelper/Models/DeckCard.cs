using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.Models
{
    public class DeckCard : ObservableObject
    {
        private int count;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public Card Card { get; set; }
        
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); }
        }

        [Indexed]
        public int CardId { get; set; }

        [Indexed]
        public int DeckId { get; set; }
    }
}
