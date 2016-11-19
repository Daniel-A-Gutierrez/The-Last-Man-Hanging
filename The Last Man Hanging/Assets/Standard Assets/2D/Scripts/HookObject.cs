using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class HookObject : MonoBehaviour
{

    [SerializeField]
    float hookSpeed;
    [SerializeField]
    float hitRadius;
    public bool RETURN;
    public Vector2 playerPosition;
    Vector2 normalizedVelocityFactor;
    [SerializeField]
    LayerMask whatIsGrappleable; // grips and players
    [SerializeField]
    LayerMask hooks; //hooks
    public bool isHooked;
    Vector2 hookedPosition;
    public int parentID;
    public string hookID;
    public float maxDistance;
    public GameObject player;
    public bool isTensioned;
    public AudioClip myClip;
#pragma warning disable CS0108
    AudioSource audio;
#pragma warning restore CS0108 
    public bool actuallyReturn;
    void Start()
    {
        RETURN = false;
        audio = GetComponent<AudioSource>();
    }
    public void Throw(GameObject go, char LorR)
    {
        isHooked = false;
        isTensioned = false;
        this.maxDistance = go.GetComponent<HardCodedGrapple>().maxRopeLength;
        if (hookSpeed == 0 | hitRadius == 0)
        {
            hookSpeed = 10f;
            hitRadius = 2f;
        }
        playerPosition = go.transform.position;
        parentID = go.GetComponent<HardCodedGrapple>().PlayerNumber;
        hookID = "" + parentID + LorR;

        Vector2 target = GameObject.Find("sample-reticle").GetComponent<Aiming>().getAimVector();
        target.Normalize();
        transform.position = playerPosition + target * hitRadius + target * go.GetComponent<BoxCollider2D>().size.magnitude * .5f;
        GetComponent<Rigidbody2D>().velocity = target * hookSpeed ;
        player = go;
    }



    void Update()
    {
        Vector2 moveDirection = gameObject.GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 135;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (isHooked & !RETURN) // if youre swinging.
        {
            transform.position = hookedPosition;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            float distance = Vector2.Distance(playerPosition, transform.position);
            if (distance >= player.GetComponent<HardCodedGrapple>().slackLength)
            {
                isTensioned = true;
            }
            else
            {
                isTensioned = false;
            }
        }
        else if (!RETURN & !isHooked)
        {
            float distance = Vector2.Distance(playerPosition, transform.position);
            if (distance > maxDistance)
            {
                RETURN = true;
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius, whatIsGrappleable);
            Collider2D[] otherHooks = Physics2D.OverlapCircleAll(transform.position, hitRadius, hooks);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    if (parentID != 0)
                    {
                        if (colliders[i].gameObject != player)
                        {
                            transform.position = colliders[i].gameObject.transform.position;
                            Vector2 zero = new Vector2(0, 0);
                            GetComponent<Rigidbody2D>().velocity = zero;
                            hookedPosition = transform.position;
                            isHooked = true;
                            player.GetComponent<HardCodedGrapple>().slackLength = distance - .1f;
                            audio.Play();
                        }
                    }
                }
            }

            for (int i = 0; i < otherHooks.Length; i++)
            {

                if (otherHooks[i].gameObject != gameObject)
                {
                    if (parentID != 0)
                    {
                        if (otherHooks[i].GetComponent<HookObject>().hookID.ToCharArray()[0] != hookID.ToCharArray()[0])
                        {
                            //hook collision with another player's hook
                            GameObject tempPlayer = otherHooks[i].GetComponent<HookObject>().player;
                            if (otherHooks[i].GetComponent<HookObject>().hookID.EndsWith("R"))
                            {
                                tempPlayer.GetComponent<HardCodedGrapple>().RHookOut = false;
                            }
                            if (otherHooks[i].GetComponent<HookObject>().hookID.EndsWith("L"))
                            {
                                tempPlayer.GetComponent<HardCodedGrapple>().LHookOut = false;
                            }


                            Destroy(otherHooks[i].gameObject); //WAIT 

                            if (hookID.EndsWith("R"))
                            {
                                player.GetComponent<HardCodedGrapple>().RHookOut = false;
                            }
                            if (hookID.EndsWith("L"))
                            {
                                player.GetComponent<HardCodedGrapple>().LHookOut = false;
                            }

                            Destroy(gameObject);

                        }
                    }
                }
            }

        }
        else if (RETURN)
        {
            if (true)
            {
                isHooked = false;
                isTensioned = false;
                normalizedVelocityFactor = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);
                normalizedVelocityFactor.Normalize();
                GetComponent<Rigidbody2D>().velocity = normalizedVelocityFactor * hookSpeed + player.GetComponent<Rigidbody2D>().velocity;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius, whatIsGrappleable);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject == player)
                    {
                        if (hookID.EndsWith("R"))
                        {
                            player.GetComponent<HardCodedGrapple>().RHookOut = false; // shits broken af
                        }
                        if (hookID.EndsWith("L"))
                        {
                            player.GetComponent<HardCodedGrapple>().LHookOut = false; // shits broken af
                        }
                        Destroy(gameObject);
                    }
                }
            }
            if (!isHooked & !actuallyReturn)
            {
                float distance = Vector2.Distance(playerPosition, transform.position);
                if (distance > maxDistance + 1)
                {
                    RETURN = true;
                    actuallyReturn = true;
                }
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius, whatIsGrappleable);
                Collider2D[] otherHooks = Physics2D.OverlapCircleAll(transform.position, hitRadius, hooks);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        if (parentID != 0)
                        {
                            if (colliders[i].gameObject != player)
                            {
                                transform.position = colliders[i].gameObject.transform.position;
                                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                hookedPosition = transform.position;
                                isHooked = true;
                                RETURN = false;
                                player.GetComponent<HardCodedGrapple>().slackLength = distance - .1f;
                                audio.Play();
                            }
                        }
                    }
                }
                for (int i = 0; i < otherHooks.Length; i++)
                {
                    if (otherHooks[i].gameObject != gameObject)
                    {
                        if (parentID != 0)
                        {
                            if (otherHooks[i].GetComponent<HookObject>().hookID.ToCharArray()[0] != hookID.ToCharArray()[0])
                            {
                                //hook collision with another player's hook
                                GameObject tempPlayer = otherHooks[i].GetComponent<HookObject>().player;
                                if (otherHooks[i].GetComponent<HookObject>().hookID.EndsWith("R"))
                                {
                                    tempPlayer.GetComponent<HardCodedGrapple>().RHookOut = false;
                                }
                                if (otherHooks[i].GetComponent<HookObject>().hookID.EndsWith("L"))
                                {
                                    tempPlayer.GetComponent<HardCodedGrapple>().LHookOut = false;
                                }


                                Destroy(otherHooks[i].gameObject); //WAIT 

                                if (hookID.EndsWith("R"))
                                {
                                    player.GetComponent<HardCodedGrapple>().RHookOut = false;
                                }
                                if (hookID.EndsWith("L"))
                                {
                                    player.GetComponent<HardCodedGrapple>().LHookOut = false;
                                }

                                Destroy(gameObject);

                            }
                        }
                    }
                }
            }
        }
    }
}
