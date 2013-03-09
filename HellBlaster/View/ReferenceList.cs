using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace HellBlaster.View
{
	public class ReferenceList : ObservableCollection<Reference>
	{
		public ReferenceList()
		{
			Reference refe = new Reference();
			refe.projectName = "Uno";
			refe.Name = "core";
			refe.version = "1.1";
			Add(refe);
			refe = new Reference();
			refe.projectName = "Dos";
			refe.Name = "core";
			refe.version = "1.2";
			Add(refe);
		}
	}
}
