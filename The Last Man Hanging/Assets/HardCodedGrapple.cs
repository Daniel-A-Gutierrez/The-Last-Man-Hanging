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
    float directionVectorRotation;



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
        if (Mathf.Abs(toReturn.x) <= .002 | float.IsNaN(toReturn.x))
        {
            toReturn.x = 0;
        }
        if(degrees < -90 |degrees > 90)
        {
            toReturn.y *= -1;
            toReturn.x *=  -1;
        }
        return toReturn;
    }

    float DotProduct(Vector2 a, Vector2 b)
    {
        return (a.x * b.x + a.y * b.y);
    }
    Vector2 Perpindicularize(Vector2 a, bool clockwise)
    {
        Vector2 b = new Vector2(0,0);
        bool posX = false;
        bool posY = false;
        if(a.x > 0)
        {
            posX = true;
        }
        if(a.y > 0)
        {
            posY = true;
        }
        if(clockwise)
        {
            if(posX)
            {
                if(posY)
                {
                    b.y = -a.x;
                    b.x = a.y;
                }
                if(!posY)
                {
                    b.x = a.y;
                    b.y = -a.x;
                }
            }
            if(!posX)
            {
                if (posY)
                {
                    b.x = a.y;
                    b.y = -a.x;
                }
                if (!posY)
                {
                    b.x = a.y;
                    b.y = -a.x;
                }
            }

        }
        else
        {
            if (posX)
            {
                if (posY)
                {
                    b.x = -a.y;
                    b.y = a.x;
                }
                if (!posY)
                {
                    b.y = a.x;
                    b.x = -a.y;
                }
            }
            if (!posX)
            {
                if (posY)
                {
                    b.x = -a.y;
                    b.y = a.x;
                }
                if (!posY)
                {
                    b.x = -a.y;
                    b.y = a.x;
                }
            }

        }
        return b;

    }
    float oldDirectionVectorRotation;


    void Swing(char LorR)
    {
        float mass = GetComponent<Rigidbody2D>().mass;
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        if (LorR == 'L')
        {
            Vector2 hookedPosition = hookL.transform.position;            
        }
        if (LorR == 'R')
        {
            Vector2 hookedPosition = hookR.transform.position;

            float distance = Vector2.Distance(transform.position, hookedPosition);

            Vector2 directionVector = new Vector2((hookedPosition.x - transform.position.x), (hookedPosition.y - transform.position.y));

            float deltaThetaV = directionVectorRotation - (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f - 90); // may need to be negative
            if (directionVector.x >= 0)
            {
                if(directionVector.y > 0)
                {
                    directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f - 90); //q1
                }
                else
                {
                    directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f - 90); // q4
                }
            }
            else
            {
                if (directionVector.y > 0)
                {
                    directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f + 90); // q2
                }
                else
                {
                    directionVectorRotation = (Mathf.Atan(directionVector.y / directionVector.x) * 360f / 6.283185307f + 90); //q3
                }
            }
            

            Vector2 rotatedDirectionVector = rotateVectorPlane( directionVector, directionVectorRotation);

            directionVector.Normalize();
            Vector2 impulse = directionVector * Mathf.Pow((rotateVectorPlane(velocity, directionVectorRotation)).x , 2)/distance;
            print(impulse.x + "       " + impulse.y + "        " + directionVectorRotation + " " + distance);
            GetComponent<Rigidbody2D>().AddForce(mass *impulse * Time.deltaTime, ForceMode2D.Impulse);
            Vector2 tension = -directionVector * rotateVectorPlane(velocity, directionVectorRotation).y;
            GetComponent<Rigidbody2D>().AddForce(mass * tension * Time.deltaTime, ForceMode2D.Impulse);
            if(distance > slackLength)
            {
                transform.position += (distance - slackLength) * new Vector3 (directionVector.x, directionVector.y);
            }
            //Vector2 impulse = directionVector * Mathf.Pow((GetComponent<Rigidbody2D>().velocity.magnitude * Mathf.Cos(directionVectorRotation * 6.283185307f / 360f)), 2) / rotatedDirectionVector.magnitude;
            //

            /* 


             Vector2 v; 
             if (rotateVectorPlane(GetComponent<Rigidbody2D>().velocity, directionVectorRotation).y < 0 & 0 > directionVectorRotation & directionVectorRotation > -90 )
             {
                 v = -directionVector * rotateVectorPlane(GetComponent<Rigidbody2D>().velocity, directionVectorRotation).y;
                 //print(v.x + " " + v.y);
                 GetComponent<Rigidbody2D>().AddForce(v * Time.deltaTime, ForceMode2D.Impulse);
             }
             if (rotateVectorPlane(GetComponent<Rigidbody2D>().velocity, directionVectorRotation).y > 0 & -90 > directionVectorRotation & directionVectorRotation > -180)
             {
                 v = directionVector * rotateVectorPlane(GetComponent<Rigidbody2D>().velocity, directionVectorRotation).y;
                 //print(v.x + " " + v.y);
                 GetComponent<Rigidbody2D>().AddForce(v * Time.deltaTime, ForceMode2D.Impulse);
             }
             //impulse = directionVector * 55 * (maxRopeLength - distance) *1;

             
             */
            //print(rotatedDirectionVector.x + "       " + rotatedDirectionVector.y + "        " + directionVectorRotation);
            //get the velocity vector, copy it into vector2, rotate vector 2 into the plane, add an impulse that is the difference between the two.
            /* velocity1 + dv + velocity2 
             velocity2 is perpindicular to direction vector. Its magnitude is the dot product of vector1 and the direction perpindicular to the direction 
             vector in the x direciton of v1.*/
            //
            //Vector2 perpindicularDirectionVector = Perpindicularize(directionVector, rotateVectorPlane(velocity, directionVectorRotation).x <= 0);
            //GetComponent<Rigidbody2D>().AddForce(directionVector*Mathf.Sin((directionVectorRotation-oldDirectionVectorRotation)*2*Mathf.PI/360f)*velocity.magnitude *Time.deltaTime, ForceMode2D.Impulse);
            //need to use rotate vector and if x is positive its counter clock wise, and if it is negative, it is clockwise. 
            //oldDirectionVectorRotation = directionVectorRotation;

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
