using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Instantiate(GameManager.instance.m_playerCharacters[GameManager.instance.m_playerSelected],transform.position,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
