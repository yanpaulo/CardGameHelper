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

        public Card Card { get; set; }
        
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); }
        }

    }
}
