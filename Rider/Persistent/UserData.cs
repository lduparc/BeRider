using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Rider.Models;

namespace Rider.Persistent
{
    public static class UserData
    {
        private static readonly IsolatedStorageSettings isolatedStorage = IsolatedStorageSettings.ApplicationSettings;

        public static List<int> WeightPickerSource = new List<int> { 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150 };
        public static List<Speed.Unit> UnitPickerSource = new List<Speed.Unit> { Speed.Unit.Km, Speed.Unit.Mi };
        public static List<string> LanguagePickerSource = new List<string>() { "Français", "English" };

        public static readonly string LanguageKey = "LanguageKey";
        public static readonly string UnitKey = "UnitKey";
        public static readonly string WeightKey = "WeightKey";
        public static readonly string LocationToggleKey = "LocationKey";
        public static readonly string UnitChanged = "OnUnitChanged";
        public static readonly string ListSessionKey = "ListSessionKey";
        public static readonly string SessionIndexKey = "SessonIndexKey";
        public static readonly string AlreadyLaunchedKey = "AlreadyLaunchedKey";
        public static readonly string ShowWizardKey = "ShowWizardKey";
        public static readonly string SessionSelectedIndexKey = "SessionSelectedIndexKey";

        public static void Add<TEntity>(string key, TEntity entity)
        {
            if (isolatedStorage.Contains(key))
            {
                isolatedStorage[key] = entity;
                return;
            }

            isolatedStorage.Add(key, entity);
        }

        public static TEntity Get<TEntity>(string key)
        {
            if (isolatedStorage.Contains(key))
                return (TEntity)isolatedStorage[key];

            return default(TEntity);
        }

        public static void Remove(string key)
        {
            if (isolatedStorage.Contains(key))
                isolatedStorage.Remove(key);
        }

    }
}
