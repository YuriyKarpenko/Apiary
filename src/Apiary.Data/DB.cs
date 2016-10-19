//#define EF7
//#undef EF7

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
#if EF
using Microsoft.Data.Sqlite;
#else
#endif
using System.Data.SQLite;

using IT;
using IT.Data;
using Apiary.Data.Servise;

namespace Apiary.Data
{
#if EF7
#else
#endif

    public class DB : ILog
    {
        public static readonly DB Instance;
		static DB()
		{
			Instance = new DB();
		}

        public static string GenerateConnectionString()
        {
            var csb = new SQLiteConnectionStringBuilder("Data Source=|DataDirectory|db.sqlite");

            //csb.BaseSchemaName = "";
            csb.BinaryGUID = true;
            //csb.BusyTimeout = ;
            //csb.CacheSize = ;
            //csb.DataSource = ;
            csb.DateTimeFormat = SQLiteDateFormats.ISO8601;
            //csb.DateTimeFormatString = "";
            //csb.DateTimeKind = DateTimeKind.Local;
            //csb.DefaultDbType = System.Data.DbType.AnsiString;
            //csb.DefaultIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            //csb.DefaultTimeout = ;
            //csb.DefaultTypeName = ;
            //csb.Enlist = ;
            csb.FailIfMissing = false;
            //csb.Flags = SQLiteConnectionFlags.Default;
            //csb.ForeignKeys = true;
            //csb.FullUri = ;
            //csb.HexPassword = ;
            //csb.IsFixedSize = ;
            //csb.IsReadOnly = ;
            csb.JournalMode = SQLiteJournalModeEnum.Persist;
            //csb.Keys = ;
            //csb.LegacyFormat = ;
            //csb.MaxPageCount = ;
            //csb.NoDefaultFlags = ;
            //csb.NoSharedFlags = ;
            //csb.PageSize = ;
            //csb.Password = ;
            csb.Pooling = false;
            //csb.PrepareRetries = ;
            //csb.ProgressOps = ;
            //csb.ReadOnly = ;
            //csb.RecursiveTriggers = ;
            //csb.SetDefaults = ;
            csb.SyncMode = SynchronizationModes.Normal;
            //csb.ToFullPath = ;
            //csb.Uri = ;
            csb.UseUTF16Encoding = false;
            //csb.Version = ;
            //csb.VfsName = ;
            //csb.ZipVfsVersion = ;

            return csb.ConnectionString;
        }


        private IDbContext context;
        //  services


        public IDbConnection Connection => context.Connection;
        public IS_Dictionary S_Dictionary { get; private set; }
        public IS_Family S_Family { get; private set; }

#if EF
#else
        public void CreateDb()
        {
            this.Debug("()");
            try
            {
                //System.Data.SQLite.EF6.SQLiteProviderFactory
                this.Connection.OpenIfClosed();

                var scripts = Apiary.Data.Properties.Resources.CreateDB.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                var cmd = this.Connection.CreateCommand();
                foreach (var sql in scripts)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                this.Error(ex, "()");
                throw;
            }
            finally
            {
                this.Connection.CloseIfNotClosed();
            }
        }
#endif

        public DB()
        {
            this.context = new DbContext(new SQLiteConnection(GenerateConnectionString()));
            this.S_Dictionary = new S_Dictionary(this.context);
            this.S_Family = new S_Family(this.context);
        }

    }
}
