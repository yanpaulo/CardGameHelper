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

        private void Button_Clicked(object sender, EventArgs e)
        {
            viewModel.CreateCard();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.DoSearch();
        }
    }
}