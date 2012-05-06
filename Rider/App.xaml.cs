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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Rider.ViewModels;
using System.Threading;
using System.Globalization;
using Rider.Persistent;
using Rider.Utils;

namespace Rider
{
    public partial class App : Application
    {
        public PhoneApplicationFrame RootFrame { get; private set; }
        public static DatabaseManager database;

        public App()
        {
            // Gestionnaire global pour les exceptions non interceptées. 
            UnhandledException += Application_UnhandledException;

            // Initialisation Silverlight standard
            InitializeComponent();

            // Initialisation spécifique au téléphone
            InitializePhoneApplication();

            // Affichez des informations de profilage graphique lors du débogage.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Affichez les compteurs de fréquence des trames actuels.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Affichez les zones de l'application qui sont redessinées dans chaque frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Activez le mode de visualisation d'analyse hors production, 
                // qui montre les zones d'une page sur lesquelles une accélération GPU est produite avec une superposition colorée.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Désactivez la détection d'inactivité de l'application en définissant la propriété UserIdleDetectionMode de l'objet
                // PhoneApplicationService de l'application sur Désactivé.
                // Attention :- À utiliser uniquement en mode de débogage. Les applications qui désactivent la détection d'inactivité de l'utilisateur continueront de s'exécuter
                // et seront alimentées par la batterie lorsque l'utilisateur ne se sert pas du téléphone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            int idx = UserData.Get<int>(UserData.LanguageKey);
            string culture = "fr-FR";
            if (idx == 1) culture = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (UserData.Get<bool>(UserData.LocationToggleKey))
                ViewModelController.StartLocationService();

            if (database == null)
                database = new DatabaseManager();
            database.Open();
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (UserData.Get<bool>(UserData.LocationToggleKey))
                ViewModelController.StartLocationService();
            if (database == null)
                database = new DatabaseManager();
            database.Open();
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            ViewModelController.StopLocationService();
            if (database != null)
            {
                database.Close();
                database = null;
            }
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            ViewModelController.StopLocationService();
            if (database != null)
            {
                database.Close();
                database = null;
            }
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Échec d'une navigation ; arrêt dans le débogueur
                System.Diagnostics.Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Une exception non gérée s'est produite ; arrêt dans le débogueur
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Initialisation de l'application téléphonique

        // Éviter l'initialisation double
        private bool phoneApplicationInitialized = false;

        // Ne pas ajouter de code supplémentaire à cette méthode
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Créez le frame, mais ne le définissez pas encore comme RootVisual ; cela permet à l'écran de
            // démarrage de rester actif jusqu'à ce que l'application soit prête pour le rendu.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Gérer les erreurs de navigation
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Garantir de ne pas retenter l'initialisation
            phoneApplicationInitialized = true;
        }

        // Ne pas ajouter de code supplémentaire à cette méthode
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Définir le Visual racine pour permettre à l'application d'effectuer le rendu
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Supprimer ce gestionnaire, puisqu'il est devenu inutile
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

    }
}