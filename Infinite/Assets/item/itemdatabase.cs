using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class itemdatabase : MonoBehaviour {

	//[SerializeField]
	public List<item> itemdata =new List<item>();

	public List<int> score = new List<int> ();
	// Use this for initialization


	private JsonData database;

	void Start () {
		//itemdata.Add (new item ("sowd", 1, 100, item.ItemType.weapon,true));
		//itemdata.Add (new item ("gun", 0, 100,item.ItemType.weapon,true));
		database=JsonMapper.ToObject(File.ReadAllText(Application.dataPath+"/StreamingAssets/items.json"));

		Debug.Log (database[0]["name"].ToString());
		Constructdatabase ();

		////load

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void  Constructdatabase(){
		for(int i=0;i<database.Count;i++){

			itemdata.Add (new item(database[i]["name"].ToString(),(int)database[i]["id"],(int)database[i]["money"],database[i]["typename"].ToString(),
				(bool)database[i]["isequip"],database[i]["path"].ToString(),database[i]["description"].ToString()));

		}
	}






	public item Search_database(int id){


		for (int i = 0; i < itemdata.Count; i++)
			if (id == itemdata [i].id) {
				Debug.Log (itemdata [i].name);
				return itemdata [i];
			}
		return null;
	}



}
	