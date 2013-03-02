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
	public class ReferenceTests
	{
		[Test]
		public void OneDefaultReferenceHas100AsVersion()
		{
			FileReference refe = new FileReference();
			Assert.AreEqual("1.0.0", refe.Version);
		}

		[Test]
		public void ICanCreateAReferenceWithOneSpecificVersion()
		{
			FileReference refe = new FileReference("2.0.0");
			Assert.AreEqual("2.0.0", refe.Version);
		}


		[Test]
		public void ICanUpdateAReferenceWithANewVersion()
		{
			FileReference refe = new FileReference("2.0.0");
			refe.UpdateTo("2.5.0");
			Assert.AreEqual("2.5.0", refe.Version);
		}

		[Test]
		public void ICanCreateAReferenceWithOneSpecificVersionAndPath()
		{
			FileReference refe = new FileReference("2.0.0",@"R:\libs\mylib.dll");
			Assert.AreEqual("2.0.0", refe.Version);
			Assert.AreEqual(@"R:\libs\mylib.dll", refe.Path);
		}

		[Test]
		public void ICanCreateAReferenceWithOneSpecificNameVersionAndPath()
		{
			FileReference refe = new FileReference("mylib","2.0.0", @"R:\libs\mylib.dll");
			Assert.AreEqual("2.0.0", refe.Version);
			Assert.AreEqual(@"R:\libs\mylib.dll", refe.Path);
			Assert.AreEqual("mylib", refe.Name);
		}
	}
}
