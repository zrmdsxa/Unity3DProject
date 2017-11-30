﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour {

	public float m_xSpeed = 200.0f;
	public float m_ySpeed = 200.0f;

	public Transform m_hand;

	public Transform m_cameraPivot;

	//these must be public for access to sync with weapon
	public float m_xDeg = 0.0f;
	public float m_yDeg = 0.0f;

	float m_yMinLimit = -35.0f;
	float m_yMaxLimit = 60.0f;


	void Start () {
		Vector3 angles = transform.eulerAngles;
        m_xDeg = angles.x;
        //m_yDeg = angles.y;
	}
	
	void LateUpdate () {
		if (Input.GetKeyDown(KeyCode.P)){
			Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

		m_xDeg += Input.GetAxis("Mouse X") * m_xSpeed * Time.deltaTime;
		m_yDeg -= Input.GetAxis("Mouse Y") * m_ySpeed * Time.deltaTime;


		RotateCamera();
	}

	void RotateCamera(){
		
		//rotation 1 is the player's global rotation for camera
		Quaternion rotation1 = Quaternion.Euler(0,m_xDeg,0);

		transform.rotation = rotation1;

		//rotation is the player's global rotation for the hand
		Quaternion rotation2 = Quaternion.Euler(m_yDeg,m_xDeg,0);
		
		m_yDeg = ClampAngle(m_yDeg,m_yMinLimit,m_yMaxLimit);

		m_hand.rotation = rotation2;

		m_cameraPivot.rotation = rotation2;

		//Debug.Log(m_hand.rotation.eulerAngles.x);

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
