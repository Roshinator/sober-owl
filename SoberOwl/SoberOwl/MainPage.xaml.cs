using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SoberOwl
{
    public partial class MainPage : ContentPage
    {
        static readonly string SAVE_LOC = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"savedata.json");
        protected SaveData Data;

        public MainPage()
        {
            InitializeComponent();
            (App.Current as App).Relaunch += UpdateAll;
        }

        ~MainPage()
        {
            (App.Current as App).Relaunch -= UpdateAll;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ReloadFile();
            UpdateAll();
        }

        async void SaveMoney(System.Object sender, System.EventArgs e)
        {
            if (DateTime.Now.Date.Subtract(Data.LastClicked.Date).Days > 0)
            {
                if (DateTime.Now.Date.Subtract(Data.LastClicked.Date).Days > 1)
                {
                    Data.Streak = 0;
                    await DisplayAlert("Streak Broken", "[Nice words here]", "OK");
                }
                Data.Streak++;
                Data.HighestStreak = Math.Max(Data.HighestStreak, Data.Streak);
                Data.MoneySaved += Data.Cost;
                Data.LastClicked = DateTime.Now;
                Save();
            }
            UpdateAll();
        }

        async void UpdatePrice(System.Object sender, System.EventArgs e)
        {
            Data.Cost = await GetNewPrice();
        }

        void SetDirtyDay(System.Object sender, System.EventArgs e)
        {
            Data.LastClicked = Data.LastClicked.AddDays(-1);
            UpdateAll();
        }

        async Task<double> GetNewPrice()
        {
            double price;
            string result = "";
            while (!Double.TryParse(result, out price))
            {
                result = await DisplayPromptAsync(title: "What is the price of a nono item?", "Enter the value here", keyboard: Keyboard.Numeric);
                if (result == null)
                {
                    result = "";
                }
            }
            return price;
        }

        void UpdateAll()
        {
            UpdateMoneyNum();
            UpdateStreakNum();
            if (DateTime.Now.Date.Subtract(Data.LastClicked.Date).Days == 0)
            {
                logbutton.Text = "You have logged for today";
                logbutton.BackgroundColor = Color.FromHex("34c759");
            }
            else
            {
                logbutton.Text = "I saved me some MONEY 🤑🤑🤑";
                logbutton.BackgroundColor = Color.FromHex("007aff");
            }
        }

        void UpdateMoneyNum()
        {
            moneynum.Text = "$" + Math.Round(Data.MoneySaved, 2).ToString();
        }
        void UpdateStreakNum()
        {
            streaknum.Text = Data.Streak.ToString() + (Data.Streak != 1 ? " Days" : " Day");
            maxstreaknum.Text = Data.HighestStreak.ToString() + (Data.HighestStreak != 1 ? " Days" : " Day");
        }

        void Save()
        {
            File.WriteAllText(SAVE_LOC, SaveData.GetSaveText(Data));
        }

        async Task ReloadFile()
        {
            try
            {
                Data = SaveData.GetSaveFromText(File.ReadAllText(SAVE_LOC));
            }
            catch (FileNotFoundException)
            {
                Data = new SaveData
                {
                    MoneySaved = 0,
                    Streak = 0,
                    LastClicked = DateTime.Now.AddDays(-1),
                    Cost = await GetNewPrice(),
                    HighestStreak = 0
                };
            }
        }

        async void ClearData(System.Object sender, System.EventArgs e)
        {
            try
            {
                File.Delete(SAVE_LOC);
                await ReloadFile();
                UpdateAll();
            }
            catch (Exception) { }
        }
    }
}

