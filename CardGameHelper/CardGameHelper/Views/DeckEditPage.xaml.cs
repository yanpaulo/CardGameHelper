﻿using CardGameHelper.Models;
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
            if (viewModel.CanPersist)
            {
                var toolbarItem = new ToolbarItem() { Text = "Save" };
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
            var deckCard = (sender as Element).BindingContext as DeckCard;
            viewModel.AddCard(deckCard);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Search();
        }

        private async void SaveToolbarItem_Click(object sender, EventArgs e)
        {
            await viewModel.SaveDeckAsync();
            await Navigation.PopAsync();
        }
    }
}