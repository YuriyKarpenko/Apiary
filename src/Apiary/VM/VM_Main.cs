using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using Apiary.Data.Model;
using Apiary.V;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Main : VM_BaseDb
	{
		private readonly MemCache<object, VM_Base> vms = new MemCache<object, VM_Base>();
		//public Menu MainMenu { get; private set; }
		public MenuItem[] MainMenu { get; private set; }
		//public MenuItemEasy Menu_Dic { get; private set; }

		public VM_Main()
		{
			this.Init_Menu();
		}


		void Init_Menu()
		{
			//this.Menu_Dic =
			//	new MenuItemEasy("Справочники",
			//		new ICommand[] { Commands.Dic_FamilyOperation, Commands.Dic_FamilyProperty }
			//	);

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
				new MenuItem(){ Command = ApplicationCommands.New }, 
				new MenuItem(){ Command = ApplicationCommands.Open }, 
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
			w.CommandBindings.Add(ApplicationCommands.New, this.Act_New);
			w.CommandBindings.Add(ApplicationCommands.Open, this.Act_Open);

		}

		void Act_New(ExecutedRoutedEventArgs e)
		{
			DB.Instance.CreateDb();
		}

		void Act_Open(ExecutedRoutedEventArgs e)
		{
			var item = new M_Beehive() { Address = "Address1", Comment = "Comment1", Name = "Name1" };
            db.S_Dictionary.R_Beehive.Set(item);
		}

		void Act_Dictionary(ExecutedRoutedEventArgs e)
		{
			this.Debug("({0})", e.Parameter);
			try
			{
				VM_Base vm = null;

				switch ((Tables)e.Parameter)
				{
					case Tables.Beehive:
						vm = this.vms[e.Parameter, () => new VM_Dictionary<M_Beehive, IM_Beehive>(db.S_Dictionary.R_Beehive)];
						new V.V_Dictionary().ShowDialog(vm, this.window, false);
						break;

					case Tables.Operation:
						vm = this.vms[e.Parameter, () => new VM_Dictionary<M_Operation, IM_Operation>(db.S_Dictionary.R_FamilyOperation)];
						new V.V_Dictionary().ShowDialog(vm, this.window, false);
						break;

					case Tables.FamilyProperty:
						vm = this.vms[e.Parameter, () => new VM_Dictionary<M_FamilyProperty, IM_FamilyProperty>(db.S_Dictionary.R_FamilyProperty)];
						new V.V_Dictionary().ShowDialog(vm, this.window, false);
						break;
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "({0})", e.Parameter);
			}
		}

		void Act_Beehive(ExecutedRoutedEventArgs e)
		{
            this.Content_Set(new VM_Beehive());
		}

		void Act_Family(ExecutedRoutedEventArgs e)
		{
            this.Content_Set(new VM_Family());
		}

		#endregion
	}
}
