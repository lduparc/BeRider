﻿#pragma checksum "C:\Users\Administrator\Documents\Visual Studio 2010\Projects\Rider\Rider\Views\FluxPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D5C0DFA71EFCFC26A33988A143C501EE"
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
using Microsoft.Phone.Shell;
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
    
    
    public partial class FluxPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleFirst;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleSecond;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton refresh;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem settings;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem about;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Rider;component/Views/FluxPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ApplicationTitleFirst = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleFirst")));
            this.ApplicationTitleSecond = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleSecond")));
            this.refresh = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("refresh")));
            this.settings = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("settings")));
            this.about = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("about")));
        }
    }
}

