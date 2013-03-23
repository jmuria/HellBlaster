using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using HellBlaster.Domain;
using HellBlaster.Interfaces;

namespace HellBlaster.VS10
{
	public class VS10ProjectReader
	{

		public IVersionRetriever VersionRetriever { get; set; }

		protected string ReferenceTag		{ get { return "Reference"; } }
		protected string ProjectTag			{ get { return "Project"; } }
		protected string VersionAttribute	{ get { return "Version"; } }
		protected string PathTag			{ get { return "HintPath"; } }
		protected string IncludeAttr		{ get { return "Include"; } }
		protected string FileContent;
		protected string LoadedProjectPath;
		public string LastError { get; protected set; }


		public VS10ProjectReader()
		{
			VersionRetriever = new VersionRetriever();
		}

		protected XNamespace Namespace
		{
			get
			{
				XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
				return ns;
			}
		}

		public List<FileReference> FindFileReferences(string projectfilecontent)
		{
			try
			{
				List<FileReference> foundFileRefs = null;
				IEnumerable<XElement> filerefs=	FindFileReferenceElements(projectfilecontent);

				if (filerefs != null)
				{
					foundFileRefs = new List<FileReference>();
					foreach (XElement fileRefInXML in filerefs)
					{
						string refPath = ReferencePath(fileRefInXML);
						if(!String.IsNullOrEmpty(refPath))
							foundFileRefs.Add(CreateFileRefFromXML(fileRefInXML));
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

		private FileReference CreateFileRefFromXML(XElement fileRefInXML)
		{			

			return new FileReference(NameInsideAttribute(fileRefInXML), FindRightVersion(fileRefInXML), ReferencePath(fileRefInXML));
		}

		private string FindRightVersion(XElement fileRefInXML)
		{
			string version = String.Empty;
			if (VersionRetriever != null)
				version = VersionRetriever.FileVersion(ReferencePath(fileRefInXML));

			if (String.IsNullOrEmpty(version))
				version = VersionInsideAttribute(fileRefInXML);
			return version;
		}

		protected IEnumerable<XElement> FindFileReferenceElements(string projectfilecontent)
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

			return includeStr.Split(',')[0];
			//return includeStr.Substring(0, posFinal);
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
			
			string[] IncludeAttrElements = includeStr.Split(',');
			if (IncludeAttrElements.Count() > 1)
				return IncludeAttrElements[1].Split('=')[1];
			else
				return null;
			
		}



		public void Read(string projectPath)
		{
			try
			{
				LoadedProjectPath = projectPath;
				using(StreamReader reader=new FileInfo(projectPath).OpenText())
					FileContent = reader.ReadToEnd();
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}
		}

		public List<FileReference> FindFileReferences()
		{
			return FindFileReferences(FileContent);
		}

		
	}
}
