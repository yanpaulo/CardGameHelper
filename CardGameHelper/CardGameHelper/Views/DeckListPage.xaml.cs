using CardGameHelper.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardGameHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeckListPage : ContentPage
    {
        private DeckListViewModel viewModel;

        public DeckListPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            BindingContext = viewModel = new DeckListViewModel();
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var item = e.Item as DeckListViewModelItem;

            var editViewModel = new DeckEditViewModel(item.Deck.AsCopy(), true);
            await Navigation.PushAsync(new DeckEditPage(editViewModel));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void SelectDeckButton_Clicked(object sender, EventArgs e)
        {
            var item =
                (sender as Button).BindingContext as DeckListViewModelItem;
            viewModel.SelectDeck(item);
        }

        private async void RemoveDeckButton_Clicked(object sender, EventArgs e)
        {
            var item =
                (sender as Button).BindingContext as DeckListViewModelItem;
            await viewModel.RemoveDeckAsync(item);
        }

        private async void CreateToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeckCreatePage());
        }
    }
}
