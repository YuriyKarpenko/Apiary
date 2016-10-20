using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Apiary.V
{
	class MenuItemEasy
	{
		public string Header { get; set; }
		public ICommand Command { get; set; }
		public IEnumerable<MenuItemEasy> Items { get; set; }

		public MenuItemEasy(ICommand cmd)
		{
			this.Command = cmd;
		}

		public MenuItemEasy(string header, IEnumerable<ICommand> cmds)
		{
			this.Header = header;
			this.Items = cmds.Select(i => new MenuItemEasy(i));
		}
	}
}
