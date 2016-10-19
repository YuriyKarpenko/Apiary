using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using IT;
using IT.Data;

using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IR_FamilyInfoProperty : IRepositoryBase<IM_FamilyInfoProperty>
	{
		IEnumerable<IM_FamilyInfoProperty> GetByFamily(IM_Family family);
	}

	class R_FamilyInfoProperty : RepositoryBase<M_FamilyInfoProperty, IM_FamilyInfoProperty>, IR_FamilyInfoProperty
	{
#if EF
#else

		#region cmd

		IDbCommand cmd_GetByFamily => base.Cmd($@"
	SELECT D.* 
	FROM [{Tables.FamilyInfoProperty}] D
		INNER JOIN [{Tables.FamilyProperty}] P ON P.Id = D.FamilyPropertyId
	WHERE [Hidden] = 0 AND [FamilyId] = @FamilyId
	ORDER BY p.Order", CommandType.Text, "FamilyId");

		#endregion


		public R_FamilyInfoProperty(IDbContext context) : base(context, Tables.FamilyInfoProperty) { }


		public IEnumerable<IM_FamilyInfoProperty> GetByFamily(IM_Family family)
		{
			Contract.NotNull(family, "family");
			this.Debug("()");
			try
			{
				this.cmd_GetByFamily.SetParameter("FamilyId", family.Id);
				var res = this.cmd_GetByFamily.ExecuteListReflection<M_FamilyInfoProperty>();
				return res;
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
#endif


	}
}
