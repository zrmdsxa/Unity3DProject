using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	public string m_moveStatus = "idle";
	public bool m_walkByDefault = true;
	public float m_gravity = 20.0f;

	// Movement Speeds 
	public float m_jumpSpeed = 8.0f;
	public float m_runSpeed = 10.0f;
	public float m_walkSpeed = 4.0f;
	public float m_turnSpeed = 250.0f;
	public float m_moveBackwardsMultiplier = 0.75f;


	// internal variables 
	private float m_speedMultiplier = 0.0f; 
	private bool m_grounded = false;
	private Vector3 m_moveDirection = Vector3.zero;
	private bool m_isWalking = false;
	private bool m_jumping = false;
	private bool m_mouseSideDown = false;
	private CharacterController m_controller; 
	private Animator m_animationController; 


	void Awake() {
		// get the controllers 
		m_controller = GetComponent<CharacterController>();
		m_animationController = GetComponent<Animator>();
	}

	void Update() {
		m_moveStatus = "idle";
		m_isWalking = m_walkByDefault;

		// hold run to run 
		if(Input.GetAxis("Run") != 0) {
			m_isWalking = !m_walkByDefault;
		}

		if(m_grounded) {
			// if the player is steering with the right mouse button ... A/D strafe 
			if(Input.GetMouseButton(1)) {
				m_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			} else {
				m_moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
			}

			// auto-move button pressed 
			if(Input.GetButtonDown("Toggle Move")) {
				m_mouseSideDown = !m_mouseSideDown;
			}

			// player moved or otherwise inturrupted auto-move
			if(m_mouseSideDown && (Input.GetAxis("Vertical") != 0 || 
				Input.GetButton("Jump")) || (Input.GetMouseButton(0) && Input.GetMouseButton(1))) {

				m_mouseSideDown = false;
			}

			// L+R MouseButton Movement 
			if((Input.GetMouseButton(0) && Input.GetMouseButton(1)) || m_mouseSideDown) {
				m_moveDirection.z = 1;
			}

			// if not strafing with right mouse and horizontal, check for strafe keys 
			if(!(Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0)) {
				m_moveDirection.x -= Input.GetAxis("Strafing");
			}

			// if moving forward/backwards and sideways at the same time, compensate for distance
			if(((Input.GetMouseButton(1) && Input.GetAxis("Horizontal") !=0) || 
				Input.GetAxis("Strafing") != 0) && Input.GetAxis("Vertical") != 0) {

				m_moveDirection *= 0.707f; // TODO: Fuck you Magic Number
			}

			// apply the move backwards multiplier if not moving forwards only. 
			if((Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0) || 
				Input.GetAxis("Strafing") != 0 || Input.GetAxis("Vertical") < 0) {

				m_speedMultiplier = m_moveBackwardsMultiplier;
			} else {
				m_speedMultiplier = 1f;
			}

			// use the run or the walkspeed 
			m_moveDirection *= m_isWalking ? m_walkSpeed * m_speedMultiplier : m_runSpeed * m_speedMultiplier;

			// Jump 
			if(Input.GetButton("Jump")) {
				m_jumping = true;
				m_moveDirection.y = m_jumpSpeed;
			}

			// tell the animator whats going on 
			if(m_moveDirection.magnitude > 0.05f) { // TODO: Fuck you magic number 
				m_animationController.SetBool("isWalking", true);
			} else {
				m_animationController.SetBool("isWalking", false);
			}

			m_animationController.SetFloat("Speed", m_moveDirection.z); 
			m_animationController.SetFloat("Direction", m_moveDirection.x);

			// transform direction 
			m_moveDirection = transform.TransformDirection(m_moveDirection);
		} // end if grounded 

		// Character must face the same direction as the camera when the right mouse button is down
		if(Input.GetMouseButton(1)) {
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
		} else {
			transform.Rotate(0, Input.GetAxis("Horizontal") * m_turnSpeed * Time.deltaTime, 0);
		}

		// apply gravity 
		m_moveDirection.y -= m_gravity * Time.deltaTime;

		// move charactercontroller and check if grounded 
		m_grounded = ((m_controller.Move(m_moveDirection * Time.deltaTime)) & CollisionFlags.Below) != 0;

		// reset jumping after grounded 
		m_jumping = m_grounded ? false : m_jumping;

		if(m_jumping) {
			m_moveStatus = "jump";
		}
	}	
}
