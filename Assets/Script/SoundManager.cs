using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    Globalvariable gv;
    public List<AudioSource> audioList;
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
        if(gv.bSound ==1)
        {
            if (audioList[0].isPlaying == false)
                audioList[0].Play();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void PlaySound(string name)
    {
        switch (name)
        {
            case "Death":
                audioList[1].Play();
                break;
            case "AdReward":
                audioList[2].Play();
                break;
            case "GetGold":
                audioList[3].Play();
                break;
            case "Jump":
                audioList[4].Play();
                break;
        }
    }
    public void SetAudio(bool flag)
    {
        for (int i = 0; i < audioList.Count; i++)
        {
            audioList[i].mute = flag;
        }
        if(flag == false)
        {
            if(audioList[0].isPlaying ==false)
                audioList[0].Play();
        }
    }
}
