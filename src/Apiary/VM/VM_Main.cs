using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using Apiary.Data;
using Apiary.M;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Main : VM_BaseContent
	{
		private readonly MemCache<object, VM_Base> vms = new MemCache<object, VM_Base>();

		public bool WithHidden { get { return VM_Global.WithHidden.Value; } set { VM_Global.WithHidden.Value = value; } }
		public MenuItem[] MainMenu { get; private set; }


		public VM_Main()
		{
			this.Init_Menu();
		}


		void Init_Menu()
		{
			this.MainMenu = new MenuItem[]{
				new MenuItem(){Header = "Справочники",
					Items = {
						new MenuItem(){ Command = Commands.Dic_FamilyProperty, CommandParameter = Tables.FamilyProperty },
						new MenuItem(){ Command = Commands.Dic_Operation, CommandParameter = Tables.Operation },
						new MenuItem(){ Command = Commands.Dic_Beehive, CommandParameter = Tables.Beehive },
					},
				},
				new MenuItem(){ Command = Commands.Beehive },
				new MenuItem(){ Command = Commands.Family },
				new MenuItem(){ Command = Commands.FamilyOperations },

				new MenuItem() { Command = Commands.ShowAll, IsCheckable = true, /*HorizontalAlignment = HorizontalAlignment.Right*/ },
#if DEBUG
				new MenuItem(){ Command = ApplicationCommands.Replace },
				new MenuItem(){ Command = ApplicationCommands.Open },
#endif

			};
			//BindingOperations.SetBinding(this.MainMenu[5], MenuItem.IsCheckedProperty, new Binding("global::Apiary.VM.VM_Global.WithHidden.Value"));
			BindingOperations.SetBinding(this.MainMenu[4], MenuItem.IsCheckedProperty, new Binding("WithHidden"));
		}


		#region actions

		protected override void Init_Command_Internal(Window w)
		{
			base.Init_Command_Internal(w);

			w.CommandBindings.Add(Commands.Dic_Beehive, this.Act_Dictionary);
			w.CommandBindings.Add(Commands.Dic_Operation, this.Act_Dictionary);
			w.CommandBindings.Add(Commands.Dic_FamilyProperty, this.Act_Dictionary);

			w.CommandBindings.Add(Commands.Beehive, this.Act_Beehive);
			w.CommandBindings.Add(Commands.Family, this.Act_Family);
			w.CommandBindings.Add(Commands.FamilyOperations, this.Act_FamilyOperations);
			w.CommandBindings.Add(ApplicationCommands.Replace, this.Act_New);
			w.CommandBindings.Add(ApplicationCommands.Open, this.Act_Open);

			w.CommandBindings.Add(Commands.ShowAll, e => global::Apiary.VM.VM_Global.WithHidden.Value = !VM_Global.WithHidden.Value);
		}

		private void Act_New(ExecutedRoutedEventArgs e)
		{
			DB.Instance.CreateDb();
		}

		private void Act_Open(ExecutedRoutedEventArgs e)
		{
			this.ShowDictionary(this.db.List_Family(true).ToArray(), this.db.Set_Family);
			//db.Set_Family(new M_Family() { BeehiveId = 1, Comment = "Comment1", Name = "Fam 1" });
		}

		private void Act_Dictionary(ExecutedRoutedEventArgs e)
		{
			this.Debug("({0})", e.Parameter);
			try
			{
				switch ((Tables)e.Parameter)
				{
					case Tables.Beehive:
						this.ShowDictionary(this.db.List_Beehive(true), this.db.Set_Beehive);
						break;

					case Tables.Operation:
						this.ShowDictionary(this.db.List_Operation(true), this.db.Set_Operation);
						break;

					case Tables.FamilyProperty:
						this.ShowDictionary(this.db.List_FamilyProperty(true), this.db.Set_FamilyProperty);
						break;
				}
			}
			catch (Exception ex)
			{
				this.Warn(ex, "({0})", e.Parameter);
			}
		}
		private void ShowDictionary<T>(IEnumerable<T> list, Action<IEnumerable<T>> saveList) where T : class, IEntityBase
		{
			this.Debug("()");
			try
			{
				var title = typeof(T).GetNameFromAttributes();
				var vm = new VM_Dictionary<T>(list, i => i.Hide = true);
				if (VM_Dialog.Show<V.UC_Dictionary>($"Справосник: {title}", vm, null))
					saveList(vm.List.List);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_Beehive(ExecutedRoutedEventArgs e)
		{
			this.Content_Set(new VM_Beehive(e.Parameter?.ToString().To<long>()));
		}

		private void Act_Family(ExecutedRoutedEventArgs e)
		{
			this.Content_Set(new VM_Family());
		}

		private void Act_FamilyOperations(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				var vm = new VM_FamilyOperations();
				this.Content_Set(vm);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion
	}
}
