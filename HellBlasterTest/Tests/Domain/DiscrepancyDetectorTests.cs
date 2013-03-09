using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests.Domain
{
	[TestFixture]
	public class DiscrepancyDetectorTests
	{

		[Test]
		public void WhenThereIsNoReferenceThereIsNoDiscrepancy()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			Assert.AreEqual(0, dc.Discrepancies().Count);
		}


		[Test]
		public void WhenThereIsAOnlyReferenceThereIsNoDiscrepancy()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework","2.4",String.Empty));
			Assert.AreEqual(0, dc.Discrepancies().Count);
		}

		[Test]
		public void WhenThereAreTwoDifferentReferencesThereIsNoDiscrepancy()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("core", "1.0", String.Empty));
			Assert.AreEqual(0, dc.Discrepancies().Count);
		}

		[Test]
		public void WhenThereAreTwoReferencesWithTheSameVersionThereIsNoDiscrepancy()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			Assert.AreEqual(0, dc.Discrepancies().Count);
		}

		[Test]
		public void WhenThereAreTwoSameReferencesWithTheDifferentVersionThereAreTwoDiscrepancies()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.5", String.Empty));
			Assert.AreEqual(2, dc.Discrepancies().Count);			
		}

		[Test]
		public void WhenThereAreTwoSameReferencesWithTheDifferentVersionAndMoreReferencesThereAreTwoDiscrepancies()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("core", "1.0", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.5", String.Empty));
			Assert.AreEqual(2, dc.Discrepancies().Count);
		}

		[Test]
		public void WhenThereAreThreeSameReferencesWithTheDifferentVersionAndMoreReferencesThereAreThreeDiscrepancies()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("core", "1.0", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.5", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.6", String.Empty));
			Assert.AreEqual(3, dc.Discrepancies().Count);
			Assert.AreEqual("nunit.framework", dc.Discrepancies()[0].Name);
			Assert.AreEqual("2.4", dc.Discrepancies()[0].Version);
			Assert.AreEqual("nunit.framework", dc.Discrepancies()[1].Name);
			Assert.AreEqual("2.5", dc.Discrepancies()[1].Version);
			Assert.AreEqual("nunit.framework", dc.Discrepancies()[2].Name);
			Assert.AreEqual("2.6", dc.Discrepancies()[2].Version);
		}


		[Test]
		public void WhenTheSameReferenceIsCheckedTheNumberOfDiscrepanciesDoesNotIncrease()
		{
			DiscrepancyDetector dc = new DiscrepancyDetector();
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			dc.AddReference(new FileReference("core", "1.0", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.5", String.Empty));
			dc.AddReference(new FileReference("nunit.framework", "2.4", String.Empty));
			Assert.AreEqual(2, dc.Discrepancies().Count);
			Assert.AreEqual("nunit.framework", dc.Discrepancies()[0].Name);
			Assert.AreEqual("2.4", dc.Discrepancies()[0].Version);
			Assert.AreEqual("nunit.framework", dc.Discrepancies()[1].Name);
			Assert.AreEqual("2.5", dc.Discrepancies()[1].Version);			
		}
	}
}

