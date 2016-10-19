using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Apiary.Data.Model;
using Apiary.Data.Repositoty;

namespace Apiary.Data.Servise
{

    public interface IS_Family
    {
        IR_FamilyInfoProperty R_FamilyProperties { get; }
        IR_Family R_Family { get; }

        IM_FamilyInfo Get_FamilyInfo(IM_Family item);
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


        public IM_FamilyInfo Get_FamilyInfo(IM_Family item)
        {
            var propAll = this._R_FamilyProperty.List();
            var prop = this.R_FamilyProperties.GetByFamily(item);
            var propRes = propAll
                .Select(x => (IM_PropertyInfo)new M_PropertyInfo(item, x, prop.FirstOrDefault(i => i.FamilyPropertyId == i.Id)))
                .ToList();
            return new M_FamilyInfo(item, propRes);
        }
    }
}
