using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IT;
using Apiary.Data.Model;
using Apiary.Data.Repositoty;

namespace Apiary.Data.Servise
{

	public interface IS_Family : ILog
	{
		IR_FamilyInfoProperty R_FamilyProperties { get; }
		IR_Family R_Family { get; }

		IM_FamilyInfo Get_FamilyInfo(IM_Family family);
		IEnumerable<IM_FamilyInfo> Get_ByBeehive(IM_Beehive beehive);
	}


	class S_Family : IS_Family
	{
		private IDbContext context;
		private IR_FamilyInfoProperty _R_FamilyProperties = null;
		private IR_FamilyProperty _R_FamilyProperty = null;
		private IR_Family _R_Family = null;

		public IR_Family R_Family => this._R_Family = this._R_Family ?? new R_Family(this.context);
		public IR_FamilyInfoProperty R_FamilyProperties => this._R_FamilyProperties = this._R_FamilyProperties ?? new R_FamilyInfoProperty(this.context);


		public S_Family(IDbContext context)
		{
			this.context = context;
			this._R_FamilyProperty = new R_FamilyProperty(this.context);
		}


		public IM_FamilyInfo Get_FamilyInfo(IM_Family family)
		{
			this.Debug("()");
			try
			{
				var propAll = this._R_FamilyProperty.List();
				var prop = this.R_FamilyProperties.GetByFamily(family);
				var propRes = propAll
					.Select(x => (IM_PropertyInfo)new M_PropertyInfo(family, x, prop.FirstOrDefault(i => i.FamilyPropertyId == i.Id)))
					.ToList();
				return new M_FamilyInfo(family, propRes);

			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public IEnumerable<IM_FamilyInfo> Get_ByBeehive(IM_Beehive beehive)
		{
			this.Debug("()");
			try
			{
				var res = this.R_Family.GetByBeehive(beehive);
				return res.Select(i => this.Get_FamilyInfo(i));
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
	}
}
