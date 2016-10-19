using System;
using System.Collections.Generic;
using System.Linq;

using IT;
using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IR_Family : IRepositoryDic<IM_Family>
	{
		IEnumerable<IM_Family> GetByBeehive(IM_Beehive beehive);
	}

	class R_Family : RepositoryBaseDic<M_Family, IM_Family>, IR_Family
	{
#if EF
#else
		public R_Family(IDbContext context) : base(context, Tables.Family)
		{
		}

		//public override IM_Family Get(long id)
		//{
		//	var res = base.Get(id);
		//	//res.Beehive = DB.Instance.R_Beehive.Get(res.BeehiveId);
		//	return res;
		//}
#endif

		public IEnumerable<IM_Family> GetByBeehive(IM_Beehive beehive)
		{
			Contract.NotNull(beehive, "beehive");
			this.Debug("()");
			try
			{
				if (beehive != null)
					return this.List(true).Where(i => i.BeehiveId == beehive.Id);
				return null;
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
	}
}
