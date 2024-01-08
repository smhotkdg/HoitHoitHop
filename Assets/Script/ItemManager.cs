using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour {

    // Use this for initialization
    public Text PriceText;
    public int Price;
    public int index;
    bool UnLock = false;
    Globalvariable gv;
	void Start () {
		
	}
    private void Awake()
    {
        gv = Globalvariable.Instance;
    }
    private void OnEnable()
    {
        if (index < 0)
            return;
        if(gv.characters[index]==0)
        {
            PriceText.text = Price.ToString();
        }
        else
        {
            UnLockObj();
        }
        if(index ==0)
        {
            UnLockObj();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
    void UnLockObj()
    {
        this.transform.Find("Buy").gameObject.SetActive(false);
        this.transform.Find("Object").gameObject.SetActive(true);
    }
    public void Buy()
    {
        if(gv.Gold>= Price)
        {
            Debug.Log("Buy Success!!");
            gv.Gold -= Price;
            PlayerPrefs.SetInt("Gold", gv.Gold);
            PlayerPrefs.Save();
            UnLockObj();
            
            string s = "characters" + index;
            gv.characters[index] = 1;
            PlayerPrefs.SetInt(s, gv.characters[index]);
            UIManager.instance.SetGoldText();
            
        }
    }
    public void Click()
    {        
        GameObject.Find("MainCanvas").GetComponent<UIManager>().ClickItem(index);
    }
}
