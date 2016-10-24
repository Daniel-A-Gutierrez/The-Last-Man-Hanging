using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(Rigidbody2D))]
public class HookObject : MonoBehaviour
{

    // Use this for initialization
    [SerializeField] float hookSpeed;
    [SerializeField] float hitRadius;
    public bool RETURN; // will be set by the character's HardCodedGrapple Script to true when the player releases left or right
                        // mouse.
    public Vector2 playerPosition;
    Vector2 normalizedVelocityFactor;
    [SerializeField] LayerMask whatIsGrappleable;
    public bool isHooked;
    Vector2 hookedPosition;
    public int parentID;
    public string hookID;
    void Start ()
    {

        RETURN = false;
        
    }
    public void Throw(Vector2 pos, int ParentID, char LorR) // NOT LOWER CASE THROW
    {
        isHooked = false;
        if (hookSpeed == 0 | hitRadius == 0)
        { 
            hookSpeed = 10f;
            hitRadius = 2f;
        }
        playerPosition = pos;
        parentID = ParentID;
        hookID = "" + parentID + LorR;
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition); //thanks shawn for this function
        normalizedVelocityFactor = new Vector2(target.x - pos.x, target.y - pos.y);
        normalizedVelocityFactor.Normalize();
        transform.position = playerPosition + normalizedVelocityFactor * hitRadius*2;
        GetComponent<Rigidbody2D>().velocity = normalizedVelocityFactor * hookSpeed;
        
        //float hypotenuse = Mathf.Sqrt(Mathf.Pow(target.x - transform.position.x, 2) +
        //    Mathf.Pow(target.y - transform.position.y, 2));
        
        
        
    }
	// Update is called once per frame
	void Update ()
    {

        if (isHooked &!RETURN)
        {
            transform.position = hookedPosition;
            //isHooked = true;
        }
        else if(!RETURN & !isHooked)
        { 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius, whatIsGrappleable);
            for (int i = 0; i < colliders.Length; i++)
            {
                
                if (colliders[i].gameObject != gameObject )
                {
                        if (parentID != null)
                        {
                            if (colliders[i].gameObject.GetComponent<HardCodedGrapple>() == null)
                            {
                                transform.position = colliders[i].gameObject.transform.position;
                                Vector2 zero = new Vector2(0, 0);
                                GetComponent<Rigidbody2D>().velocity = zero;
                                hookedPosition = transform.position;
                                isHooked = true;
                            }
                        }
                 }
            }
        }
        if (RETURN)
        {
            isHooked = false;
            normalizedVelocityFactor = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);
            normalizedVelocityFactor.Normalize();
            GetComponent<Rigidbody2D>().velocity = normalizedVelocityFactor * hookSpeed;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius, whatIsGrappleable);
            for (int i = 0; i < colliders.Length; i++)
            {
                print(parentID);
                try
                {

                    if (colliders[i].gameObject.GetComponent<HardCodedGrapple>().PlayerNumber == parentID)
                    {
                        if (hookID.EndsWith("L"))
                        {
                            colliders[i].gameObject.GetComponent<HardCodedGrapple>().LHookOut = false; // shits broken af
                        }
                        if (hookID.EndsWith("R"))
                        {
                            colliders[i].gameObject.GetComponent<HardCodedGrapple>().RHookOut = false; // shits broken af
                        }
                        Destroy(gameObject);

                    }
                }
                catch (System.Exception e)
                {
                    //do nothing. 
                }
            }
        }

    }
}
