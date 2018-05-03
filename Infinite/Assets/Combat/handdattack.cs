using UnityEngine;
using System.Collections;

public class handdattack : MonoBehaviour {
	public float attack = 1
		;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	private void OnCollisionEnter(Collision collider){
		
		Destroy (gameObject);
	
		
		if (collider.gameObject.name == "AO3") {
			
			collider.gameObject.SendMessage("Damage",attack);
			//Destroy (collider.gameObject);
		}
		
	}
*/

	void OnTriggerEnter ( Collider col ) {
		if(col.gameObject.tag == "Player"){
			//プレイヤーと衝突した時
			Attack(col.gameObject); //攻撃する
		}
	}
	
	//攻撃する際に呼び出す（なんとなくpublicにしてある）
	public void Attack ( GameObject hit ){
		hit.gameObject.SendMessage("Damage", attack);   //相手の"Damage"関数を呼び出す
	}



}
