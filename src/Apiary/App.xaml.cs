using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using IT;
using IT.Log;

namespace Apiary
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Logger.Include_Ip = false;
			Logger.Include_ThreadId = false;
#if DEBUG
			Logger.MinLevel = TraceLevel.Verbose;
#else
			Logger.MinLevel = TraceLevel.Warning;
#endif
			Logger.MessageSmall += Logger_MessageSmall;
		}

		void Logger_MessageSmall(object sender, EventArgs<TraceLevel, string, Exception> e)
		{
			switch (e.Value1)
			{
				case TraceLevel.Error:
					MessageBox.Show(e.Value2 + "\n" + e.Value3.ToString(), Ap.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
					break;
			}
		}
	}
}
