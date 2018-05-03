using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adddtest : MonoBehaviour {
	public Inventory equip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("z")) {

			equip.additem (2);
		}

		if (Input.GetKeyDown ("x")) {

			equip.RemoveItem(2);
		}
	}
}
