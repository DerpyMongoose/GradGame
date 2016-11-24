using System;
using NUnit.Framework;
using UnityEngine;


namespace UnityTest{
	[TestFixture]
	internal class UnitSampleTest {

		[Test]
		public void SimpleCamTest(){
			var objManager = new ObjectManagerV2 ();
			objManager.mediumGlassLife = 10;
			var objBehavior = new ObjectBehavior ();
			objBehavior.life = objManager.mediumGlassLife;
			Assert.AreEqual (10, objBehavior.life);

			objManager.objDamage = 7;
			objBehavior.life -= objManager.objDamage;
			Assert.AreEqual (3, objBehavior.life);
		}
		
		[Test]
		public void SimpleAddition(){
			var obj = new ObjectBehaviourTest();
			obj.life = 10;
			obj.TestCollisionEnter (7);
			Assert.AreEqual (3, obj.life);
		}
	}
}


public partial class ObjectBehaviourTest{
	public int life;

	public void TestCollisionEnter (int objDamage){
		life -= objDamage;
	}
}