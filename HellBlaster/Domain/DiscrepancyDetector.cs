using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Domain
{
	public class DiscrepancyDetector
	{
		Dictionary<string, Dictionary<string, FileReference>> references = new Dictionary<string, Dictionary<string, FileReference>>();

		public List<FileReference> Discrepancies()
		{
			List<FileReference> discrepancies = new List<FileReference>();

			foreach (KeyValuePair<string, Dictionary<string, FileReference>> refList in references)
				if (refList.Value.Count > 1)
					AddDiscrepanciesWhitTheSameName(refList.Value,discrepancies );
																		
			return discrepancies;
		}

		private void AddDiscrepanciesWhitTheSameName(Dictionary<string, FileReference> referencesWithSameName, List<FileReference> discrepancies)
		{
			foreach (KeyValuePair<string, FileReference> versionedReference in referencesWithSameName)
				discrepancies.Add(versionedReference.Value);
		}


		public void AddReference(FileReference fileReference)
		{
			if (!references.ContainsKey(fileReference.Name))				
				references.Add(fileReference.Name,new Dictionary<string,FileReference>());				

			if(fileReference.Version!=null&&!references[fileReference.Name].ContainsKey(fileReference.Version))
				references[fileReference.Name].Add(fileReference.Version,fileReference);
		}
	}
}
