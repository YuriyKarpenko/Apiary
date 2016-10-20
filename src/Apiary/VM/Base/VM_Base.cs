using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Base : NotifyPropertyChangedBase, ILog
	{
		#region static

		static VM_Base()
		{
			try
			{
				var md = new FrameworkPropertyMetadata(PropertyChangedCallback);
				FrameworkElement.DataContextProperty.OverrideMetadata(typeof(ContentControl), md);
			}
			catch //(Exception ex)
			{
				//Microsoft.Extensions.Logging.ILogger
				throw;
			}
		}

		static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var vm = e.NewValue as VM_Base;
			if (vm != null)
				vm.Init_Command(d as FrameworkElement);
		}

		#endregion

		protected Window window { get; private set; }
		protected UserControl uc { get; private set; }

		#region init command

		protected virtual void Init_Command_Internal(Window w)
		{
			this.window = w;
			w.CommandBindings.Add(ApplicationCommands.Close, this.Act_Close);
			w.CommandBindings.Add(SystemCommands.CloseWindowCommand, this.Act_Close);
		}
		protected virtual void Init_Command_Internal(UserControl uc) { this.uc = uc; }
		protected virtual void Init_Command_Internal(FrameworkElement fe) { }

		void Init_Command(FrameworkElement e)
		{
			this.Trace("()");
			try
			{
				if (e is Window)
					this.Init_Command_Internal(e as Window);
				else if (e is UserControl)
					this.Init_Command_Internal(e as UserControl);
				else
					this.Init_Command_Internal(e);

			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion

		protected virtual void Act_Close(ExecutedRoutedEventArgs e)
		{
			this.window?.Close();
		}

	}

}
