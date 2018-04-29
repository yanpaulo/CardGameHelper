using CardGameHelper.Models;
using CardGameHelper.ViewModels;
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
    public partial class DeckEditPage : ContentPage
    {
        DeckEditViewModel viewModel;
        public DeckEditPage()
        {
            InitializeComponent();
            viewModel = BindingContext as DeckEditViewModel;
        }

        public DeckEditPage(DeckEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            if (viewModel.EditMode)
            {
                var toolbarItem = new ToolbarItem() { Text = "Save", Icon = "icon_save.png" };
                toolbarItem.Clicked += SaveToolbarItem_Click;
                ToolbarItems.Add(toolbarItem);
            }
        }

        private void CreateCardButton_Clicked(object sender, EventArgs e)
        {
            viewModel.CreateCard();
        }

        private void AddCardButton_Clicked(object sender, EventArgs e)
        {
            var deckCard = (sender as Element).BindingContext as DeckEditViewModelItem;
            viewModel.AddCard(deckCard);
        }


        private void RemoveCardButton_Clicked(object sender, EventArgs e)
        {
            var deckCard = (sender as Element).BindingContext as DeckEditViewModelItem;
            viewModel.RemoveCard(deckCard);
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            viewModel.NotifyModelChange();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Search();
        }

        private async void SaveToolbarItem_Click(object sender, EventArgs e)
        {
            viewModel.SaveDeck();
            await Navigation.PopAsync();
        }

        private async void DeckEditPage_Appearing(object sender, EventArgs e)
        {
            if (!viewModel.EditMode)
            {
                viewModel.Deck = CardAppContext.Instance.SelectedDeck;

                if (!viewModel.DeckCards.Any())
                {
                    var response = await DisplayAlert("Tip", "There are no cards on this deck. Would you like to go to the Edit page and add some cards?", "Yes", "No");
                    if (response)
                    {
                        await Navigation.PushAsync(new DeckEditPage(viewModel.AsOriginalCopy()));
                    } 
                }
                
            }
        }
    }
}