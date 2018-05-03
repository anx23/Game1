using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {



	public GameObject panel;
	public GameObject shop;
	public GameObject inventory;
	// Use this for initialization
	void Start () {
		shop.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Openshop(){

		panel.SetActive (true);
	}

	public void Buy(){
		shop.SetActive (true);

	}

	public void sell(){
		inventory.SetActive (true);

	}


	public void Close(){

		shop.SetActive (false);
		inventory.SetActive (false);

	}

}
