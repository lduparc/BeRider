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
using System.Collections.Generic;
using System.Windows.Resources;
using System.IO;
using System.ComponentModel;
using System.Threading;
using Newtonsoft.Json;
using Rider.Persistent;

namespace Rider.Location
{
    public class FakeLocationMessenger : FakeLocation
    {
        Stack<FakeGeoCoordinateObject> events;
        LocationService.Mode mode;

        public FakeLocationMessenger(IEnumerable<FakeGeoCoordinateObject> events)
        {
            this.events = new Stack<FakeGeoCoordinateObject>(events);
        }

        public FakeLocationMessenger(LocationService.Mode mode = LocationService.Mode.JsonFile)
        {
            this.events = new Stack<FakeGeoCoordinateObject>();
            this.mode = mode;
            asyncLocationWorker = new BackgroundWorker();
            asyncLocationWorker.WorkerSupportsCancellation = true;

            asyncLocationWorker.DoWork += (s, args) => { AsyncLocationWorkerCallback(s, args); };
            asyncLocationWorker.RunWorkerCompleted += (s, args) => { AsyncLocationWorkerEnded(s, args); };

            asyncLocationWorker.RunWorkerAsync();
        }

        private void AsyncLocationWorkerEnded(object s, RunWorkerCompletedEventArgs args)
        {
            //sif (asyncLocationWorker.IsBusy) asyncLocationWorker.CancelAsync();
        }

        private void ReadJsonFile(object s, DoWorkEventArgs args)
        {
            BackgroundWorker worker = s as BackgroundWorker;
            bool exit = false;

            string road = string.Format("Resources/JsonRoads/{0}.json", Configuration.FAKE_JSON_ROAD);
            using (Stream stream = Application.GetResourceStream(new Uri(road, UriKind.Relative)).Stream)
            {
                using (TextReader textReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(textReader))
                    {
                        while (jsonReader.Read() && !exit)
                        {
                            FakeGeoCoordinateObject loc = new FakeGeoCoordinateObject();
                            loc.Course = double.NaN;
                            if (jsonReader.TokenType == JsonToken.StartObject)
                            {
                                do
                                {
                                    if (worker.CancellationPending == true) {
                                        args.Cancel = true;
                                        exit = true;
                                    }
                                    if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals("lat"))
                                    {
                                        jsonReader.Read();
                                        loc.Latitude = Convert.ToDouble(jsonReader.Value);
                                    }
                                    else if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals("lng"))
                                    {
                                        jsonReader.Read();
                                        loc.Longitude = Convert.ToDouble(jsonReader.Value);
                                    }
                                    else if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals("bearing"))
                                    {
                                        jsonReader.Read();
                                        loc.Course = Convert.ToDouble(jsonReader.Value);
                                    }
                                } while (jsonReader.TokenType != JsonToken.EndObject && jsonReader.Read() && !exit);
                                if (!exit) {
                                    this.events.Push(loc);
                                    Thread.Sleep(Configuration.FAKE_LOCATION_DELAY);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AsyncLocationWorkerCallback(object s, DoWorkEventArgs args)
        {
            if (mode == LocationService.Mode.JsonFile)
            {
                ReadJsonFile(s, args);
            }
            else
            {
                // TODO : Wikansit
            }
        }

        private FakeGeoCoordinateObject Current
        {
            get
            {
                return (events.Count > 0) ? events.Pop() : null;
            }
        }

        public void Close() {
            if (asyncLocationWorker != null && asyncLocationWorker.WorkerSupportsCancellation)
                asyncLocationWorker.CancelAsync();
            this.Stop();
        }
        
        protected override void StartGetCurrentPosition()
        {
            FakeGeoCoordinateObject loc = Current;
            if (loc != null) this.UpdateLocation(loc);
        }
    }
}
