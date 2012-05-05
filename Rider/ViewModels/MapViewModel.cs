
using System.Collections.Generic;
using Rider.Maps;
using System.Device.Location;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Rider.Utils;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Rider.Models;

namespace Rider.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public static string PinLocationChanged = "PinLocationChanged";

        private Pushpin currentLocationPin = new Pushpin();
        private Image arrowImage = new Image();
        private int defaultZoom = 15;
        private GeoCoordinate _mapCenter;
        private double _zoomLevel;
        private BaseTileSource _currentMap;
        private bool pinLocationLoaded;

        public MapViewModel()
        {
            _availableMapSources = new List<BaseTileSource> 
            {
                new Google {Name = "Google Street", MapType = GoogleType.Street},
                new Google {Name = "Google Hybrid", MapType = GoogleType.Hybrid},
                new BingRoad {Name = "Bing Road"},
                new BingAerial{ Name = "Bing Aerial"},
//                new Mapnik {Name = "OSM Mapnik"},
//                new OsmaRender {Name = "OsmaRender"},
            };

            ZoomLevel = defaultZoom;
            Messenger.Default.Register<GeoCoordinate>(this, Location.LocationService.locationChanged, newLocation => OnLocationChanged(newLocation));
        }

        public void OnLocationChanged(GeoCoordinate newLocation)
        {
            // Initialize Pin if it's not already there.
            if (!pinLocationLoaded)
            {
                // Reset default template and set image.
                currentLocationPin.Template = null;
                currentLocationPin.IsHitTestVisible = false;

                arrowImage.Source = new BitmapImage(new Uri("/Images/maps/arrow.png", UriKind.Relative));
                arrowImage.Width = 270;
                arrowImage.Height = arrowImage.Width;
                arrowImage.RenderTransformOrigin = new Point(0.5, 0.5);

                pinLocationLoaded = true;
            }

            CompositeTransform compositeTransform = new CompositeTransform();
            compositeTransform.Rotation = newLocation.Course;
            compositeTransform.TranslateX = -arrowImage.Width / 2;
            compositeTransform.TranslateY = arrowImage.Height / 2;

            arrowImage.RenderTransform = compositeTransform;
            currentLocationPin.Content = arrowImage;
            currentLocationPin.Tag = -1;

            // Update pin location.
            currentLocationPin.Location = newLocation;
            MapCenter = currentLocationPin.Location;

            Messenger.Default.Send(currentLocationPin, PinLocationChanged);
        }

        #region map properties

        public GeoCoordinate MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter == value) return;
                _mapCenter = value;
                NotifyPropertyChanged("MapCenter");
            }
        }

        public double ZoomLevel
        {
            get
            {
                return _zoomLevel;
            }
            set
            {
                if (value == _zoomLevel) return;
                if (value >= 1)
                {
                    _zoomLevel = value;
                }
                NotifyPropertyChanged("ZoomLevel");
            }
        }

        public  BaseTileSource CurrentMap
        {
            get
            {
                if (_currentMap == null && 
                    _availableMapSources != null && 
                    _availableMapSources.Count > 0)
                {
                    _currentMap = _availableMapSources[0];
                }
                return _currentMap;
            }
            set
            {
                if (value.Equals(CurrentMap)) return;
                {
                    _currentMap = value;
                }
                NotifyPropertyChanged("CurrentMap");
            }
        }

        private List<BaseTileSource> _availableMapSources;
        [DoNotSerialize]
        public List<BaseTileSource> AvailableMapSources
        {
            get
            {
                return _availableMapSources;
            }
            set
            {
                _availableMapSources = value;
                NotifyPropertyChanged("AvailableMapSources");
            }
        }

        #endregion

        #region actions

        public void ShowNextMap()
        {
            var newIdx = AvailableMapSources.IndexOf(CurrentMap) + 1;
            CurrentMap = AvailableMapSources[newIdx > AvailableMapSources.Count - 1 ? 0 : newIdx];
        }

        public void ShowPreviousMap()
        {
            var newIdx = AvailableMapSources.IndexOf(CurrentMap) - 1;
            CurrentMap = AvailableMapSources[newIdx < 0 ? AvailableMapSources.Count - 1 : newIdx];
        }

        #endregion
    }
}
