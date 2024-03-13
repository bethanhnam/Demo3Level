﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using DG.Tweening;

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

    private List<Action> actionUpdate;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        actionUpdate = new List<Action>();
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
                actionUpdate[i]();
            }
            actionUpdate.Clear();
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

        rectTrsSuccessPurchase.anchoredPosition = new Vector2(0, 200);
        rectTrsSuccessPurchase.DOAnchorPosY(-150, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            rectTrsSuccessPurchase.DOAnchorPosY(200, 0.3f).SetUpdate(true).SetDelay(1.5f);
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

        rectTrsFailPurchase.anchoredPosition = new Vector2(0, 200);
        rectTrsFailPurchase.DOAnchorPosY(-150, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            rectTrsFailPurchase.DOAnchorPosY(200, 0.3f).SetUpdate(true).SetDelay(1.5f);
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
        builder.AddProduct(PackName.ball_1.ToString(), ProductType.Consumable);
        builder.AddProduct(PackName.ball_2.ToString(), ProductType.Consumable);
        builder.AddProduct(PackName.ball_3.ToString(), ProductType.Consumable);
        builder.AddProduct(PackName.ball_4.ToString(), ProductType.Consumable);
        builder.AddProduct(PackName.ball_5.ToString(), ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(PackName productId, Action _action)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId.ToString());

            action = _action;
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                actionUpdate.Add(ActiveLoadingPurchase);
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                actionUpdate.Add(ActivePurchaseFaill);
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
            apple.RestoreTransactions((result,message) =>
            {
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
            actionUpdate.Add(action);
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
        return PurchaseProcessingResult.Complete;
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        actionUpdate.Add(DeactiveLoadingPurchase);

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
}

public enum PackName
{
    remove_ads,
    ball_1,
    ball_2,
    ball_3,
    ball_4,
    ball_5
}

