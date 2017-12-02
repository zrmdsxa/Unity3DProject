using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponScript : MonoBehaviour
{

    public Transform m_rightHand;

    public Text m_ammoText;
    public Text m_grenadeText;
    public Text m_pickupText;

    public GameObject m_grenadePrefab;
    private GameObject m_currentWeapon = null;
    private bool m_hasWeapon = false;

    private GameObject m_closestWeapon = null;

    private bool m_reloading = false;

    private float m_reloadTime;
    private float m_currentReloadTime;

    //need access to xdeg and ydeg tor global hand rotations
    private PlayerCameraScript m_PCS_script;
    private GunScript m_GunScript;

    private Animator m_animationController;

    //0 = 5.56mm
    //1 = 9mm
    //2 = grenade
    private int[] m_storedAmmo = new int[3];


    // Use this for initialization
    void Start()
    {
        m_PCS_script = GetComponent<PlayerCameraScript>();
        m_animationController = GetComponent<Animator>();
        m_storedAmmo[0] = 0;
        m_storedAmmo[1] = 0;
        m_storedAmmo[2] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GamePaused())
        {
            if (Input.GetButtonDown("Use"))
            {
                UseKey();
            }
            if (m_hasWeapon)
            {
                if (Input.GetAxis("Attack") != 0)
                {
                    //Debug.Log("PlayerWeaponScript:Fire");
                    if (m_GunScript.Fire())
                    {
                        m_PCS_script.AddRecoil();
                    }
                }
                if (Input.GetButtonDown("Reload"))
                {

                    if (!m_reloading)
                    {
                        if (m_storedAmmo[m_GunScript.m_ammoType] > 0 && m_GunScript.GetRemainingBullets() < m_GunScript.m_maxRounds)
                        {
                            m_reloading = true;
                            m_animationController.SetBool("isReloading", true);
                            m_reloadTime = m_GunScript.StartReload();
                            m_currentReloadTime = 0.0f;
                            m_storedAmmo[m_GunScript.m_ammoType] += m_GunScript.GetRemainingBullets();
                            m_GunScript.ReloadBullets(0);
                        }
                    }
                }

            }
            if (m_reloading)
            {
                if (m_currentReloadTime < m_reloadTime)
                {
                    m_currentReloadTime += Time.deltaTime;
                }
                else if (m_currentReloadTime >= m_reloadTime)
                {
                    m_reloading = false;
                    m_animationController.SetBool("isReloading", false);
                    //Debug.Log("reloaded:" + Mathf.Clamp(m_GunScript.m_maxRounds, 1, m_storedAmmo[m_GunScript.m_ammoType]) + " bullets");
                    m_GunScript.ReloadBullets(Mathf.Clamp(m_GunScript.m_maxRounds, 1, m_storedAmmo[m_GunScript.m_ammoType]));
                    m_storedAmmo[m_GunScript.m_ammoType] -= Mathf.Clamp(m_GunScript.m_maxRounds, 1, m_storedAmmo[m_GunScript.m_ammoType]);
                }
            }
            //2 = grenade
            if (m_storedAmmo[2] > 0)
            {
                if (Input.GetButtonDown("Grenade"))
                {
                    ThrowGrenade();
                }
            }
            UpdateText();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Ammo")
        {
            m_closestWeapon = other.gameObject;
            m_pickupText.text = "Press <E> to pick up " + other.name;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (m_closestWeapon == other.gameObject)
        {
            ClearClosestWeapon();
        }
    }

    void ClearClosestWeapon()
    {
        m_closestWeapon = null;
        m_pickupText.text = "";
    }

    void ThrowGrenade()
    {
        m_storedAmmo[2] -= 1;
        GameObject grenade = Instantiate(m_grenadePrefab, m_rightHand.position + m_rightHand.forward * 0.5f, m_rightHand.rotation);
        grenade.GetComponent<Rigidbody>().velocity = ((m_rightHand.forward * 7.0f) + (m_rightHand.up * 2.0f));
    }

    void UseKey()
    {
        if (m_closestWeapon != null)
        {
            if (m_closestWeapon.tag == "Weapon")
            {


                if (m_hasWeapon)
                {
                    m_storedAmmo[m_GunScript.m_ammoType] += m_GunScript.GetRemainingBullets();
                    Destroy(m_currentWeapon);
                    //Debug.Log("Destroyed weapon");
                }

                //Debug.Log("Picked up weapon");
                m_currentWeapon = m_closestWeapon.gameObject;
                m_GunScript = m_currentWeapon.GetComponent<GunScript>();
                Debug.Log(m_GunScript);
                m_hasWeapon = true;

                //parent+put the gun where the hand is and set rotation based on xdeg and ydeg
                m_currentWeapon.transform.parent = m_rightHand;
                m_currentWeapon.transform.position = m_rightHand.position;
                m_currentWeapon.transform.rotation = Quaternion.Euler(m_PCS_script.m_yDeg, m_PCS_script.m_xDeg, 0);
                m_currentWeapon.GetComponent<Collider>().enabled = false;
            }
            else if (m_closestWeapon.tag == "Ammo")
            {
                m_storedAmmo[0] += 30;
                m_storedAmmo[1] += 20;
                m_storedAmmo[2] += 1;
                Destroy(m_closestWeapon);
                //Debug.Log("Picked up ammo");
            }

            ClearClosestWeapon();

        }
    }

    void UpdateText()
    {
        if (m_GunScript != null)
        {
            m_ammoText.text = m_currentWeapon.name + ": " + m_GunScript.GetRemainingBullets().ToString("0") + " | " + m_storedAmmo[m_GunScript.m_ammoType].ToString("0");
        }
        else
        {
            m_ammoText.text = "";
        }
        m_grenadeText.text = "G x " + m_storedAmmo[2].ToString("0");
    }
}
