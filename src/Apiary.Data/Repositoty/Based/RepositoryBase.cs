using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using IT;
using IT.Data;
using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
#if EF
    public class RepositoryBase<T> : IRepository<T> where T : EntityBase, new()
	{
	#region IRepositiry<FamilyProperty> Members

#if GUID
        public T virtual Get(Guid id);	//	Refresh
#else
        public virtual T Get(long id) {
			return DB7.Using(db =>
			{
				return db.Set<T>().FirstOrDefault(x => x.Id == id);
			}, false);
        }
#endif	//	GUID
        public virtual T Create()
        {
            var res = new T();
            //this.data.Add(res);
            return res;
        }

        public virtual IEnumerable<T> List()
        {
			return DB7.Using(db =>
			{
				var en = db.Set<T>();
				return en.ToList();
			}, false);
        }

#if EF7
        public virtual int Delete(T item)
		{
			var res = DB7.Using(db => db.Remove<T>(item));
			return res.State == Microsoft.Data.Entity.EntityState.Deleted ? 1 : 0;
		}

		public virtual int Set(T item)
		{
			var needState = item.IsNew() ? Microsoft.Data.Entity.EntityState.Added : Microsoft.Data.Entity.EntityState.Modified;
			var res = DB7.Using(db => db.Update<T>(item));
			return res.State == needState ? 1 : 0;
		}
#else
        public virtual int Delete(T item)
		{
			var res = DB7.Using(db => db.Set<T>().Remove(item));
            //return (res ).State == Microsoft.Data.Entity.EntityState.Deleted ? 1 : 0;
            return 1;
		}

		public virtual int Set(T item)
		{
            //var needState = item.IsNew() ? Microsoft.Data.Entity.EntityState.Added : Microsoft.Data.Entity.EntityState.Modified;
            if (item.IsNew())
            {
                var res = DB7.Using(db => db.Set<T>().Add(item), true);
                //return res.State == needState ? 1 : 0;
                return 1;
            }
            else
                return DB7.Using(db => db.SaveChanges(), false);
		}
#endif

	#endregion
    }
    public class RepositoryBaseDic<T> : RepositoryBase<T> where T : EntityDic, new()
    {
        public virtual IEnumerable<T> List(bool all = false)
        {
            return DB7.Using(db =>
            {
                var en = db.Set<T>().Where(x => all || !x.Hide);
                return en.ToList();
            }, false);
        }
    }

