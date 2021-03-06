﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*Οι αναβαθμίσεις των πύργων ανα επίπεδο*/

public static class TowerUpgrades {

	private static int damage;
	private static float atk_cool;
	private static float proj_sp;
	private static int eff_val;
	private static float range;

	public static void setTowerUpgrades(int i,int lvl){
		damage = 0;
		atk_cool=0;
		proj_sp=0;
		eff_val=0;
		range = 0;

		switch(i){
		//Normal
		case 1:
			if(lvl == 1){
				damage = 10;
				atk_cool = 0.6f;
				proj_sp = 10;
			}else{
				damage = 12;
				atk_cool = 0.6f;
				range = 1.5f;
			}
			break;
		//Slow
		case 2:
			if(lvl == 1){
				range = 1.7f;
			}else{
				eff_val = 3;
			}
			break;
		//Buff
		case 3:
			if(lvl == 1){
				range = 2.2f;
			}else{
				eff_val = 3;
			}
			break;
		//Roundhouse
		case 4:
			if(lvl == 1){
				damage = 5;
				atk_cool = 1.4f;
				proj_sp = 7;
			}else{
				damage = 7;
				atk_cool = 1.2f;
				range = 2.2f;
			}
			break;
		//Global
		case 5:
			if(lvl == 1){
				damage = 15;
				atk_cool = 1.5f;
			}else{
				damage = 20;
				atk_cool = 1f;
			}
			break;
		//AOE TODO
		case 6:
			if(lvl == 1){
				damage = 5;
				atk_cool = 2f;
				proj_sp = 10;
			}else{
				damage = 7;
				atk_cool = 1.8f;
				range = 2.2f;
			}
			break;
		}
	

	}

	public static float Range {
		get {
			return range;
		}
	}

	public static int Damage {
		get {
			return damage;
		}
	}

	public static float Atk_cool {
		get {
			return atk_cool;
		}
	}

	public static float Proj_sp {
		get {
			return proj_sp;
		}
	}

	public static int Eff_val {
		get {
			return eff_val;
		}
	}
}
