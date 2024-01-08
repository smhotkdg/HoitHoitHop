using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdManager : MonoBehaviour
{

    // Use this for initialization    

    Globalvariable gv;
    public static AdManager instance;
    private void Awake()
    {        
        gv = Globalvariable.Instance;
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {

    }
    IEnumerator LoadAd()
    {        
        AdsMobManager.instance.LoadAd();
        yield return new WaitForSeconds(25);
        if (AdsMobManager.instance.rewardBasedVideo.IsLoaded() == false)
        {
            StartCoroutine(LoadAd());
        }
    }

    IEnumerator LoadAdVideo()
    {
        AdsMobManager.instance.LoadVideoAds();
        yield return new WaitForSeconds(25);
        if (AdsMobManager.instance.BasedVideo.IsLoaded() == false)
        {
            StartCoroutine(LoadAdVideo());
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    void Update()
    {

    }
    public void ShowVideo()
    {
        if(gv.NoAds ==1)
        {
            return;
        }
        //UnityAdsObj.GetComponent<UnityAdsHelper>().ShowVideoAd();


        //if (AdsMobManager.instance.BasedVideo.IsLoaded())
        //{
        //    AdsMobManager.instance.BasedVideo.Show();
        //}
        //else
        //{
            UnityAdsHelper.instance.ShowVideoAd();
            //StartCoroutine(LoadAdVideo());
        //}
        
    }
    public void ShowReward(int number)
    {

        //if (AdsMobManager.instance.rewardBasedVideo.IsLoaded())
        //{
        //    AdsMobManager.instance.rewardBasedVideo.Show();
        //}
        //else
        //{
        Debug.Log("Button Ads");
        UnityAdsHelper.instance.ShowRewardedAd();
            //StartCoroutine(LoadAd());
        //}
    }
}

