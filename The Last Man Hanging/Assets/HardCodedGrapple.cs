using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class HardCodedGrapple : MonoBehaviour {

    // Use this for initialization
    public float slackLength;
    [SerializeField] public int PlayerNumber;
    public readonly float gravity;
    bool LMBDepressed;
    bool RMBDepressed;
    [SerializeField] public float maxRopeLength;
    public bool LHookOut;
    public bool RHookOut;
    Vector2 oldV;
    Vector2 acceleration;
    Vector2 oldForce;
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
        if(maxRopeLength == 0)
        {
            maxRopeLength = 1;

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
    Vector2 rotateVectorPlane(Vector2 start, float degrees) //degrees is positive counter clockwise, negative clockwise.
    {
        Vector2 toReturn = new Vector2(0,0);
        
        float tan = Mathf.Tan(degrees* 6.28318530718f/360f);
        toReturn.x = 1;
        toReturn.y = 1;
        //now to determine which quadrant the point will lie in in the new plane
        if((tan*start.x > start.y))
        {
            //toReturn.x = -1;
            toReturn.y = -1;
        //point is above the new x axis
        }
        if((-start.y*tan > start.x))  //-1/tan * xy = yp   -yp*tan = xy if -yp*tan < xp
        {
            //toReturn.y = -1;
            toReturn.x = -1;
            //point is to the right of the y axis
        }
        toReturn.y *= Mathf.Sqrt(Mathf.Pow( (start.x + start.y *tan)/(tan*(tan + 1/tan)) - start.x,2) + 
            Mathf.Pow((start.x + start.y * tan) / ((tan + 1 / tan)) -start.y ,2)  );
        toReturn.x *= Mathf.Sqrt(Mathf.Pow(start.x, 2) + Mathf.Pow(start.y, 2) - Mathf.Pow(toReturn.y, 2)) ; 
        return toReturn;
    }
    void Swing(char LorR)
    {
        if(LorR == 'L')
        {
            
            Vector2 hookedPosition = hookL.transform.position;
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 30));
            
        }
        if (LorR == 'R')
        {/*
            Vector2 hookedPosition = hookR.transform.position;
            float distance = Vector2.Distance(transform.position, hookedPosition);
            Vector2 directionVector = new Vector2((hookedPosition.x - transform.position.x), (hookedPosition.y - transform.position.y));
            directionVector.Normalize();
            float swingAngleFromY = Mathf.Atan(directionVector.x / directionVector.y); // in rads, left of y is - right is + 
            print(swingAngleFromY);
            Vector2 perpindicularDirectionVector = new Vector2(Mathf.Sin(swingAngleFromY), Mathf.Cos(swingAngleFromY)); 
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            //just fucking rotate it 90 degrees
            Vector2 perpindicularVelocityVector = new Vector2(velocity.x * perpindicularDirectionVector.x , velocity.y *perpindicularDirectionVector.y);
            GetComponent<Rigidbody2D>().AddForce(-oldForce, ForceMode2D.Impulse);
            if (distance > slackLength)
            {
                
                oldForce = new Vector2(directionVector.x * GetComponent<Rigidbody2D>().mass * perpindicularVelocityVector.sqrMagnitude  / distance, GetComponent<Rigidbody2D>().mass * directionVector.y * perpindicularVelocityVector.sqrMagnitude / distance) ;
                GetComponent<Rigidbody2D>().AddForce(oldForce, ForceMode2D.Impulse ) ;
                //add enough force to make the centripedal acceleration, plus all of the force in the direction opposite to the rope. 
                //force opposite to the centripedal acceleration would be found by 2 instances of the time divided by delta t * mass. 
                //force to add as centripedal would be from 

            }
            */
            Vector2 hookedPosition = hookR.transform.position;
            float distance = Vector2.Distance(transform.position, hookedPosition);
            Vector2 directionVector = new Vector2((hookedPosition.x - transform.position.x), (hookedPosition.y - transform.position.y));
            float directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f - 90);
            Vector2 rotatedDirectionVector = rotateVectorPlane(directionVector, directionVectorRotation);
            print(rotatedDirectionVector.x + " " + rotatedDirectionVector.y + " " + directionVectorRotation);
            //get the velocity vector, copy it into vector2, rotate vector 2 into the plane, add an impulse that is the difference between the two.

            //print(rotateVectorPlane(test, -15).x + "  ,   " + rotateVectorPlane(test, -15).y + "  ? " + Mathf.Tan(-15 * 6.28318530718f / 360f));
            //print(rotateVectorPlane(test, 15).x + "  ,   " + rotateVectorPlane(test, 15).y + "  ? " + Mathf.Tan(15 * 6.28318530718f / 360f));


        }


    }

	void Update ()
    {

        if (Input.GetKey(KeyCode.Mouse0)) 
        {

            print("fl");
        }
        Vector2 newV = GetComponent<Rigidbody2D>().velocity;
        acceleration = (newV - oldV) / Time.deltaTime;
        oldV = GetComponent<Rigidbody2D>().velocity;
        //check if LMB has been dperesssed since the last frame, if so spawn hookL, and if that hook is currently out,
        // feed it the players position, and if the LMB has been raised and the hook isnt returning yet, set it to return
        if (Input.GetKey(KeyCode.Mouse0) & ! LHookOut) // check that last frame lmb wasnt down and now it is.
        {            
            hookL = (GameObject)(Instantiate(Resources.Load("HookPrefab")));
            LHookOut = true;
            hookL.GetComponent<HookObject>().Throw(gameObject, 'L');
            print("Spawn LHook");
        }
        LMBDepressed = Input.GetKey(KeyCode.Mouse0);
        if(LHookOut)
        {
            hookL.GetComponent<HookObject>().playerPosition = transform.position;
            if(!LMBDepressed & !hookL.GetComponent<HookObject>().RETURN)
            {
                hookL.GetComponent<HookObject>().RETURN = true;
                GetComponent<DistanceJoint2D>().enabled = false;
            }
            if (LMBDepressed & hookL.GetComponent<HookObject>().isTensioned)
            {
                Swing('L');
            }
        }
        

        // same thing for right hook
        if (Input.GetKey(KeyCode.Mouse1) & !RHookOut) // check that last frame lmb wasnt down and now it is.
        {
            hookR = (GameObject)(Instantiate(Resources.Load("HookPrefab")));
            RHookOut = true;
            hookR.GetComponent<HookObject>().Throw(gameObject , 'R');
        }
        RMBDepressed = Input.GetKey(KeyCode.Mouse1);
        if (RHookOut)
        {
            hookR.GetComponent<HookObject>().playerPosition = transform.position;
            if (!RMBDepressed & !hookR.GetComponent<HookObject>().RETURN)
            {
                hookR.GetComponent<HookObject>().RETURN = true;
                GetComponent<DistanceJoint2D>().enabled = false;
            }
            if (RMBDepressed & hookR.GetComponent<HookObject>().isTensioned)
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
