using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

	public Transform m_rightHand;
    private GameObject m_currentWeapon = null;
	private bool m_hasWeapon = false;

	//need access to xdeg and ydeg tor global hand rotations
	private PlayerCameraScript m_PCS_script;	

    // Use this for initialization
    void Start()
    {
		m_PCS_script = GetComponent<PlayerCameraScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }


	//warn: will pick up all weapons within range
    void OnTriggerStay(Collider other)
    {
        Debug.Log("in other collider");
        if (Input.GetButtonDown("Use"))
        {
            if (other.tag == "Weapon")
            {
				if (m_hasWeapon){
					Destroy(m_currentWeapon);
					Debug.Log("Destroyed weapon");
				}
				Debug.Log("Picked up weapon");
				m_currentWeapon = other.gameObject;
				m_hasWeapon = true;

				//parent+put the gun where the hand is and set rotation based on xdeg and ydeg
				other.transform.parent = m_rightHand;
				other.transform.position = m_rightHand.position;
				other.transform.rotation = Quaternion.Euler(m_PCS_script.m_yDeg,m_PCS_script.m_xDeg,0);
				other.enabled = false;
				Debug.Log("pick up weapon x:"+other.transform.rotation.eulerAngles.x);
            }
        }

    }
}
