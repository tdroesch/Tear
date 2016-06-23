using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum ButtonType {START, OVER, PLAYING};

public class Menu : MonoBehaviour {
	ButtonType button = ButtonType.START;
	public Button start;
	public Button respawn;
	public Button end;
	public Image back;
//	public Animator hero;

	ColorBlock colorBlock;
	ColorBlock selectedColorBlock;
	Button buttonFocus;

	bool controllerJump = false;

	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameEnd += GameEnd;
		colorBlock = respawn.colors;
		selectedColorBlock = colorBlock;
		selectedColorBlock.normalColor = colorBlock.highlightedColor;
		buttonFocus = respawn;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")){
			controllerJump = true;
		}if (Input.GetButtonUp("Jump")){
			controllerJump = false;
		}
		switch (button){
		case ButtonType.START :
			back.gameObject.SetActive(true);
			respawn.gameObject.SetActive(false);
			end.gameObject.SetActive(false);
			if (controllerJump){
				GameEventManager.OnGameStart();
			}
			break;
		case ButtonType.OVER:
			respawn.gameObject.SetActive(true);
			end.gameObject.SetActive(true);
			if (controllerJump){
				buttonFocus.onClick.Invoke();
			}
			if (Input.GetAxis("Vertical")>0.1f){
				respawn.colors = selectedColorBlock;
				end.colors = colorBlock;
				buttonFocus = respawn;
			}
			if (Input.GetAxis("Vertical")<-0.1f){
				end.colors = selectedColorBlock;
				respawn.colors = colorBlock;
				buttonFocus = end;
			}
			break;
		case ButtonType.PLAYING:
			back.gameObject.SetActive(false);
			respawn.gameObject.SetActive(false);
			end.gameObject.SetActive(false);
			break;
		}
	}
//
//	void OnGUI(){ // 50 250
//		switch (button){
//			case ButtonType.START :
//				GUI.DrawTexture(new Rect(0.0f,0.0f,Screen.width,Screen.height),back);
//				Rect centeredRect = new Rect(Screen.width/2-50, Screen.height/2, 150, 50);
//				if (GUI.Button (centeredRect, "",start))
//				{
//					GameEventManager.OnGameStart();
//				}
//				if (controllerJump){
//					GameEventManager.OnGameStart();
//				}
//				break;
//			case ButtonType.OVER:
//				Rect upperRect = new Rect(Screen.width/2-50, Screen.height/2-30, 150, 50);
//				Rect lowerRect = new Rect(Screen.width/2-50, Screen.height/2+30, 150, 50);
//				GUI.SetNextControlName("Respawn");
//				if (GUI.Button (upperRect, "Respawn"))
//				{
//					GameEventManager.OnRespawn();
//				}
//				GUI.SetNextControlName("End");
//				if (GUI.Button (lowerRect, "End"))
//				{
//					GameEventManager.OnGameEnd();
//				}
//
//
//				if (controllerJump){
//					if (GUI.GetNameOfFocusedControl().Equals("Respawn")){
//						GameEventManager.OnRespawn();
//					}
//					else {
//						GameEventManager.OnGameEnd();
//					}
//				}
//				if (Input.GetAxis("Vertical")>0.1f){
//					GUI.FocusControl("Respawn");
//				}
//				if (Input.GetAxis("Vertical")<0.1f){
//					GUI.FocusControl("End");
//				}
//				break;
//		}
//	}
	void GameStart(){
		button = ButtonType.PLAYING;
	}

	void GameOver(){
		button = ButtonType.OVER;
	}

	void GameEnd(){
		GameEventManager.GameStart -= GameStart;
		GameEventManager.GameOver -= GameOver;
		GameEventManager.Respawn -= Respawn;
		GameEventManager.GameEnd -= GameEnd;
	}

	void Respawn(){
		button = ButtonType.PLAYING;
	}

	public void StartGame(){
		GameEventManager.OnGameStart();
	}
	public void RespawnGame(){
		GameEventManager.OnRespawn();
	}
	public void EndGame(){
		GameEventManager.OnGameEnd();
	}
}
