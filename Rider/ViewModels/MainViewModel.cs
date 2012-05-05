using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Rider;
using Rider.Persistent;
using System.IO;
using System.Xml;
using System.Net;


namespace Rider.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<ItemViewModel> Home { get; private set; }
        public ObservableCollection<ItemViewModel> News { get; private set; }
        public ObservableCollection<ItemViewModel> Maps { get; private set; }
        public ObservableCollection<ItemViewModel> Gallery { get; private set; }
        private string _panoramaHomeTitle = "";
        private string _panoramaNewsTitle = "";
        private string _panoramaMapsTitle = "";
        private string _panoramaGalleryTitle = "";

        public MainViewModel()
        {
            this.Home = new ObservableCollection<ItemViewModel>();
            this.News = new ObservableCollection<ItemViewModel>();
            this.Maps = new ObservableCollection<ItemViewModel>();
            this.Gallery = new ObservableCollection<ItemViewModel>();
        }

        #region properties

        public string PanoramaHomeTitle
        {
            get
            {
                return _panoramaHomeTitle;
            }
            set
            {
                if (value != _panoramaHomeTitle)
                {
                    _panoramaHomeTitle = value;
                    NotifyPropertyChanged("PanoramaHomeTitle");
                }
            }
        }

        public string PanoramaNewsTitle
        {
            get
            {
                return _panoramaNewsTitle;
            }
            set
            {
                if (value != _panoramaNewsTitle)
                {
                    _panoramaNewsTitle = value;
                    NotifyPropertyChanged("PanoramaNewsTitle");
                }
            }
        }

        public string PanoramaMapsTitle
        {
            get
            {
                return _panoramaMapsTitle;
            }
            set
            {
                if (value != _panoramaMapsTitle)
                {
                    _panoramaMapsTitle = value;
                    NotifyPropertyChanged("PanoramaMapsTitle");
                }
            }
        }

        public string PanoramaGalleryTitle
        {
            get
            {
                return _panoramaGalleryTitle;
            }
            set
            {
                if (value != _panoramaGalleryTitle)
                {
                    _panoramaGalleryTitle = value;
                    NotifyPropertyChanged("PanoramaGalleryTitle");
                }
            }
        }

        public bool IsDataLoaded { get; private set; }

        #endregion

        public void LoadData()
        {
            #region home

            this.PanoramaHomeTitle = "Seance";

            this.Home.Add(new ItemViewModel()
            {
                Title = "A la une",
                Identifier = 1,
                Details = "Toute l'info a portee de main"
            });

            this.Home.Add(new ItemViewModel()
            {
                Title = "On the road",
                Identifier = 2,
                Details = "Circuits et evenements en temps reel"
            });

            this.Home.Add(new ItemViewModel()
            {
                Title = "L'instant T",
                Identifier = 3,
                Details = "Galerie photos pour ne rien oublier !"
            });

            #endregion

            #region news

            this.PanoramaNewsTitle = "A la une";

            this.News.Add(new ItemViewModel()
            {
                Title = "All in one",
                Identifier = 0,
                SmartInfo = "Toute l'actu !",
                Link = "/Views/FluxPage.xaml",
                Details = "Ajouter, supprimer vos sources Facebook, Twitter et Rss. Vous ne connaissez pas d'adresse ? Faites votre choix parmis notre selection !"
            });

            this.News.Add(new ItemViewModel()
            {
                Title = "Favoris",
                Identifier = 1,
                SmartInfo = "",
                Link = "/Views/ManageFavoriesFluxPage.xaml",
                Details = "Ajouter, supprimer des sources de vos favories."
            });

            List<RssViewModel> fav = UserData.Get<List<RssViewModel>>("news_favory");
            int newsfavoryCount = fav == null ? 0 : fav.Count;
            this.News[1].SmartInfo = string.Format("{0} {1}", newsfavoryCount, "favoris");

            #endregion

            #region maps

            this.PanoramaMapsTitle = "On the road";

            this.Maps.Add(new ItemViewModel()
            {
                Title = "On Live",
                SmartInfo = "Bientot disponible",
                Link = "/Views/MapPage.xaml",
                Details = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur"
            });

            this.Maps.Add(new ItemViewModel()
            {
                Title = "Cartes",
                Link = "",
                SmartInfo = "Bientot disponible",
                Details = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur"
            });

            #endregion

            #region gallery

            this.PanoramaGalleryTitle = "L'instant T";

            this.Gallery.Add(new ItemViewModel()
            {
                Title = "Galerie",
                Link = "",
                SmartInfo = "Bientot disponible",
                Details = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur"
            });

            this.Gallery.Add(new ItemViewModel()
            {
                Title = "Capturer",
                Link = "",
                SmartInfo = "Bientot disponible",
                Details = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur"
            });

            #endregion

            this.IsDataLoaded = true;
        }

    }
}