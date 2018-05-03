using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour {

	// Use this for initialization
	Inventory inventory;
	void Start () {
		GameObject player = GameObject.FindWithTag ("player");
		inventory = player.GetComponent<Inventory> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){

		int getid = col.gameObject.GetComponent<Itemdata> ().items.id;
	
		inventory.additem (getid);
		Destroy (col.gameObject);
	}



}
