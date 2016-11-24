using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class UnitSampleTest {

	[Test]
	public void SimpleCamTest(){
		//Assert.That (2 + 2 == 4);
		var test = GameObject.FindObjectOfType<AutoPowerupScript>() as AutoPowerupScript;
		Assert.That (test.findSth()!=null);

	}

	public void RunUnitTest(){
		SimpleCamTest ();
	}
}
