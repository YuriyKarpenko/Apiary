using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Apiary.Data
{
	public interface IEntityBase
	{
		[Key, Editable(false), Browsable(false)]
		[Display(AutoGenerateField = false, Order = -100)]
#if GUID
		Guid Id { get; set; }
#else
		long Id { get; set; }
#endif

		[Browsable(false)]
		DateTime Created { get; set; }

		[Browsable(false)]
		DateTime Modified { get; set; }

		[Display(AutoGenerateField = true, Name = "Сктыть", Order = 100)]
		[Browsable(true)]
		bool Hide { get; set; }


		[Browsable(false)]
		bool IsNew { get; }
	}
}

#if GUID
#else
#endif
namespace Apiary.Data.Model
{
	[DebuggerDisplay("{Id} c:{Created} m:{Modified} h:{Hide}")]
	public abstract class EntityBase : IEntityBase
	{
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key, Editable(false), Browsable(false)]
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

		[Display(AutoGenerateField = true, Name = "Сктыть", Order = 100)]
		[Browsable(true)]
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


		public EntityBase()
		{
			this.Created = DateTime.Now;
		}

		public override string ToString()
		{
			return $"{base.ToString()} : {Id} c{Created} m{Modified} h{Hide}";
		}
	}
}
