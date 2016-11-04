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
		IEnumerable<IM_FamilyOperation> List_ByManyIds(long[] familyIds, long[] operationIds);
		IEnumerable<IM_FamilyOperation> List_ByManyIds(long? familyId, long? operationId, DateTime? date);
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

		private IDbCommand cmdFilter => base.Cmd($@"
	SELECT * FROM [{Tables.FamilyOperation}]
	WHERE 1 = 1
		AND (@FamilyId IS NULL OR [FamilyId] = @FamilyId) 
		AND (@OperationId IS NULL OR [OperationId] = @OperationId)
		AND (@Date IS NULL OR @Date <= [DateWrite] AND [DateWrite] < @Date + 1)
	", CommandType.Text, "FamilyId", "OperationId", "Date");

		#endregion

		public R_FamilyOperation(IDbContext context) : base(context, Tables.FamilyOperation)
		{
		}

		public IEnumerable<IM_FamilyOperation> List_ByManyIds(long[] familyIds, long[] operationIds)
		{
			Contract.NotNull(familyIds, "familyIds");
			Contract.NotNull(operationIds, "operationIds");
			this.Debug("()");
			try
			{
				var pF = string.Join(", ", familyIds);
				var pO = string.Join(", ", operationIds);

				var cmd = this.cmdByManyIds;
				cmd.SetParameter("FamilyIds", pF);
				cmd.SetParameter("OperationIds", pO);
				var res = cmd.ExecuteListReflection<M_FamilyOperation>();
				return res;
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public IEnumerable<IM_FamilyOperation> List_ByManyIds(long? familyId, long? operationId, DateTime? date)
		{
			this.Debug("()");
			try
			{
				var cmd = this.cmdFilter;
				cmd.SetParameter("FamilyId", familyId);
				cmd.SetParameter("OperationId", operationId);
				cmd.SetParameter("Date", date?.Date);
				var res = cmd.ExecuteListReflection<M_FamilyOperation>();
				return res;
			}
			catch (Exception ex)
			{
				this.Warn(ex, $"() : {cmdFilter.CommandText}");
				throw;
			}
		}

#endif
	}
}
