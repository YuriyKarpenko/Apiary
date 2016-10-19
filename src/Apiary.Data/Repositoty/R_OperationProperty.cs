using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using IT;
using IT.Data;
using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IR_OperationProperty : IRepositoryBase<IM_OperationProperty>
	{
		IEnumerable<IM_OperationProperty> GetByOperation(IM_Operation operation);
	}

	class R_OperationProperty : RepositoryBase<M_OperationProperty, IM_OperationProperty>, IR_OperationProperty
	{
#if EF
#else
		#region cmd

		protected override IDbCommand cmdSelect => base.Cmd($@"
	SELECT * FROM [{Tables.OperationProperty}]
	WHERE @All = 1 OR [Hide] = 0
	ORDER BY [Order]", CommandType.Text, "All");

		protected IDbCommand cmdByOperation => base.Cmd($@"
	SELECT * FROM [{Tables.OperationProperty}]
	WHERE [OperationId] = @OperationId
	ORDER BY [Order]", CommandType.Text, "OperationId");

		#endregion

		public R_OperationProperty(IDbContext context) : base(context, Tables.OperationProperty)
        {
		}

		public IEnumerable<IM_OperationProperty> GetByOperation(IM_Operation operation)
		{
			Contract.NotNull(operation, "operation");
			this.Debug("()");
			try
			{
				this.cmdByOperation.SetParameter("OperationId", operation.Id);
				var res = this.cmdByOperation.ExecuteListReflection<M_OperationProperty>();
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
