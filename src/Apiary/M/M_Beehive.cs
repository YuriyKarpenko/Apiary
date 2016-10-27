using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using IT;
using Apiary.Data;

namespace Apiary.M
{
	class M_Beehive : M_Base, IM_Beehive
	{
		[MaxLength(50), StringLength(50), Required, Display(ShortName = "Название", Name = "Название", Order = 10)]
		public string Name { get; set; }

		[MaxLength(50), StringLength(50), Display(Name = "Адрес", Order = 20)]
		public string Address { get; set; }

		[MaxLength(255), StringLength(255), Display(Name = "Коментарий", Order = 30)]
		public string Comment { get; set; }


		public override string ToString()
		{
			return $"{this.Name}; {this.Address}";
		}
	}

	static class IM_Beehive_Extentions
	{
		public static M_Beehive ToModel(this IM_Beehive value)
		{
			var res = new M_Beehive();
			UtilsReflection.ClonePropertyTo(value, res);
			return res;
		}

		public static IEnumerable<M_Beehive> ToModel(this IEnumerable<IM_Beehive> value)
		{
			return value.Select(i => i.ToModel());
		}
	}
}
