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



	void Start ()
    {
        
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
    }


    Vector2 rotateVectorPlane(Vector2 start, float degrees) //degrees is positive counter clockwise, negative clockwise.
    {
        Vector2 toReturn = new Vector2(0,0);
        
        float tan = Mathf.Tan(degrees* 6.28318530718f/360f);
        toReturn.x = 1;
        toReturn.y = 1;
        //now to determine which quadrant the point will lie in in the new plane
        if((tan*start.x > start.y))
        {
            toReturn.y = -1;
        //point is above the new x axis
        }
        if((-start.y*tan > start.x))  //-1/tan * xy = yp   -yp*tan = xy if -yp*tan < xp
        {
            toReturn.x = -1;
            //point is to the right of the y axis
        }
        toReturn.y *= Mathf.Sqrt(Mathf.Pow( (start.x + start.y *tan)/(tan*(tan + 1/tan)) - start.x,2) + 
            Mathf.Pow((start.x + start.y * tan) / ((tan + 1 / tan)) -start.y ,2)  );
        toReturn.x *= Mathf.Sqrt(Mathf.Pow(start.x, 2) + Mathf.Pow(start.y, 2) - Mathf.Pow(toReturn.y, 2)) ; 
        if (toReturn.x <= .002 | float.IsNaN(toReturn.x))
        {
            toReturn.x = 0;
        }
        return toReturn;
    }



    void Swing(char LorR)
    {
        if(LorR == 'L')
        {
            Vector2 hookedPosition = hookL.transform.position;            
        }
        if (LorR == 'R')
        {
            Vector2 hookedPosition = hookR.transform.position;
            float distance = Vector2.Distance(transform.position, hookedPosition);
            Vector2 directionVector = new Vector2((hookedPosition.x - transform.position.x), (hookedPosition.y - transform.position.y));
            float directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f - 90);
            Vector2 rotatedDirectionVector = rotateVectorPlane(directionVector, directionVectorRotation);
            print(rotatedDirectionVector.x + " " + rotatedDirectionVector.y + " " + directionVectorRotation);
            //get the velocity vector, copy it into vector2, rotate vector 2 into the plane, add an impulse that is the difference between the two.
        }
    }



	void Update ()
    {

        Vector2 newV = GetComponent<Rigidbody2D>().velocity;
        acceleration = (newV - oldV) / Time.deltaTime;
        oldV = GetComponent<Rigidbody2D>().velocity;
        if (Input.GetKey(KeyCode.Mouse0) & ! LHookOut) 
        {            
            hookL = (GameObject)(Instantiate(Resources.Load("HookPrefab")));
            LHookOut = true;
            hookL.GetComponent<HookObject>().Throw(gameObject, 'L');
        }
        LMBDepressed = Input.GetKey(KeyCode.Mouse0);
        if(LHookOut)
        {
            hookL.GetComponent<HookObject>().playerPosition = transform.position;
            if(!LMBDepressed & !hookL.GetComponent<HookObject>().RETURN)
            {
                hookL.GetComponent<HookObject>().RETURN = true;
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
