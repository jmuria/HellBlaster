using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	class ReferenceInProjectUpdaterTests
	{
		string vs10withOneFileReference;

		[SetUp]
		public void SetUp()
		{
			vs10withOneFileReference = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
			  "<Project ToolsVersion=\"4.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">"
				  + "<DebugType>pdbonly</DebugType>"
				  + "<Optimize>true</Optimize>"
				  + "<OutputPath>bin\\Release\\</OutputPath>"
				  + "<DefineConstants>TRACE</DefineConstants>"
				  + "<ErrorReport>prompt</ErrorReport>"
				  + "<WarningLevel>4</WarningLevel>"
				+ "<ItemGroup>    "
			  + "    <Reference Include=\"nunit.framework, Version=2.6.1.12217, Culture=neutral, PublicKeyToken=96d09a1eb7f44a77, processorArchitecture=MSIL\">"
					+ "<SpecificVersion>False</SpecificVersion>"
					+ "<HintPath>libs\\nunit.framework.dll</HintPath>"
				  + "</Reference>    "
				+ "</ItemGroup>"
			  + "</Project>";
		}

		[Test]
		public void WhenIUpdateTheSameReferenceInAProjectTheReferenceIsTheSame()
		{

			VS10ProjectWriter pr = new VS10ProjectWriter(vs10withOneFileReference);

			FileReference newRef = new FileReference("nunit.framework","2.6.1.12217",@"libs\nunit.framework.dll");
			String projectXML = pr.UpdateReference(newRef);

			VS10ProjectReader prReader = new VS10ProjectReader();
			List<FileReference> refs = prReader.FindFileReferences(projectXML);
			Assert.AreEqual(@"libs\nunit.framework.dll", refs[0].Path);
			Assert.AreEqual("2.6.1.12217", refs[0].Version);
		}


		[Test]
		public void WhenIUpdateANewReferenceInAProjectTheReferenceChanges()
		{

			VS10ProjectWriter pr = new VS10ProjectWriter(vs10withOneFileReference);

			FileReference newRef = new FileReference("nunit.framework","2.7", @"libs\2.7\nunit.framework.dll");
			String projectXML=pr.UpdateReference(newRef);

			VS10ProjectReader prReader = new VS10ProjectReader();
			List<FileReference> refs = prReader.FindFileReferences(projectXML);
			Assert.AreEqual(@"libs\2.7\nunit.framework.dll", refs[0].Path);
			Assert.AreEqual("2.7", refs[0].Version);
			Assert.AreEqual("nunit.framework", refs[0].Name);
		}
	}
}
