using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    // Use this for initialization\
    public Button GameCenter_1;
    public Button GameCenter_2;

    public Button Rank_1;
    public Button Rank_2;

    public Button AdsButton;
    public Text ShopGoldText;
    public List<GameObject> ItemList;
    public AdManager AdsManager;
    public GameObject ShopPanel;
    public GameObject MainPanel;
    public GameObject ReStartPanel;
    public GameObject InstructionImage;
    public Text ScoreText;
    public Text BestScoreText;
    public Text GoldText;
    public GameObject CurrentScoreText;
    Globalvariable gv;
    public GameObject NoadsButton;

    public static UIManager instance;

    private void Awake()
    {
        gv = Globalvariable.Instance;
        MakeInstance();
        SetSound();
        AdsButton.onClick.AddListener(AdsClick);
        //GameCenter_1.onClick.AddListener(ClickGameCenter);
        //GameCenter_2.onClick.AddListener(ClickGameCenter);
        Rank_1.onClick.AddListener(ClickRank);
        Rank_2.onClick.AddListener(ClickRank);
    }
    void ClickGameCenter()
    {
        GPGSManager.instance.Login();
    }
    void ClickRank()
    {
        GPGSManager.instance.ShowLeaderboard();
    }
    void AdsClick()
    {
        AdManager.instance.ShowReward(1);
    }
    void Start () {
        if(gv.GameStarted ==false)
        {
            InstructionImage.SetActive(false);
            MainPanel.SetActive(true);
            ReStartPanel.SetActive(false);
            CurrentScoreText.SetActive(false);
        }
        else
        {
            InstructionImage.SetActive(true);
            CurrentScoreText.SetActive(true);
            CurrentScoreText.GetComponent<Text>().text = 0.ToString();
            MainPanel.SetActive(false);
            ReStartPanel.SetActive(false);
        }

        SetNOads();
    }

    public void SetScore(int score)
    {
        CurrentScoreText.GetComponent<Text>().text = score.ToString();
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void SetNOads()
    {
        if(gv.NoAds ==1)
        {
            NoadsButton.SetActive(false);
        }
        else
        {
            NoadsButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        if(bHome == true)
        {
            ReStartGame();
            bHome = false;
        }
        else
        {
            InstructionImage.SetActive(true);
            MainPanel.SetActive(false);
            CurrentScoreText.SetActive(true);
            CurrentScoreText.GetComponent<Text>().text = 0.ToString();
            gv.GameStarted = true;
        }
    }
    
    public void GameIsOver()
    {
        gv.TotalGameScore += gv.currentScore;
        int rand = Random.Range(50, 100);
        Debug.Log("totalGameScore = " + gv.TotalGameScore + "RandomScore = " + rand);
        if (gv.TotalGameScore >= rand)
        {
            AdsManager.ShowVideo();
            gv.TotalGameScore = 0;
        }        
        StartCoroutine(EndGame());
        
    }
    public void SetGoldText()
    {
        GoldText.text = gv.Gold.ToString();
        ShopGoldText.text = gv.Gold.ToString();
    }
    IEnumerator EndGame()
    {        
        yield return new WaitForSeconds(0.6f);
        CurrentScoreText.SetActive(false);
        InstructionImage.SetActive(false);
        if (gv.currentScore > gv.bestScore)
        {
            gv.bestScore = gv.currentScore;
        }

        ScoreText.text = gv.currentScore.ToString();
        BestScoreText.text = gv.bestScore.ToString();
        GoldText.text = gv.Gold.ToString();
                
        PlayerPrefs.SetInt("bestScore", gv.bestScore);
        PlayerPrefs.SetInt("Gold", gv.Gold);

        PlayerPrefs.Save();
        gv.currentScore = 0;
        ReStartPanel.SetActive(true);
    }
    public void GotoURL()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCW8OB_8BSiR1-SIVcJu8u_Q");
    }
    bool bHome = false;
    public void GoToHome()
    {
        MainPanel.SetActive(true);
        ReStartPanel.SetActive(false);
        bHome = true;
    }
    public void ReStartGame()
    {
        InstructionImage.SetActive(true);
        CurrentScoreText.SetActive(true);
        CurrentScoreText.GetComponent<Text>().text = 0.ToString();        
        ReStartPanel.SetActive(false);        
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        gv.GameStarted = true;
    }
    int type = 0;
    public void PressShop()
    {
        if(gv.SelectedIndex >=0)
            ItemList[gv.SelectedIndex].transform.Find("Check").gameObject.SetActive(true);
        ShopPanel.SetActive(true);
        ShopGoldText.text = gv.Gold.ToString();
        if (MainPanel.activeSelf == true)
        {
            MainPanel.SetActive(false);
            type = 1;
        }
        if(ReStartPanel.activeSelf == true)
        {
            ReStartPanel.SetActive(false);
            type = 2;
        }
    }
    public void BackShop()
    {
        ShopPanel.SetActive(false);
        if(type ==1)
            MainPanel.SetActive(true);
        if (type == 2)
            ReStartPanel.SetActive(true);
    }
    public GameObject SoundOn;
    public GameObject SoundOff;

    public GameObject SoundOn2;
    public GameObject SoundOff2;
    public void SetSound(bool flag)
    {
        if(flag == true)
        {
            SoundOn.SetActive(false);
            SoundOff.SetActive(true);
            SoundOn2.SetActive(false);
            SoundOff2.SetActive(true);
            gv.bSound = 0;
            PlayerPrefs.SetInt("bSound", gv.bSound);
        }
        if (flag == false)
        {
            SoundOn.SetActive(true);
            SoundOff.SetActive(false);
            SoundOn2.SetActive(true);
            SoundOff2.SetActive(false);
            gv.bSound = 1;
            PlayerPrefs.SetInt("bSound", gv.bSound);
        }
        SoundManager.instance.SetAudio(flag);
    }
    public void SetSound()
    {
        bool flag = false;
        if (gv.bSound == 0)
        {
            SoundOn.SetActive(false);
            SoundOff.SetActive(true);
            SoundOn2.SetActive(false);
            SoundOff2.SetActive(true);
            flag = true;
            gv.bSound = 0;
            PlayerPrefs.SetInt("bSound", gv.bSound);
        }
        if (gv.bSound == 1)
        {
            SoundOn.SetActive(true);
            SoundOff.SetActive(false);
            SoundOn2.SetActive(true);
            SoundOff2.SetActive(false);
            flag = false;
            gv.bSound = 1;
            PlayerPrefs.SetInt("bSound", gv.bSound);
        }
        SoundManager.instance.SetAudio(flag);
    }

    public void ClickItem(int index)
    {
        for(int i=0; i< ItemList.Count; i++)
        {
            ItemList[i].transform.Find("Check").gameObject.SetActive(false);
        }
        ItemList[index].transform.Find("Check").gameObject.SetActive(true);
        gv.SelectedIndex = index;
    
        PlayerPrefs.SetInt("SelectedIndex", index);
        PlayerPrefs.Save();
    }
}
