﻿#pragma checksum "C:\Users\Administrator\documents\visual studio 2010\Projects\Rider\Rider\Views\Settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "60C7CB18235B4B666FD294B44F9D0CBE"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.225
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Rider.Views {
    
    
    public partial class Settings : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleFirst;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleSecond;
        
        internal System.Windows.Controls.TextBlock sep;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleThird;
        
        internal Microsoft.Phone.Controls.ToggleSwitch ToggleLocationService;
        
        internal Microsoft.Phone.Controls.ListPicker WeightPicker;
        
        internal Microsoft.Phone.Controls.ListPicker UnitPicker;
        
        internal Microsoft.Phone.Controls.ListPicker LanguagePicker;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Rider;component/Views/Settings.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ApplicationTitleFirst = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleFirst")));
            this.ApplicationTitleSecond = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleSecond")));
            this.sep = ((System.Windows.Controls.TextBlock)(this.FindName("sep")));
            this.ApplicationTitleThird = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleThird")));
            this.ToggleLocationService = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("ToggleLocationService")));
            this.WeightPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("WeightPicker")));
            this.UnitPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("UnitPicker")));
            this.LanguagePicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("LanguagePicker")));
        }
    }
}
