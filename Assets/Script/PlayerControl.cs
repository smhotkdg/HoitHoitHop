using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl instance;    
    private Rigidbody2D myBody; //ref to rigidbody
    private SpriteRenderer mySprite; //ref to SpriteRenderer
    private bool doJump = false, startMoving = false; //few bools
    private AudioSource audioS; //ref to  AudioSource

    [SerializeField]
    private float jumpForce = 5f, moveSpeed = 2f; //move and jump speed
    [SerializeField]
    private GameObject bubbleEffect; //ref to child bubble effect object
    
    public bool StartMovingBool //getter and setter
    {
        get { return startMoving; }
        set { startMoving = value; }
    }

    Globalvariable gv;

    

    void OnEnable()
    {
        
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        gv = Globalvariable.Instance;
    }

    // Use this for initialization
    void Start()
    {   //get the component
        mySprite = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        audioS = GetComponent<AudioSource>();
        myBody.bodyType = RigidbodyType2D.Static;//set the body type
        StartCoroutine(BlinkEye()); //start eye blink coroutine

        CameraControl.instance.PlayerSettings(); //call camera script method
        if (gv.SelectedIndex != 0 && gv.SelectedIndex <= 16 && gv.SelectedIndex != -1)
        {
            Vector2 localScale = new Vector2();
            localScale.x = 0.7f;
            localScale.y = 0.7f;
            this.transform.localScale = localScale;
            this.GetComponent<CircleCollider2D>().radius = 0.3f;
        }
        else
        {
            Vector2 localScale = new Vector2();
            localScale.x = 0.6f;
            localScale.y = 0.6f;
            this.transform.localScale = localScale;
            this.GetComponent<CircleCollider2D>().radius = 0.4f;
        }
    }

    // Update is called once per frame
    void Update()
    {   //if game is over
        //if (GameManager.instance.gameOver)
            //return; //return 
        //else start moving
        StartMoving();
        
        //when movuse is click and start moving is true
        if (Input.GetMouseButtonDown(0) && startMoving)
        {   //jump sound is played
           // audioS.PlayOneShot(vars.jumpSound);
            gv.currentScore++; //score is increased by 1
            UIManager.instance.SetScore(gv.currentScore);
            doJump = true; //jump is true
        }

    }

    void FixedUpdate()
    {   //if start moving is false
        if (!startMoving)
            return;//return
        //else give body a velocity to move
        myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        //if jump is true
        if (doJump == true)
            Jump();//then jump
    }
    //method which detect the colliders
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) //if its water
        {
            bubbleEffect.SetActive(true); //we activate bubble effect
            myBody.velocity = new Vector2(myBody.velocity.x, -5.5f);//set the velocity
            myBody.gravityScale = -myBody.gravityScale;//change the gravity direction
            jumpForce = -jumpForce;//change the jump force direction
        }

        if (other.CompareTag("Enemy")) //if its the enemy
        {
            StartCoroutine(Death());//we start death
        }

        if (other.CompareTag("PickUp"))//if its the pickup
        {
            //audioS.PlayOneShot(vars.starSound);//we play pickup sound
            //GameManager.instance.currentPoints++;//increase the current point
            //GameManager.instance.points++; //increase the points
            gv.Gold++;
            //GameManager.instance.Save();//save it
            SoundManager.instance.PlaySound("GetGold");
            other.gameObject.SetActive(false);//deactivate the star
        }
    }
    //method called when object exit the collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))//if its water
        {
            bubbleEffect.SetActive(false); //deactivate the bubble effect
            myBody.velocity = new Vector2(myBody.velocity.x, 4f); //set the velocity
            myBody.gravityScale = -myBody.gravityScale;//change the gravity direction
            jumpForce = -jumpForce;//change the jump force direction
        }
    }
    //method which ake player jump
    void Jump()
    {
        SoundManager.instance.PlaySound("Jump");
        myBody.velocity = Vector2.zero;//at satrt we set velocity to zero
        myBody.velocity = Vector3.up * jumpForce;//then add the real jump force
        doJump = false;//jump is false
    }
    //coroutine which make blink eye effect
    IEnumerator BlinkEye()
    {   //we set the close eye sprite
        if (gv.SelectedIndex == 0)
            mySprite.sprite = gv.DefaultSprite1;
        else
        {
            if (gv.SelectedIndex > -1)
                mySprite.sprite = gv.characters_sprite[gv.SelectedIndex-1];
            else
                mySprite.sprite = gv.DefaultSprite1;
        }        
        yield return new WaitForSeconds(0.25f);//after 0.25f sec
        //we set the open eye sprite
        if (gv.SelectedIndex == 0)
            mySprite.sprite = gv.DefaultSprite2;
        else
        {
            if (gv.SelectedIndex > -1)
                mySprite.sprite = gv.characters_sprite_anim[gv.SelectedIndex-1];
            else
                mySprite.sprite = gv.DefaultSprite2;
        }
        
        yield return new WaitForSeconds(1f);//after 1 sec
        StartCoroutine(BlinkEye());//we repeat the coroutine
    }

    //method which is called at the start of game when the player start moving on 1st tap
    void StartMoving()
    {   //checn for tap and start moving is false and get started is true
        //if (Input.GetMouseButtonDown(0) && !startMoving && GameUI.instance.GameStarted == true)
        if (Input.GetMouseButtonDown(0) && gv.GameStarted == true) 
        {
            startMoving = true; //start moing is true
            myBody.bodyType = RigidbodyType2D.Dynamic;//set body type
            myBody.simulated = true;//simulated to true
        }
    }

    IEnumerator Death()//death coroutine
    {
        //ShareScreenShot.instance.TakeScreenshot();//take screenshot        
        yield return new WaitForSeconds(0.1f);//after 1 frame
        //GameManager.instance.gameOver = true;//game over is true
        GameObject deathEffect = ObjectPooling.instance.GetDeathEffect();//death effect
        deathEffect.transform.position = transform.position;//set the position
        //GameUI.instance.GameIsOver();
        UIManager.instance.GameIsOver();
        gv.GameStarted = false;
        SoundManager.instance.PlaySound("Death");
        deathEffect.SetActive(true);//activate it
        gameObject.SetActive(false);//set game object deactive

        
    }
}
