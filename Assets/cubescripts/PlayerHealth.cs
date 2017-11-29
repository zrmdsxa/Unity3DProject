using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
public AudioClip playerhurt;
	private AudioSource playerAS;
	public float fullhealth;
	public GameObject deathFX;
	float currentHealth;
	PlayerMovementScript controlMovement;
//hud variables
public Slider healthSlider;
public Image DamageScreen;
 bool damaged = false;
Color damagedColour = new Color(0f,0f,0f, 0.5f);
float smoothColour = 5f;


	// Use this for initialization
	void Start () {
		currentHealth = fullhealth;
		controlMovement = GetComponent<PlayerMovementScript>();
//hud initializtion
healthSlider.maxValue = fullhealth;
healthSlider.value = fullhealth;
damaged = false;
playerAS = GetComponent<AudioSource>();



	}
	
	// Update is called once per frame
	void Update () {
		if(damaged){
			DamageScreen.color = damagedColour;
		}else {
			DamageScreen.color = Color.Lerp(DamageScreen.color, Color.clear, smoothColour*Time.deltaTime);
		}
		damaged = false;
	}
	public void addDamage(float damage){
		if(damage <= 0 ) return;
		currentHealth -=  damage;
		playerAS.clip = playerhurt;
		playerAS.Play();
	playerAS.PlayOneShot(playerhurt);
	healthSlider.value = currentHealth;
	damaged = true;



		if(currentHealth <= 0){
			makeDead();
		}

	}
	public void makeDead(){
	Destroy(gameObject);
		GameObject particleDie = Instantiate(deathFX, transform.position, transform.rotation);
		Destroy(particleDie, 1f);//destroys it fore x amount of seconds

	}
}
