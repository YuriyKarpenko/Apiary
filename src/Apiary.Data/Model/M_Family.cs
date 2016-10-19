using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Apiary.Data
{
    [Description("Семья (Для справочника)")]
    public interface IM_Family : IEntityDic
    {
#if GUID
		Guid BeehiveId { get; set; }
#else
        long BeehiveId { get; set; }
#endif

        [Display(Name = "Дата рождения")]
        DateTime BirthDay { get; set; }

        [Display(Name = "Дата смерти")]
        DateTime? DeathDay { get; set; }

        [MaxLength(255)]
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
#if GUID
		public Guid BeehiveId { get; set; }
#else
		public long BeehiveId { get; set; }
#endif

        [Display(Name="Дата рождения")]
		public DateTime BirthDay { get; set; }

        [Display(Name="Дата смерти")]
		public DateTime? DeathDay { get; set; }

		[MaxLength(255)]
		public string Comment { get; set; }

		//public virtual M_Beehive Beehive { get; set; }
		//public virtual IEnumerable<FamilyOperations> Opetations { get; set; }
	}
}
