using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPGCharactwerController : MonoBehaviour {

	public string m_moveStatus = "idle";
	public bool m_walkByDefault = true;
	public float m_gravity = 20.0f;

	//movements speeds
	public float m_jumpSpeed = 8.0f;
	public float m_runSpeed = 10.0f;
	public float m_walkSpeed = 4.0f;
	public float m_turnSpeed = 250.0f;
	public float m_moveBackwardMultiplier = 0.75f;

	//internl varibles
	private float m_speedMultiplier = 0.0f;

	private bool m_grounded = false;
	private Vector3 m_Diirection = Vector3.zero;

	private bool m_isWalking = false;
	private bool m_jumping = false;
	private bool m_mouseSideDown = false;
	private CharacterController m_controller;
	private Animator m_animationController;

	void Awake(){
		//get the controls
		m_controller = GetComponent<CharacterController>();
		m_animationController = GetComponent<Animator>();
	
	}
	void Update(){
		m_moveStatus = "idle";
		m_isWalking = m_walkByDefault;
			//hold r to run
		if(Input.GetAxis("Run") != 0){
			m_isWalking = !m_walkByDefault;
		}
	
		if(m_grounded){
			//if the player is steering with the right mouse button...A/D strafe
				Debug.Log("Tick");
			if(Input.GetMouseButton(1)){
				m_Diirection = new Vector3(Input.GetAxis("Horizontal") , 0, Input.GetAxis("Vertical"));
			}else{
				m_Diirection = new Vector3(0,0, Input.GetAxis("Vertical"));
			}
					//auto move button press
			if(Input.GetButtonDown("Toggle Move")){
				m_mouseSideDown = !m_mouseSideDown;
			}
			//player moved or otherwise interupted automove
			if(m_mouseSideDown && (Input.GetAxis("Vertical") != 0 || Input.GetButton("Jump") || (Input.GetMouseButton(0 )&& Input.GetMouseButton(1)))){
				m_mouseSideDown = false;

			}
			//l+r mouse button movement
			if((Input.GetMouseButton(0) && Input.GetMouseButton(1))|| m_mouseSideDown){
				m_Diirection.z = 1;
			}

			// if not strafing with right mouse and horizonatl check for strafe keys
			if(!(Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0)) {
				m_Diirection.x -= Input.GetAxis("Strafing");


			}
			//if moving foward backward and sideways at the same timne compensate for distance
			if(((Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0) || Input.GetAxis("Strafing") != 0) && Input.GetAxis("Vertical") != 0){
				m_Diirection *=  0.707f;
			}
			//apply the move backwards multplier if not moving foward only
			if((Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0) || Input.GetAxis("Strafing")!= 0 || Input.GetAxis("Vertical") <0){
				m_speedMultiplier = m_moveBackwardMultiplier;
			}else{
				m_speedMultiplier = 1f;
			}
		

			// use the run or the walk speed
			m_Diirection *= m_isWalking ? m_walkSpeed * m_speedMultiplier : m_runSpeed * m_speedMultiplier;


			// jump 
			if(Input.GetButton("Jump")){
				m_jumping = true;
				m_Diirection.y = m_jumpSpeed;
			}
			// tell the animator whats going on
			if(m_Diirection.magnitude > 0.05f){
				m_animationController.SetBool("isWalking",true);
			}else{
				m_animationController.SetBool("isWalking",false);
			}

			m_animationController.SetFloat("Speed", m_Diirection.z);
			m_animationController.SetFloat("Direction", m_Diirection.x);
			//tranform direction
			m_Diirection = transform.TransformDirection(m_Diirection);

		// end of grounded
		}
		
		//character must face the same direction as the camera with the right mouse buttion down
		if(Input.GetMouseButton(1) ){
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
		} else {
			transform.Rotate(0,Input.GetAxis("Horizontal") *  m_turnSpeed * Time.deltaTime,0);
		}
		m_Diirection.y -= m_gravity *Time.deltaTime;
		m_grounded = ((m_controller.Move(m_Diirection * Time.deltaTime)) & CollisionFlags.Below) != 0;	

		//reset jumping after grounded
		m_jumping = m_grounded ? false : m_jumping;

		if(m_jumping){
			m_moveStatus = "jump";
		}
	}

}
