using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

using IT;

namespace Apiary.VM
{
	public interface IPropertyRecord
	{
		string Key { get; }
	}
	//public interface IPropertyRecord<TValue> : IPropertyRecord
	//{
	//	TValue Value { get; }
	//}


	public class PropertyRecord : IPropertyRecord
	{
		#region static

		protected static List<PropertyInfo> GetProperties<T>()
		{
			var res = typeof(T).GetProperties(true);
			return res
				.Where(i => i.CanWrite)
				.OrderBy(i => i.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? 10)
				.ToList();
		}

		protected static string GetCaption(MemberInfo value)
		{
			return value.GetNameFromAttributes(value.Name);
		}

		protected static IEnumerable<IPropertyRecord> GenerateItems<T>(T value)
		{
			var props = GetProperties<T>();
			return props.Select(i => GenerateItem<T>(value, i)).ToArray();
		}

		protected static IPropertyRecord GenerateItem<T>(T value, PropertyInfo pi)
		{
			var caption = GetCaption(pi);

			//var miGet = typeof()
			//var getter = test_properties.CreateGetter(value);
			//var setter = test_properties.CreateSetter(value);

			//var v = getter(value);
			//setter(value, "poh");

			var tItem = typeof(PropertyRecord<>).MakeGenericType(pi.PropertyType);
			var item = Activator.CreateInstance(tItem, new object[] { value, caption, pi });
			//item = Activator.CreateInstance(tItem, new object[] { value, caption, getter, setter });
			//var item = new PropertyRecord<MemberInfo, object>(value, caption, this.GetValue<object>, SetValue<object>);
			return (IPropertyRecord)item;
		}

		#endregion

		protected object data;

		public string Key { get; private set; }


		public PropertyRecord(object data, string caption)
		{
			this.data = data;
			this.Key = caption;
		}
	}

	[DebuggerDisplay("{Key} [{Value}]")]
	public class PropertyRecord<TValue> : PropertyRecord, IPropertyRecord
	{
		private PropertyInfo pi;

		public TValue Value
		{
			get { return (TValue)(this.pi != null ? pi.GetValue(data) : data); }
			set
			{
				if (pi == null)
					this.data = value;
				else
					this.pi.SetValue(data, value);
			}
		}

		public PropertyRecord(object data, string caption, PropertyInfo pi = null)
			: base(data, caption)
		{
			this.pi = pi;
		}
	}

	//public class PropertyRecord<T, TValue> : PropertyRecord, IPropertyRecord<TValue>
	//{
	//	private new T data;
	//	public Func<T, TValue> GetValue;
	//	public Action<T, TValue> SetValue;

	//	public TValue Value { get; set; }
	//	//{
	//	//	get { return this.GetValue(data); }
	//	//	set { this.SetValue(data, value); }
	//	//}

	//	public PropertyRecord(T data, string caption, TValue value)
	//		: base(data, caption)
	//	{
	//		this.Value = value;
	//	}

	//	public PropertyRecord(T data, string caption, Func<T, TValue> getValue, Action<T, TValue> setValue)
	//		: base(data, caption)
	//	{
	//		this.data = data;
	//		this.GetValue = getValue;
	//		this.SetValue = setValue;
	//	}
	//}
}
