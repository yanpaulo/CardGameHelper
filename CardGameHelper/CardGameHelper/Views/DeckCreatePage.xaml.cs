using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardGameHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeckCreatePage : ContentPage
    {
        public DeckCreatePage()
        {
            InitializeComponent();
        }

        private void SaveToolbarItem_Clicked(object sender, EventArgs e)
        {
            var name = NameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                DisplayAlert("Error", "Please anter a name for the Deck.", "Ok");
                return;
            }
            if (CardAppContext.Instance.Decks.Any(d => d.Name == name))
            {
                DisplayAlert("Error", "There is already an existing Deck with this name.", "Ok");
                return;
            }

            var deck = new Deck { Name = NameTextBox.Text };
            CardAppContext.Instance.AddDeck(deck);
            Navigation.PopAsync();
        }
    }
}