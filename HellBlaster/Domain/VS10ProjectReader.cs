﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace HellBlaster.Domain
{
	public class VS10ProjectReader
	{

		protected string ReferenceTag		{ get { return "Reference"; } }
		protected string ProjectTag			{ get { return "Project"; } }
		protected string VersionAttribute	{ get { return "Version"; } }
		protected string PathTag			{ get { return "HintPath"; } }
		protected string IncludeAttr		{ get { return "Include"; } }
		private string FileContent;
		public string LastError { get; protected set; }


		protected XNamespace Namespace
		{
			get
			{
				XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
				return ns;
			}
		}

		public List<FileReference> FindReferences(string projectfilecontent)
		{
			try
			{
				List<FileReference> foundFileRefs = null;
				IEnumerable<XElement> filerefs=	FindFileReferences(projectfilecontent);

				if (filerefs != null)
				{
					foundFileRefs = new List<FileReference>();
					foreach (XElement fref in filerefs)
					{
						string refPath = ReferencePath(fref);
						if(!String.IsNullOrEmpty(refPath))
							foundFileRefs.Add(new FileReference(NameInsideAttribute(fref),VersionInsideAttribute(fref), ReferencePath(fref)));
					}
				}
				return foundFileRefs;
			}
			catch (ArgumentNullException)
			{
				return null;
			}
			catch(System.Xml.XmlException)
			{
				return null;
			}
		}

		protected IEnumerable<XElement> FindFileReferences(string projectfilecontent)
		{
			XElement project = XElement.Parse(projectfilecontent);
			return FindFileReferences(project);
		}

		protected IEnumerable<XElement> FindFileReferences(XElement rootElement)
		{

			if (IsAVisualStudioProject(rootElement))
				return AllReferencesIntheProject(rootElement);
			else
				return null;
		}

		protected bool IsAVisualStudioProject(XElement project)
		{
			return project != null && project.Name.LocalName == ProjectTag;
		}

		protected IEnumerable<XElement> AllReferencesIntheProject(XElement project)
		{
			return from fref in project.Descendants(Namespace + ReferenceTag)
				   where HasReferencePath(fref)
				   select fref  ;
		}


		protected string NameInsideAttribute(XElement fref)
		{
			string includeStr = fref.Attribute(IncludeAttr).Value;
			
			int posFinal = includeStr.IndexOf(',');
			return includeStr.Substring(0, posFinal);
		}

		protected string ReferencePath(XElement fref)
		{
			return PathElement(fref).Value;
		}

		private bool HasReferencePath(XElement fref)
		{
			return PathElement(fref) != null;
		}

		private XElement PathElement(XElement fref)
		{
			return fref.Elements(Namespace + PathTag).FirstOrDefault();
		}

		protected string VersionInsideAttribute(XElement fref)
		{
			string includeStr = fref.Attribute(IncludeAttr).Value;
			int pos = includeStr.IndexOf(VersionAttribute) + VersionAttribute.Length+1;
			int posFinal = includeStr.IndexOf(',', pos);

			return includeStr.Substring(pos, posFinal-pos);
		}



		public void Read(string projectPath)
		{
			try
			{
				FileContent = new FileInfo(projectPath).OpenText().ReadToEnd();
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}
		}

		public List<FileReference> FindReferences()
		{
			return FindReferences(FileContent);
		}
	}
}
