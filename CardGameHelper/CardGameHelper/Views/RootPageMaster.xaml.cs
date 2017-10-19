using CardGameHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardGameHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPageMaster : ContentPage
    {
        public ListView ListView;

        public RootPageMaster()
        {
            InitializeComponent();

            BindingContext = new RootPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class RootPageMasterViewModel
        {
            public ObservableCollection<RootPageMenuItem> MenuItems { get; set; } 
                = new ObservableCollection<RootPageMenuItem>(new[]
                    {
                        new RootPageMenuItem { Title = "Main", TargetType = typeof(AboutPage) },
                        new RootPageMenuItem { Title = "Manage Decks", TargetType = typeof(DeckEditPage) },
                        new RootPageMenuItem { Title = "About", TargetType = typeof(AboutPage) },
                    });
        }
    }
}