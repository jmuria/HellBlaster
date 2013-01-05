using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace HellBlaster.Domain
{
	public class VS10ProjectWriter : VS10ProjectReader
	{
		private string vs10ProjectContent;

		public VS10ProjectWriter(string projectxml)
		{
			// TODO: Complete member initialization
			this.vs10ProjectContent = projectxml;
		}

		public string UpdateReference(FileReference newRef)
		{
			StringBuilder modifiedXML = new StringBuilder();
			
						using (XmlWriter xw = XmlWriter.Create(modifiedXML, XMLSettings()))
			{
				XElement project = XElement.Parse(vs10ProjectContent);
				IEnumerable<XElement> filerefsElts = FindFileReferences(project);
				if (filerefsElts != null)
				{
					XElement foundRefElt = (from elt in filerefsElts
											where (NameInsideAttribute(elt) == newRef.Name)
											select elt).First();
					if (foundRefElt != null)
					{
						UpdateReference(newRef, foundRefElt);
						project.Save(xw);
					}
				}
			}
			return modifiedXML.ToString();
		}

		private void UpdateReference(FileReference newRef, XElement foundRefElt)
		{
			XElement pathElt = foundRefElt.Elements(Namespace + PathTag).FirstOrDefault();
			pathElt.Value = newRef.Path;
			XAttribute attr = foundRefElt.Attribute(IncludeAttr);
			attr.Value = attr.Value.Replace(VersionInsideAttribute(foundRefElt), newRef.Version);
		}

		private static XmlWriterSettings XMLSettings()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = false;
			settings.NewLineOnAttributes = true;
			return settings;
		}
	}
}
