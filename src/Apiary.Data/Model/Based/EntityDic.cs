using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Apiary.Data
{
    public interface IEntityDic : IEntityBase
    {
        string Name { get; set; }
    }
}

namespace Apiary.Data.Model
{
    [DebuggerDisplay("{Id} '{Name}' c:{Created} m:{Modified} h:{Hide}")]
    public abstract class EntityDic : EntityBase, IEntityDic
    {
        [StringLength(50), Required, Display(ShortName = "Название", Name = "Название", Order = 1)]
        public string Name { get; set; }


        public override string ToString()
        {
            return $"{base.ToString()} n{Name}";
        }
    }

}
