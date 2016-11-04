using System;
using System.ComponentModel.DataAnnotations;

namespace Apiary.Data
{
    /// <summary>
    /// Запись для значения параметра операции с семьей (журнал)
    /// </summary>
    public interface IM_FamilyOperation : IEntityBase
    {
		DateTime DateWrite { get; set; }

#if GUID
		Guid FamilyId { get; set; }
		Guid FamilyOperationId { get; set; }
		Guid FamilyPropertyId { get; set; }
#else
        long FamilyId { get; set; }
		long OperationId { get; set; }
        long FamilyPropertyId { get; set; }
#endif

        string Value { get; set; }

        string Comment { get; set; }

    }
}

    namespace Apiary.Data.Model
{
    /// <summary>
    /// Запись для значения параметра операции с семьей
    /// </summary>
    public class M_FamilyOperation : EntityBase, IM_FamilyOperation
	{
        [Display(Name = "Дата")]
		public DateTime DateWrite { get; set; }

#if GUID
		public Guid FamilyId { get; set; }
		public Guid FamilyOperationId { get; set; }
		public Guid FamilyPropertyId { get; set; }
#else
        public long FamilyId { get; set; }
		public long OperationId { get; set; }
        public long FamilyPropertyId { get; set; }
#endif

        [MaxLength(50), Display(Name = "Значение")]
        public string Value { get; set; }

        [MaxLength(255), Display(Name = "Коментарий")]
        public string Comment { get; set; }


		//public virtual M_Family Family { get; set; }
		//public virtual M_Operation FamilyOperation { get; set; }
		//public virtual M_FamilyProperty FamilyProperty { get; set; }
	}
}
