using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantControl : MonoBehaviour {
	static Animator my_anim;
	public float Speed = 10.0f;
	public float rotationSpeed = 100.0f;
	// Use this for initialization
	void Start () {
		my_anim = GetComponent<Animator>();

		
	}
	
	// Update is called once per frame
	void Update () {
		float translation = Input.GetAxis("Vertical") * Speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0,0,translation);
		transform.Rotate(0,rotation,0);

		if(Input.GetButtonDown("Jump")){
			my_anim.SetTrigger("isJumping");
			my_anim.SetBool("isIdle", false);

		}
		if(translation != 0){
			my_anim.SetBool("isRunning",true);
			my_anim.SetBool("isIdle", true);		
			
		}else{
			my_anim.SetBool("isRunning",false);
		}
		
	}
}
