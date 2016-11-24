using System;
using NUnit.Framework;
using UnityEngine;


namespace UnityTest{
	[TestFixture]
	internal class UnitSampleTest {

		[Test]
		public void SimpleCamTest(){
			var test = GameManager.instance.audioManager;
			Assert.That (test.findSth()!=null);

		}
		
		[Test]
		public void SimpleAddition(){
			Assert.That (2 + 2 == 4);
		}
	}
}