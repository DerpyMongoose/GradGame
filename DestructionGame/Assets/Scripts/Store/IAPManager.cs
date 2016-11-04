using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {

	public static IAPManager Instance { set; get; }

	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;

	public static string OBJECT_50 = "object50";
	public static string OBJECT_80 = "object80";
	public static string OBJECT_120 = "noads120";

	private void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		// if unity purchasing reference is not setup 
		if (m_StoreController == null){

			// start to configure connection to purchase
			InitializePurchasing ();
		}
	}

	void InitializePurchasing(){
		//if we have already connected to purchase
		if(IsInitialized()){
			// done purchase
			return;
		}

		// create a builder passing in unity store
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		builder.AddProduct (OBJECT_50, ProductType.Consumable);
		builder.AddProduct (OBJECT_80, ProductType.Consumable);
		builder.AddProduct (OBJECT_120, ProductType.NonConsumable);

		UnityPurchasing.Initialize (this, builder);
	}

	private bool IsInitialized(){
		//check if both purchasing references are initialized
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void Buy50Objects(){
		BuyProductID (OBJECT_50);
	}

	public void Buy80Objects(){
		BuyProductID (OBJECT_80);
	}

	public void BuyNoAds(){
		BuyProductID (OBJECT_120);
	}

	void BuyProductID(string productId){
		
		if (IsInitialized ()) {
			Product product = m_StoreController.products.WithID (productId);
		
			// if lookup found product for this device's store
			if (product != null && product.availableToPurchase) {
				Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase (product);
			} else {
				Debug.Log ("BuyProductID: FAIL. Not purchasing, either not found or is not available");
			}
		} else {
			Debug.Log ("BuyProductID FAIL. Not initialized");
		}

	}

	// Restore purchase for apple specifically:
	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, OBJECT_50, StringComparison.Ordinal))
		{
			Debug.Log("You bought 50 objects, have fun");
			// The consumable item has been successfully purchased, add 50 object to the player's in-game score.
			/**** ScoreManager.score += 100;***/
		}
		// Or ... a non-consumable product has been purchased by this user.
		else if (String.Equals(args.purchasedProduct.definition.id, OBJECT_80, StringComparison.Ordinal))
		{
			Debug.Log("You bought 80 objects, thanks");
			//Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
		}
		// Or ... a subscription product has been purchased by this user.
		else if (String.Equals(args.purchasedProduct.definition.id, OBJECT_120, StringComparison.Ordinal))
		{
			Debug.Log("You bought your way out of ads - COOL ");
			//Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			// TODO: The subscription item has been successfully purchased, grant this to the player.
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		// Return a flag indicating whether this product has completely been received
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
