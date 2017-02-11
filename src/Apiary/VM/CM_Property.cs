using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Apiary.Data;

using IT;
using Apiary.M;

namespace Apiary.VM
{
	interface ICM_Property
	{
		bool IsEditMode { get; }
	}

	class CM_Property_List : ICM_Property
	{
		public bool IsEditMode { get; set; }
		public IPropertyRecord[] List { get; protected set; }


		public CM_Property_List(IPropertyRecord[] list)
		{
			this.List = list;
		}
	}

	class CM_Property_Value : ICM_Property
	{
		public bool IsEditMode { get; set; }
		public object Value { get; private set; }


		public CM_Property_Value(object value)
		{
			this.Value = value;
			//this.Value_Set(data);
		}


		//protected virtual void Value_Set(TBase value)
		//{
		//	this.Value = value;
		//}
		//protected virtual IEnumerable<IPropertyRecord> GenerateItems(TBase value)
		//{
		//	return GenerateItems<TBase>(value);
		//}
	}

	//class CM_PropertyFamilyInfo : CM_PropertyItem<IM_FamilyInfo>
	//{
	//	public CM_PropertyFamilyInfo(IM_FamilyInfo data, IPropertyRecord[] props = null) : base(data, props == null, false)
	//	{
	//		this.List = props;
	//	}

	//	protected override IEnumerable<IPropertyRecord> GenerateItems(IM_FamilyInfo value)
	//	{
	//		var res1 = GenerateItems(value.Family);
	//		var res2 = new List<IPropertyRecord>();
	//		foreach (var p in value.Properties)
	//		{
	//			res2.Add(new PropertyRecord<string>(p.Value, p.Name));
	//		}
	//		return res1.Union(res2);
	//	}
	//}
}
