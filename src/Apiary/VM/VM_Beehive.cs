using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using IT;
using IT.WPF;
using Apiary.Data;
using Apiary.M;

namespace Apiary.VM
{
	class VM_Beehive : VM_BaseDb<IM_Beehive>
	{
		public IEnumerableProperty<IM_FamilyInfo> Families { get; private set; }


		public VM_Beehive(long? id)
		{
			if (id.HasValue)
				this.SelfList?.Select(i => i.Id == id.Value, true);
		}


		public override void Set_Self(IM_Beehive value)
		{
			base.Set_Self(value);
			var prop = new CM_PropertyItem<IM_Beehive>(this.self);
			this.Set_SelfProperty(prop);
			this.Act_Refresh(null);
		}

		protected override void Init_Internal()
		{
			base.Init_Internal();
			this.SelfList = new IEnumerableProperty<IM_Beehive>(() => this.db.List_Beehive(), this.Set_Self);
			this.Families = new IEnumerableProperty<IM_FamilyInfo>(Family_Get, this.Family_Select);
			this.Content_Set(this.Families);
		}


		private IEnumerable<IM_FamilyInfo> Family_Get()
		{
			var res = this.db.Get_FamilyInfoByBeehive(this.self).ToArray();
			return res;
		}
		private void Family_Select(IM_FamilyInfo value)
		{

		}


		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);

			uc.CommandBindings.Add(ApplicationCommands.Delete, this.Act_Delete, e => e.CanExecute = this.Families?.HasSelected ?? false);
			uc.CommandBindings.Add(Commands.Edit, this.Act_Edit, e => e.CanExecute = this.Families?.HasSelected ?? false);
			uc.CommandBindings.Add(ApplicationCommands.New, this.Act_New, e => e.CanExecute = this.self != null);
			uc.CommandBindings.Add(NavigationCommands.Refresh, this.Act_Refresh);
		}

		private void Act_Delete(ExecutedRoutedEventArgs e)
		{
			try
			{

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
				M_Family newItem = new M_Family();// (Apiary.Data.Model.M_Family)this.db.S_Family.R_Family.Create();
				newItem.BeehiveId = this.self.Id;
				M_Family.Beehives = this.SelfList.List;
				var vm = new VM_BaseEdit<M_Family>(newItem);
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
				this.Families.Reset(this.Family_Get());
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion
	}
}
