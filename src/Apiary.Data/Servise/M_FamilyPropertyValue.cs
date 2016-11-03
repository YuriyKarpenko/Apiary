using System.ComponentModel.DataAnnotations;

using Apiary.Data.Model;

namespace Apiary.Data
{
	public interface IM_FamilyPropertyValue : IM_FamilyProperty
	{
		[MaxLength(50), Display(Name = "Последнее значение", AutoGenerateField = true, Order = 30)]
		string Value { get; set; }

		[MaxLength(255), Display(Name = "Последний коментарий", AutoGenerateField = true, Order = 40)]
		string Comment { get; set; }
	}


	class M_FamilyPropertyValue : M_FamilyProperty, IM_FamilyPropertyValue
	{
		[MaxLength(50), Display(Name = "Последнее значение", AutoGenerateField = true, Order = 30)]
		public string Value { get; set; }

		[MaxLength(255), Display(Name = "Последний коментарий", AutoGenerateField = true, Order = 40)]
		public string Comment { get; set; }

		public M_FamilyPropertyValue(IM_FamilyProperty property, IM_FamilyInfoProperty value)
		{
			IT.Contract.NotNull(property, "property");
			IT.UtilsReflection.ClonePropertyTo(property, this);
			this.Value = value?.Value;
			this.Comment = value?.Comment;
		}
	}
}
