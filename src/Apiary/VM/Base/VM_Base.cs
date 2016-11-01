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

		protected static DbProvider Db = DbProvider.Instance;

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


		public static MessageBoxResult MsgDlg(string message, MessageBoxButton btns)
		{
			return MessageBox.Show(App.Current.MainWindow, message, Ap.AppCaption, btns, MessageBoxImage.Question);
		}

		#endregion 

		#region events

		public event EventHandler<EventArgs<FrameworkElement>> ChangeDataContext_FE;
		public event EventHandler<EventArgs<UserControl>> ChangeDataContext_UC;
		public event EventHandler<EventArgs<Window>> ChangeDataContext_W;

		#endregion 

		protected Window window { get; private set; }
		protected UserControl uc { get; private set; }

		#region init command

		protected virtual void Init_Command_Internal(Window w)
		{
			this.window = w;
			this.ChangeDataContext_W?.Invoke(this, new EventArgs<Window>(w));
		}
		protected virtual void Init_Command_Internal(UserControl uc)
		{
			this.uc = uc;
			this.ChangeDataContext_UC?.Invoke(this, new EventArgs<UserControl>(uc));
		}
		protected virtual void Init_Command_Internal(FrameworkElement fe)
		{
			this.ChangeDataContext_FE?.Invoke(this, new EventArgs<FrameworkElement>(fe));
		}

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

	}

	class VM_BaseWindow : VM_Base
	{
		protected override void Init_Command_Internal(Window w)
		{
			this.Debug("()");
			try
			{
				base.Init_Command_Internal(w);
				w.CommandBindings.Add(ApplicationCommands.Close, this.Act_Close);
				w.CommandBindings.Add(SystemCommands.CloseWindowCommand, this.Act_Close);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		protected virtual void Act_Close(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				if (this.window != null)
				{
					this.window.DialogResult = false;
					this.window.Close();
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}
	}
}
