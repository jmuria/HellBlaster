using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;
using System.Reflection;
using System.IO;

namespace HellBlasterTest.Tests.Domain
{
	[TestFixture]
	public class VersionRetrieverTests
	{
		[Test]
		public void WhenTheFilePathIsNullTheVersionIsNull()
		{
			VersionRetriever vt = new VersionRetriever();
			Assert.IsNull(vt.FileVersion(null));
		}

		[Test]
		public void WhenTheStringIsNotAPathTheVersionIsNull()
		{
			VersionRetriever vt = new VersionRetriever();
			Assert.IsNull(vt.FileVersion("Pipapaparopo"));
		}

		[Test]
		public void WhenTheFileDoesNotExistTheVersionIsNull()
		{
			VersionRetriever vt = new VersionRetriever();
			Assert.IsNull(vt.FileVersion("Pipapaparopo"));
		}

		[Test]
		public void WhenTheFileDoesNotHaveVersionTheVersionIsNull()
		{
			VersionRetriever vt = new VersionRetriever();
			Assert.IsNull(vt.FileVersion(Path.Combine(Environment.GetEnvironmentVariable("windir"),"bootstat.dat")));
		}

		[Test]
		public void WhenTheFileDoesHaveVersionTheVersionIsRetrieved()
		{
			VersionRetriever vt = new VersionRetriever();			
			string version = vt.FileVersion(Assembly.GetCallingAssembly().Location);//nunit.framework.dll 2.5.9.10348		
			Assert.IsNotNull(version);
			Assert.AreEqual("2.5.9.10348", version);
		}

	}
}
