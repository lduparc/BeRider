﻿#pragma checksum "C:\Users\Administrator\documents\visual studio 2010\Projects\Rider\Rider\Views\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FD872AC07A5632A1BCF6EE0C660C21EC"
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ProgressIndicator shellProgress;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama panorama;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleFirst;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleSecond;
        
        internal System.Windows.Controls.TextBlock distanceText;
        
        internal System.Windows.Controls.TextBlock speedText;
        
        internal System.Windows.Controls.Button sessionButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton carte;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton about;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton parametre;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Rider;component/Views/MainPage.xaml", System.UriKind.Relative));
            this.shellProgress = ((Microsoft.Phone.Shell.ProgressIndicator)(this.FindName("shellProgress")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.panorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("panorama")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitleFirst = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleFirst")));
            this.ApplicationTitleSecond = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleSecond")));
            this.distanceText = ((System.Windows.Controls.TextBlock)(this.FindName("distanceText")));
            this.speedText = ((System.Windows.Controls.TextBlock)(this.FindName("speedText")));
            this.sessionButton = ((System.Windows.Controls.Button)(this.FindName("sessionButton")));
            this.carte = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("carte")));
            this.about = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("about")));
            this.parametre = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("parametre")));
        }
    }
}

