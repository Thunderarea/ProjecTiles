﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

//Περιέχει όλα τα στοιχεία του παίκτη και τις κατάλληλες μεθόδους
public class Player : MonoBehaviour {

	//PLAYER DATA FOR IN GAME
	private int money;
	private int lives;
	private int score = 0;
	private int endScore; //Endscore = score*(1+diff/10) + ((1+diff/10)*gold)/2
	//PLAYER DATA FOR STATISTICS
	private List<int> killist = new List<int> ();
	private List<int> finishList = new List<int> ();
	private List<int> sellTowers = new List<int> ();//να το φτιαξω
	private List<int> totalTowers = new List<int> ();
	private int negativeScore = 0, positiveScore = 0;
	private int totalMoneys = 0,usedMoneys = 0;
	private Stopwatch chronometer = new Stopwatch();
	private int hours,minutes,seconds;

	// Use this for initialization
	void Start () {
		lives = LevelHandler.SelectedLives;
		money = LevelHandler.SelectedMoneys;

		UpdateHealth();
		UpdateTextGold();
		UpdateScore (0);
		totalMoneys = money;

		int lengthEnemies = Resources.LoadAll ("Prefabs/Enemies", typeof(GameObject)).Length;
		for (int i = 0; i < lengthEnemies; i++) {
			killist.Add(0);
			finishList.Add(0);
		}
		int lengthTowers = Resources.LoadAll ("Prefabs/Towers",typeof(GameObject)).Length;
		for (int i = 0; i < lengthTowers; i++) {
			totalTowers.Add (0);
			sellTowers.Add (0);
		}
	}

	//Μεταφέρει τα στοιχεία του παίκτη που χρησιμευουν στα στατιστικά στην StatisticsData 
	public void TransferDataInStatistics()
	{
		StatisticsData.Hours += hours;
		StatisticsData.Minutes += minutes;
		StatisticsData.Seconds += seconds;
		StatisticsData.TotalMoneys += totalMoneys;
		StatisticsData.UsedMoneys += usedMoneys;
		StatisticsData.SetEndScoreAndHighscores = endScore;//να μην προστιθεται ποτε των ποτων!!!και να μην χρησιμοποιειται ποτε το EndScore
		StatisticsData.NegativeScore += negativeScore;
		StatisticsData.PositiveScore += positiveScore;
		StatisticsData.Waves += GetComponent<WaveControler> ().WavesIndex;
		StatisticsData.Score += score;
		StatisticsData.Lives += (LevelHandler.SelectedLives - lives); 

		for(int i=0; i < killist.Count; i++){
			StatisticsData.Killist [i] += killist [i];
		}
		for (int i = 0; i < finishList.Count; i++) {
			StatisticsData.FinishList [i] += finishList [i];
		}
		for (int i = 0; i < totalTowers.Count; i++) {
			StatisticsData.TotalTowers[i] += totalTowers [i];
		}
		for (int i = 0; i < sellTowers.Count; i++) {
			StatisticsData.SellTowers[i] += sellTowers [i];
		}
	}

	//Υπολογισμός του τελικού σκορ
	public void CaclculateEndScore(){
		int timeInSeconds = Mathf.RoundToInt((chronometer.ElapsedMilliseconds)/1000);
		if ((timeInSeconds / 10f) != 0)
			//Endscore = score*(1+diff/10) + ((1+diff/10)*gold)/2
			endScore = Mathf.RoundToInt(score*(1+(GameData.Difficulty/10f)) + (1+(GameData.Difficulty/10f))*(money/2f));
		else
			endScore = score;
		/*print (totalMoneys-usedMoneys+" gold");
		print (lives+" lives");
		print (money+" money");
		print (score+" score");
		print (endScore+" endscore");*/
	}

	//"Προσθέτει ένα enemy" είτε στην λίστα killist είτε στην finishList αυξάνοντας την κατάλληλη θέση της λίστας κατά 1
	public void AddInEnemieList(int id,bool isKill) 
	{
		if (isKill)
			killist [id]++;
		else 
			finishList [id]++;
	}

	//Ενημερώνει το text του Health
	public void  UpdateHealth(){
		GameObject.Find ("HealthText").GetComponent<Text> ().text = lives.ToString ();
	}

	//Καλεί την συνάρτηση που ενημερώνει το text του Gold και ενημερώνει την κατάλληλη λίστα(usedMoneys ή totalMoneys)
	public void UpdateGold(int value){
		money += value;
		UpdateTextGold ();
		if (value < 0)
			usedMoneys -= value;
		else if(value > 0)
			totalMoneys += value;
	}

	//"Προσθέτει ένα tower" είτε στην λίστα totalTowers είτε στην sellTowers αυξάνοντας την κατάλληλη θέση της λίστας κατά 1
	public void AddInTowerList(int id,bool creation)
	{
		if (creation)
			totalTowers [id]++;
		else
			sellTowers [id]++;

	}

	//Ενημερώνει το text του gold
	public void UpdateTextGold()
	{
		GameObject.Find ("GoldText").GetComponent<Text> ().text = money.ToString ();
	}

	//Ενημερώνει το text του score και ενημερώνει είτε το negativeScore είτε το positiveScore
	public void UpdateScore(int scorePoints)
	{
		score += scorePoints;
		GameObject.Find ("ScoreText").GetComponent<Text> ().text = score.ToString ();
		if (scorePoints < 0)
			negativeScore -= scorePoints;
		else if(scorePoints > 0)
			positiveScore += scorePoints;
	}

	//Ελέγχει τις ζωές του παίκτη
	public void ControLives()
	{
		if (lives <= 0)
			this.gameObject.GetComponent<FlowController> ().EndGame ();//κάλεσε την συνάρτηση για τον τερματισμό του παιχνιδιού
	}

	//CONTROL CHRONOMETER
	public void StartChronometer(){
		chronometer.Start ();
	}
	public void StopChronometer(){
		chronometer.Stop ();
	}

	//τερματίζει το χρονόμετρο και ενημερώνει τις κατάλληλες μεταβλητές
	public void EndChronometer()
	{
		chronometer.Stop ();
		System.TimeSpan time = chronometer.Elapsed;
		hours = time.Hours;
		minutes = time.Minutes;
		seconds = time.Seconds;
	}

	//GETTERS
	public int Money{
		get{ return money;}
		set{ money = value;}
	}

	public int Lives{
		get{ return lives;}
		set{ lives = value;}
	}

	public int Score{
		get{ return score; }
		set{ score = value; }
	}

	public int EndScore{
		get{ return endScore; }
		set{ endScore = value; }
	}

	public int NegativeScore{
		get{ return negativeScore; }
	}

	public int PositiveScore{
		get{ return positiveScore; }
	}

	public int TotalMoneys{
		get{ return totalMoneys; }
	}

	public int UsedMoneys{
		get{ return usedMoneys; }
	}

	public List<int> Killist{
		get{ return killist; }
	}

	public List<int> FinishList{
		get{ return finishList; }
	}

	public List<int> TotalTowers{
		get{ return totalTowers; }
	}

	public List<int> SellTowers{
		get{ return sellTowers; }
	}

	public int Hours{
		get{ return hours; }
	}

	public int Minutes{
		get{ return minutes; }
	}

	public int Seconds{
		get{ return seconds; }
	}
}
