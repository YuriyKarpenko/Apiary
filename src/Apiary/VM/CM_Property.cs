using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Apiary.Data;

using IT;

namespace Apiary.VM
{
	public interface IPropertyRecord
	{
		string Key { get; }
	}

	class PropertyRecord<T, TValue> : IPropertyRecord
	{
		private Func<T, string> GetCaption;
		private Func<T, TValue> GetValue;
		private Action<T, TValue> SetValue;
		private T data;

		public string Key => this.GetCaption(data);
		public TValue Value
		{
			get { return this.GetValue(data); }
			set { this.SetValue(data, value); }
		}

		public PropertyRecord(T data, Func<T, string> getCaption, Func<T, TValue> getValue, Action<T, TValue> setValue)
		{
			this.data = data;
			this.GetCaption = getCaption;
			this.GetValue = getValue;
			this.SetValue = setValue;
		}
	}



	class CM_PropertyBase<T> : IT.NotifyPropertyChangedOnly //where T : IEntityBase
	{
		public IPropertyRecord[] List { get; private set; }

		public CM_PropertyBase(string caption, List<T> data, Func<T, string> getCaption, Func<T, string> getValue, Action<T, string> setValue)
		{
			this.List = data.Select(i => new PropertyRecord<T, string>(i, getCaption, getValue, setValue)).ToArray();
		}

	}

	class CM_PropertyItem<T> 
	{
		static List<PropertyInfo> GetProperties()
		{
			var res = typeof(T).GetProperties();
			return res
				.Where(i => i.CanWrite)
				.Where(i => i.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true)
				.OrderBy(i => i.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetOrder() ?? 0)
				.ToList();
		}

		static string GetCaption(MemberInfo value)
		{
			return value.GetNameFromAttributes(value.Name);
			//var aa = null
			//	?? value.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(true)?.Name
			//	?? value.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(true)?.ShortName
			//	?? value.GetCustomAttribute<DisplayNameAttribute>(true)?.DisplayName
			//	?? value.GetCustomAttribute<DescriptionAttribute>(true)?.Description
			//	;
			//return aa ?? value.Name;
		}

		public T Value { get; private set; }

		public IPropertyRecord[] List { get; private set; }


		public CM_PropertyItem(T data) 
		{
			this.Value = data;
			var props = GetProperties();
			this.List = props.Select(i => this.GenerateItem(i)).ToArray();
		}


		private IPropertyRecord GenerateItem(MemberInfo value)
		{
			return new PropertyRecord<MemberInfo, object>(value, GetCaption, this.GetValue<object>, SetValue<object>);
		}

		private TValue GetValue<TValue>(MemberInfo value)
		{
			if (value is FieldInfo)
				return (TValue)(value as FieldInfo).GetValue(this.Value);
			if (value is PropertyInfo)
				return (TValue)(value as PropertyInfo).GetValue(this.Value);
			return default(TValue);
		}

		private void SetValue<TValue>(MemberInfo value, TValue v)
		{
			object vv;
			switch (value.MemberType)
			{
				case MemberTypes.Field:
					var vf = value as FieldInfo;
					vv = Convert.ChangeType(v, vf.FieldType);
					vf.SetValue(this.Value, vv);
					break;

				case MemberTypes.Property:
					var vp = value as PropertyInfo;
					vv = Convert.ChangeType(v, vp.PropertyType);
					vp.SetValue(this.Value, vv);
					break;
			}
		}

		//class DataItemMI : DataItem<MemberInfo, object>
		//{

		//}
	}

	class CM_PropertyFamily : CM_PropertyBase<IM_PropertyInfo>
	{
		public CM_PropertyFamily(IM_Family data) :
			base($"Свойства {data.Name}",
				DB.Instance.S_Family.Get_FamilyInfo(data).Properties,
				i => i.Name,
				i => i.Value,
				(i, v) => i.Value = v)
		{

		}
	}
}
