using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using DG.Tweening;
using Unity.Burst.Intrinsics;

public class IapControl : MonoBehaviour, IStoreListener
{

	public static IapControl Instance;

	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;

	Action action;

	private bool isInitializing = false;
	private bool isInitialized = false;
	private int initializationCount = 0;

	[SerializeField]
	private CanvasGroup cvLoadingIAP;
	[SerializeField]
	private GameObject objLoadingIAP;
	[SerializeField]
	private RectTransform rectTrsSuccessPurchase;
	[SerializeField]
	private GameObject objSuccessPurchase;
	[SerializeField]
	private RectTransform rectTrsFailPurchase;
	[SerializeField]
	private GameObject objFailPurchase;
	[SerializeField]
	[Sirenix.OdinInspector.ShowInInspector]
	public List<Action> actionUpdate;
	public List<string> actionUpdatesstr;
	public bool isReady = false;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		actionUpdate = new List<Action>();
		actionUpdatesstr = new List<string>();
		Instance = this;
	}

	void Start()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}

	}

	void Update()
	{
		if (actionUpdate != null && actionUpdate.Count > 0)
		{
			for (int i = 0; i < actionUpdate.Count; i++)
			{
				Debug.Log(actionUpdatesstr[i]);
				actionUpdate[i]();
			}
			Debug.Log(actionUpdate.Count);
			actionUpdate.Clear();
			//actionUpdatesstr.Clear();
			Debug.Log(actionUpdate.Count);
		}
	}

	private void ActiveLoadingPurchase()
	{
		if (!objLoadingIAP.activeSelf)
		{
			objLoadingIAP.SetActive(true);
		}
		if (DOTween.IsTweening(cvLoadingIAP))
		{
			DOTween.Kill(cvLoadingIAP);
		}
		cvLoadingIAP.alpha = 0.1f;
		cvLoadingIAP.DOFade(1, 0.3f).SetUpdate(true);
	}

	private void DeactiveLoadingPurchase()
	{
		if (DOTween.IsTweening(cvLoadingIAP))
		{
			DOTween.Kill(cvLoadingIAP);
		}
		cvLoadingIAP.DOFade(0, 0.3f).SetUpdate(true).OnComplete(
			() =>
			{
				if (objLoadingIAP.activeSelf)
				{
					objLoadingIAP.SetActive(false);

				}
			}
			);
	}

	private void ActivePurchaseSuccess()
	{
		if (!objSuccessPurchase.activeSelf)
		{
			objSuccessPurchase.SetActive(true);
		}

		if (DOTween.IsTweening(rectTrsSuccessPurchase))
		{
			DOTween.Kill(rectTrsSuccessPurchase);
		}

		rectTrsSuccessPurchase.anchoredPosition = new Vector2(0, 0);
		rectTrsSuccessPurchase.DOAnchorPosY(1, 3f).SetUpdate(true).OnComplete(() =>
		{
			//objSuccessPurchase.SetActive(false);
			//rectTrsSuccessPurchase.DOAnchorPosY(1000, 0.3f).SetUpdate(true).SetDelay(1.5f);
		});
	}

	private void ActivePurchaseFaill()
	{
		if (!objFailPurchase.activeSelf)
		{
			objFailPurchase.SetActive(true);
		}

		if (DOTween.IsTweening(rectTrsFailPurchase))
		{
			DOTween.Kill(rectTrsFailPurchase);
		}

		rectTrsFailPurchase.anchoredPosition = new Vector2(0, 0);
		rectTrsFailPurchase.DOAnchorPosY(1, .3f).SetUpdate(true).OnComplete(() =>
		{
			//objFailPurchase.SetActive(false);
			//rectTrsFailPurchase.DOAnchorPosY(1000, 0.3f).SetUpdate(true).SetDelay(1.5f);
			//isReady = false;
		});
	}

	public void InitializePurchasing()
	{
		if (IsInitialized())
		{
			return;
		}

		if (isInitializing) return;
		if (isInitialized) return;
		if (initializationCount >= 5) return;
		isInitializing = true;
		initializationCount++;

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(PackName.remove_ads.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_10.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_30.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_60.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_130.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_250.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.magic_600.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.power_15.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.power_45.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.power_90.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.combo.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.remove_ads_pack.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.day_vip_3.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.day_vip_7.ToString(), ProductType.Consumable);
		builder.AddProduct(PackName.day_vip_30.ToString(), ProductType.Consumable);
		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyProductID(PackName productId, Action _action)
	{
		if (IsInitialized() && isInitialized && !isInitializing)
		{
			Product product = m_StoreController.products.WithID(productId.ToString());

			Debug.Log(1111111);

			action = _action;
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				actionUpdate.Add(ActiveLoadingPurchase);
				actionUpdatesstr.Add("ActiveLoadingPurchase");
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				actionUpdate.Add(ActivePurchaseFaill);
				actionUpdatesstr.Add("ActivePurchaseFaill");
			}
		}
		else
		{
			//Tracker.LogEvent(new Feature_IAP_FAIL()
			//{
			//    eventName = Feature_IAP_FAIL.EVENT_NAME.iap_fail,
			//    action_name = Feature_IAP_FAIL.ACTION_NAME._purchase,
			//    product_id = productId,
			//    fail_type = "Not initialized"
			//});

			//Tracker.LogEvent(new Feature_IAP_FAIL()
			//{
			//    eventName = Feature_IAP_FAIL.EVENT_NAME.iap_fail,
			//    action_name = Feature_IAP_FAIL.ACTION_NAME._purchase,
			//    product_id = productId,
			//    fail_type = (Application.internetReachability == NetworkReachability.NotReachable) ?
			//    "Not initialized_NoInternet" : "Not initialized_Internet"
			//});
			Debug.Log("BuyProductID FAIL. Not initialized.");

			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				InitializePurchasing();
			}

			actionUpdate.Add(ActivePurchaseFaill);
			actionUpdatesstr.Add("ActivePurchaseFaill");
		}
	}

	public string getPrice(PackName packName)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(packName.ToString());
			if (product != null)
			{
				return product.metadata.localizedPriceString;
			}
			return null;
		}
		else
		{
			return null;
		}
	}

	public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				InitializePurchasing();
			}
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
			apple.RestoreTransactions((result, message) =>
			{
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				UIManager.instance.shopPanel.restorePanel.Open();
				UIManager.instance.shopPanel.restorePanel.getText("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			UIManager.instance.shopPanel.restorePanel.Open();
			UIManager.instance.shopPanel.restorePanel.getText("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		isInitializing = false;
		isInitialized = true;
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		//if (LoadingControl.instance != null && LoadingControl.instance.gameObject != null)
		//{
		//    LoadingControl.instance.DeactiveLoadingIap();
		//}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		// A consumable product has been purchased by this user.
		bool validPurchase = true;

		//#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR 
		//        //Prepare the validator with the secrets we prepared in the Editor
		//        // obfuscation window.
		//        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
		//                                  AppleTangle.Data(), Application.identifier);

		//        try
		//        {
		//            //On Google Play, result has a single product ID.
		//            // On Apple stores, receipts contain multiple products.
		//            var result = validator.Validate(args.purchasedProduct.receipt);
		//            // For informational purposes, we list the receipt(s)
		//            Debug.Log("Receipt is valid. Contents:");
		//            foreach (IPurchaseReceipt productReceipt in result)
		//            {
		//                Debug.Log(productReceipt.productID);
		//                Debug.Log(productReceipt.purchaseDate);
		//                Debug.Log(productReceipt.transactionID);
		//            }
		//        }
		//        catch (IAPSecurityException)
		//        {
		//            Debug.Log("Invalid receipt, not unlocking content");
		//            validPurchase = false;
		//        }
		//#endif
		if (validPurchase)
		{
			Debug.Log(args.purchasedProduct.definition.id + "..succeed");
			Debug.Log(actionUpdatesstr);
			Debug.Log(actionUpdatesstr.Count);
			actionUpdatesstr.Add("action");
			actionUpdate.Add(action);
			actionUpdatesstr.Add("ActivePurchaseSuccess");
			actionUpdate.Add(ActivePurchaseSuccess);


			//if (args.purchasedProduct.definition.id == "remove_ads")
			//{
			//    PlayerPrefs.SetInt("removeads", 1);
			//    //     FirebaseControl.instance.LogUserProperties("durable_remove_ads", "0");
			//}


			//   FirebaseControl.instance.LogEventIapFB(args.purchasedProduct.metadata.isoCurrencyCode, args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.definition.id);

			//AdjustEvent adjustEvent = new AdjustEvent("pwy6sj");
			//adjustEvent.setRevenue((double)args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.metadata.isoCurrencyCode);

			//Adjust.trackEvent(adjustEvent);


			//FirebaseControl.instance.LogEventIAP3days(0);
		}
		actionUpdate.Add(DeactiveLoadingPurchase);
		actionUpdatesstr.Add("DeactiveLoadingPurchase");
		Debug.Log(actionUpdate.Count);
		return PurchaseProcessingResult.Complete;
	}
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		actionUpdate.Add(DeactiveLoadingPurchase);
		actionUpdatesstr.Add("DeactiveLoadingPurchase");
		Debug.Log(actionUpdate.Count);
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

		//Tracker.LogEvent(new Feature_IAP_FAIL()
		//{
		//    eventName = Feature_IAP_FAIL.EVENT_NAME.iap_fail,
		//    action_name = Feature_IAP_FAIL.ACTION_NAME._purchase,
		//    product_id = product.definition.storeSpecificId,
		//    fail_type = failureReason.ToString()
		//});
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		isInitializing = false;
		isInitialized = false;

		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		isInitializing = false;
		isInitialized = false;

		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}
	public void FailTapToClose()
	{
		objFailPurchase.SetActive(false);
		rectTrsFailPurchase.DOAnchorPosY(1000, 0.3f).SetUpdate(true).SetDelay(1.5f);
	}
	public void SuccessTapToClose()
	{
		objSuccessPurchase.SetActive(false);
		rectTrsSuccessPurchase.DOAnchorPosY(1000, 0.3f).SetUpdate(true).SetDelay(1.5f);
	}
}


public enum PackName
{
	remove_ads,
	magic_10,
	magic_30,
	magic_60,
	magic_130,
	magic_250,
	magic_600,
	power_15,
	power_45,
	power_90,
	combo,
	remove_ads_pack,
	day_vip_3,
	day_vip_7,
	day_vip_30
}

