using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Rider.Persistent;
using Rider.Models;
using System.Windows.Data;
using Rider.Resources;
using System.Threading;

namespace Rider.Views
{
    public partial class Settings : PhoneApplicationPage
    {
        private LocalizedStrings _resources = Application.Current.Resources["LocalizedStrings"] as LocalizedStrings;
        private bool languagePickerLoaded;
        private bool weightPickerLoaded;
        private bool unitPickerLoaded;
        private bool locationToggleLoaded;

        private string locationServiceEnabled  = AppResource.ResourceManager.GetString("SettingsLocationServiceModeEnabled");
        private string locationServiceDisabled = AppResource.ResourceManager.GetString("SettingsLocationServiceModeDisabled");

        public Settings()
        {
            InitializeComponent();

            WeightPicker.ItemsSource = UserData.WeightPickerSource;
            UnitPicker.ItemsSource = UserData.UnitPickerSource;
            LanguagePicker.ItemsSource = UserData.LanguagePickerSource;
        }

        #region locationService

        private void ToggleLocationService_Loaded(object sender, RoutedEventArgs e)
        {
            bool enabled = UserData.Get<bool>(UserData.LocationToggleKey);
            this.ToggleLocationService.IsChecked = enabled;
            ToggleLocationService.Content = enabled ? locationServiceEnabled : locationServiceDisabled;
            this.locationToggleLoaded = true;
        }

        private void ToggleLocationService_Checked(object sender, RoutedEventArgs e)
        {
            if (this.locationToggleLoaded)
            {
                UserData.Add<bool>(UserData.LocationToggleKey, true);
                ToggleLocationService.Content = locationServiceEnabled;
            }
        }

        private void ToggleLocationService_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.locationToggleLoaded)
            {
                UserData.Add<bool>(UserData.LocationToggleKey, false);
                ToggleLocationService.Content = locationServiceDisabled;
            }
        }

        #endregion

        #region weightPicker

        private void WeightPicker_Loaded(object sender, RoutedEventArgs e)
        {
            int idx = UserData.WeightPickerSource.IndexOf(UserData.Get<int>(UserData.WeightKey));
            this.WeightPicker.SelectedItem = UserData.WeightPickerSource[idx == -1 ? 0 : idx];
            this.weightPickerLoaded = true;
        }

        private void WeightPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.weightPickerLoaded)
                UserData.Add<int>(UserData.WeightKey, (int)this.WeightPicker.SelectedItem);
        }

        #endregion

        #region unitPicker

        private void UnitPicker_Loaded(object sender, RoutedEventArgs e)
        {
            this.UnitPicker.SelectedItem = UserData.Get<Speed.Unit>(UserData.UnitKey);
            this.unitPickerLoaded = true;
        }

        private void UnitPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.unitPickerLoaded)
            {
                Speed.Unit unit = (Speed.Unit)Enum.Parse(typeof(Speed.Unit), this.UnitPicker.SelectedItem.ToString(), false);
                UserData.Add<Speed.Unit>(UserData.UnitKey, unit);
            }
        }

        #endregion

        #region languagePicker

        private void LanguagePicker_Loaded(object sender, RoutedEventArgs e)
        {
            int idx = UserData.Get<int>(UserData.LanguageKey);
            this.LanguagePicker.SelectedItem = UserData.LanguagePickerSource[idx == -1 ? 0 : idx];
            this.languagePickerLoaded = true;
        }

        private void LanguagePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.languagePickerLoaded)
            {
                int idx = UserData.LanguagePickerSource.IndexOf(this.LanguagePicker.SelectedItem.ToString());
                UserData.Add<int>(UserData.LanguageKey, idx);

                if (idx == 1) _resources.ChangeLanguage("en-US");
                else _resources.ChangeLanguage("fr-FR");

                NavigationService.Navigate(new Uri(NavigationService.Source + "?Refresh=true", UriKind.Relative));
            }
        }

        #endregion

    }
}