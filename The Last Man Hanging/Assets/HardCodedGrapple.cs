using UnityEngine;
using System.Collections;

public class HardCodedGrapple : MonoBehaviour {

    // Use this for initialization
    [SerializeField] public float slackLength;
    [SerializeField] public int PlayerNumber;
    public readonly float gravity;
    bool LMBDepressed;
    bool RMBDepressed;
    public bool LHookOut;
    public bool RHookOut;
    GameObject hookL;
    GameObject hookR; //hahaha
   // Rigidbody2D body;

	void Start ()
    {
        //body = GetComponent<Rigidbody2D>();
        
        LMBDepressed = false;
        RMBDepressed = false;
        LHookOut = false;
        RHookOut = false;
        if (PlayerNumber == 0 )
        {
            PlayerNumber = 1;
        }
        if (slackLength == 0)
        {
            slackLength = 1;
        }
        //body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame 
    /* this method is broken and pointless rn
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        //none of this works. 
        if(collision.gameObject.GetComponent<HookObject>().hookID.Equals("" + PlayerNumber + 'L' ))
        {
            LHookOut = false;

        }
        if (collision.gameObject.GetComponent<HookObject>().hookID == ("" + PlayerNumber + 'R'))
        {
            RHookOut = false;

        }

    }
    */
    void Swing(char LorR)
    {
        if(LorR == 'L')
        {
            
            Vector2 hookedPosition = hookL.transform.position;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 30));
            
        }
        if (LorR == 'R')
        {
            Vector2 hookedPosition = hookR.transform.position;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 30));
        }


    }

	void Update ()
    {
        //check if LMB has been dperesssed since the last frame, if so spawn hookL, and if that hook is currently out,
        // feed it the players position, and if the LMB has been raised and the hook isnt returning yet, set it to return
        if (Input.GetKey(KeyCode.Mouse0) & ! LHookOut) // check that last frame lmb wasnt down and now it is.
        {            
            hookL = (GameObject)(Instantiate(Resources.Load("HookPrefab")));
            LHookOut = true;
            hookL.GetComponent<HookObject>().Throw(transform.position, PlayerNumber, 'L');
        }
        LMBDepressed = Input.GetKey(KeyCode.Mouse0);
        if(LHookOut)
        {
            hookL.GetComponent<HookObject>().playerPosition = transform.position;
            if(!LMBDepressed & !hookL.GetComponent<HookObject>().RETURN)
            {
                hookL.GetComponent<HookObject>().RETURN = true;
            }
            if (LMBDepressed & hookL.GetComponent<HookObject>().isHooked)
            {
                
                Swing('L');
            }
        }
        

        // same thing for right hook
        if (Input.GetKey(KeyCode.Mouse1) & !RHookOut) // check that last frame lmb wasnt down and now it is.
        {
            hookR = (GameObject)(Instantiate(Resources.Load("HookPrefab")));
            RHookOut = true;
            hookR.GetComponent<HookObject>().Throw(transform.position , PlayerNumber, 'R');
        }
        RMBDepressed = Input.GetKey(KeyCode.Mouse1);
        if (RHookOut)
        {
            hookR.GetComponent<HookObject>().playerPosition = transform.position;
            if (!RMBDepressed & !hookR.GetComponent<HookObject>().RETURN)
            {
                hookR.GetComponent<HookObject>().RETURN = true;
            }
            if (RMBDepressed & hookR.GetComponent<HookObject>().isHooked)
            {

                Swing('R');
            }
        }

    }
}
/*SO lets iron out the conditions im doing. On keypress Left Click, spawn a Grapple Object with Velocity V and trajectory
to the mouse pointer.

 The Grapple Object is has a rigidbody. The only thing necessary in its script is that it attaches , and sets its center to 
 the things center that it grappled on to. It is generated from a prefab on buttonpress at the character location, 
 using     GameObject go = (GameObject)Instantiate(Resources.Load("MyPrefab")); ;
 fixes itself to whatever it hits as long as the button is being held down, and comes back at a fixed velocity. The
 player should have a counter for each one he has out, and if it is at 2, he cannot make more. the counter is incremented when
 one is thrown, and decremented when it is lost. 

 
 When it is attached and the player is at a distance d from the point, the rope becomes tensioned and the player starts to 
 swing, enacting a new set of forces on the player.*/
