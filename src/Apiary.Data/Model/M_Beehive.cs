using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
	/// <summary>
	/// Улей
	/// </summary>
	[Description("Улей")]
	public interface IM_Beehive : IEntityDic
    {
		[MaxLength(50), Display(Name = "Адрес", Order = 2)]
		string Address { get; set; }
		[MaxLength(255), Display(Name = "Коментарий", Order = 3)]
		string Comment { get; set; }
    }
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Улей
	/// </summary>
	[Description("Улей"), DisplayName("Улей")]
	public class M_Beehive : EntityDic, IM_Beehive
	{
		[MaxLength(50), Display(Name = "Адрес", Order = 2)]
		public string Address { get; set; }
		[MaxLength(255), Display(Name = "Коментарий", Order = 3)]
		public string Comment { get; set; }

#if EF
#endif
        //public virtual IEnumerable<IFamily> Families { get; set; }
	}
}
