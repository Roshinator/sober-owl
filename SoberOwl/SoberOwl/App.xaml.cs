using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoberOwl
{
    public partial class App : Application
    {
        public delegate void RelaunchEvent();
        public event RelaunchEvent Relaunch;
        public App ()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart ()
        {
            if (Relaunch != null)
                Relaunch();
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
            if (Relaunch != null)
                Relaunch();
        }
    }
}

