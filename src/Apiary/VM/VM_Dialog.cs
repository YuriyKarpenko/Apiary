using System;
using System.Windows;
using System.Windows.Input;

using IT;
using IT.Log;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Dialog : VM_BaseWindow
	{
		public static bool Show<TView>(string title, VM_Base vm, Window owner = null, Action<CanExecuteRoutedEventArgs> canSave = null) where TView : FrameworkElement
		{
			Logger.ToLogFmt(null, System.Diagnostics.TraceLevel.Verbose, null, "{0}.()", typeof(VM_Dialog));
			try
			{
				var w = new V.V_Dialog();
				var uc = Activator.CreateInstance<TView>();
				uc.DataContext = vm;
				return w.ShowDialog(new VM_Dialog(title, uc, canSave), owner);
			}
			catch (Exception ex)
			{
				Logger.ToLogFmt(null, System.Diagnostics.TraceLevel.Error, ex, "{0}.()", typeof(VM_Dialog));
				throw;
			}
		}


		private Action<CanExecuteRoutedEventArgs> canSave;
		private Action<CanExecuteRoutedEventArgs> canSaveDef = e => { e.CanExecute = true; };

		public string Title { get; private set; }
		public FrameworkElement Content { get; private set; }


		private VM_Dialog(string title, FrameworkElement content, Action<CanExecuteRoutedEventArgs> canSave)
		{
			this.Title = title;
			this.Content = content;
			this.canSave = canSave;
		}

		protected override void Init_Command_Internal(Window w)
		{
			base.Init_Command_Internal(w);

			w.CommandBindings.Add(ApplicationCommands.Save, this.Act_Save_Intrnal, this.canSave ?? this.canSaveDef);
		}

		protected virtual void Act_Save_Intrnal(ExecutedRoutedEventArgs e)
		{
			if(this.window != null)
				this.window.DialogResult = true;
			//this.Act_Close(e);
		}

		private void Act_Save(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Save_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

	}
}
