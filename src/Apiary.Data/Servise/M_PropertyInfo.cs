using System.ComponentModel.DataAnnotations;

using Apiary.Data.Model;

namespace Apiary.Data
{
    public interface IM_PropertyInfo : IM_FamilyInfoProperty
    {
        [Display(Name = "Тип значения")]
        [EnumDataType(typeof(TypeOfParam))]
        long Type { get; set; }

        string Name { get; set; }
    }

    class M_PropertyInfo : M_FamilyInfoProperty, IM_PropertyInfo
    {
        public long Type { get; set; }

        public string Name { get; set; }


        public M_PropertyInfo(IM_Family family, IM_FamilyProperty property, IM_FamilyInfoProperty value)
        {
            this.Comment = value?.Comment;
            this.FamilyId = value?.FamilyId ?? family.Id;
            this.FamilyPropertyId = value?.FamilyPropertyId ?? property.Id;
            this.Hide = property.Hide;
            this.Name = property.Name;
            this.Type = property.Type;
            this.Value = value?.Value;
        }
    }
}
