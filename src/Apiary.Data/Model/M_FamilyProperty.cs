using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
    /// <summary>
    /// Характеристика семьи (самомстоятельная кострукция)
    /// </summary>
    [Description("Хар-ка семьи")]
    public interface IM_FamilyProperty : IEntityDic
    {
        long Type { get; set; }

        //public string Unit { get; set; }

        long Order { get; set; }
    }
}

namespace Apiary.Data.Model
{
    /// <summary>
    /// Характеристика семьи (самомстоятельная кострукция)
    /// </summary>
    [Description("Хар-ка семьи"), DisplayName("Параметр")]
	public class M_FamilyProperty : EntityDic, IM_FamilyProperty
	{
		[Display(Name = "Тип значения", Order = 2)]
		[EnumDataType(typeof(TypeOfParam))]
		public long Type { get; set; }

		//[DisplayName("Единицы")]
		//public string Unit { get; set; }

		[Display(Name = "Сортировка", Order = 4)]
		public long Order { get; set; }

	}
}
