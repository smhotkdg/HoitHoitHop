using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

//gpg
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;

public class GPGSManager : MonoBehaviour {

    Globalvariable gv;

    public static GPGSManager instance;
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
    private void Start()
    {
        Init();
    } 
    void Init()
    {
        //PlayGamesClientConfiguration conf = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesClientConfiguration conf = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(conf);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        //Login();
    }
    public void Login()
    {
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) => {
                // handle success or failure     `                    
                if (success)
                {
                    gv.iGoogle = 1;
                }
            });
        }
    }

    public void AddAchive(int index)
    {
        if (Social.localUser.authenticated == true)
        {
            switch (index)
            {
               
            }

        }    

    }
    public void LogOut()
    {
        if (Social.localUser.authenticated == true)
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
            gv.iGoogle = 0;
        }
        else
        {
        }
    }

    public void OnAddScoreToLeaderBorad(double score, string key)
    {
        if (Social.localUser.authenticated)
        {
            //Social.Active.ReportScore(score, key, (bool success) =>
            PlayGamesPlatform.Instance.ReportScore((long)score, key, (bool success) =>
              {
                  if (success)
                  {
                      Debug.Log("Update Score Success");
                  }
                  else
                  {
                      Debug.Log("Update Score Fail");
                  }
              });
        }
    }

    public void ShowAchivementClick()
    {
        if (Social.localUser.authenticated == true)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Login();
        }
    }

    public void AddScore()
    {
        OnAddScoreToLeaderBorad(gv.bestScore , GPGSIds.leaderboard_score);
    }
    public void AddScoreMoney()
    {

        //OnAddScoreToLeaderBorad(gv.TotalMoney / gv.scaleFactor, GPGSIds.leaderboard);
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated == true)
        {
            AddScore();            
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Login();
        }
    }
    bool onSaving = false;
    string SaveData = string.Empty;
    public void SaveToCloud()
    {
        StartCoroutine(Save());
    }
    IEnumerator Save()
    {
        Debug.Log("Try to Save Data");

        while(gv.iGoogle==0)
        {
            Login();
            yield return new WaitForSeconds(2f);
        }
        onSaving = true;

        string id = Social.localUser.id;
        string filename = string.Format("{0}_DATA", id);
        

        OpenSaveGame(filename, true);
    }
    void OpenSaveGame(string _fileName, bool _saved)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;
        if(_saved == true)
        {
            //save
            savedClient.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSaveGameOpnedToSave);
        }
        else
        {
            //load
            savedClient.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnsaveGameOpenedtoRead);
        }
    }
    void OnSaveGameOpnedToSave(SavedGameRequestStatus _Status, ISavedGameMetadata _data)
    {
        if(_Status == SavedGameRequestStatus.Success)
        {
            //SaveGame(_data, gv.saveByte, DateTime.Now.TimeOfDay);
        }
        else
        {            
            Debug.Log("save Fail");
        }
    }
    void SaveGame(ISavedGameMetadata _data, byte[] _byte, TimeSpan _playTime)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder.WithUpdatedPlayedTime(_playTime).WithUpdatedDescription("Saved at " + DateTime.Now);

        SavedGameMetadataUpdate updatedata = builder.Build();
        savedClient.CommitUpdate(_data, updatedata, _byte, OnsaveGameWritten);
    }
    void OnsaveGameWritten(SavedGameRequestStatus _status, ISavedGameMetadata _data)
    {
        onSaving = false;
        if(_status == SavedGameRequestStatus.Success)
        {            
            Debug.Log("Save Completet");            
        }
        else
        {            
            Debug.Log("Written Save fail");

        }
    }

    public void LoadFromCloud()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        Debug.Log("Try to Load Data");

        while (gv.iGoogle == 0)
        {
            Login();
            yield return new WaitForSeconds(2f);
        }
        onSaving = true;

        string id = Social.localUser.id;
        string filename = string.Format("{0}_DATA", id);        

        OpenSaveGame(filename, false);
    }
    void OnsaveGameOpenedtoRead(SavedGameRequestStatus _status,ISavedGameMetadata _data)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            LoadGameData(_data);
        }
        else
        {            
            Debug.Log("Load Fail");
        }
    }
    void LoadGameData(ISavedGameMetadata _data)
    {
        ISavedGameClient savedClient = PlayGamesPlatform.Instance.SavedGame;
        savedClient.ReadBinaryData(_data, OnSaveGameDataRead);
    }
    void OnSaveGameDataRead(SavedGameRequestStatus _status,byte[] _byte)
    {
        if(_status == SavedGameRequestStatus.Success)
        {
            //gv.saveByte = _byte;
            //if(gv.saveByte !=null)
            //{
            //    //DataInfo dataInfo;                

            //    if(dataInfo != null)
            //    {
                   
            //    }
            //    else
            //    {                    
            //        Debug.Log("dataInfo Null");
            //    }
            //}
        }
        else
        {            
            Debug.Log("Load Fail");
        }
    }
}
