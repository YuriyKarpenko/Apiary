using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Apiary.M
{
	[DebuggerDisplay("{Id} c:{Created} m:{Modified} h:{Hide}")]
	public abstract class M_Base : Apiary.Data.IEntityBase
	{
#if GUID
#else
#endif
		[Display(AutoGenerateField = false, Order = -100)]
#if GUID
		public Guid Id { get; set; }
#else
		public long Id { get; set; }
#endif

		//[DataType( System.ComponentModel.DataAnnotations.DataType.)]

		[Browsable(false)]
		public DateTime Created { get; set; }

		[Browsable(false)]
		public DateTime Modified { get; set; }

		[Browsable(true)]
		[Display(AutoGenerateField = true, Name = "Сктыть", Order = 100)]
		public bool Hide { get; set; }

		[Browsable(false)]
		public bool IsNew
		{
			get
			{
#if GUID
				return this.Id == Guid.Empty;
#else
				return this.Id == 0;
#endif
			}
		}


		public override string ToString()
		{
			return $"{base.ToString()} : {Id} c{Created} m{Modified} h{Hide}";
		}
	}
}
