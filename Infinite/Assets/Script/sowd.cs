using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sowd : MonoBehaviour {

	public float attack = 30;

	public GameObject bloodeffect;

	public bool isplayer=true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




	void OnTriggerEnter ( Collider col ) {
		

		if (isplayer) {
			if (col.gameObject.tag == "Enemy") {
				//プレイヤーと衝突した時
				Attack (col.gameObject); //攻撃する
				Debug.Log("hit");
				Instantiate (bloodeffect,transform.position,transform.rotation);

			}
		} else {
			if(col.gameObject.tag == "Player"){
				//プレイヤーと衝突した時
				Attack(col.gameObject); //攻撃する
				Debug.Log("hit");
			}

		}

	}




	/*

	void OnCollisionEnter ( Collision col ) {


		if (isplayer) {
			if (col.gameObject.tag == "Enemy") {
				//プレイヤーと衝突した時
				Attack (col.gameObject); //攻撃する
				Debug.Log("hit");
			}
		} else {
			if(col.gameObject.tag == "Player"){
				//プレイヤーと衝突した時
				Attack(col.gameObject); //攻撃する
				Debug.Log("hit");
			}

		}

	}
*/







	//攻撃する際に呼び出す（なんとなくpublicにしてある）


	public void Attack ( GameObject hit ){
		hit.gameObject.SendMessage("Damage", attack);   //相手の"Damage"関数を呼び出す
	}




}
