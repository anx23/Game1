using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {

	public List<item> inventory=new List<item>();
	public itemdatabase data;
	public GameObject itemobj;
	public GameObject panel;
	public GameObject Menu;
	int itemstock=6;
	List<GameObject> slots=new List<GameObject>();

	// Use this for initialization
	public GameObject weapon;
	public Transform weaponposition;


	JsonMapper playerdata;
	JsonData player_inventory;




	public enum ToolType{
		use,
		buy,
		sell,

	}
	public ToolType toolname;

	void Start () {
		//weaponposition = weapon.transform;
		//data =GetComponent<itemdatabase> ();
		//itemobj.transform.FindChild("Text").
		itemobj.transform.Find("Tool/Text").gameObject.GetComponent<Text> ().text = toolname.ToString ();
		for (int i = 0; i <= itemstock; i++) {
			var prefab=Instantiate (itemobj);
			slots.Add (prefab);
			inventory.Add (new item());
			prefab.transform.SetParent (panel.transform);
			//slots [i].transform.Find ("Tool").gameObject.GetComponent<Text> ().text = toolname.ToString ();
		}

		GameObject go = GameObject.Find ("GameManager");
		if (go != null) {
			if (go.GetComponent<GameControll> ().isFirst==false)
				load ();

		}
		//additem (1);
		//additem (1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q"))
			//additem (0);
			OpenPanel();
		//item tes=data.Search_database (1);
	//	Debug.Log (tes.name);







	}


	public void load(){
		if (!File.Exists (Application.dataPath + "/StreamingAssets/inventory.json")) {
			Debug.Log ("Not Exist");
			return;
		} else {
			player_inventory=JsonMapper.ToObject(File.ReadAllText(Application.dataPath+"/StreamingAssets/inventory.json"));

			int[] loadid=new int[player_inventory.Count];
			for(int i=0;i<player_inventory.Count;i++){
				loadid [i] = (int)player_inventory [i] ["id"];
				additem (loadid[i]);
				/*
			inventory.Add (new item(player_inventory[i]["name"].ToString(),(int)player_inventory[i]["id"],(int)player_inventory[i]["money"],player_inventory[i]["typename"].ToString(),
				(bool)player_inventory[i]["isequip"],player_inventory[i]["path"].ToString()));
				*/

			}

		}
	

	}


	public void save(){
		//playerdata = JsonMapper.ToJson (inventory);
		File.WriteAllText (Application.dataPath+"/StreamingAssets/inventory.json",JsonMapper.ToJson(inventory));
		Debug.Log ("セーブしました");
	}


	public void Reset(){
		File.Delete (Application.dataPath + "/StreamingAssets/inventory.json");
		Debug.Log ("新たなる混沌へ");
	}

	public void additem(int id ){
		
		for(int i=0;i<inventory.Count;i++){

			if (inventory [i].id == id) {
				slots [i].GetComponent<Itemdata> ().amount++;
				slots [i].GetComponent<Itemdata> ().UpdateData ();
				return;
			}

		}


		/*

		for (int i = 0; i < data.itemdata.Count; i++)
			if (id == data.itemdata [i].id) {
				Debug.Log (data.itemdata [i].name);
				item itemadd= data.itemdata [i];
				Debug.Log (itemadd.name);
			}
		*/




		item itemadd = data.Search_database (id);
		//Debug.Log (itemadd.name);

		for(int i=0;i<inventory.Count;i++){

			if (inventory [i].id == -1) {
				inventory [i]=itemadd;
				slots [i].GetComponent<Itemdata> ().amount++;
				slots [i].GetComponent<Itemdata> ().items = itemadd;
				slots [i].GetComponent<Itemdata> ().number=i;
				slots [i].GetComponent<Itemdata> ().SetData();
				//slots [i].GetComponent<Image> ().sprite = itemadd.icon;
				Debug.Log (inventory [i].name);
				return;
			}

		}

	}



	public void RemoveItem(int id){


		for(int i=0;i<inventory.Count;i++){

			if (inventory [i].id == id) {
				slots [i].GetComponent<Itemdata> ().amount--;
				slots [i].GetComponent<Itemdata> ().UpdateData ();

				if (slots [i].GetComponent<Itemdata> ().amount == 0) {
					inventory [i] = new item ();
					slots [i].GetComponent<Itemdata> ().items=new item();
					slots [i].GetComponent<Itemdata> ().UpdateData ();


				}

				return;
			}

		}




	}

	void equipchanger(GameObject weapon){

	
		Destroy (weapon.gameObject);
		Instantiate (weapon,weaponposition);

		this.weapon = weapon;
	}




	public void typechanger(string typename){
		for(int i=0;i<inventory.Count;i++){
			slots [i].GetComponent<Itemdata> ().items.changetype (typename);

		}
	}




	public void panelchange(){
		//itemobj.transform.Find("Tool/Text").gameObject.GetComponent<Text> ().text = toolname.ToString ();
		for(int i=0;i<slots.Count;i++){
			
			slots [i].GetComponent<Itemdata> ().toolname.text = toolname.ToString ();
				


		}

	}




	void OnTriggerEnter(Collider col){

		if (col.tag == "item") {
			additem (col.GetComponent<Itemdata> ().items.id);
			Destroy (col.gameObject);
		}

	}



	public void OpenPanel(){

		Menu.SetActive (true);
	}

	public void ClosePanel(){

		Menu.SetActive (false);
	}


	public void ReturnMenu(){
		SceneManager.LoadScene ("Op_Combat");
	}

}
