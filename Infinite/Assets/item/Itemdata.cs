using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Itemdata : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{
	public item items;
	public int number;
	public int amount=0;

	// Use this for initialization
	 
	Playe_Stautus ps;
	public Inventory equip;
	public  GameObject child;
	public Text toolname;
	public Text numText;
	private GameObject sellamount;
	[SerializeField]
	private GameObject panel;
	public Text itemname;

	[SerializeField]
	CombatSystem cs;

	Text descriptionText;


	void Start () {
		//GameObject player=GameObject.FindWithTag ("Player");
		//pl = player.GetComponent<playerlife> ();
		//equip = player.GetComponent<Inventory> ();
		//toolname = child.transform.Find ("Text").gameObject.GetComponent<Text>();
		//child.enabled = false;
		//toolname.enabled=false;
		equip=GameObject.Find("Inventory").GetComponent<Inventory>();
		//child.GetComponent<Button> ().onClick.AddListener (UseItem);
		//child.GetComponent<Button>().enabled=false;
		toolname.enabled = false;
		itemname.text = items.name;
		//numText.text = amount.ToString();
		//sellamount = transform.FindChild ("sell").gameObject;
		descriptionText=GameObject.Find("DescriptionText").GetComponent<Text>();
		descriptionText.text = items.description;
		descriptionText.enabled = false;
		panel = transform.parent.gameObject;

		if (items.id == -1) {
			child.SetActive (false);

		}
		GameObject	player = GameObject.FindWithTag ("Player");
		cs = player.GetComponent<CombatSystem> ();

		ps = player.GetComponent<Playe_Stautus> ();
		/*
		if (items.name=="HpUp") {
			child.GetComponent<Button> ().onClick.AddListener (HpUp);
		}
		if (items.name=="MpUp1") {
			child.GetComponent<Button> ().onClick.AddListener (MpUp);
		}
*/

		/*
		if (items.type == item.ItemType.hpUp1) {
			child.GetComponent<Button> ().onClick.AddListener (HpUp);
		}
		if (items.type == item.ItemType.mpUp1) {
			child.GetComponent<Button> ().onClick.AddListener (MpUp);
		}
		if (items.type == item.ItemType.food) {
			
			this.amount--;
			if (amount == 0) {
				items = new item ();
			}
		}


		if (items.type == item.ItemType.weapon) {
			Destroy (equip.gameObject);
			equip.weapon = items.prehab;
			Instantiateobj ();
		}*/


		/*
		if(toolname.text=="sell")
			child.GetComponent<Button> ().onClick.AddListener (sellitem);
		if(toolname.text=="use")
			child.GetComponent<Button> ().onClick.AddListener (useitem);

		if(toolname.text=="buy")
			child.GetComponent<Button> ().onClick.AddListener (buyitem);
			*/
	



	}
	
	// Update is called once per frame
	void Update () {

	
	}




	public  void OnPointerEnter (PointerEventData eventData)
	{

		descriptionText.enabled = true;
		descriptionText.text = items.description;
		//child.GetComponent<Button>().enabled = true;
		toolname.enabled = true;
		//base.OnPointerEnter (eventData);
		//child.te

			
	}


	public  void OnPointerExit (PointerEventData eventData)
	{
		//base.OnPointerExit (eventData);
		//child.GetComponent<Button>().enabled=false;
		descriptionText.enabled = false;
		toolname.enabled = false;
	}





	public	void UseItem(){
	//	amount--;
		//if (amount == 0) {
			equip.RemoveItem (items.id);

		//}

	}






	public void sellitem(){
		//child.GetComponent<Button> ().onClick.AddListener (sellitem);
		amount--;
		if (amount == 0) {
			items = new item ();
		}

	}

	public void buyitem(){

		equip.additem (items.id);
	}



	public void Instantiateobj(){

		GameObject instance= Instantiate (Resources.Load ("Inventory/Prefab/Book/"+ this.items.path)as GameObject);
	
		UseItem ();
	}

	public void MpUp(){

		ps.mp += 10;
		if (ps.mp > 100)
			ps.mp = 100;
		UseItem ();
		Debug.Log ("Mpup");
	}


	public void HpUp(){
		ps.hp+= 10;
		if (ps.mp > 100)
			ps.mp = 100;
		UseItem ();
		Debug.Log ("Hpup");
	}



	public void LevelUP(){
		ps.level++;
		UseItem ();
	}



	void Controll(){

		if (this.items.id == -1) {
			child.GetComponent<Button> ().enabled = false;
		} else {

			child.GetComponent<Button>().enabled=true;
		}


	}



	public void SetData(){


			itemname.text = items.name;
			numText.text = amount.ToString ();
		    //descriptionText.text = items.description;
			child.SetActive (true);
		if (items.type==item.ItemType.HpUp) {
				child.GetComponent<Button> ().onClick.AddListener (HpUp);
			}
		if (items.type==item.ItemType.MpUp) {
				child.GetComponent<Button> ().onClick.AddListener (MpUp);
			}
		/*
			if (items.type == item.ItemType.food) {
				//pl.lifeup (100);
				this.amount--;
				if (amount == 0) {
					items = new item ();
				}
			}

*/
			if (items.type == item.ItemType.Weapon) {
				Destroy (equip.gameObject);
				equip.weapon = items.prehab;
				Instantiateobj ();
			}
		if (items.type == item.ItemType.Book) {
			child.GetComponent<Button> ().onClick.AddListener (Instantiateobj);

		}

		if (items.type == item.ItemType.Extra) {
			child.GetComponent<Button> ().onClick.AddListener (LevelUP);

		}
	}




	public void UpdateData(){
		
		//amount ui update
		if(amount!=0)
		numText.text = amount.ToString ();

		if (amount == 0) {


			itemname.text = " ";
			numText.text = " ";
			child.SetActive (false);
		}
	}


	
}
