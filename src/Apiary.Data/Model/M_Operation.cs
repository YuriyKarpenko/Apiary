using System.ComponentModel;

namespace Apiary.Data
{
	/// <summary>
	/// Дествия с семьей
	/// </summary>
	[Description("Операции с семьей")]
    public interface IM_Operation : IEntityDic { }
}

namespace Apiary.Data.Model
{
	/// <summary>
	/// Дествия с семьей
	/// </summary>
	[Description("Операции с семьей"), DisplayName("Дествия")]
	public class M_Operation : EntityDic, IM_Operation
	{
	}
}
