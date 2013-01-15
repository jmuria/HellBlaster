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


		[Test]
		public void WhenTheStringIsEmptyIGetANUllReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindReferences(null as string));
		}


		[Test]
		public void WhenTheStringIsNotAnXMLIGetANullReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindReferences("This is not a xml"));
		}

		[Test]
		public void WhenTheStringIsNotAnVS10FileIGetANullReferenceList()
		{
			VS10SolutionReader pr = new VS10SolutionReader();
			Assert.IsNull(pr.FindReferences("Microsoft Visual Studio Solution File, Format Version 9.00"));
		}

		[Test]
		public void WhenTheStringContainsAProjectIGetReferenceListWithOneItem()
		{
			VS10SolutionReader pr = new VS10SolutionReader();

			List<VS10Project> refs = pr.FindReferences(vs10withOneProject);
			Assert.IsNotNull(refs);
			Assert.AreEqual(1, refs.Count);
		}
	}
}
