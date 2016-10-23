using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using Apiary.M;
using Apiary.V;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Main : VM_BaseDb
	{
		private readonly MemCache<object, VM_Base> vms = new MemCache<object, VM_Base>();
		//public Menu MainMenu { get; private set; }
		public object[] MainMenu { get; private set; }
		public MenuItemEasy[] Menu_Dic { get; private set; }

		public VM_Main()
		{
			this.Init_Menu();
		}


		void Init_Menu()
		{
			this.Menu_Dic = new MenuItemEasy[]{
				new MenuItemEasy("Справочники",
					new ICommand[] { Commands.Dic_Operation, Commands.Dic_FamilyProperty }
				),
				new MenuItemEasy("Справочники",
					new ICommand[] { Commands.Dic_Operation, Commands.Dic_FamilyProperty }
				),
			};

			this.MainMenu = new object[]/*MenuItem[]*/{
				new MenuItem(){Header = "Справочники",
					Items = {
						new MenuItem(){ Command = Commands.Dic_FamilyProperty, CommandParameter = Tables.FamilyProperty },
						new MenuItem(){ Command = Commands.Dic_Operation, CommandParameter = Tables.Operation },
						new MenuItem(){ Command = Commands.Dic_Beehive, CommandParameter = Tables.Beehive },
					},
				},
				new MenuItem(){ Command = Commands.Beehive },
				new MenuItem(){ Command = Commands.Family },
				new MenuItem(){ Command = ApplicationCommands.Replace },
				new MenuItem(){ Command = ApplicationCommands.Open },
				new MenuItemEasy[]{
				new MenuItemEasy("Справочники",
					new ICommand[] { Commands.Dic_Operation, Commands.Dic_FamilyProperty }
				) },
			};
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
			w.CommandBindings.Add(ApplicationCommands.Replace, this.Act_New);
			w.CommandBindings.Add(ApplicationCommands.Open, this.Act_Open);

		}

		void Act_New(ExecutedRoutedEventArgs e)
		{
			DB.Instance.CreateDb();
		}

		void Act_Open(ExecutedRoutedEventArgs e)
		{
			var item = new M_Family() { BeehiveId = 1, Comment = "Comment1", Name = "Fam 1" };
			db.Set_Family(item);
		}

		void Act_Dictionary(ExecutedRoutedEventArgs e)
		{
			this.Debug("({0})", e.Parameter);
			try
			{
				switch ((Tables)e.Parameter)
				{
					case Tables.Beehive:
						this.ShowDictionary(this.db.List_Beehive(), this.db.Set_Beehive);
						break;

					case Tables.Operation:
						this.ShowDictionary(this.db.List_Operation(), this.db.Set_Operation);
						break;

					case Tables.FamilyProperty:
						this.ShowDictionary(this.db.List_FamilyProperty(), this.db.Set_FamilyProperty);
						break;
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "({0})", e.Parameter);
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
				throw;
			}
		}

		void Act_Beehive(ExecutedRoutedEventArgs e)
		{
			this.Content_Set(new VM_Beehive(e.Parameter?.ToString().To<long>()));
		}

		void Act_Family(ExecutedRoutedEventArgs e)
		{
			this.Content_Set(new VM_Family());
		}

		#endregion
	}
}
