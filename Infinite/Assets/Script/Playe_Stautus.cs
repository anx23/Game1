using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playe_Stautus : MonoBehaviour {

	public int level=1;
	public int maxhp=100;
	public int maxmp=100;
	public int hp=100;
	public int mp=100;
	public Slider hpvar;
	public Slider mpvar;
	public Text levelText;

	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) {
			if (go.GetComponent<GameControll> ().isFirst==false)
				Load ();
			
		}
	
		SetLevel ();
		StartCoroutine ("MpUp");
	}
	
	// Update is called once per frame
	void Update () {
		hpvar.value = hp;
		mpvar.value = mp;
		levelText.text = level.ToString();

	}



	IEnumerator MpUp(){
		while (true) {

			yield return new WaitForSeconds (1.0f);
			if (mp < 100)
				mp += 1;


		}
	}







	void LevelUp(){

		level++;
		SetLevel ();
	}

	void SetLevel(){


		switch (level) {
		case 1:
			maxhp = 100;
			maxmp = 100;
			break;
		case 2:
			maxhp = 110;
			maxmp = 110;
			break;
		case 3:
			maxhp = 120;
			maxmp = 120;
			break;
		case 4:
			maxhp = 130;
			maxmp = 130;
			break;
		case 5:
			maxhp = 140;
			maxmp = 140;
			break;
		case 6:
			maxhp = 150;
			maxmp = 140;
			break;
		case 7:
			maxhp = 155;
			maxmp = 145;
			break;


		}

	}



	public void save(){

		PlayerPrefs.SetInt ("My_Hp",hp);
		PlayerPrefs.SetInt ("My_Mp",mp);
		PlayerPrefs.SetInt ("My_Level",level);
		//Save position
		PlayerPrefs.SetFloat("PlayerX", transform.position.x);
		PlayerPrefs.SetFloat("PlayerY", transform.position.y);
		PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
		GameObject go = GameObject.Find ("GameManager");
		if (go.GetComponent<GameControll> ().isFirst == true) {
			go.GetComponent<GameControll> ().isFirst = false;
		}

	}


	public void Load(){
		hp = PlayerPrefs.GetInt ("My_Hp");
		mp = PlayerPrefs.GetInt ("My_mp");
		level=PlayerPrefs.GetInt("My_Level");
		transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));



	}

	public void Damage ( int damage ) {

		hp-= damage; //体力から差し引く
	
		if(hp <= 0){
			//体力が0以下になった時
			hp=0;
			Dead(); //死亡処理
		}
	}

	//死亡処理
	public void Dead () {
		anim.SetTrigger ("dead");
		GameOver(); //ゲームオーバーにする
	}

	public void GameOver () {
		GameObject go = GameObject.Find ("GameManager");
		if (go.GetComponent<GameControll> ().isFirst == true) {
			SceneManager.LoadScene ("Op_Combat");
		} else {
			Load ();
		}
	}

}
