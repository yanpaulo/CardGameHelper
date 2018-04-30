using CardGameHelper.Models;
using CardGameHelper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CardGameHelper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Properties.ContainsKey("FirstUse"))
            {
                MainPage = new RootPage();
            }
            else
            {
                Properties.Add("FirstUse", false);
                MainPage = new IntroHostPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            CardGameDb.Instance.UpdateDeck(CardAppContext.Instance.SelectedDeck);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
