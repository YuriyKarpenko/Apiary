using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using IT;
using Apiary.Data;

namespace Apiary.M
{
	class M_FamilyOperation : M_BaseDic, IM_FamilyOperation
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
	}


	static class IM_FamilyOperation_Extentions
	{
		public static M_FamilyOperation ToModel(this IM_FamilyOperation value)
		{
			return M_Base.BaseToModel<M_FamilyOperation>(value);
		}

		public static IEnumerable<M_FamilyOperation> ToModel(this IEnumerable<IM_FamilyOperation> value)
		{
			return value.Select(i => i.ToModel());
		}
	}
}
