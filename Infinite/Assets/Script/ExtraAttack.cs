using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraAttack : MonoBehaviour {
	public float attack = 10;
	float time;


	// Use this for initialization
	void Start () {
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 5.0f)
			Destroy (this.gameObject);
	}



	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
			Debug.Log ("hitEX");
			StartCoroutine(StayAttack (col.gameObject));
		}


	}


	IEnumerator StayAttack(GameObject hit){
		while (true) {
			hit.gameObject.SendMessage ("Damage", attack);
			yield return new WaitForSeconds (1.0f);
		}
	}


}
