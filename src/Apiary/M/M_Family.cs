using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Apiary.Data;

namespace Apiary.M
{
	[LookupBindingProperties("Beehives", "Name", "Id", "BeehiveId")]
	class M_Family : M_Base, IM_Family
	{
		[Browsable(false)]
		public static IEnumerable<IM_Beehive> Beehives { get; set; }


		//[Browsable(false)]
		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		public long BeehiveId { get; set; }

		[Display(Name = "Название", AutoGenerateField = true, Order = 10)]
		public string Name { get; set; }

		[Display(Name = "Дата рождения", AutoGenerateField = true, Order = 20)]
		public DateTime BirthDay { get; set; }

		[Display(Name = "Коментарий", AutoGenerateField = true, Order = 30)]
		public string Comment { get; set; }

		[Display(Name = "Дата смерти", AutoGenerateField = true, Order = 40)]
		public DateTime? DeathDay { get; set; }

		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		public virtual IM_Beehive Beehive => Beehives.SingleOrDefault(i => i.Id == this.BeehiveId);


		public M_Family()
		{
			this.BirthDay = DateTime.Now;
		}

		public override string ToString()
		{
			return $"{this.Beehive} -> {this.Id}:{this.Name}";
		}
	}

	static class IM_Family_Extentions
	{
		public static M_Family ToModel(this IM_Family value)
		{
			var res = new M_Family();
			IT.UtilsReflection.ClonePropertyTo(value, res);
			return res;
		}

		public static IEnumerable<M_Family> ToModel(this IEnumerable<IM_Family> value)
		{
			var res = value.Select(i => i.ToModel());
			return res;
		}
	}
}
