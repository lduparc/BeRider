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
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Rider.Resources
{
    public class LocalizedStrings : INotifyPropertyChanged
    {
        private static readonly Rider.Resources.AppResource localizedResources = new Rider.Resources.AppResource();
        public Rider.Resources.AppResource LocalizedValue { get { return localizedResources; } }

        public LocalizedStrings() { }

        public void ChangeLanguage(String codeLang)
        {
            CultureInfo newCulture = new CultureInfo(codeLang);
            Resources.AppResource.Culture = newCulture;

            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            OnPropertyChanged("Localizedresources");
        }

        #region INotifyPropertyChanged Members

        PropertyChangedEventHandler propertyChanged;
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.propertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
