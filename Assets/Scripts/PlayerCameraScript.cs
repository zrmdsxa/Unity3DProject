using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour {

	public float m_xSpeed = 200.0f;
	//public float m_ySpeed = 200.0f;

	//public Transform m_torso;

	float m_xDeg = 0.0f;
	//float m_yDeg = 0.0f;

	//float m_yMinLimit = -20.0f;
	//float m_yMaxLimit = 70.0f;


	void Start () {
		Vector3 angles = transform.eulerAngles;
        m_xDeg = angles.x;
        //m_yDeg = angles.y;
	}
	
	void LateUpdate () {

		m_xDeg += Input.GetAxis("Mouse X") * m_xSpeed * Time.deltaTime;
		//m_yDeg -= Input.GetAxis("Mouse Y") * m_ySpeed * Time.deltaTime;


		RotateCamera();
	}

	void RotateCamera(){
		
		
		Quaternion rotation1 = Quaternion.Euler(0,m_xDeg,0);

		transform.rotation = rotation1;

		//Quaternion rotation2 = Quaternion.Euler(m_yDeg,0,0);
		
		//m_yDeg = ClampAngle(m_yDeg,m_yMinLimit,m_yMaxLimit);

		//m_torso.localRotation = rotation2;

	}
	//ClampAngle - keeps the angle between 0 and 360 degrees
	private float ClampAngle(float angle, float min, float max)
    {
        float ret = 0.0f;
		if(angle < -360f){
			angle += 360f;
		}
		if(angle > 360f){
			angle -= 360f;
		}
        return Mathf.Clamp(angle,min,max);
    }
}
