﻿#pragma checksum "C:\Users\Administrator\documents\visual studio 2010\Projects\Rider\Rider\Views\MapPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E45CB448B3D88831F823774697A233EB"
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
using Microsoft.Phone.Controls.Maps;
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
    
    
    public partial class MapPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleFirst;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleSecond;
        
        internal System.Windows.Controls.TextBlock sep;
        
        internal System.Windows.Controls.TextBlock ApplicationTitleThird;
        
        internal Microsoft.Phone.Controls.Maps.Map Map;
        
        internal System.Windows.Controls.TextBlock textBlock;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton previous;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton locker;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton startstop;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton next;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Rider;component/Views/MapPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ApplicationTitleFirst = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleFirst")));
            this.ApplicationTitleSecond = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleSecond")));
            this.sep = ((System.Windows.Controls.TextBlock)(this.FindName("sep")));
            this.ApplicationTitleThird = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitleThird")));
            this.Map = ((Microsoft.Phone.Controls.Maps.Map)(this.FindName("Map")));
            this.textBlock = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock")));
            this.previous = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("previous")));
            this.locker = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("locker")));
            this.startstop = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("startstop")));
            this.next = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("next")));
        }
    }
}

