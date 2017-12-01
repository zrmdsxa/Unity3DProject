using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    //public string m_moveStatus = "idle";
    public float m_gravity = 20.0f;

    //public Collider m_weaponHitBox;

    //movement speeds

    public float m_jumpSpeed = 8.0f;
    public float m_runSpeed = 10.0f;
    public float m_walkSpeed = 4.0f;
    //public float m_turnSpeed = 250.0f;
    //public float m_moveBackwardsMultiplier = 0.75f;
    //public Transform mesh;

    //internal variables
    //private float m_speedMultiplier = 1.0f;
    private bool m_grounded = false;
    private Vector3 m_moveDirection = Vector3.zero;
    private bool m_isWalking = false;
    private bool m_jumping = false;

    private bool m_isAlive = true;
    private bool m_mouseSideDown = false;
    //private CharacterController m_controller;
    private Rigidbody m_rb;
    private Animator m_animationController;

    private int m_attackState;

    void Awake()
    {
        //get the controllers
        //m_controller = GetComponent<CharacterController>();
        m_rb = GetComponent<Rigidbody>();
        m_animationController = GetComponent<Animator>();

        //m_weaponHitBox.enabled = false;
    }

    void Update()
    {
        //m_moveStatus = "idle";

        //hold run to run
        if (m_isAlive)
        {

            if (Input.GetAxis("Run") != 0)
            {
                m_isWalking = false;
                //m_animationController.SetBool("isRunning",true);

            }
            else
            {
                m_isWalking = true;
                //m_animationController.SetBool("isRunning",false);
            }

            if (m_grounded)
            {


                m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                //use the run of the walkspeed
                m_moveDirection *= m_isWalking ? m_walkSpeed : m_runSpeed;

                //jump
                if (Input.GetButton("Jump"))
                {
                    m_jumping = true;
                    m_moveDirection.y = m_jumpSpeed;

                }

                //tell the animator whats going on
                if (m_moveDirection.z != 0)
                {   //TODO: Fuck you magic number
                    //m_animationController.SetBool("isWalking",true);
                }
                else
                {
                    //m_animationController.SetBool("isWalking",false);
                }

                m_animationController.SetFloat("Speed", m_moveDirection.magnitude);
                m_animationController.SetFloat("DirectionLR", m_moveDirection.x);
                m_animationController.SetFloat("DirectionFB", m_moveDirection.z);

                //transform direction
                if (m_moveDirection.magnitude > m_runSpeed)
                {
                    m_moveDirection = m_moveDirection.normalized * m_runSpeed;
                }
                m_moveDirection = transform.TransformDirection(m_moveDirection);



            }//end if grounded
            else
            {
                m_moveDirection.y -= m_gravity * Time.deltaTime;
            }


            //Debug.Log(m_moveDirection);
            //move character controller and check if grounded
            //m_grounded = ((m_controller.Move(m_moveDirection * Time.deltaTime)) & CollisionFlags.Below) != 0;

            m_rb.velocity = m_moveDirection;


            //transform.localPosition += m_moveDirection * Time.deltaTime;
            
            m_grounded = Physics.Raycast(transform.position,transform.up * -1f,0.1f,LayerMask.GetMask("Default","Shootable"));
            //Debug.Log(m_grounded);
            //reset jumping after grounded
            m_jumping = m_grounded ? false : m_jumping;
            //Debug.Log(m_jumping);
            if (m_jumping)
            {
                m_animationController.SetBool("Jump", true);
            }
            else
            {
                m_animationController.SetBool("Jump", false);
            }

            //Debug.Log(m_rb.velocity);

        }

    }

	public void PlayerDied(){
		m_isAlive = false;
        m_rb.velocity = Vector3.zero;
        enabled = false;
	}
}
