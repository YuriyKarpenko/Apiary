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
		//IR_FamilyInfoProperty R_FamilyProperties { get; }
		IR_Family R_Family { get; }

		IM_FamilyInfo Get_FamilyInfo(IM_Family family);
		IEnumerable<IM_FamilyInfo> Get_ByBeehive(IM_Beehive beehive);
		IEnumerable<IM_FamilyOperation> Get_Journal(IM_Family family, IM_Operation operation, DateTime? date);
	}


	class S_Family : IS_Family
	{
		private IDbContext context;
		private IR_Family _R_Family = null;
		private IR_FamilyInfoProperty _R_FamilyProperties = null;
		private IR_FamilyProperty _R_FamilyProperty = null;
		private IR_FamilyOperation _R_FamilyOperation = null;

		public IR_Family R_Family => this._R_Family = this._R_Family ?? new R_Family(this.context);
		//public IR_FamilyInfoProperty R_FamilyProperties => this._R_FamilyProperties = this._R_FamilyProperties ?? new R_FamilyInfoProperty(this.context);


		public S_Family(IDbContext context)
		{
			this.context = context;
			this._R_FamilyProperty = new R_FamilyProperty(this.context);
			this._R_FamilyProperties = new R_FamilyInfoProperty(this.context);
			this._R_FamilyOperation = new R_FamilyOperation(this.context);
		}


		public IM_FamilyInfo Get_FamilyInfo(IM_Family family)
		{
			this.Debug("()");
			try
			{
				var propAll = this._R_FamilyProperty.List();
				var prop = this._R_FamilyProperties.Get_ByFamily(family);
				var propRes = propAll
					.Select(x => (IM_FamilyPropertyValue)new M_FamilyPropertyValue(x, prop.FirstOrDefault(i => i.FamilyPropertyId == i.Id)))
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
				var res = this.R_Family.Get_ByBeehive(beehive);
				return res.Select(i => this.Get_FamilyInfo(i));
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		public IEnumerable<IM_FamilyOperation> Get_Journal(IM_Family family, IM_Operation operation, DateTime? date)
		{
			this.Debug("()");
			try
			{
				var op = this._R_FamilyOperation.List_ByManyIds(family?.Id, operation?.Id, date);
				return op;
				//var fams = op.Select(i => i.FamilyId).ToArray();

				//if ((fams?.Length ?? 0) > 0)
				//	return this.R_Family.List()
				//		.Where(i => fams.Contains(i.Id))
				//		.ToArray();
				//else
				//	return new IM_Family[] { };
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}
	}
}
