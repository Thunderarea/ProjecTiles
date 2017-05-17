﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSelector : MonoBehaviour {

	public void SelectLevel(int i){
		LevelHandler.PickLevel(i);
	}

	public void Survival(bool s)
	{
		LevelHandler.IsSurvival = s;
		Application.LoadLevel("MainGame");
	}

	public void SelectCustom(string path){
		LevelHandler.ReadCustom(path);
		Application.LoadLevel("MainGame");
	}
}