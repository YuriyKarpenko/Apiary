using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using Apiary.M;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Family : VM_BaseEditHierarchy<M.M_Family, M.M_Family>
	{

		public VM_Family()
			: base(Db.List_Family(), "Выбор семьи :")
		{
		}


		protected override void OnMasterSelect(M_Family value)
		{
			base.OnMasterSelect(value);
			this.Properties_Set(new CM_PropertyItem<M.M_Family>(value));
			this.Init_Content();
		}


		protected override void Init_Internal()
		{
			base.Init_Internal();
		}

		private void Init_Content()
		{
			if (this.Master_List.HasSelected)
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

			uc.CommandBindings.Add(ApplicationCommands.Properties, this.Act_Property, e => e.CanExecute = this.Master_List.HasSelected);
		}

		void Act_Property(ExecutedRoutedEventArgs e)
		{
		}

		#endregion
	}
}
