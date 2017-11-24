﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public string m_moveStatus = "idle";
    public float m_gravity = 20.0f;

	//public Collider m_weaponHitBox;

    //movement speeds

    public float m_jumpSpeed = 8.0f;
    public float m_runSpeed = 10.0f;
    public float m_walkSpeed = 4.0f;
    public float m_turnSpeed = 250.0f;
    public float m_moveBackwardsMultiplier = 0.75f;
	//public Transform mesh;

    //internal variables
    private float m_speedMultiplier = 1.0f;
    private bool m_grounded = false;
    private Vector3 m_moveDirection = Vector3.zero;
    private bool m_isWalking = false;
    private bool m_jumping = false;
	private bool m_mouseSideDown = false;
    private CharacterController m_controller;
    private Animator m_animationController;

	private int m_attackState;

    void Awake()
    {
        //get the controllers
        m_controller = GetComponent<CharacterController>();
        m_animationController = GetComponent<Animator>();
		m_attackState = Animator.StringToHash("UpperTorso.attack");

		//m_weaponHitBox.enabled = false;
    }

    void Update()
    {
        m_moveStatus = "idle";

        //hold run to run
        if (Input.GetAxis("Run") != 0)
        {
            m_isWalking = true;
			m_animationController.SetBool("isRunning",true);

        } else{
			m_isWalking = false;
			m_animationController.SetBool("isRunning",false);
		}

        if (m_grounded)
        {

			m_moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

			//use the run of the walkspeed
			m_moveDirection *= m_isWalking ? m_walkSpeed*m_speedMultiplier : m_runSpeed * m_speedMultiplier;

			//jump
			if(Input.GetButton("Jump")){
				m_jumping = true;
				m_moveDirection.y = m_jumpSpeed;

			}

			//tell the animator whats going on
			if(m_moveDirection.z != 0){	//TODO: Fuck you magic number
				m_animationController.SetBool("isWalking",true);
			}
			else{
				m_animationController.SetBool("isWalking",false);
			}

			m_animationController.SetFloat("Speed",m_moveDirection.z);
			//m_animationController.SetFloat("Direction",m_moveDirection.x);

			//transform direction
			m_moveDirection = transform.TransformDirection(m_moveDirection);
			


        }//end if grounded

		//Character must face the same direction as the camera when the right mouse button is down
		// if(Input.GetMouseButton(1)){
		// 	transform.rotation = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
		// }
		// else{
		// 	transform.Rotate(0,Input.GetAxis("Horizontal") * m_turnSpeed * Time.deltaTime,0);
		// }
		//apply gravity
		m_moveDirection.y -= m_gravity * Time.deltaTime;
		
		Debug.Log(m_moveDirection);
		//move character controller and check if grounded
		m_grounded = ((m_controller.Move(m_moveDirection * Time.deltaTime)) & CollisionFlags.Below) !=0 ;

		//reset jumping after grounded
		m_jumping = m_grounded ? false : m_jumping;

		if(m_jumping){
			m_moveStatus = "jump";
		}

		//is the player attacking?
		AnimatorStateInfo currentUpperTorsoState = m_animationController.GetCurrentAnimatorStateInfo(1);
		// if (currentUpperTorsoState.fullPathHash == m_attackState){
		// 	//m_weaponHitBox.enabled = true;
		// }
		// else{
		// 	if(Input.GetButtonDown("Attack")){
		// 		m_animationController.SetBool("isAttacking",true);
		// 	}else{
		// 		m_animationController.SetBool("isAttacking",false);
		// 		//m_weaponHitBox.enabled = false;
		// 	}
		// }
    }
}
