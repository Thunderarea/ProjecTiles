﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerInteractions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Εμφανίζει το βεληνεκές του πύργου εαν στοχεύσουμε με το ποντίκι το πύργο
	public void OnMouseEnter(){
		if(this.name != "Global Tower"){
			Color c = transform.Find("Range").GetComponentInChildren<SpriteRenderer>().color;
			transform.Find("Range").GetComponentInChildren<SpriteRenderer>().color =  new Color(c.r,c.g,c.b,100/255f);
		}
	
	}

    //Εξαφανίζεται το βεληνεκές αφού το ποντίκι φύγει από το πύργο
    public void OnMouseExit(){
		Color c = transform.Find("Range").GetComponentInChildren<SpriteRenderer>().color;
		transform.Find("Range").GetComponentInChildren<SpriteRenderer>().color = new Color(c.r,c.g,c.b,0);;

	}



}
