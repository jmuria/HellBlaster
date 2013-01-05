using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class VS10ProjectReaderTests
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
		public void WhenTheStringIsEmptyIGetANUllReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindReferences(null as string));
		}


		[Test]
		public void WhenTheStringIsNotAnXMLIGetANullReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindReferences("This is not a xml"));
		}


		[Test]
		public void WhenTheStringIsNotAnVS10XMLIGetANullReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindReferences("<xml><sample>10</sample></xml>"));
		}


		[Test]
		public void WhenTheStringContainsARerennceIGetReferenceListWithOneItem()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			
			List<FileReference> refs = pr.FindReferences(vs10withOneFileReference);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1,refs.Count);
		}

		[Test]
		public void WhenTheReferenceHasAVersionICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			
			List<FileReference> refs = pr.FindReferences(vs10withOneFileReference);
			Assert.AreEqual("2.6.1.12217", refs[0].Version);
		}

		[Test]
		public void WhenTheReferenceHasAFilePathICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();

			List<FileReference> refs = pr.FindReferences(vs10withOneFileReference);
			Assert.AreEqual(@"libs\nunit.framework.dll", refs[0].Path);
		}

		[Test]
		public void WhenTheReferenceHasANameICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();

			List<FileReference> refs = pr.FindReferences(vs10withOneFileReference);
			Assert.AreEqual("nunit.framework", refs[0].Name);
		}
	}
}
