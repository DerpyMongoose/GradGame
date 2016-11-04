using UnityEngine;
using System.Collections;

public class IAPObjects: MonoBehaviour {

	// Use this for initialization
	public void ShowObjects (bool exitable) {
		//show ();
		//his.isExitable = exitable;
	}
	
	public void Buy50Objects(){
		IAPManager.Instance.Buy50Objects();
	}
	public void Buy80Objects(){
		IAPManager.Instance.Buy80Objects();
	}
	public void Buy120Objects(){
		IAPManager.Instance.BuyNoAds ();
	}
}
