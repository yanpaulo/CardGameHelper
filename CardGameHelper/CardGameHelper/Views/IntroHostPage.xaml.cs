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
    public partial class IntroHostPage : ContentPage
    {
        public IntroHostPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var intro = new IntroPage();
            intro.Disappearing += delegate
            {
                App.Current.MainPage = new RootPage();
            };
            await Navigation.PushModalAsync(intro);
        }
    }
}