#else	//	EF

	public abstract class RepositoryBase<T, I> : DataAdapterBase, IRepositoryBase<I>
		where I : IEntityBase
		where T : class, I, IEntityBase, new()
	{
		//protected List<T> data = new List<T>();
		//protected readonly PropertyInfo[] Properties = typeof(T).GetProperties();
		protected readonly string[] PropertyNames = typeof(T).GetProperties()
			.Where(i => i.CanWrite)
			.Select(i => i.Name)
			.ToArray();

		#region cmd

		protected virtual IDbCommand cmdSelect { get; set; }
		protected virtual IDbCommand cmdSelect1 { get; set; }
		protected virtual IDbCommand cmdDelete { get; set; }
		protected virtual IDbCommand cmdInsert { get; set; }
		protected virtual IDbCommand cmdUpdate { get; set; }

		#endregion

		protected RepositoryBase(IDbContext context) : base(context.Connection) { }
		protected RepositoryBase(IDbContext context, Tables table) : this(context)
		{
			this.Debug("()");
			try
			{
				this.cmdDelete = base.Cmd($"DELETE FROM [{table}] WHERE Id = @Id", CommandType.Text, "@Id");
				this.cmdInsert = this.GenerateInsertCmd(table);
				this.cmdSelect = base.Cmd($"SELECT * FROM [{table}] WHERE @All = 1 OR [Hide] = 0", CommandType.Text, "All");
				this.cmdSelect1 = base.Cmd($"SELECT * FROM [{table}] WHERE Id = @Id", CommandType.Text, "@Id");
				this.cmdUpdate = this.GenerateUpdateCmd(table);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public RepositoryBase(IDbCommand @cmdSelect, IDbCommand @cmdSelect1, IDbCommand @cmdDelete = null, IDbCommand @cmdInsert = null, IDbCommand @cmdUpdate = null)
			: base(@cmdSelect.Connection)
		{
			try
			{
				Contract.NotNull(this.cmdSelect, "cmdSelect");
				Contract.NotNull(this.cmdSelect1, "cmdSelect1");
				this.cmdDelete = @cmdDelete;
				this.cmdInsert = @cmdInsert;
				this.cmdSelect = @cmdSelect;
				this.cmdSelect1 = @cmdSelect1;
				this.cmdUpdate = @cmdUpdate;
				this.Debug("()");
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		#region IRepositiry<FamilyProperty> Members

		public virtual I Create()
		{
			this.Debug("()");
			try
			{
				var res = new T();
				//this.data.Add(res);
				return res;
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

#if GUID
        public T virtual Get(Guid id);	//	Refresh
#else
		public virtual I Get(long id)
		{
			this.Debug("()");
			try
			{
				return this.cmdSelect1 == null ? null : this.cmdSelect.ExecuteListReflection<T>().FirstOrDefault();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
#endif //GUID

		public virtual int Delete(I item)
		{
			this.Info($"({item})");
			try
			{
				return this.ExecuteCmd(cmdDelete, item);
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({item})");
				throw;
			}
		}

		public virtual IEnumerable<I> List(bool withHidden = false)
		{
			this.Debug($"({withHidden})");
			try
			{
				if (this.cmdSelect.Parameters.Contains("All"))
					this.cmdSelect.SetParameter("All", withHidden);
				var res = this.cmdSelect.ExecuteListReflection<T>();
				return res;
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({withHidden}) : {this.cmdSelect.CommandText}");
				throw;
			}
			//return null;
		}

		public virtual int Set(I item)
		{
			this.Debug($"({item})");
			try
			{
				if (item.IsNew)
					item.Created = DateTime.Now;
				item.Modified = DateTime.Now;
				return this.ExecuteCmd(item.IsNew ? this.cmdInsert : this.cmdUpdate, item);
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({item})");
				throw;
			}
		}

		public virtual int Set(IEnumerable<I> items)
		{
			try
			{
				return this.Connection.DoWorkInTran<int>(conn =>
				{
					var res = 0;
					foreach (var item in items)
					{
						res += this.Set(item);
					}
					return res;
				});
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({items})");
				throw;
			}
		}

		#endregion

		protected int ExecuteCmd(IDbCommand cmd, I item)
		{
			this.Debug($"('{cmd}', {item})");
			try
			{
				if (cmd != null)
				{
					//	fill parametres
					this.FillParametres(cmd, item);
					//	execute command
					return cmd.ExecuteNonQueryInTran();
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, $"('{cmd.CommandText}', {item})");
				throw;
			}

			return 0;
		}


		private void FillParametres(IDbCommand cmd, I item)
		{
			this.Debug($"('{cmd}', {item})");
			try
			{
				var properties = item.GetType().GetProperties();
				foreach (var p in cmd.Parameters.Cast<IDataParameter>())
				{
					var pi = properties.FirstOrDefault(i => p.ParameterName.Contains(i.Name));
					if (pi == null)
					{
						this.Warn(null, "() Missing parameter '{0}'", p.ParameterName);
					}
					else
					{
						p.Value = pi.GetValue(item, null);
					}
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, $"('{cmd}', {item})");
				throw;
			}
		}

		private IDbCommand GenerateInsertCmd(Tables table)
		{
			this.Debug($"({table})");
			try
			{
				var fields = this.PropertyNames
					.Where(i => i != "Id")
					//.Select(i => $"{i}")
					.ToArray();
				var parametres = fields
					.Select(i => $"@{i}")
					.ToArray();

				var sql = $@"
	INSERT INTO [{table}] (
		[{string.Join("],\n[", fields)}]
	) VALUES (
		{string.Join(",\n", parametres)}
	)";

				return base.Cmd(sql, CommandType.Text, parametres);
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({table})");
				throw;
			}
		}

		private IDbCommand GenerateUpdateCmd(Tables table)
		{
			this.Debug($"({table})");
			try
			{
				var fields = this.PropertyNames
					.Where(i => i != "Id")
					.Select(i => $"[{i}] = @{i}")
					.ToArray();

				var sql = $"UPDATE [{table}] SET \n\t{string.Join(",\n\t", fields)}\n WHERE Id = @Id";

				return base.Cmd(sql, CommandType.Text, this.PropertyNames);
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({table})");
				throw;
			}
		}
	}

	public class RepositoryBaseDic<T, I> : RepositoryBase<T, I>, IRepositoryDic<I>
		where I : IEntityDic, IEntityBase
		where T : class, I, IEntityDic, IEntityBase, new()
	{
		//protected RepositoryBaseDic(IDbContext context) : base(context) { }
		protected RepositoryBaseDic(IDbContext context, Tables table) : base(context, table) { }
		public RepositoryBaseDic(IDbCommand @cmdSelect, IDbCommand @cmdSelect1, IDbCommand @cmdDelete, IDbCommand @cmdInsert, IDbCommand @cmdUpdate)
			: base(@cmdSelect, @cmdSelect1, @cmdDelete, @cmdInsert, @cmdUpdate) { }

	}


#endif
}
