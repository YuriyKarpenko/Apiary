﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using IT;
using Apiary.Data;

namespace Apiary.M
{
	[DebuggerDisplay("{Id} c:{Created} m:{Modified} h:{Hide}")]
	public abstract class M_Base : IT.NotifyPropertyChangedOnly, IEntityBase, ILog
	{
		public static T BaseToModel<T>(object value) where T : M_Base, new() 
		{
			var res = UtilsReflection.ClonePropertyTo(value, new T());
			res.HasModified = false;
			return res;
		}


#if GUID
#else
#endif
		private MemCache<string, object> cacheType = new MemCache<string, object>();

		[Display(AutoGenerateField = false, Order = -100)]
#if GUID
		public Guid Id { get; set; }
#else
		public long Id { get; set; }
#endif

		[Browsable(false)]
		public DateTime Created { get; set; }

		[Browsable(false)]
		public DateTime Modified { get; set; }

		[Browsable(true)]
		[Display(AutoGenerateField = true, Name = "Скрыть", Order = 100)]
		public bool Hide
		{
			get { return this.Get<bool>("Hide"); }
			set { this.Set("Hide", value); }
		}


		[Browsable(false)]
		public bool IsNew
		{
			get
			{
#if GUID
				return this.Id == Guid.Empty;
#else
				return this.Id == 0;
#endif
			}
		}

		[Browsable(false)]
		public bool HasModified { get; set; }


		public override string ToString()
		{
			return $"{base.ToString()} -> {Id}: c{Created} m{Modified} h{Hide}";
		}


		protected T Get<T>(string propName)
		{
			//IT.Log.Logger.ToLogFmt(null, TraceLevel.Info, null, $"({this.GetType()}.{propName})");
			try
			{
				var res = this.GetCache<T>()[propName];
				return res;
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({propName})");
			}

			return default(T);
		}

		protected void Set<T>(string propName, T value)
		{
			try
			{
				this.GetCache<T>()[propName] = value;
				this.OnPropertyChanged(propName);
				this.HasModified = true;
			}
			catch (Exception ex)
			{
				this.Error(ex, $"({propName}, {value})");
			}
		}


		private MemCache<string, T> GetCache<T>()
		{
			var cache = (MemCache<string, T>)this.cacheType[typeof(T).FullName, () => new MemCache<string, T>()];
			return cache;
		}
	}


	[DebuggerDisplay("{Id}:{Name} h:{Hide}")]
	class M_BaseDic : M_Base, Apiary.Data.IEntityDic
	{
		[Display(AutoGenerateField = true, Name = "Название", ShortName = "Название", Order = 10)]
		[MaxLength(50), StringLength(50), Required]
		public string Name
		{
			get { return this.Get<string>("Name"); }
			set { this.Set("Name", value); }
		}

	}
}
