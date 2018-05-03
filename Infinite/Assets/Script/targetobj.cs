using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetobj : MonoBehaviour {



	playerlockon pl;
	ZombieAI enemy;

	void Start(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		pl = player.GetComponent<playerlockon> ();
		enemy = GetComponent<ZombieAI> ();
	}



	void Update(){
		if(enemy.isdead)
			pl.obj.Remove (this.gameObject);
	}




	void OnBecameInvisible() {
		Debug.Log ("Invisible");
		pl.obj.Remove (this.gameObject);
	}


	void OnBecameVisible() {
		
		Debug.Log ("visible");
		if(!enemy.isdead)
		pl.obj.Add (this.gameObject);
	}



}
