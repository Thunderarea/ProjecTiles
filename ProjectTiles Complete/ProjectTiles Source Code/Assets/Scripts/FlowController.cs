﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
 
//Controls the whole game
public class FlowController : MonoBehaviour {

	//SCENES
	public GameObject gameOverUI;
	public GameObject pauseUI;
	public GameObject levelCompleteUI;
	public GameObject StatisticsUI;
	//
	private float oldTimeScale;
	private int numberOFEnemies = 0;
	private GameObject gameFlow;
	private bool completeLevel = false;//εχει συμπληρωθει το level;
	private bool waveStart = false;
	private int oldScene;

	// Use this for initialization
	void Start () {
		GetComponent<GridController>().enabled = true;
		GetComponent<Player>().enabled = true;
		GetComponent<Player> ().StartChronometer ();
		gameFlow = GameObject.Find("GameFlow");
		//GetComponent<WaveControler>().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp(KeyCode.P)) && !gameOverUI.activeSelf && !levelCompleteUI.activeSelf) {
			if (StatisticsUI.activeSelf)
				Return ();
			else
				Pause();
		}

		if (waveStart) {
			if (numberOFEnemies == 0 && GetComponent<WaveControler> ().OutOfWaves && !GetComponent<WaveControler> ().EndOfWaves)
				GameObject.Find ("Start_and_Speed_Button").GetComponent<Image> ().sprite = (Sprite)Resources.Load ("Sprites/GUI/startlevel", typeof(Sprite));
			else if (!completeLevel && gameFlow.GetComponent<WaveControler> ().EndOfWaves && numberOFEnemies == 0 && gameFlow.GetComponent<Player>().Lives > 0) {
				//!completeLevel:για να ειμαστε σιγουροι πως δεν θα καλεστει ξανα 
				completeLevel = true;
				LevelComplete ();
			}
		}
	}

	//Starts a wave
	public void startWaveControler(){
		GetComponent<WaveControler>().enabled = true;
		waveStart = true;
	}

	//Gets tower id when a player clicks the tower ui,creates the object,and starts hovering it
	public void TowerHover(int i){
		GameObject tower1,tower2,tower3,tower4,tower5,tower6;
		GameObject new_tower = null;

		tower1 = (GameObject)Resources.Load("Prefabs/Towers/Tower",typeof(GameObject));
		tower2 = (GameObject)Resources.Load("Prefabs/Towers/Slow Tower",typeof(GameObject));
		tower3 = (GameObject)Resources.Load("Prefabs/Towers/Buff Tower",typeof(GameObject));
		tower4 = (GameObject)Resources.Load("Prefabs/Towers/RoundHouse Tower",typeof(GameObject));
		tower5 = (GameObject)Resources.Load("Prefabs/Towers/Global Tower",typeof(GameObject));
		tower6 = (GameObject)Resources.Load("Prefabs/Towers/Canon Tower", typeof(GameObject));;

		Vector2 coords =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		coords = new Vector2(Mathf.Round(coords.x),Mathf.Round(coords.y));

		switch(i){
		case 1:
			new_tower = Instantiate(tower1,coords,Quaternion.identity);
			new_tower.name = "Tower";
			break;
		case 2:
			new_tower = Instantiate(tower2,coords,Quaternion.identity);
			new_tower.name = "Slow Tower";
			break;
		case 3:
			new_tower = Instantiate(tower3,coords,Quaternion.identity);
			new_tower.name = "Buff Tower";
			break;
		case 4:
			new_tower = Instantiate(tower4,coords,Quaternion.identity);
			new_tower.name = "RoundHouse Tower";
			break;
		case 5:
			new_tower = Instantiate(tower5,coords,Quaternion.identity);
			new_tower.name = "Global Tower";
			break;
		case 6:
			new_tower = Instantiate(tower6,coords,Quaternion.identity);
			new_tower.name = "Canon Tower";
			break;
		}
		new_tower.GetComponent<Hover>().enabled = true;
		new_tower.GetComponent<Hover>().setTowerCost(TowerCost.getCost(i));



	}

	//Handles speed up/down
	public void StartWaves_UpdateSpeed(){
		if(GetComponent<WaveControler>().OutOfWaves && numberOFEnemies==0 && !GetComponent<WaveControler>().EndOfWaves){
			//!GetComponent<WaveControler>().EndOfWaves: αλλιως θα καλούνταν η callWave χωρις βεβαια να κανει κατι αλλα θα εμποδιζε την ταχυτητα να αυξηθει(στο τελευταιο κυμα)
			if(Time.timeScale == 2)
				Time.timeScale = 1;
			GetComponent<WaveControler>().callWave();
			GameObject.Find("Start_and_Speed_Button").GetComponent<Image>().sprite = (Sprite)Resources.Load("Sprites/GUI/speedup",typeof(Sprite));
		}else{
			AdjustGameSpeed();
		}
	}

	public void AdjustGameSpeed(){
		if(Time.timeScale == 1){
			Time.timeScale = 2;
			GameObject.Find("Start_and_Speed_Button").GetComponent<Image>().sprite = (Sprite)Resources.Load("Sprites/GUI/speedup_full",typeof(Sprite));
		}else if(Time.timeScale == 2){
			Time.timeScale = 1;
			GameObject.Find("Start_and_Speed_Button").GetComponent<Image>().sprite = (Sprite)Resources.Load("Sprites/GUI/speedup",typeof(Sprite));
		}
	}
		
	//SCENES IN GAME
	//Handles game over conditions
	//Τερματίζει το παιχνίδι εμφανίζει την κατάλληλη οθόνη και κάνει τις απαραίτητες ενέργειες πριν το τερματίσει
	public void EndGame()
	{
		BeforeGameEnds (false);
		Time.timeScale = 0;
		gameOverUI.SetActive (true);
		GameObject.Find ("TextScoreGameOver").GetComponent<Text> ().text = gameFlow.GetComponent<Player> ().EndScore.ToString ();
		if(StatisticsData.IsHighscore)
			GameObject.Find("ScoreOrHighscoreGO").GetComponent<Text>().text = "HIGH SCORE";
		else
			GameObject.Find("ScoreOrHighscoreGO").GetComponent<Text>().text = "SCORE";
	}
	//Handles level complete conditions
	//Καλείται όταν συμπληρωθει το level εμφανίζει την κατάλληλη οθόνη και κάνει τις απαραίτητες ενέργειες πριν γίνει αυτό
	public void LevelComplete(){
		BeforeGameEnds (true);
		if(LevelHandler.Selected_level >= GameData.Progress){
			GameData.Progress = LevelHandler.Selected_level + 1;
			GameData.Save ();
		}
		Time.timeScale = 0;
		levelCompleteUI.SetActive (true);
		GameObject.Find ("TextScoreLevelComplete").GetComponent<Text> ().text = gameFlow.GetComponent<Player> ().EndScore.ToString (); 
		if(StatisticsData.IsHighscore)
			GameObject.Find("ScoreOrHighscoreLC").GetComponent<Text>().text = "HIGH SCORE";
		else
			GameObject.Find("ScoreOrHighscoreLC").GetComponent<Text>().text = "SCORE";
	}
	//Handles pause menu
	//Καλειται όταν πατηθεί το escape και εμφανίζει το μενού της παύσης
	public void Pause()
	{
		pauseUI.SetActive (!pauseUI.activeSelf);
		if (pauseUI.activeSelf) {
			GameObject cover = new GameObject("Cover");
			cover.transform.position = new Vector3(0,0,-5);
			cover.transform.localScale = new Vector3(10000,10000,1);
			cover.AddComponent<BoxCollider2D>();

			GetComponent<Player> ().StopChronometer ();
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		} else {
			GameObject cov = GameObject.Find("Cover");
			Destroy(cov.gameObject);
			GetComponent<Player> ().StartChronometer ();
			Time.timeScale = oldTimeScale;
		}
	}

	//MENU BUTTON IN GAME
	//Gets triggered by "CONTINUE" button on pause menu
	//Καλείται όταν πατήθεί το κουμπι continue κάνοντας τις απαραίτητες ενέργειες
	public void Continue(){
		pauseUI.SetActive (false);
		GameObject cov = GameObject.Find("Cover");
		Destroy(cov.gameObject);
		Time.timeScale = oldTimeScale;
		GetComponent<Player> ().StartChronometer ();
	}

	//Handles restarting
	//Καλείται όταν πατήθεί το κουμπι restart κάνοντας τις απαραίτητες ενέργειες
	public void Restart()
	{
		if(pauseUI.activeSelf)
		{
			BeforeGameEnds (false);
		}
		SceneManager.LoadScene ("MainGame");
		Time.timeScale = 1f;
		StatisticsData.NumbersOfGames++;
	}

	//Goto main menu
	//Καλείται όταν πατήθεί το κουμπι menu κάνοντας τις απαραίτητες ενέργειες
	public void Menu()
	{
		if(pauseUI.activeSelf){
			BeforeGameEnds (false);
		}
		SceneManager.LoadScene ("MainMenu");
		Time.timeScale = 1f;
	}

	//Goto level select menu
	//Καλείται όταν πατήθεί το κουμπι continue από την οθόνη levelComplete κάνοντας τις απαραίτητες ενέργειες
	public void ContinueLevelCompleteUI()
	{
		SceneManager.LoadScene ("LevelSelect");
		Time.timeScale = 1f;
	}

	//Gets triggered by "STATISTICS" button,shows statistics 
	//Καλείται όταν πατήθεί το κουμπι Statistics κάνοντας τις απαραίτητες ενέργειες
	public void StatisticsButton(bool mainStatistics)
	{
		if (levelCompleteUI.activeSelf) {
			levelCompleteUI.SetActive (false);
			oldScene = 0;
		} else if (gameOverUI.activeSelf) {
			gameOverUI.SetActive (false);
			oldScene = 1;
		}
		StatisticsUI.SetActive (true);
		GetComponent<StatisticsMenuController> ().enabled = true;
		GetComponent<StatisticsMenuController> ().ShowStatistics (mainStatistics);
	}

	//Gets triggered by "RETURN" button,hides statistics 
	//Καλείται όταν πατήθεί το κουμπι return κάνοντας τις απαραίτητες ενέργειες
	public void Return()
	{
		GetComponent<StatisticsMenuController> ().enabled = false;
		StatisticsUI.SetActive (false);
		if (oldScene == 0)
			levelCompleteUI.SetActive (true);
		else
			gameOverUI.SetActive (true);
	}

	//Some statistics handling
	//Καλείται πριν τερματιστεί ένα παιχνίδι κάνοντας τις απαραίτητες ενέργειες
	public void BeforeGameEnds(bool win)
	{
		if(win)
			StatisticsData.Wins++;
		else 
			StatisticsData.Loses++;
		gameFlow.GetComponent<Player> ().CaclculateEndScore ();
		GetComponent<Player> ().EndChronometer();
		GetComponent<Player> ().TransferDataInStatistics();//warning: always after EndChronometer
		StatisticsData.Save();
	}

	//GETTERS
	public int NumbersOfEnemies{
		get{ return numberOFEnemies;}
		set{ numberOFEnemies = value;}
	}
}

