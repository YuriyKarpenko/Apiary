using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Apiary.Data;

namespace Apiary.M
{
	[LookupBindingProperties("Beehives", "Name", "Id", "BeehiveId")]
	class M_Family : M_BaseDic, IM_Family
	{
		[Browsable(false)]
		public static IEnumerable<M_Beehive> Beehives { get; set; }


		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		public long BeehiveId
		{
			get { return this.Get<long>("BeehiveId"); }
			set { this.Set("BeehiveId", value); }
		}

		[Display(Name = "Дата рождения", AutoGenerateField = true, Order = 20)]
		public DateTime BirthDay
		{
			get { return this.Get<DateTime>("BirthDay"); }
			set { this.Set("BirthDay", value); }
		}

		[Display(Name = "Коментарий", AutoGenerateField = true, Order = 30)]
		public string Comment
		{
			get { return this.Get<string>("Comment"); }
			set { this.Set("Comment", value); }
		}

		[Display(Name = "Дата смерти", AutoGenerateField = true, Order = 40)]
		public DateTime? DeathDay
		{
			get { return this.Get<DateTime?>("DeathDay"); }
			set { this.Set("DeathDay", value); }
		}

		[Browsable(false)]
		[Display(Name = "Улей", AutoGenerateField = true, Order = 1)]
		public virtual M_Beehive Beehive => Beehives?.SingleOrDefault(i => i.Id == this.BeehiveId);


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
			return M_Base.BaseToModel<M_Family>(value);
		}

		public static IEnumerable<M_Family> ToModel(this IEnumerable<IM_Family> value)
		{
			var res = value.Select(i => i.ToModel());
			return res;
		}
	}
}
