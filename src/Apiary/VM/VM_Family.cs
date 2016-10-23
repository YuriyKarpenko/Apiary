using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Family : VM_BaseDb<M.M_Family>
	{

		public CM_PropertyFamily Properties { get; private set; }


		public VM_Family()
		{
		}

		public override void Set_Self(M.M_Family value)
		{
			base.Set_Self(value);
			this.Set_SelfProperty(new CM_PropertyFamily(value));
			this.Init_Content();
		}


		protected override void Init_Internal()
		{
			base.Init_Internal();
			this.SelfList = new IEnumerableProperty<M.M_Family>(() => this.db.List_Family(false).ToList(), this.Set_Self);
		}

		private void Init_Content()
		{
			if (this.self != null)
			{

			}
			else
			{
				this.Content_Set(null);
			}
		}


		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);

			uc.CommandBindings.Add(ApplicationCommands.Properties, this.Act_Property, e => e.CanExecute = this.self != null);
		}

		void Act_Property(ExecutedRoutedEventArgs e)
		{
		}

		#endregion
	}
}
