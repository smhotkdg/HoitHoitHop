using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour {

    public float bgspeed; //ref to moving speed 
    Vector2 offset = Vector2.zero; //tile set offset

    [SerializeField]
    private Renderer bgPlaneMat; //ref to background material
    [SerializeField]
    private bool moveWithPlayer = false, mountains = true;

    Globalvariable gv;

    private void Awake()
    {
        gv = Globalvariable.Instance;
    }

    void OnEnable()
    {
        
    }

    void Start()
    {      
    }

    // Update is called once per frame
    void Update ()
    {
        //if (gv.GameStarted)//when game is over we save the time
           // gv.timeDiff = Time.time;
        //if game is over or player is not moving and move with player is true
        //if (gv.GameStarted  || PlayerControl.instance.StartMovingBool == false
        //    && moveWithPlayer == true)//we return
        //    return;
        //else we set the offset
        offset = new Vector2((Time.time - gv.timeDiff) * bgspeed, 0);
        bgPlaneMat.material.mainTextureOffset = offset;//and change texture offset

	}
}
