using UnityEngine;

public class GUIManager : MonoBehaviour {

	public GUIText coinText, gameOverText, instructionsText, runnerText, gameEndText;

	private static GUIManager instance;
	
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
//		GameEventManager.GameOver += GameOver;
		GameEventManager.GameEnd += GameEnd;
		//gameOverText.enabled = false;
		//gameEndText.enabled = false;
	}

	public static void SetCoin(int coins){
		instance.coinText.text = coins.ToString();

		}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.OnGameStart();
		}
	}
	
	private void GameStart () {
		gameOverText.enabled = false;
		gameEndText.enabled = false;
		instructionsText.enabled = false;
		runnerText.enabled = false;
		enabled = false;
	}
	
	private void GameOver () {
		gameOverText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}

	private void GameEnd(){
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameEnd -= GameEnd;
		gameEndText.enabled = true;
		instructionsText.enabled = true;
		enabled = true;
	}
}