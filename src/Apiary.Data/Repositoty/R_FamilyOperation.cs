using System;
using System.Collections.Generic;
using System.Data;

using IT;
using IT.Data;
using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IR_FamilyOperation : IRepositoryBase<IM_FamilyOperation>
	{
		IEnumerable<IM_FamilyOperation> ByManyIds(long[] familyIds, long[] operationIds);
	}


	class R_FamilyOperation : RepositoryBase<M_FamilyOperation, IM_FamilyOperation>, IR_FamilyOperation
	{
#if EF
#else
		#region cmd

		private IDbCommand cmdByManyIds => base.Cmd($@"
	SELECT * FROM [{Tables.FamilyOperation}]
	WHERE 1 = 1
		AND (@FamilyIds = 0 OR [FamilyId] IN (@FamilyIds)) 
		AND (@OperationIds = 0 OR [OperationId] IN (@OperationIds)) 
	", CommandType.Text, "FamilyIds", "OperationIds");

		#endregion

		public R_FamilyOperation(IDbContext context) : base(context, Tables.FamilyOperation)
		{
		}

		public IEnumerable<IM_FamilyOperation> ByManyIds(long[] familyIds, long[] operationIds)
		{
			Contract.NotNull(familyIds, "familyIds");
			Contract.NotNull(operationIds, "operationIds");
			this.Debug("()");
			try
			{
				var pF = string.Join(", ", familyIds);
				var pO = string.Join(", ", operationIds);
				this.cmdByManyIds.SetParameter("FamilyIds", pF);
				this.cmdByManyIds.SetParameter("OperationIds", pO);
				var res = this.cmdByManyIds.ExecuteListReflection<M_FamilyOperation>();
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
