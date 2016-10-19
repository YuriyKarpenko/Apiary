using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Apiary.Data.Model;
using Apiary.Data.Repositoty;

namespace Apiary.Data.Servise
{
    public interface IS_Dictionary
    {
        IR_FamilyProperty R_FamilyProperty { get; }
        IR_Operation R_FamilyOperation { get; }
        IR_Beehive R_Beehive  { get; }
    }

    class S_Dictionary : IS_Dictionary
    {
        private IDbContext context;
        private IR_FamilyProperty _R_FamilyProperty = null;
        private IR_Operation _R_FamilyOperation = null;
        private IR_Beehive _R_Beehive = null;


        public IR_FamilyProperty R_FamilyProperty => this._R_FamilyProperty = this._R_FamilyProperty ?? new R_FamilyProperty(this.context);
        public IR_Operation R_FamilyOperation => this._R_FamilyOperation = this._R_FamilyOperation ?? new R_Operation(this.context);
        public IR_Beehive R_Beehive => this._R_Beehive = this._R_Beehive ?? new R_Beehive(this.context);


        public S_Dictionary(IDbContext context)
        {
            this.context = context;
        }
    }
}
