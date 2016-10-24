using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .04f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private bool m_JustGrounded = true;
        const float m_PlayerAcceleration = .002f;
        bool suspended2 = false;
        bool suspended1 = false;
        bool suspended3 = false;
        bool suspended4 = false;
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            m_Grounded = false;
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                    m_JustGrounded = false;
                }
            }
            if (m_JustGrounded == true)
            {
                m_Rigidbody2D.position.Set(m_Rigidbody2D.position.x, m_Rigidbody2D.position.y);
                //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                m_JustGrounded = false;
            }
            m_Anim.SetBool("Ground", m_Grounded);
            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            ForceMode2D impulse = ForceMode2D.Impulse;
            if (m_Grounded)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    m_Rigidbody2D.AddForce(new Vector2(-m_MaxSpeed, 0), impulse);
                    suspended1 = false;
                }
                if ((Input.GetKeyUp(KeyCode.A) & !suspended1 )| suspended2)
                {
                    m_Rigidbody2D.AddForce(new Vector2(m_MaxSpeed, 0), impulse);
                    suspended2 = false;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    m_Rigidbody2D.AddForce(new Vector2(m_MaxSpeed, 0), impulse);
                    suspended3 = false;
                }
                if ((Input.GetKeyUp(KeyCode.D) & !suspended3) | suspended4)
                {
                    m_Rigidbody2D.AddForce(new Vector2(-m_MaxSpeed, 0), impulse);
                    suspended4 = false; 
                }
                
            }
            if (!m_Grounded)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    suspended1 = true;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    suspended3 = true;
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    suspended2 = true;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    suspended4 = true;
                }
                if (suspended1 & suspended2)
                {
                    suspended1 = false;
                    suspended2 = false;
                }
                if (suspended3 & suspended4)
                {
                    suspended3 = false;
                    suspended4 = false;
                }
            }

        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                /*
                if (move > 0)
                {
                    //m_Rigidbody2D.velocity = (new Vector2(move * (m_Rigidbody2D.velocity.y + m_PlayerAcceleration), m_Rigidbody2D.velocity.y));
                    
                    if (m_Rigidbody2D.velocity.x > m_MaxSpeed)
                    {
                        //m_Rigidbody2D.velocity = new Vector2(m_MaxSpeed, m_Rigidbody2D.velocity.y);
                    }
                }
                else if (move < 0)
                {
                    //m_Rigidbody2D.velocity = new Vector2(move * (m_Rigidbody2D.velocity.x - m_PlayerAcceleration), m_Rigidbody2D.velocity.y);
                    if (m_Rigidbody2D.velocity.x < -m_MaxSpeed)
                    {
                        //m_Rigidbody2D.velocity = new Vector2(-m_MaxSpeed, m_Rigidbody2D.velocity.y);
                    }
                }
                else if (move == 0) {
                    if (m_Rigidbody2D.velocity.x < 0)
                    {
                       // m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + m_PlayerAcceleration, m_Rigidbody2D.velocity.y);
                        if (m_Rigidbody2D.velocity.x >= 0)
                        {
                           // m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                        }
                    }
                    else
                    {
                        //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - m_PlayerAcceleration, m_Rigidbody2D.velocity.y);
                        if (m_Rigidbody2D.velocity.x <= 0)
                        {
                            //m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                        }
                    }
                }
                */
                
                /*
                if (move > 0)
                { 
                    m_Rigidbody2D.AddForce(new Vector2(m_MaxSpeed*.19f, 0));
                    if (m_Rigidbody2D.velocity.x < 0)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(m_MaxSpeed * .19f, 0));
                    }
                }
                if (move<0)
                { 

                    m_Rigidbody2D.AddForce(new Vector2(-m_MaxSpeed*.19f, 0));
                    if(m_Rigidbody2D.velocity.x > 0)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(-m_MaxSpeed * .19f, 0));
                    }

                }
                if (move == 0 & m_Rigidbody2D.velocity.x != 0)
                {
                    if (m_Rigidbody2D.velocity.x < 0)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(m_MaxSpeed * .45f, 0));
                    }
                    if (m_Rigidbody2D.velocity.x > 0)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(-m_MaxSpeed * .45f, 0));
                    }

                }
                */
                
                // m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_JustGrounded = true;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
