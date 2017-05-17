﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemy: MonoBehaviour {

    [SerializeField]
    private int health;

    [SerializeField]
    private float coefSpeed;

   // private float cStemp;

    [SerializeField]
    private int worth;

    private GameObject gameFlow;   
    private GameObject g;
    private Vector2 vectorNext;
    private bool flag;

    

    // Use this for initialization
    public void Initialize (int i) {
		gameFlow = GameObject.Find("GameFlow");
        flag = false;
        List<GameObject> listStarTiles = gameFlow.GetComponent<GridController>().GetStartTiles();
        g = listStarTiles[i].GetComponent<PathTile>().getNextTile_Random();
        vectorNext = g.GetComponent<Tile>().getCoords();
    }


    void Update()
    {
        Movement(coefSpeed, ref vectorNext, ref flag, ref g);
    }


    public void Movement(float coefSpeed,ref Vector2 vectorNext,ref bool flag,ref GameObject g){

		Vector3 dir = g.transform.position - this.transform.position;
		float angle = (Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg)+90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * 20);

		transform.position = Vector2.MoveTowards(transform.position, vectorNext,coefSpeed*Time.deltaTime);
		if (Vector2.Distance((Vector2)transform.position, vectorNext) < 0.1 && !flag) {
			g = g.GetComponent<PathTile>().getNextTile_Random();
			vectorNext = g.GetComponent<Tile>().getCoords();
			if (g.GetComponent<PathTile>().NextTiles.Count == 0)
			{
				flag = true;
			}
		}
		if (Mathf.Approximately (transform.position.x, vectorNext.x) && Mathf.Approximately (transform.position.y, vectorNext.y) && flag) {
			DestroyEnemy();
			gameFlow.GetComponent<FlowController> ().Lives--;
			gameFlow.GetComponent<FlowController>().ControLives();
			gameFlow.GetComponent<FlowController> ().UpdateHealth();
		}
	}

	public void MainHit(int hitpoints)
	{
		health -= hitpoints;
		if (health <= 0) {
			DestroyEnemy();
			gameFlow.GetComponent<FlowController> ().Kill++;
			gameFlow.GetComponent<FlowController> ().Money += worth;
			gameFlow.GetComponent<FlowController> ().UpdateGold();
		}
        
	}
	
	public void DestroyEnemy()
	{
		Destroy (this.gameObject);
		gameFlow.GetComponent<FlowController> ().NumbersOfEnemies--;
		gameFlow.GetComponent<FlowController> ().ControlNumOfEnemies();
	}

    public void EffectHit(string effect, int value)
    {

        if(effect == "Slow")
        {
            coefSpeed /= value;
        }
        else if(effect == "Restore Movement")
        {
            coefSpeed *= value;
        }
    }
}
//Vector2 dir = new Vector2(vectorNext.x-transform.position.x,vectorNext.y-transform.position.y);
//transform.Translate(speed*Time.deltaTime*dir.normalized,Space.World);