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
using System.Linq;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Resources;
using Community.CsharpSqlite.SQLiteClient;
using System.Runtime.CompilerServices;
using System.Device.Location;
using Community.CsharpSqlite;
using Rider.Utils;
using Rider.ViewModels;
using Rider.Models;
using Rider.Location;
using Microsoft.Phone.Controls.Maps;

namespace Rider.Utils
{
    public class DatabaseManager
    {
        private const string TAG = "Database";

        private static string DB_NAME = "rider";
        private static string DB_NAME_TEST = "rider_test";
        private static string DB_FINAL_EXT = "sqlite3";
        private static string DB_UNMOUNT_EXT = "db";
        private static int DB_CHUNCK = 1;

        private static string idSessionColumn;
        private static string titleSessionColumn;
        private static string detailsSessionColumn;
        private static string distanceSessionColumn;
        private static string timeSessionColumn;
        private static string averageSpeedSessionColumn;
        private static string maxSpeedSessionColumn;
        private static string kcalSessionColumn;
        private static string sportSessionColumn;
        private static string dateSessionColumn;

        private static string idSessionLocationColumn;
        private static string latLocationColumn;
        private static string lngLocationColumn;

        private SqliteConnection database = null;
        private SqliteTransaction transaction = null;
        private SqliteCommand cmd;
        private bool Computing { get; set; }
        private bool TestMode { get; set; }

        public bool IsOpen { get; private set; }

        #region Initialization

        public DatabaseManager()
            : this(DB_NAME, false) { }

        public DatabaseManager(bool test)
            : this(test ? DB_NAME_TEST : DB_NAME, test) { }

        private DatabaseManager(string dbName, bool test)
        {
            TestMode = test;
            string finalDatabaseName = string.Format("{0}.{1}", dbName, DB_FINAL_EXT);
            using (IsolatedStorageFile dataStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!dataStorage.FileExists(finalDatabaseName))
                    mountDatabase(dataStorage, finalDatabaseName);

                database = new SqliteConnection(string.Format("Version=3,uri=file:{0}", finalDatabaseName));

                idSessionColumn = string.Format("@{0}", SessionViewModel.ID_COLUMN_NAME);
                titleSessionColumn = string.Format("@{0}", SessionViewModel.TITLE_COLUMN_NAME);
                detailsSessionColumn = string.Format("@{0}", SessionViewModel.DETAILS_COLUMN_NAME);
                distanceSessionColumn = string.Format("@{0}", SessionViewModel.DISTANCE_COLUMN_NAME);
                timeSessionColumn = string.Format("@{0}", SessionViewModel.DURATION_COLUMN_NAME);
                averageSpeedSessionColumn = string.Format("@{0}", SessionViewModel.AVERAGE_SPEED_COLUMN_NAME);
                maxSpeedSessionColumn = string.Format("@{0}", SessionViewModel.MAX_SPEED_COLUMN_NAME);
                kcalSessionColumn = string.Format("@{0}", SessionViewModel.KCAL_COLUMN_NAME);
                sportSessionColumn = string.Format("@{0}", SessionViewModel.SPORT_COLUMN_NAME);
                idSessionLocationColumn = string.Format("@{0}", LocationService.SESSION_ID_COLUMN_NAME);
                latLocationColumn = string.Format("@{0}", LocationService.LAT_COLUMN_NAME);
                lngLocationColumn = string.Format("@{0}", LocationService.LNG_COLUMN_NAME);
                dateSessionColumn = string.Format("@{0}", SessionViewModel.DATE_COLUMN_NAME);

                cmd = database.CreateCommand();
                cmd.Parameters.Add(idSessionColumn, null);
                cmd.Parameters.Add(titleSessionColumn, null);
                cmd.Parameters.Add(detailsSessionColumn, null);
                cmd.Parameters.Add(distanceSessionColumn, null);
                cmd.Parameters.Add(timeSessionColumn, null);
                cmd.Parameters.Add(averageSpeedSessionColumn, null);
                cmd.Parameters.Add(maxSpeedSessionColumn, null);
                cmd.Parameters.Add(kcalSessionColumn, null);
                cmd.Parameters.Add(sportSessionColumn, null);
                cmd.Parameters.Add(idSessionLocationColumn, null);
                cmd.Parameters.Add(latLocationColumn, null);
                cmd.Parameters.Add(lngLocationColumn, null);
                cmd.Parameters.Add(dateSessionColumn, null);
            }
        }

        #endregion

        #region Using

        public void Open()
        {
            if (database != null && !IsOpen)
            {
                database.Open();
                IsOpen = true;
            }
        }

