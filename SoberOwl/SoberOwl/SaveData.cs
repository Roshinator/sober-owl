using System;
using Newtonsoft.Json;
namespace SoberOwl
{
    public struct SaveData
    {
        public int Streak;
        public double MoneySaved;
        public DateTime LastClicked;
        public double Cost;
        public int HighestStreak;

        public static string GetSaveText(SaveData sd)
        {
            return JsonConvert.SerializeObject(sd);
        }

        public static SaveData GetSaveFromText(string text)
        {
            return JsonConvert.DeserializeObject<SaveData>(text);
        }
    }
}

