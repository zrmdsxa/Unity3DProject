using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGCameraController : MonoBehaviour {

	public Transform m_Target;

	public float  m_heightOffSet = 1.7f;
	public float  m_distance = 12.0f;
	public float  m_offsetFromWall = 0.1f;
	public float  m_minDistance = 0.6f;
	public float  m_MaxDistance = 20f;
	public float  m_xSpeed = 20f;
	public float  m_ySpeed = 20f;
	public float m_yMinLimit = -80f;
	public float m_yMaxLimit = 80f;
	public float  m_zoomSpeed = 5.0f;
	public float  m_autoRotationSpeed = 5.0f;
	public LayerMask m_collisionLayers = -1;
	public bool m_alwaysRotatedToRearOffTarget = false;
	public bool m_alwaysMouseInputX = true;
	public bool m_allowMouseInputY = true;


	private float m_xDeg = 0.0f;
	private float m_yDeg = 0.0f;
	private float m_currentDistance;
	private float m_desiredDistance;
	private float m_correctedDistance;
	private bool m_rotatebehind = false;
	private bool m_mouseSideButton = false;


	void Start(){
		Vector3 angles = transform.eulerAngles;
		m_xDeg = angles.x;
		m_yDeg = angles.y;
		Vector3 distance = m_Target.position - transform.position;
		m_currentDistance = distance.magnitude;
		m_desiredDistance = distance.magnitude;
		m_correctedDistance = m_currentDistance;
		if(m_alwaysRotatedToRearOffTarget){
			m_rotatebehind = true;

		}
	}

	void LateUpdate(){
		//auto move button pressed
		if(Input.GetButton("Toggle Move")){
			m_mouseSideButton = !m_mouseSideButton;
		}

		// player moved or interupted the auto-move
		if(m_mouseSideButton && (Input.GetAxis("Vertical") !=0 || Input.GetButton("Jump") ||  (Input.GetMouseButton(0)&& Input.GetMouseButton(1) ))){
			m_mouseSideButton = false;

		}
			// if either mousebutton ared own , let the mouse govern cameras postion
			if(GUIUtility.hotControl == 0){// if = to 0 nothing has been pressed if it doesnt eaual to zero then something has been pressed ie mouse  space bar 
				if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
					//check to see if the mouse input is alowed on the axis
					if(m_alwaysMouseInputX){
						m_xDeg += Input.GetAxis("Mouse X") * m_xSpeed * 0.02f; //TODO: fuck you magic number

					}else {
						RotateBehindTarget();
					}
					if(m_allowMouseInputY){
						m_xDeg -= Input.GetAxis("Mouse X") * m_xSpeed * 0.02f; //TODO: fuck you magic number

					
					}
					if(!m_alwaysRotatedToRearOffTarget){
						m_rotatebehind = false;
					}

				}else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || m_rotatebehind || m_mouseSideButton){
				RotateBehindTarget();

				}
			}// end od of guiutility.hotcontrols check

			// ensure the cameras pitch is with

			m_yDeg = ClampAngles(m_yDeg, m_yMinLimit, m_yMaxLimit);
			Quaternion rotation = Quaternion.Euler(m_yDeg, m_xDeg, 0);
			// calculated the desired distance
			m_desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * m_zoomSpeed * Mathf.Abs(m_desiredDistance);

			m_desiredDistance = Mathf.Clamp(m_desiredDistance, m_minDistance,m_MaxDistance);

			m_correctedDistance = m_desiredDistance;

			// calculate desired camera posotion
			Vector3 vTargetOffSet = new Vector3(0,-m_heightOffSet, 0);
			Vector3 position =  m_Target.transform.position - (rotation * Vector3.forward * m_desiredDistance + vTargetOffSet);


			// check for collisiopn using the true target desired registration point as set by height

			RaycastHit CollionHit;
			Vector3 trueTargetPosition = new Vector3(m_Target.transform.position.x, m_Target.transform.position.y, m_Target.transform.position.z);
			// if there is a collision we can correct it
			bool isCorrected = false;
			if(Physics.Linecast(trueTargetPosition, position, out CollionHit, m_collisionLayers)){
			
			// calculate the distance from the original estimates position to the collision
			// location , subtracting the safe offset distancr from the object we hit the offset will keep the camera from being right on top of the surface we hit which usually shows up as the surface geometry getting partiall clipped by the cameras near clip plane 
			


				m_correctedDistance = Vector3.Distance(trueTargetPosition,CollionHit.point) - m_offsetFromWall;
				isCorrected = true;

			}

			if(!isCorrected || m_correctedDistance > m_currentDistance){
				m_currentDistance = Mathf.Lerp(m_currentDistance, m_correctedDistance, Time.deltaTime * m_zoomSpeed );
			}else {
				m_currentDistance = m_collisionLayers;

			}
			//keep within the limits
			m_currentDistance = Mathf.Clamp(m_currentDistance, m_minDistance, m_MaxDistance);
			//recalaculate the postion based on curren distance
			position = m_Target.transform.position - (rotation * Vector3.forward * m_currentDistance);
			//finally set the rotation and position of the camera

			transform.rotation = rotation;
			transform.position = position;



	}

	private void RotateBehindTarget(){
		float targetRotationAngle = m_Target.transform.eulerAngles.y;
		float cuurentRotationAngle = transform.eulerAngles.y;
		m_xDeg = Mathf.Lerp(cuurentRotationAngle, targetRotationAngle, m_autoRotationSpeed * Time.deltaTime);
		if(targetRotationAngle == cuurentRotationAngle){
			if(!m_alwaysRotatedToRearOffTarget){
				m_rotatebehind = false;
			}
		}else{
			m_rotatebehind = true;
		}

	}

	private float ClampAngles(float Angle, float min, float max){
		
		if(Angle < -360f){
			Angle += 360f;
		}
		if(Angle < -360f){
			Angle -= 360f;
		}

		return  Mathf.Clamp(Angle,min,max);
	}

}
