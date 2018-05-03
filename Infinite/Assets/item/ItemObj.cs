using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour {



	Inventory iv;
	public int id;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("Inventory");
		iv=go.GetComponent<Inventory> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter ( Collider col ) {



		if (col.gameObject.tag == "Player") {
			//プレイヤーと衝突した時

			iv.additem (id);
			Destroy (this.gameObject);
		}

	}


}
