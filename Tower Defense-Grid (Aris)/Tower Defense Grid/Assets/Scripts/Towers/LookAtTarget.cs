﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {
	private Enemy target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		target = transform.GetComponentInChildren<Tower>().Enem2;
		if(target != null){
			Vector3 diff = target.transform.position - transform.position;
			diff.Normalize();

			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90); 

			/*float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90); 
			Vector3 diff = target.transform.position - transform.position;
			diff.Normalize();

			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 270;
			Quaternion q = Quaternion.AngleAxis(rot_z,Vector3.forward);

			transform.rotation = Quaternion.Slerp(transform.rotation,q,Time.deltaTime*200);*/
		}

	}
}
