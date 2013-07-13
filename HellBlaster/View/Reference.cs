using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HellBlaster.View
{
	public class Reference : INotifyPropertyChanged
	{
		public string projectName { get; set; }
		public string Name { get; set; }

		private string myversion;
		public string version { get { return myversion; } set { myversion = value; OnPropertyChanged("version"); } }

		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public bool hasDiscrepancy { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
