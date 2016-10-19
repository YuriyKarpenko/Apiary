using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
    /// <summary>
    /// Связь между FamilyOperation и FamilyProperty + Order
    /// </summary>
    public interface IM_OperationProperty : IEntityBase
    {
#if GUID
		Guid FamilyId { get; set; }
		Guid FamilyPropertyId { get; set; }
#else
        long OperationId { get; set; }
        long FamilyPropertyId { get; set; }
#endif

        [Display(Name = "Сортировка")]
        long Order { get; set; }
    }
}

namespace Apiary.Data.Model
{
    /// <summary>
    /// Связь между FamilyOperation и FamilyProperty + Order
    /// </summary>
    public class M_OperationProperty : EntityBase, IM_OperationProperty
    {
        //public DateTime DateWrite { get; set; }
#if GUID
		public Guid FamilyId { get; set; }
		public Guid FamilyPropertyId { get; set; }
#else
        public long OperationId { get; set; }
        public long FamilyPropertyId { get; set; }
#endif

        [Display(Name = "Сортировка")]
        public long Order { get; set; }

        //public virtual M_Operation FamilyOperation { get; set; }
        //public virtual M_FamilyProperty FamilyProperty { get; set; }
    }
}
