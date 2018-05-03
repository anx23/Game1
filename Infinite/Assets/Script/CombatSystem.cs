using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class CombatSystem : MonoBehaviour {
	//プレーヤーパラメータ
	public float rotationSpeed = 180.0f;	
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	Vector3 playerDir;

	private	Animator anim;
	Playe_Stautus playerStatus;

	bool combochance=false;
	[SerializeField]
	bool can_move;
	bool extra;
	//音
	private AudioSource audiosourse;
	public AudioClip se1;
	public float audiotime;
	//ロックオンシステム
	GameObject locktarget;
	GameObject playerdirection;	
	playerlockon pllock;

	#region マテリアル取得
	public GameObject []meshchildTrans;
	public Material[] originalMaterial;
	public GameObject meshs;
	#endregion
	//攻撃
	public bool isAatack;
	public GameObject extraAttackobj;
	//遠距離攻撃
	public GameObject shot;
	public Transform hand;
	public 	bool fall=false;

	//特殊移動
	public float dashSpeed=3.0f;
	float timer;
	public bool dash;
	public GameObject dashEfect;
	public Material shadowMaterial;
	public float time = 0;
	Vector3 dir2;
	public bool canInput = true;
	float mx=0, my=0;


	/// <summary>
	/// State.
	/// </summary>
	public enum state
	{
		normal,
		combat,
		guard,
		attack,
		extra,
		moving,
	}


	public state ps=state.normal;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerdirection = GameObject.Find ("playerdirection");
		pllock = GetComponent<playerlockon> ();
	
		playerStatus = GetComponent<Playe_Stautus> ();
	//	meshchildTrans = meshs.GetComponentsInChildren<Transform> ();
		//originalMaterial = meshs.transform.GetComponentsInChildren<MeshRenderer>();
		audiosourse=GetComponent<AudioSource>();

		//通常時のマテリアル取得
		meshchildTrans = new GameObject[ meshs.transform.childCount ];
		originalMaterial = new Material[ meshs.transform.childCount ];
		for(int i=0;meshs.transform.childCount>i;i++)
		{
			meshchildTrans[i] = meshs.transform.GetChild(i).gameObject;
			originalMaterial [i] = meshchildTrans[i].GetComponent<SkinnedMeshRenderer> ().material;
		}

		/*
		for(int i=0;meshs.transform.childCount>i;i++)
		{
			meshchildTrans[i].GetComponent<SkinnedMeshRenderer>().enabled=false;
		}
*/
	


	}
	
	// Update is called once per frame
	void Update () {
		//mpSlider.value = mp;


			CharacterController controller = GetComponent<CharacterController> ();
	//移動できる場合
		if (can_move) {
			//着地状態
			if (controller.isGrounded) {
					//通常時
				if (ps == state.normal) {
					float v = Input.GetAxis ("Vertical");	// 上下のキー入力を取得
					float h = Input.GetAxis ("Horizontal");	// 左右のキー入力を取得

					Vector3 forward = playerdirection.transform.TransformDirection (Vector3.forward); 
					Vector3 right = playerdirection.transform.TransformDirection (Vector3.right); 


					moveDirection = h * right + v * forward; 

					playerDir = moveDirection*speed*0.7f;
				
					moveDirection *= speed;
					anim.SetFloat ("x", h);
					anim.SetFloat ("y", v);


					if (playerDir.magnitude > 0.1f) {
						Quaternion q = Quaternion.LookRotation (playerDir);			// 向きたい方角をQuaternionn型に直す .
						transform.rotation = Quaternion.RotateTowards (transform.rotation, q, rotationSpeed * Time.deltaTime);	// 向きを q に向けてじわ～っと変化させる.
					}

					//sound 
					if (moveDirection.magnitude > 0.2f) {
						audiotime += Time.deltaTime;

						if (audiotime >= 0.3f) {
							audiosourse.PlayOneShot (se1);
							audiotime = 0;
						}
					}

				}
				//戦闘時
				if (ps == state.combat) {

					float h = Input.GetAxis ("Horizontal");
					float v = Input.GetAxis ("Vertical");
					moveDirection = new Vector3 (h, 0, v);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed;
					anim.SetFloat ("x", h);
					anim.SetFloat ("y", v);
				}



				if (Input.GetButton ("Jump")) {
					moveDirection.y = jumpSpeed;
					anim.SetTrigger ("Jump");
				} 
			} else {
				anim.ResetTrigger ("Jump");
				Fall ();
			}


			//anim.SetTrigger ("land");





			//rolling
			if (Input.GetKeyDown ("v")) {
				anim.SetTrigger ("roll");
				anim.applyRootMotion = true;
			}

			if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("roll") && anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.9f) {
				anim.applyRootMotion = false;
			}
			///////////////////////

			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move (moveDirection * Time.deltaTime);


		} else {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Jump")) {
				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move (moveDirection * Time.deltaTime);
			}
		}
		/////
	

		//ExtraDash ();
		if (Input.GetKey ("left shift")) {
			extra = true;
		} else {
			//can_move = true;
			extra=false;
		}



		if (extra == true) {
			ExtraDash ();
		}

		if (dash == true)
			ExtraDash ();
	





		if( ps==state.combat){
			melee ();

			//CounterAtack ();
		}

		#region アニメーション管理
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("move")||anim.GetCurrentAnimatorStateInfo (0).IsName ("Fall")) {

			can_move = true;
		} else {

			can_move = false;
		}

		//////
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.1f) {

			anim.ResetTrigger ("combo");
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.9f) {
			combochance = true;
		} else {
			//anim.ResetTrigger ("Attack");
			combochance = false;
		}


		if(combochance==true){
			if(Input.GetKeyDown("e")){

				anim.SetTrigger ("combo");
			}

		}


		if (anim.GetCurrentAnimatorStateInfo (0).IsName("EndAttack")||anim.GetCurrentAnimatorStateInfo (0).IsName ("DashAttack")&& anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.1f) {
			anim.applyRootMotion = true;


		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") ||anim.GetCurrentAnimatorStateInfo (0).IsName("EndAttack")&& anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.9f) {
			anim.applyRootMotion = false;
			anim.ResetTrigger ("Attack");

		}


	

		#endregion
	
		Fall ();


	}

	//アニメーションイベント
	void StartShot(){

		Shot ();
	}


	void CastStart(){
		ExtraExposion ();
	
	}


	//攻撃
	void melee(){



		
		if (Input.GetKeyDown ("e")) {
			if(!anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") ||(!anim.GetCurrentAnimatorStateInfo (0).IsName ("EndAttack")))
			anim.SetTrigger ("Attack");

		}




		/// 回避
		if (Input.GetKeyDown ("g")) {
			anim.SetTrigger ("back");
			StartCoroutine (avoid());
		}
		if (Input.GetKeyDown ("r")) {
			if (playerStatus.mp> 20) {
				anim.SetTrigger ("shot");
				anim.applyRootMotion = true;
			}
		}

		if (Input.GetKeyDown ("t")) {
			if(playerStatus.mp>80)
				anim.SetTrigger ("Explosion");
			//ExtraExposion();
		}

		//ExtraDash
		if (Input.GetKey ("left shift")) {
			extra = true;
		} else {
			//can_move = true;
			extra=false;
		}

		

		if (extra == true) {
			ExtraDash ();
		}

		if (dash == true)
			ExtraDash ();
	}

	//回避
	IEnumerator avoid(){


		can_move = false;
		anim.applyRootMotion = true;
		yield return new WaitForSeconds (1.0f);
		can_move = true;
		anim.applyRootMotion = false;
	}


	/*
	void dashattack(){
		float v = Input.GetAxis ("Vertical");
		if(v>0){
			if (Input.GetKeyDown ("e")) {
				anim.SetTrigger ("dash_attack");
			}
		}


	}

	void CounterAtack(){

		if(pllock.target.GetComponent<nearenemy>().iscounter){
			if (Input.GetKeyDown ("e")) {
				myhp.iscounter = true;
			//	anim.SetTrigger ("counter");
				anim.applyRootMotion = false;
			//	transform.position=Vector3.Lerp (transform.position,pllock.target.transform.position,Time.deltaTime*2.0f);
			}
		}

	}
*/


	void Shot(){
		GameObject go=	Instantiate (shot, hand.transform.position,transform.rotation);
		go.GetComponent<Rigidbody>().AddForce (transform.forward*1000f);
		playerStatus.mp -= 20;

	}






	void ExtraDash(){
		extra = true;

		if (canInput) {
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			Vector3 moveDir = new Vector3 (h, 0, v);
			//moveDir = transform.TransformDirection (moveDirection);
			//moveDir *= speed;
		
			//方向決定
			if (moveDir.magnitude != 0) {
				dir2 = moveDir;
				if (h > 0)
					mx = 1;
			   if(h<0)
					mx=-1;

				if (v > 0)
					my = 1;
				if (v < 0)
					my = -1;
			
			
				dash = true;

			}
		}
		if (dash == true) {
			canInput = false;
			time += Time.deltaTime;
			var count1 = meshchildTrans.Length;


			if (time < 1.2f) {
				//transform.Translate (Time.deltaTime *dashSpeed * dir2.normalized);
				GetComponent<Rigidbody> ().AddForce (Time.deltaTime *dashSpeed * dir2.normalized);
				anim.SetBool ("S_dash", true);
				anim.SetFloat ("x", mx);
				anim.SetFloat ("y", my);
				for (int i = 0; i < count1; i++) {
					meshchildTrans [i].GetComponent<SkinnedMeshRenderer> ().material = shadowMaterial;
				}

			}
			else if(time<1.4f){
				var count = originalMaterial.Length;

				for (int i = 0; i < count; i++) {
					meshchildTrans [i].GetComponent<SkinnedMeshRenderer> ().material = originalMaterial [i];
				}
				anim.SetBool ("S_dash", false);
			}
			else {
				mx = 0;
				my = 0;
				canInput = true;

				can_move = true;
				time = 0;
				dash = false;
				extra = false;
			}
		}


	}






	void ExtraExposion(){
		
		Transform enemytrans = pllock.target.transform;
		Instantiate (extraAttackobj,enemytrans.position+new Vector3(0,0.3f,0),enemytrans.rotation);

		playerStatus.mp -= 80;
		if (playerStatus.mp< 0)
			playerStatus.mp = 0;
	}






	//落下時
	void Fall(){

		RaycastHit hit;
		if (Physics.Raycast (transform.position, -transform.up, 2.0f)) {
			anim.ResetTrigger ("fall");
			if (fall) {
				anim.SetTrigger ("land");
				fall = false;
			} else {
				
				fall = false;
			}
			
		
		} else {

			fall = true;
		}

		if (fall)
			anim.SetTrigger ("fall");
		
	}



	public void TypeChange(string i){
		
		ps = (state)Enum.Parse (typeof(state), i);
	}


}
