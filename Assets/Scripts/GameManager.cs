using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Intro, Menu, Play, Win, Lose }

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameState m_currentState;

    public GameObject[] m_gameStates;


    float m_introTimer = 1.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
		for (int i = 0;i< m_gameStates.Length;i++){
			m_gameStates[i].SetActive(false);
		}
		Debug.Log((int)GameState.Intro);
        m_currentState = GameState.Intro;
		m_gameStates[(int)GameState.Intro].SetActive(true);

    }

    void Update()
    {
        switch (m_currentState)
        {
            case GameState.Intro:
                UpdateIntro();
                break;
            case GameState.Menu:
                break;
            case GameState.Play:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    void UpdateIntro()
    {
        if (m_introTimer <= 0.0f)
        {
            Debug.Log("Change to MENU");
            ChangeGameState(GameState.Menu);
        }
        else
        {
            m_introTimer -= Time.deltaTime;
        }
    }

	void ChangeGameState(GameState newState){
		m_gameStates[(int)m_currentState].SetActive(false);
		m_currentState = newState;
		m_gameStates[(int)m_currentState].SetActive(true);
	}

	public void MenuButtonPlay(){
		ChangeGameState(GameState.Play);
	}
}
