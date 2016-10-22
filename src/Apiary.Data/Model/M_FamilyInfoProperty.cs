using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
	/// <summary>
	/// Привязка значений последних параметров к семье (избыточность)
	/// </summary>
	public interface IM_FamilyInfoProperty : IEntityBase
	{
#if GUID
		Guid FamilyId { get; set; }
		Guid FamilyPropertyId { get; set; }
#else
		long FamilyId { get; set; }
		long FamilyPropertyId { get; set; }
#endif

		[MaxLength(50), Display(Name = "Последнее значение", AutoGenerateField = true, Order = 30)]
		string Value { get; set; }

		[MaxLength(255), Display(Name = "Последний коментарий", AutoGenerateField = true, Order = 40)]
		string Comment { get; set; }

	}
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Привязка значений последних параметров к семье (избыточность)
	/// </summary>
	class M_FamilyInfoProperty : EntityBase, IM_FamilyInfoProperty
	{
#if GUID
		[Association("FK_FamilyInfoProperty2Family", "FamilyId", "Family.Id")]
		public Guid FamilyId { get; set; }
		[Association("FK_FamilyInfoProperty2FamilyProperty", "", "FamilyProperty.Id")]
		public Guid FamilyPropertyId { get; set; }
#else
		[Association("FK_FamilyInfoProperty2Family", "FamilyId", "Family.Id")]
		public long FamilyId { get; set; }
		[Association("FK_FamilyInfoProperty2FamilyProperty", "", "FamilyProperty.Id")]
		public long FamilyPropertyId { get; set; }
#endif

		[MaxLength(50), Display(Name = "Последнее значение", AutoGenerateField = true, Order = 30)]
		public string Value { get; set; }

		[MaxLength(255), Display(Name = "Последний коментарий", AutoGenerateField = true, Order = 40)]
		public string Comment { get; set; }


		//public virtual Family Family { get; set; }
		//public virtual FamilyProperty FamilyProperty { get; set; }
	}
}
