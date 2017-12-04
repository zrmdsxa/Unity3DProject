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

    private GameObject m_closestItem = null;
    

    private bool m_reloading = false;

    private float m_reloadTime;
    private float m_currentReloadTime;  //goes from 0 to the reload time

    //need access to xdeg and ydeg tor global hand rotations
    private PlayerCameraScript m_PCS_script;
    private GunScript m_GunScript;  //current gun's gunscript

    private Animator m_animationController;

    //0 = 5.56mm
    //1 = 9mm
    //2 = grenade
    private int[] m_storedAmmo = new int[3];

    private GameObject[] m_gunInventory = new GameObject[2];
    
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
                if (Input.GetAxis("Attack") != 0 && m_GunScript.m_auto)
                {
                    //Debug.Log("PlayerWeaponScript:Fire");
                    if (m_GunScript.Fire())
                    {
                        m_PCS_script.AddRecoil();
                    }
                }
                else if (Input.GetButtonDown("Attack"))
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
                if(Input.GetKeyDown(KeyCode.Alpha1)){
                    //take out weapon
                    SwitchWeapons(0);
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2)){
                    //take out weapon
                    SwitchWeapons(1);
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
                    m_GunScript.CancelReload();
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
        if (other.tag == "Weapon" || other.tag == "Ammo"|| other.tag == "PickUp")
        {
            m_closestItem = other.gameObject;
            m_pickupText.text = "Press <E> to pick up " + other.name;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (m_closestItem == other.gameObject)
        {
            ClearClosestWeapon();
        }
    }

    void ClearClosestWeapon()
    {
        m_closestItem = null;
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
        //there is an item to pick up
        if (m_closestItem != null)
        {
            if (m_closestItem.tag == "Weapon")
            {
                int newWeapAmmoType = m_closestItem.GetComponent<GunScript>().GetAmmoType();
                if (m_hasWeapon)
                {
                    
                    

                    //picked up weapon that uses the same ammo as current gun

                    if (newWeapAmmoType == m_GunScript.GetAmmoType()){
                        //store the ammo from our current gun and destroy the gun
                        m_storedAmmo[m_GunScript.m_ammoType] += m_GunScript.GetRemainingBullets();
                        Destroy(m_currentWeapon);
                        //Debug.Log("Destroyed weapon");
                    }
                    //picked up weapon that uses different ammo than current gun
                    else{
                        //put our gun away
                        m_currentWeapon.SetActive(false);
                        m_GunScript.CancelReload();
                        //make sure we take ammo from our old gun
                        if(m_gunInventory[newWeapAmmoType] != null){
                            m_storedAmmo[newWeapAmmoType] += m_gunInventory[newWeapAmmoType].GetComponent<GunScript>().GetRemainingBullets();
                        }
                    }
  
                }


                //Debug.Log("Picked up weapon");
                m_gunInventory[newWeapAmmoType] = m_closestItem;
                m_currentWeapon = m_gunInventory[newWeapAmmoType];
                m_reloading = false;
                
                m_animationController.SetBool("isReloading", false);
                m_currentReloadTime = 0.0f;
                m_GunScript = m_currentWeapon.GetComponent<GunScript>();
                
                m_hasWeapon = true;

                //parent+put the gun where the hand is and set rotation based on xdeg and ydeg
                m_currentWeapon.transform.parent = m_rightHand;
                m_currentWeapon.transform.position = m_rightHand.position;
                m_currentWeapon.transform.rotation = Quaternion.Euler(m_PCS_script.m_yDeg, m_PCS_script.m_xDeg, 0);
                m_currentWeapon.GetComponent<Collider>().enabled = false;
            }
            else if (m_closestItem.tag == "Ammo")
            {
                m_storedAmmo[0] += 30;
                m_storedAmmo[1] += 20;
                m_storedAmmo[2] += 1;
                Destroy(m_closestItem);
                //Debug.Log("Picked up ammo");
            }   else if (m_closestItem.tag == "PickUp")
            {
               
                Destroy(m_closestItem);
                GetComponent<HealthScript>().AddLives();
                
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

    void SwitchWeapons(int slot){
        //make sure this gun exists in the slot
        Debug.Log("Switch to weapon:"+slot);
        if (m_gunInventory[slot] != null){
            //make sure it isn't the gun we're holding
            if (m_gunInventory[slot] != m_currentWeapon){
                m_animationController.SetBool("isReloading", false);
                m_GunScript.CancelReload();
                m_reloading = false;
                m_currentReloadTime = 0.0f;
                m_currentWeapon.SetActive(false);
                m_currentWeapon = m_gunInventory[slot];
                m_currentWeapon.SetActive(true);
                m_GunScript = m_currentWeapon.GetComponent<GunScript>();
                UpdateText();
            }
        }
    }
}
