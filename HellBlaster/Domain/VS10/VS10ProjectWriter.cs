using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using HellBlaster.Domain;

namespace HellBlaster.VS10
{
	public class VS10ProjectWriter : VS10ProjectReader
	{
		public VS10ProjectWriter()
		{

		}
		
		public VS10ProjectWriter(string projectxml)
		{
			FileContent = projectxml;
		}


		public string UpdateReference(FileReference newRef)
		{
			StringBuilder modifiedXML = new StringBuilder();

			using (XmlWriter xw = XmlWriter.Create(modifiedXML, XMLSettings()))
			{
				XElement project = XElement.Parse(FileContent);
				XElement foundRefElt=FindReference(newRef.Name, project);
				if (foundRefElt != null)
				{
					UpdateReference(newRef, foundRefElt);
					project.Save(xw);
					xw.Flush();
				}
			}
			FileContent = modifiedXML.ToString();
			return FileContent;
		}

		private XElement FindReference(string referenceName, XElement project)
		{			
			IEnumerable<XElement> filerefsElts = FindFileReferences(project);

			if (filerefsElts != null)			
				return  (from elt in filerefsElts
									  where (ReferenceNameMatches(referenceName, elt))
									  select elt).FirstOrDefault();			
			else
				return null;			
		}

		private bool ReferenceNameMatches(string referenceName, XElement elt)
		{
			return NameInsideAttribute(elt) == referenceName;
		}

		private void UpdateReference(FileReference newRef, XElement foundRefElt)
		{
			XElement pathElt = foundRefElt.Elements(Namespace + PathTag).FirstOrDefault();
			pathElt.Value = newRef.Path;
			UpdateVersionAttribute(foundRefElt, newRef.Version);
		}

		private void UpdateVersionAttribute(XElement foundRefElt, string newVersion)
		{
			XAttribute attr = foundRefElt.Attribute(IncludeAttr);
			if (!String.IsNullOrEmpty(VersionInsideAttribute(foundRefElt)))
				attr.Value = attr.Value.Replace(VersionInsideAttribute(foundRefElt), newVersion);
			else
				attr.Value = attr.Value + ", Version=" + newVersion;
		}

		private static XmlWriterSettings XMLSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = false;
			settings.NewLineOnAttributes = true;
			return settings;
		}

		public void WriteToFile()
		{
			System.IO.File.WriteAllText(LoadedProjectPath,FileContent);
		}
	}
}
