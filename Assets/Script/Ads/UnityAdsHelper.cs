using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAdsHelper : MonoBehaviour
{
    Globalvariable gv;
    private const string android_game_id = "3121789";
    private const string ios_game_id = "3121788";

    private const string rewarded_video_id = "HoitVideoReward1";
    private const string video_id = "HoitVideo1";    
    public static UnityAdsHelper instance;
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
        Initialize();
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
#elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
#endif
    }
    public void ShowVideoAd()
    {
        if (Advertisement.IsReady(video_id))
        {            
            Advertisement.Show(video_id);
            Debug.Log("ads Show");
        }
        else
        {
            Debug.Log("Not Ready");
        }
    }
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewarded_video_id))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };

            Advertisement.Show(rewarded_video_id, options);
            Debug.Log("ads Show");
        }
        else
        {
            Debug.Log("Not Ready");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");

                    // to do ...
                    // 광고 시청이 완료되었을 때 처리
                    //_Text.text = "Unity Ad 완료";     
                    gv.Gold += 30;
                    PlayerPrefs.SetInt("Gold", gv.Gold);
                    PlayerPrefs.Save();
                    SoundManager.instance.PlaySound("AdReward");
                    GameObject.Find("MainCanvas").GetComponent<UIManager>().SetGoldText();
                    break;
               
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");

                    // to do ...
                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");

                    // to do ...
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
                
        }
    }
}
