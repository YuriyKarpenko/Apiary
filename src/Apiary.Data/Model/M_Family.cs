using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Apiary.Data
{
	[Description("Семья (Для справочника)")]
	public interface IM_Family : IEntityDic
	{
		[Editable(false)]
		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		[Association("Beehive", "BeehiveId", "Id")]
#if GUID
		Guid BeehiveId { get; set; }
#else
		long BeehiveId { get; set; }
#endif

		[Display(Name = "Дата рождения", AutoGenerateField = true, Order = 20)]
		DateTime BirthDay { get; set; }

		[Display(Name = "Дата смерти", AutoGenerateField = true, Order = 40)]
		DateTime? DeathDay { get; set; }

		[MaxLength(255)]
		[Display(Name = "Коментарий", AutoGenerateField = true, Order = 30)]
		string Comment { get; set; }


		//IM_Beehive Beehive { get; set; }
	}
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Семья (Для справочника)
	/// </summary>
	[DisplayName("Семья"), Description("Семья (Для справочника)")]
	[DebuggerDisplay("{Name} h:{} b:{BirthDay} e:{DeathDay} ")]
	public class M_Family : EntityDic, IM_Family
	{
		[Editable(false)]
		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		[Association("Beehive", "BeehiveId", "Id")]
		[System.ComponentModel.Editor("ComboBox", "Beehives.Id")]
#if GUID
		public Guid BeehiveId { get; set; }
#else
		public long BeehiveId { get; set; }
#endif

		[Display(Name = "Дата рождения", AutoGenerateField = true, Order = 20)]
		public DateTime BirthDay { get; set; }

		[Display(Name = "Дата смерти", AutoGenerateField = true, Order = 40)]
		public DateTime? DeathDay { get; set; }

		[MaxLength(255)]
		[Display(Name = "Коментарий", AutoGenerateField = true, Order = 30)]
		public string Comment { get; set; }

		//public virtual M_Beehive Beehive { get; set; }
		//public virtual IEnumerable<FamilyOperations> Opetations { get; set; }
	}
}
