using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class shopItemData
{
    public string characterName;
    public Sprite gameCharacterSprite1, gameCharacterSprite2;
    public Sprite shopCharacterSprite;
    public int characterPrice;
}

public class Globalvariable : MonoBehaviour
{ 

    private static Globalvariable _instance = null;

    public static Globalvariable Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("cSingleton == null");
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitData();
        bLoad = PlayerPrefs.GetInt("bLoad");
        if (bLoad == 1)
        {
            LoadData();
        }
        bLoad = 1;
        PlayerPrefs.SetInt("bLoad", 1);
        PlayerPrefs.Save();
    }
    void InitData()
    {
        bSound = 1;
        SelectedIndex = -1;
        for (int i = 0; i < 100; i++)
        {
            characters.Add(0);            
        }
    }
    int bLoad =  0;
    void LoadData()
    {
        bool temp =false;
        for (int i = 0; i < 100; i++)
        {            
            string s = "characters" + i;
            characters[i] = PlayerPrefs.GetInt(s);
            if(characters[i] !=0)
            {
                temp = true;
            }
        }
        
        bestScore = PlayerPrefs.GetInt("bestScore");
        Gold = PlayerPrefs.GetInt("Gold");
        NoAds = PlayerPrefs.GetInt("NoAds");
        bSound = PlayerPrefs.GetInt("bSound");
        SelectedIndex = PlayerPrefs.GetInt("SelectedIndex");
        if (temp == false)
            SelectedIndex = -1;
    }
    public void AllReset()
    {
        currentScore = 0;
        bestScore = 0;
        Gold = 0;
        NoAds = 0;
        for (int i = 0; i < 100; i++)
        {            
            string s = "characters" + i;
            PlayerPrefs.SetInt(s,0);
        }

        PlayerPrefs.SetInt("currentScore", 0);
        PlayerPrefs.SetInt("bestScore", 0);
        PlayerPrefs.SetInt("Gold", 0);
        PlayerPrefs.SetInt("NoAds", 0);
        PlayerPrefs.SetInt("bSound", 1);
        PlayerPrefs.SetInt("SelectedIndex", -1);
        PlayerPrefs.Save();

    }
    [SerializeField]
    public List<int> characters = new List<int>();
    public List<Sprite> characters_sprite = new List<Sprite>();
    public List<Sprite> characters_sprite_anim = new List<Sprite>();


    public Sprite DefaultSprite1;
    public Sprite DefaultSprite2;

    public bool GameStarted = false;
    public int currentScore =0;
    public int bestScore = 0;
    public int Gold = 0;
    public int iGoogle = 0;
    public int NoAds = 0;
    public float timeDiff;
    public int GameOverCount = 0;
    public int bSound = 1;
    public int SelectedIndex = -1;
    public int TotalGameScore = 0;
}
