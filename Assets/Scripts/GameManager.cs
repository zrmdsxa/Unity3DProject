using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Intro, Menu, Play, Win, Lose }

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameState m_currentState;

    public GameObject[] m_gameStates;

	public Text m_gameTimerText;
	public float m_gameLength = 301.0f;

	public GameObject[] m_playerCharacters;

	

	public Dropdown m_dropdownCharacter;
    public Dropdown m_dropdownLevel;
	

	//private

    float m_introTimer = 1.0f;

	float m_gameTimer = 301.0f;

    public int m_playerSelected = 0;//must be public for spawning

    int m_levelSelected = 1;
	

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
		m_introTimer = 1.0f;

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
				UpdatePlay();
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

	void UpdatePlay(){
		if(m_gameTimer > 0.0f){
			m_gameTimer -= Time.deltaTime;
			if (m_gameTimer < 0){
				m_gameTimer = 0;
			}
			m_gameTimerText.text = Mathf.Floor((m_gameTimer/60)).ToString("0")+":"+Mathf.Floor((m_gameTimer%60)).ToString("00");
		}
		
	}

	void ChangeGameState(GameState newState){
		m_gameStates[(int)m_currentState].SetActive(false);
		m_currentState = newState;
		m_gameStates[(int)m_currentState].SetActive(true);
	}

	public void MenuButtonPlay(){
		SceneManager.LoadScene(m_levelSelected);
		ChangeGameState(GameState.Play);
		m_gameTimer = m_gameLength;
	}

	public void ChangeCharacter(){
		Debug.Log("Menu changed char");
		m_playerSelected = m_dropdownCharacter.value;
	}

    public void ChangeLevel(){
        Debug.Log("Menu changed level");
		m_levelSelected = m_dropdownLevel.value+1;
    }

    public void GameOver(){
        ChangeGameState(GameState.Lose);
        
    }
}