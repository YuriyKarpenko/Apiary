using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using IT;

namespace Apiary.M
{
	[DebuggerDisplay("{Id} c:{Created} m:{Modified} h:{Hide}")]
	public abstract class M_Base : IT.NotifyPropertyChangedOnly, Apiary.Data.IEntityBase, ILog
	{
#if GUID
#else
#endif
		private MemCache<Type, object> cacheType = new MemCache<Type, object>();

		[Display(AutoGenerateField = false, Order = -100)]
#if GUID
		public Guid Id { get; set; }
#else
		public long Id { get; set; }
#endif

		//[DataType( System.ComponentModel.DataAnnotations.DataType.)]

		[Browsable(false)]
		public DateTime Created { get; set; }

		[Browsable(false)]
		public DateTime Modified { get; set; }

		[Browsable(true)]
		[Display(AutoGenerateField = true, Name = "Сктыть", Order = 100)]
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
			var cache = (MemCache<string, T>)this.cacheType[typeof(T), () => new MemCache<string, T>()];
			return cache;
		}
	}
}
