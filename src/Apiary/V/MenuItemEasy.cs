using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Apiary.V
{
	class MenuItemEasy
	{
		public string Header { get; set; }
		public RoutedUICommand Command { get; set; }
		public IEnumerable<MenuItemEasy> Items { get; set; }

		public MenuItemEasy(RoutedUICommand cmd)
		{
			this.Command = cmd;
			this.Header = cmd.Text;
		}

		public MenuItemEasy(string header, IEnumerable<RoutedUICommand> cmds)
		{
			this.Header = header;
			this.Items = cmds.Select(i => new MenuItemEasy(i));
		}
	}
}
