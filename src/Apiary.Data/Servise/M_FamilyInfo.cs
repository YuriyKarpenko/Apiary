using System.Collections.Generic;

namespace Apiary.Data
{
    public interface IM_FamilyInfo
    {
        IM_Family Family { get; }
        List<IM_FamilyPropertyValue> Properties { get; }
    }

    class M_FamilyInfo : IM_FamilyInfo
    {
        public IM_Family Family { get; private set; }
        public List<IM_FamilyPropertyValue> Properties { get; private set; }


        public M_FamilyInfo(IM_Family family, List<IM_FamilyPropertyValue> properties)
        {
            this.Family = family;
            this.Properties = properties;
        }
    }
}
