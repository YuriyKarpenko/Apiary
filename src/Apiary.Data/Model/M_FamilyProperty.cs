using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
	/// <summary>
	/// Характеристики семьи (самомстоятельная кострукция)
	/// </summary>
	[Description("Характеристики семьи")]
	public interface IM_FamilyProperty : IEntityDic
	{
		[Display(Name = "Тип значения", AutoGenerateField = true, Order = 20)]
		[EnumDataType(typeof(TypeOfParam))]
		long Type { get; set; }

		//public string Unit { get; set; }

		[Display(Name = "Сортировка", AutoGenerateField = true, Order = 40)]
		long Order { get; set; }
	}
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Характеристики семьи (самомстоятельная кострукция)
	/// </summary>
	[Description("Хар-ка семьи"), DisplayName("Параметр")]
	public class M_FamilyProperty : EntityDic, IM_FamilyProperty
	{
		[Display(Name = "Тип значения", AutoGenerateField = true, Order = 20)]
		[EnumDataType(typeof(TypeOfParam))]
		public long Type { get; set; }

		//[DisplayName("Единицы")]
		//public string Unit { get; set; }

		[Display(Name = "Сортировка", AutoGenerateField = true, Order = 40)]
		public long Order { get; set; }

	}
}
