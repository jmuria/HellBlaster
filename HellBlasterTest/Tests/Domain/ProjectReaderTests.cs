using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.VS10;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class VS10ProjectReaderTests
	{

		string vs10withOneFileReference;
		string vs10withTwoFileReferences;

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


			 vs10withTwoFileReferences = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
 "<Project ToolsVersion=\"4.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\r\n" +
   "<PropertyGroup>\r\n" +
	 "<Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>\r\n" +
	 "<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>    \r\n" +
   "</PropertyGroup>\r\n" +
  "\r\n" +
 "  <ItemGroup>\r\n" +
	 "<Reference Include=\"Microsoft.VisualStudio.QualityTools.UnitTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL\" />\r\n" +
	 "<Reference Include=\"NSubstitute\">\r\n" +
	   "<HintPath>..\\packages\\NSubstitute.1.4.2.0\\lib\\NET40\\NSubstitute.dll</HintPath>\r\n" +
	 "</Reference>\r\n" +
	 "<Reference Include=\"nunit.framework, Version=2.6.1.12217, Culture=neutral, PublicKeyToken=96d09a1eb7f44a77, processorArchitecture=MSIL\">\r\n" +
	   "<SpecificVersion>False</SpecificVersion>\r\n" +
	   "<HintPath>Libraries\\nunit.framework.dll</HintPath>\r\n" +
	 "</Reference>\r\n" +
	 "<Reference Include=\"System\" />\r\n" +
	 "<Reference Include=\"System.Core\">\r\n" +
	   "<RequiredTargetFramework>3.5</RequiredTargetFramework>\r\n" +
	 "</Reference>\r\n" +
   "</ItemGroup>\r\n" +
   "<ItemGroup>\r\n" +
	 "<CodeAnalysisDependentAssemblyPaths Condition=\" '$(VS100COMNTOOLS)' != '' \" Include=\"$(VS100COMNTOOLS)..\\IDE\\PrivateAssemblies\">\r\n" +
	   "<Visible>False</Visible>\r\n" +
	 "</CodeAnalysisDependentAssemblyPaths>\r\n" +
   "</ItemGroup>\r\n" +
 "</Project>\r\n";



		}

		[Test]
		public void WhenTheStringIsEmptyIGetANUllReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindFileReferences(null as string));
		}


		[Test]
		public void WhenTheStringIsNotAnXMLIGetANullReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindFileReferences("This is not a xml"));
		}


		[Test]
		public void WhenTheStringIsNotAnVS10XMLIGetANullReferenceList()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			Assert.IsNull(pr.FindFileReferences("<xml><sample>10</sample></xml>"));
		}


		[Test]
		public void WhenTheStringContainsARerennceIGetReferenceListWithOneItem()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			
			List<FileReference> refs = pr.FindFileReferences(vs10withOneFileReference);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1,refs.Count);
		}

		[Test]
		public void WhenTheReferenceHasAVersionICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();
			
			List<FileReference> refs = pr.FindFileReferences(vs10withOneFileReference);
			Assert.AreEqual("2.6.1.12217", refs[0].Version);
		}

		[Test]
		public void WhenTheReferenceHasAFilePathICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();

			List<FileReference> refs = pr.FindFileReferences(vs10withOneFileReference);
			Assert.AreEqual(@"libs\nunit.framework.dll", refs[0].Path);
		}

		[Test]
		public void WhenTheReferenceHasANameICanRetrieveIt()
		{
			VS10ProjectReader pr = new VS10ProjectReader();

			List<FileReference> refs = pr.FindFileReferences(vs10withOneFileReference);
			Assert.AreEqual("nunit.framework", refs[0].Name);
		}

		[Test]
		public void WhenTheProjectHasTwoReferencesICanRetrieveThem()
		{
			VS10ProjectReader pr = new VS10ProjectReader();

			List<FileReference> refs = pr.FindFileReferences(vs10withTwoFileReferences);
			Assert.AreEqual("NSubstitute", refs[0].Name);
			Assert.AreEqual("nunit.framework", refs[1].Name);
		}

		
	}
}
