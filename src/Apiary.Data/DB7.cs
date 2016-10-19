using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
//using SQLite.CodeFirst.;
//using Microsoft.Data.Entity;
//using Microsoft.Data.Sqlite;
#if EF
using System.Data.Entity;
#endif

using IT;
using IT.Log;

namespace Apiary.Data
{
#if EF
    public class DB7 : DbContext, ILog
	{
		//static 

		static DB7()
		{
		}

		public static void Using(Action<DB7> act, bool isSave = true)
		{
			Logger.ToLogFmt(null, TraceLevel.Verbose, null, "()");
			try
			{
				var db = new DB7();
#if EF7
                db.Database.EnsureCreated();
#endif
                act(db);

				if (isSave)
					db.SaveChanges();
			}
			catch (Exception ex)
			{
				Logger.ToLogFmt(null, TraceLevel.Error, ex, "()");
			}
		}
		public static R Using<R>(Func<DB7, R> act, bool isSave = true)
		{
			R res = default(R);
			Using(db => { res = act(db); }, isSave);
			return res;
		}


		public DbSet<Beehive> Beehives { get; set; }
		public DbSet<Family> Family { get; set; }
		public DbSet<FamilyOperation> FamilyOperation { get; set; }
		public DbSet<FamilyProperty> FamilyProperty { get; set; }


#if EF7
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			this.Debug("()");
			try
			{
				base.OnConfiguring(optionsBuilder);

				optionsBuilder.UseSqlite(DB.Instance.Connection as DbConnection);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
#endif
	}
#endif
}
