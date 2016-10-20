using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using IT;
using IT.WPF;
using Apiary.Data;
using Apiary.Data.Model;
using Apiary.Data.Repositoty;
using Apiary.V;

namespace Apiary.VM
{
	class VM_Dictionary<T, I> : VM_BaseDb
        where I : IEntityDic
        where T : EntityDic, I, new()
	{
		IRepositoryDic<I> repo;

		public List<DataGridColumn> Columns { get { return VM_CRUD.GenerateColumns<T>(); } /*private set; */}
		public IEnumerableProperty<T> List { get; private set; }


		public VM_Dictionary(IRepositoryDic<I> repository)
			: base(typeof(I).GetAttributeValueStr<DescriptionAttribute>(a => a.Description))
		{
			Contract.NotNull(repository, "repository");

			this.repo = repository;

			this.List = new IEnumerableProperty<T>(() => this.repo.List(true).Cast<T>());

			this.Content_Set(this.List);
		}



		#region actions

		protected override void Init_Command_Internal(Window w)
		{
			base.Init_Command_Internal(w);

			w.CommandBindings.Add(ApplicationCommands.Close, this.Act_Close);
			w.CommandBindings.Add(ApplicationCommands.Delete, this.Act_Delete_Intrnal);
			//w.CommandBindings.Add(ApplicationCommands.New, this.Act_Create);
			w.CommandBindings.Add(ApplicationCommands.Save, this.Act_Save_Intrnal);
		}


		protected void Act_Delete_Intrnal(ExecutedRoutedEventArgs e)
		{
			//base.Act_Delete_Intrnal(e);
			this.List.SelectedItem.Hide = true;
			//this.repo.Delete(this.List.SelectedItem);
			//this.Act_Refresh(e);
		}

		//protected void Act_Create(ExecutedRoutedEventArgs e)
		//{
		//	//System.Windows.Controls.DataGrid.com
		//	var item = new T();
		//	//this.List.List.Add(item);
		//	//this.Act_Refresh(e);
		//	//this.List.SelectedItem = item;
		//	//this.Content_Set(this.List);
		//}

		//protected override void Act_Update_Intrnal(ExecutedRoutedEventArgs e)
		//{
		//	base.Act_Update_Intrnal(e);
		//	this.repo.Set(this.List.SelectedItem);
		//}

		protected void Act_Save_Intrnal(ExecutedRoutedEventArgs e)
		{
			//base.Act_Save_Intrnal(e);
			this.repo.Set(this.List.List);
			this.Act_Close(e);
		}
		#endregion
	}
}