        public void Close()
        {
            if (database != null && IsOpen)
            {
                database.Close();
                // Fix for SQLite on WP7 : when closing & reopen dbfile, need to close the Filestream too
                FileStream.HandleTracker.Clear();
                database = null;
            }

            DebugUtils.Log(TAG, "DB CLOSE");
            IsOpen = false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool BeginTransaction()
        {
            if (database != null && transaction == null && IsOpen)
            {
                transaction = database.BeginTransaction();
                return transaction != null;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ValidateTransaction(bool success)
        {
            if (database != null && transaction != null && IsOpen)
            {
                try
                {
                    if (success)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
                catch (Exception e)
                {
                    DebugUtils.Log(TAG, e.Message);
                    success = false;
                }
                return success;
            }
            return false;
        }

        public bool RemoveDatabase()
        {
            Close();
            IsolatedStorageFile dataStorage = IsolatedStorageFile.GetUserStoreForApplication();
            string dbName = string.Format("{0}.{1}", TestMode ? DB_NAME_TEST : DB_NAME, DB_FINAL_EXT);
            dataStorage.DeleteFile(dbName);

            return !dataStorage.FileExists(dbName);
        }

        public bool CreateDatabase()
        {
            string dbName = string.Format("{0}.{1}", TestMode ? DB_NAME_TEST : DB_NAME, DB_FINAL_EXT);

            using (IsolatedStorageFile dataStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!dataStorage.FileExists(dbName)) mountDatabase(dataStorage, dbName);
                database = new SqliteConnection(string.Format("Version=3,uri=file:{0}", dbName));

                return dataStorage.FileExists(dbName);
            }
        }

        #endregion

        #region Actions

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool InsertWithContent(string tableName, Dictionary<string, object> content)
        {
            if (!IsOpen) Open();

            if (tableName.Equals(SessionViewModel.TABLE_NAME))
                return InsertSessionWithContent(content);
            else if (tableName.Equals(LocationService.TABLE_NAME))
                return InsertLocationsWithContent(content);
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool InsertSessionWithContent(Dictionary<string, object> content)
        {
            bool success = false;
            try
            {
                cmd.CommandText = string.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}) VALUES (@{1}, @{2}, @{3}, @{4}, @{5}, @{6}, @{7}, @{8}, @{9}, @{10});",
                    SessionViewModel.TABLE_NAME,
                    SessionViewModel.ID_COLUMN_NAME,
                    SessionViewModel.TITLE_COLUMN_NAME,
                    SessionViewModel.DETAILS_COLUMN_NAME,
                    SessionViewModel.DISTANCE_COLUMN_NAME,
                    SessionViewModel.DURATION_COLUMN_NAME,
                    SessionViewModel.AVERAGE_SPEED_COLUMN_NAME,
                    SessionViewModel.KCAL_COLUMN_NAME,
                    SessionViewModel.SPORT_COLUMN_NAME,
                    SessionViewModel.MAX_SPEED_COLUMN_NAME,
                    SessionViewModel.DATE_COLUMN_NAME
                );
                cmd.Parameters[idSessionColumn].Value = (string)content[SessionViewModel.ID_COLUMN_NAME];
                cmd.Parameters[titleSessionColumn].Value = (string)content[SessionViewModel.TITLE_COLUMN_NAME];
                cmd.Parameters[detailsSessionColumn].Value = (string)content[SessionViewModel.DETAILS_COLUMN_NAME];
                cmd.Parameters[distanceSessionColumn].Value = (double)content[SessionViewModel.DISTANCE_COLUMN_NAME];
                cmd.Parameters[timeSessionColumn].Value = (string)content[SessionViewModel.DURATION_COLUMN_NAME];
                cmd.Parameters[averageSpeedSessionColumn].Value = (double)content[SessionViewModel.AVERAGE_SPEED_COLUMN_NAME];
                cmd.Parameters[kcalSessionColumn].Value = (int)content[SessionViewModel.KCAL_COLUMN_NAME];
                cmd.Parameters[sportSessionColumn].Value = (int)content[SessionViewModel.SPORT_COLUMN_NAME];
                cmd.Parameters[maxSpeedSessionColumn].Value = (double)content[SessionViewModel.MAX_SPEED_COLUMN_NAME];
                cmd.Parameters[dateSessionColumn].Value = (string)content[SessionViewModel.DATE_COLUMN_NAME];

                success = cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception e)
            {
                DebugUtils.Log(TAG, "Exception InsertSessionWithContent : " + e.Message);
                success = false;
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool InsertLocationsWithContent(Dictionary<string, object> content)
        {
            bool success = false;
            try
            {
                cmd.CommandText = string.Format("INSERT INTO {0} ({1}, {2}, {3}) VALUES (@{1}, @{2}, @{3});",
                    LocationService.TABLE_NAME,
                    LocationService.SESSION_ID_COLUMN_NAME,
                    LocationService.LAT_COLUMN_NAME,
                    LocationService.LNG_COLUMN_NAME
                );
                cmd.Parameters[idSessionLocationColumn].Value = (string)content[LocationService.SESSION_ID_COLUMN_NAME];
                cmd.Parameters[latLocationColumn].Value = (double)content[LocationService.LAT_COLUMN_NAME];
                cmd.Parameters[lngLocationColumn].Value = (double)content[LocationService.LNG_COLUMN_NAME];

                success = cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception e)
            {
                DebugUtils.Log(TAG, "Exception InsertLocationWithContent : " + e.Message);
                success = false;
            }
            return success;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteWithIdentifier(string tableName, string id)
        {
            if (!IsOpen) Open();

            bool success = false;
            string idColumn = SessionViewModel.ID_COLUMN_NAME;
            bool isLocations = false;
            if (tableName.Equals(LocationService.TABLE_NAME))
            {
                idColumn = LocationService.SESSION_ID_COLUMN_NAME;
                isLocations = true;
            }

            try
            {
                cmd.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @{1};", tableName, idColumn);

                cmd.Parameters["@" + idColumn].Value = id;

                success = cmd.ExecuteNonQuery() >= 1;
                if (isLocations) return success;

                return success && DeleteWithIdentifier(LocationService.TABLE_NAME, id);
            }
            catch (Exception e)
            {
                DebugUtils.Log(TAG, "Exception DeleteWithIdentifier: " + e.Message);
                success = false;
            }
            return success;
        }

        #endregion

        #region Computing

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FindClosestSessions(ObservableCollection<SessionViewModel> sessions)
        {
            Computing = true;

            if (!IsOpen) Open();

            cmd.CommandText = string.Format("SELECT {0}.* FROM {0} WHERE 1;", SessionViewModel.TABLE_NAME);

            int idColumnIndex = 0;
            int titleColumnIndex = 1;
            int detailsColumnIndex = 2;
            int durationColumnIndex = 4;
            int dateColumnIndex = 5;
            int maxColumnIndex = 7;
            int kcalColumnIndex = 8;
            int sportColumnIndex = 9;

            try
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader != null && reader.Read())
                    {
                        SessionViewModel session = new SessionViewModel();
                        session.Identifer = reader.GetString(idColumnIndex);
                        session.Title = reader.GetString(titleColumnIndex);
                        session.Details = reader.GetString(detailsColumnIndex);
                        session.Distance = (double)reader[SessionViewModel.DISTANCE_COLUMN_NAME];
                        session.DurationHistory = reader.GetString(durationColumnIndex);
                        session.DateHistory = reader.GetString(dateColumnIndex);
                        session.AverageSpeedHistory = (double)reader[SessionViewModel.AVERAGE_SPEED_COLUMN_NAME];
                        session.MaxSpeed = reader.GetInt32(maxColumnIndex);
                        session.KCal = reader.GetInt32(kcalColumnIndex);
                        session.Sport = reader.GetInt32(sportColumnIndex);

                        sessions.Add(session);
                    }
                }
            }
            catch (SqliteSyntaxException e)
            {
                DebugUtils.Log(TAG, "SqliteSyntaxException e:[" + e.Message + "]");
            }
            catch (SqliteExecutionException e)
            {
                DebugUtils.Log(TAG, "SqliteExecutionException e:[" + e.Message + "]");
            }

            DebugUtils.Log(TAG, string.Format("Find {0} session in database", sessions.Count));
            Computing = false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FindClosestCoordForSessionId(string id, LocationCollection coords)
        {
            Computing = true;

            if (!IsOpen) Open();

            cmd.CommandText = string.Format("SELECT {0}.* FROM {0} WHERE {1} = {2};", LocationService.TABLE_NAME, LocationService.SESSION_ID_COLUMN_NAME, id);

            int latColumnIndex = 2;
            int lngColumnIndex = 3;

            try
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader != null && reader.Read())
                    {
                        GeoCoordinate coord = new GeoCoordinate();
                        coord.Latitude = (double)reader[latColumnIndex];
                        coord.Longitude = (double)reader[lngColumnIndex];
                        coords.Add(coord);
                    }
                }
            }
            catch (SqliteSyntaxException e)
            {
                DebugUtils.Log(TAG, "SqliteSyntaxException e:[" + e.Message + "]");
            }
            catch (SqliteExecutionException e)
            {
                DebugUtils.Log(TAG, "SqliteExecutionException e:[" + e.Message + "]");
            }

