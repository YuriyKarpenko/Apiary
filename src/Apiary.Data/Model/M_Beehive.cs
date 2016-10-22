using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
	/// <summary>
	/// Улей
	/// </summary>
	[Description("Ульи")]
	public interface IM_Beehive : IEntityDic
    {
		[MaxLength(50), Display(Name = "Адрес", Order = 20)]
		string Address { get; set; }
		[MaxLength(255), Display(Name = "Коментарий", Order = 30)]
		string Comment { get; set; }
    }
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Улей
	/// </summary>
	[Description("Ульи"), DisplayName("Ульи")]
	public class M_Beehive : EntityDic, IM_Beehive
	{
		[MaxLength(50), Display(Name = "Адрес", Order = 20)]
		public string Address { get; set; }
		[MaxLength(255), Display(Name = "Коментарий", Order = 30)]
		public string Comment { get; set; }

#if EF
#endif
        //public virtual IEnumerable<IFamily> Families { get; set; }
	}
}
