﻿/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace SIS
{
    /// <summary>
    /// Script that listens to purchases and other IAP events:
    /// here we tell our game what to do when these events happen.
    /// <summary>
    public class IAPListener : MonoBehaviour
    {
        //subscribe to the most important IAP events
        Globalvariable gv;
        private void Awake()
        {
            gv = Globalvariable.Instance;
        }
        private void OnEnable()
        {
            IAPManager.purchaseSucceededEvent += HandleSuccessfulPurchase;
            IAPManager.purchaseFailedEvent += HandleFailedPurchase;
            ShopManager.itemSelectedEvent += HandleSelectedItem;
            ShopManager.itemDeselectedEvent += HandleDeselectedItem;
        }


        private void OnDisable()
        {
			IAPManager.purchaseSucceededEvent -= HandleSuccessfulPurchase;
			IAPManager.purchaseFailedEvent -= HandleFailedPurchase;
			ShopManager.itemSelectedEvent -= HandleSelectedItem;
			ShopManager.itemDeselectedEvent -= HandleDeselectedItem;
        }


        /// <summary>
        /// Handle the completion of purchases, be it for products or virtual currency.
        /// Most of the IAP logic is handled internally already, such as adding products or currency to the inventory.
        /// However, this is the spot for you to implement your custom game logic for instantiating in-game products etc.
        /// </summary>
        public void HandleSuccessfulPurchase(string id)
        {
            if (IAPManager.isDebug) Debug.Log("IAPListener reports: HandleSuccessfulPurchase: " + id);

            //differ between ids set in the IAP Settings editor
            switch (id)
            {
                //section for in app purchases
                case "noads":
                    //the user bought the item "coins", show appropriate feedback
                    gv.NoAds = 1;
                    PlayerPrefs.SetInt("NoAds", gv.NoAds);
                    PlayerPrefs.Save();
                    GameObject.Find("MainCanvas").GetComponent<UIManager>().SetNOads();
                    gv.Gold += 1000;
                    PlayerPrefs.SetInt("Gold", gv.Gold);
                    PlayerPrefs.Save();
                    UIManager.instance.SetGoldText();
                    break;         
            }
        }

        //just shows a message via our ShopManager component,
        //but checks for an instance of it first
        void ShowMessage(string text)
        {
            if (ShopManager.GetInstance())
                ShopManager.ShowMessage(text);
        }

        //called when an purchaseFailedEvent happens,
        //we do the same here
        void HandleFailedPurchase(string error)
        {
            if (ShopManager.GetInstance())
                ShopManager.ShowMessage(error);
        }


        //called when a purchased shop item gets selected
        void HandleSelectedItem(string id)
        {
            if (IAPManager.isDebug) Debug.Log("Selected: " + id);
        }


        //called when a selected shop item gets deselected
        void HandleDeselectedItem(string id)
        {
            if (IAPManager.isDebug) Debug.Log("Deselected: " + id);
        }
    }
}