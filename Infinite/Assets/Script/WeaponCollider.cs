using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour {


	private AudioSource audiosourse;
	[SerializeField]
	AudioClip sourse1;
	Animator anim;
	public BoxCollider boxcoll;
	//public GameObject effect;
	// Use this for initialization
	void Start () {
		//anim = GetComponent<Animator> ();
		boxcoll.enabled = false;
		audiosourse = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {




	}



	//アニメーションイベント
	 void ColliderOn(){
		boxcoll.enabled = true;
		//if(effect!=null)
		//Instantiate (effect,transform.position+transform.forward*0.2f+transform.up*0.5f,transform.rotation);
		if(audiosourse!=null&&sourse1!=null)
		audiosourse.PlayOneShot(sourse1);
	}


	 void ColliderOff(){
		boxcoll.enabled = false;
	}


}
