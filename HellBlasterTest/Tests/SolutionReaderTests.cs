using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class SolutionReaderTests
	{
		private string vs10withOneProject=
"Microsoft Visual Studio Solution File, Format Version 11.00\r\n"+
"# Visual Studio 2010\r\n"+
"Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"HellBlaster\", \"HellBlaster\\HellBlaster.csproj\", \"{D2AB4C1B-7F1A-452A-BE7B-A432C25C8316}\"\r\n"+
"EndProject\r\n"+
"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Solution Items\", \"Solution Items\", \"{9DAEDB08-A3A9-411A-88C6-00337FAEE0EE}\"\r\n"+
	"\tProjectSection(SolutionItems) = preProject\r\n"+
		"\t\tHellBlaster.vsmdi = HellBlaster.vsmdi\r\n"+
		"\t\tLocal.testsettings = Local.testsettings\r\n"+
		"\t\tTraceAndTestImpact.testsettings = TraceAndTestImpact.testsettings\r\n"+
	"\tEndProjectSection\r\n"+
"EndProject\r\n"+
"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"libs\", \"libs\", \"{F6B6A0B3-6182-4C34-83EC-AD41CD215CF1}\"\r\n"+
"EndProject\r\n"+
"Global\r\n"+
	"\tGlobalSection(TestCaseManagementSettings) = postSolution\r\n"+
		"\t\tCategoryFile = HellBlaster.vsmdi\r\n"+
	"\tEndGlobalSection	\r\n"+
"EndGlobal\r\n";

		
private string vs10withTwoProjects=
	"Microsoft Visual Studio Solution File, Format Version 11.00\r\n"+
"# Visual Studio 2010\r\n"+
"Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"HellBlaster\", \"HellBlaster\\HellBlaster.csproj\", \"{D2AB4C1B-7F1A-452A-BE7B-A432C25C8316}\"\r\n"+
"EndProject\r\n"+
"Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"HellBlasterTest\", \"HellBlasterTest\\HellBlasterTest.csproj\", \"{40EE51B1-4815-42E8-A42A-8E67B3D20132}\"\r\n"+
"EndProject\r\n"+
"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Solution Items\", \"Solution Items\", \"{9DAEDB08-A3A9-411A-88C6-00337FAEE0EE}\"\r\n"+
	"ProjectSection(SolutionItems) = preProject\r\n"+
		"HellBlaster.vsmdi = HellBlaster.vsmdi\r\n"+
		"Local.testsettings = Local.testsettings\r\n"+
		"TraceAndTestImpact.testsettings = TraceAndTestImpact.testsettings\r\n"+
	"EndProjectSection\r\n"+
"EndProject\r\n"+
"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"libs\", \"libs\", \"{F6B6A0B3-6182-4C34-83EC-AD41CD215CF1}\"\r\n"+
"EndProject\r\n"+
"Global\r\n"+
	"GlobalSection(TestCaseManagementSettings) = postSolution\r\n"+
		"CategoryFile = HellBlaster.vsmdi\r\n"+
	"EndGlobalSection\r\n"+
	"GlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n"+
		"Debug|Any CPU = Debug|Any CPU\r\n"+
		"Debug|Mixed Platforms = Debug|Mixed Platforms\r\n"+
		"Debug|x86 = Debug|x86\r\n"+
		"Release|Any CPU = Release|Any CPU\r\n"+
		"Release|Mixed Platforms = Release|Mixed Platforms\r\n"+
		"Release|x86 = Release|x86\r\n"+
	"EndGlobalSection\r\n"+
	"GlobalSection(ProjectConfigurationPlatforms) = postSolution		\r\n"+
	"EndGlobalSection\r\n"+
	"GlobalSection(SolutionProperties) = preSolution\r\n"+
		"HideSolutionNode = FALSE\r\n"+
	"EndGlobalSection\r\n"+
	"GlobalSection(NestedProjects) = preSolution\r\n"+
		"{F6B6A0B3-6182-4C34-83EC-AD41CD215CF1} = {9DAEDB08-A3A9-411A-88C6-00337FAEE0EE}\r\n"+
	"EndGlobalSection\r\n"+
"EndGlobal\r\n";



		[Test]
		public void WhenTheStringIsEmptyIGetANUllReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindProjects(null as string));
		}


		[Test]
		public void WhenTheStringIsNotAnXMLIGetANullReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindProjects("This is not a xml"));
		}

		[Test]
		public void WhenTheStringIsNotAnVS10FileIGetANullReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindProjects("Microsoft Visual Studio Solution File, Format Version 9.00"));
		}

		[Test]
		public void WhenTheStringContainsAProjectIGetReferenceListWithOneItem()
		{
			VS10SolutionReader pr = new VS10SolutionReader();

			List<VS10Project> refs = pr.FindProjects(vs10withOneProject);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1, refs.Count);
		}

		[Test]
		public void TheSolutionReaderCanReadTheProjectName()
		{
			VS10SolutionReader pr = new VS10SolutionReader();

			List<VS10Project> refs = pr.FindProjects(vs10withOneProject);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1, refs.Count);
			Assert.AreEqual("HellBlaster", refs[0].Name);
		}

		[Test]
		public void TheSolutionReaderCanReadTheProjectRelativePath()
		{
			VS10SolutionReader pr = new VS10SolutionReader();

			List<VS10Project> refs = pr.FindProjects(vs10withOneProject);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1, refs.Count);
			Assert.AreEqual(@"HellBlaster\HellBlaster.csproj", refs[0].RelativePath);
		}


		[Test]
		public void TheSolutionReaderCanReadMoreThanOneProject()
		{
			VS10SolutionReader pr = new VS10SolutionReader();

			List<VS10Project> refs = pr.FindProjects(vs10withTwoProjects);
			Assert.IsNotNull(refs);
			Assert.AreEqual(2, refs.Count);
			Assert.AreEqual(@"HellBlaster\HellBlaster.csproj", refs[0].RelativePath);
			Assert.AreEqual("HellBlaster", refs[0].Name);
			Assert.AreEqual(@"HellBlasterTest\HellBlasterTest.csproj", refs[1].RelativePath);
			Assert.AreEqual("HellBlasterTest", refs[1].Name);
		}
	}
}
