using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using GalaSoft.MvvmLight;

namespace Rider.Utils
{
    /// <summary>
    /// Some extensions method that allow serializing and deserializing
    /// a model to isolated storage
    /// </summary>
    public static class ApplicationExtensions
    {
        private static string GetDataFileName( Type t)
        {
            return string.Concat(t.Name, ".dat");
        }

        public static T RetrieveFromIsolatedStorage<T>(this Application app) where T : class
        {
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var dataFileName = GetDataFileName(typeof(T));
                if (appStorage.FileExists(dataFileName))
                {
                    using (var iss = appStorage.OpenFile(dataFileName, FileMode.Open))
                    {
                        try
                        {
                            return SilverlightSerializer.Deserialize(iss) as T;
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }
                }
            }
            return null;
        }

        public static void SaveToIsolatedStorage(this Application app, ViewModelBase model)
        {
            var dataFileName = GetDataFileName((model.GetType()));
            using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStorage.FileExists(dataFileName))
                {
                    appStorage.DeleteFile(dataFileName);
                }
                using (var iss = appStorage.CreateFile(dataFileName))
                {
                   SilverlightSerializer.Serialize(model,iss);                    
                }
            }
        }
    }
}
