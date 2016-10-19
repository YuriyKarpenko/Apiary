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

        [MaxLength(50), Display(Name = "Последнее значение")]
        string Value { get; set; }

        [MaxLength(255), Display(Name = "Последний коментарий")]
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
		public Guid FamilyId { get; set; }
		public Guid FamilyPropertyId { get; set; }
#else
        public long FamilyId { get; set; }
        public long FamilyPropertyId { get; set; }
#endif

        [MaxLength(50), Display(Name = "Последнее значение")]
        public string Value { get; set; }

        [MaxLength(255), Display(Name = "Последний коментарий")]
        public string Comment { get; set; }


        //public virtual Family Family { get; set; }
        //public virtual FamilyProperty FamilyProperty { get; set; }
    }
}