            DebugUtils.Log(TAG, string.Format("Find {0} coord in database", coords.Count));
            Computing = false;
        
        }

        #endregion

        #region Internal Constructor

        private void mountDatabase(IsolatedStorageFile dataStorage, String dbName)
        {
            IsolatedStorageFileStream destStream = new IsolatedStorageFileStream(
                dbName,
                System.IO.FileMode.OpenOrCreate,
                System.IO.FileAccess.ReadWrite,
                dataStorage
            );

            byte[] buffer = new byte[1024];


            if (TestMode) copyStream(string.Format("{0}.{1}", DB_NAME_TEST, DB_UNMOUNT_EXT), destStream, buffer);
            else
            {
                for (int count = 0; count < DB_CHUNCK; count++)
                {
                    copyStream(string.Format("{0}{1}.{2}", DB_NAME, count, DB_UNMOUNT_EXT), destStream, buffer);
                }
            }

            destStream.Flush();
            destStream.Close();
        }

        private void copyStream(string fileName, IsolatedStorageFileStream dest, byte[] buffer)
        {
            Uri uri = new Uri("Resources/Database/" + fileName, UriKind.Relative);
            StreamResourceInfo s = Application.GetResourceStream(uri);
            System.IO.Stream src = s.Stream;

            int len;
            while ((len = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, len);
            }

            src.Close();
        }

        #endregion
    }
}
