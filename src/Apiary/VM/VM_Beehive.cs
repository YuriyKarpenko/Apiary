using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using IT;
using IT.WPF;
using Apiary.Data;
using Apiary.M;

namespace Apiary.VM
{
	class VM_Beehive : VM_BaseEditHierarchy<IM_Beehive, IM_Beehive>
	{
		public IEnumerableProperty<M_Family> Families { get; private set; }


		public VM_Beehive(long? id)
			: base(DbProvider.Instance.List_Beehive(), "Выбор улья :")
		{
			if (id.HasValue)
				this.Master_List?.Select(i => i.Id == id.Value, true);
		}


		protected override void OnMasterSelect(IM_Beehive value)
		{
			base.OnMasterSelect(value);

			var prop = new CM_Property_Value(value);
			this.Properties_Set(prop);
			this.Act_Refresh(null);
		}

		protected override void Init_Internal()
		{
			base.Init_Internal();
			this.Families = new IEnumerableProperty<M_Family>(Family_Get, this.Family_Select);
			this.Content_Set(this.Families);
		}


		private IEnumerable<M_Family> Family_Get()
		{
			var res = this.db.List_Family_ByBeehive(this.Master_List.SelectedItem)?.ToArray();
			return res;
		}

		private void Family_Select(IM_Family value)
		{

		}


		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);
			//	properties
			//	families
			uc.CommandBindings.Add(ApplicationCommands.Delete, this.Act_Delete, e => e.CanExecute = this.Families?.HasSelected ?? false);
			uc.CommandBindings.Add(Commands.Edit, this.Act_Edit, e => e.CanExecute = this.Families?.HasSelected ?? false);
			uc.CommandBindings.Add(ApplicationCommands.New, this.Act_New, e => e.CanExecute = this.Master_List.HasSelected);
			uc.CommandBindings.Add(NavigationCommands.Refresh, this.Act_Refresh);
		}

		private void Act_Delete(ExecutedRoutedEventArgs e)
		{
			try
			{
				var editItem = this.Families.SelectedItem;
				if (MsgDlg($"Семья {editItem} умерла ?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					editItem.DeathDay = DateTime.Now;
					editItem.Hide = true;
					this.db.Set_Family(editItem);

					this.Act_Refresh(e);
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_Edit(ExecutedRoutedEventArgs e)
		{
			try
			{
				var editItem = this.Families.SelectedItem;
				var vm = new CM_Property_Value(editItem);
				//var vm = new CM_PropertyFamilyInfo(this.Families.SelectedItem);
				if (VM_Dialog.Show<V.UC_EditItem>("Редактирование семьи", new { Value = editItem }, null))
					this.db.Set_Family(editItem);

				this.Act_Refresh(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_New(ExecutedRoutedEventArgs e)
		{
			try
			{
				M_Family newItem = new M_Family();
				newItem.BeehiveId = this.Master_List.SelectedItem.Id;
				var vm = new CM_Property_Value(newItem);
				if (VM_Dialog.Show<V.UC_EditItem>("Редактирование семьи", vm, null))
					this.db.Set_Family(newItem);

				this.Act_Refresh(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_Refresh(ExecutedRoutedEventArgs e)
		{
			try
			{
				this.Families.Reset();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion
	}
}
