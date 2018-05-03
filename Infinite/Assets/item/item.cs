using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]//この属性を使ってインスペクター上で表示
public class item {

	public string name;
	public int id;
	public int money;
	//public int quantity;
	public string typename;

	public bool isequip;

	public string path;
	/// <summary>
	/// T	/// </summary>
	public Sprite icon;
	public GameObject prehab;
	public  string description;

	public enum ItemType{

		Weapon,
		Food,
		MpUp,
		HpUp,
		Book,
		Extra,

	}

	public ItemType type;

	public item(string name_,int id_,int money_,string typename){

		name = name_;
		id = id_;
		money = money_;
	
		this.typename = typename;
		type = (ItemType)Enum.Parse (typeof(ItemType), typename);
	}

	public item(string name_,int id_,int money_,string typename,bool have){

		name = name_;
		id = id_;
		money = money_;

		this.typename = typename;
		isequip = have;
		type = (ItemType)Enum.Parse (typeof(ItemType), typename);
	}

	public item(string name_,int id_,int money_,string typename,bool have,string path_,string description_){

		name = name_;
		id = id_;
		money = money_;

		this.typename = typename;
		isequip = have;
		path = path_;
		//Sprite = Resources.Load ("Inventory/UI"+path)as Sprite;
		//prehab=Resources.Load<GameObject>("Inventory/prefab"+path);
		type = (ItemType)Enum.Parse (typeof(ItemType), typename);
		description = description_;
	}

	public item(){
		this.id = -1;



	}


	public void changetype(String typename){
		type = (ItemType)Enum.Parse (typeof(ItemType), typename);

	}


}
