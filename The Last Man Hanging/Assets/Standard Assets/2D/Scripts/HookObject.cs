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
        AudioSource audio;
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
            transform.position = playerPosition + target * hitRadius + target * go.GetComponent<BoxCollider2D>().size.magnitude * 1.05f;
            GetComponent<Rigidbody2D>().velocity = target * hookSpeed;
            player = go;
        }



        void Update()
        {

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
            if (RETURN)
            {
                isHooked = false;
                isTensioned = false;
                normalizedVelocityFactor = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y);
                normalizedVelocityFactor.Normalize();
                GetComponent<Rigidbody2D>().velocity = normalizedVelocityFactor * hookSpeed;
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
        }
    }

