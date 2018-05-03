using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class playerlockon : MonoBehaviour {

	public List<GameObject> obj=new List<GameObject>();
	public List<GameObject> nearenemys = new List<GameObject> ();
	public List<GameObject> farenemys = new List<GameObject> ();
	public GameObject target;
	GameObject nearnestenemy;
	public bool lockon;
	Vector3 lookposition;
	Animator anim;
	Vector3 default_campos;
	Quaternion default_camrot;
	CombatSystem combat;
	void Start(){
		default_campos = Camera.main.transform.position;
		default_camrot = Camera.main.transform.rotation;
		anim = GetComponent<Animator> ();
		combat = GetComponent<CombatSystem> ();

	}


	void Update(){
		if (obj.Count != 0) {
			foreach (GameObject enemy in obj) {

				if (enemy.tag == "Enemy" && enemy.GetComponent<ZombieAI> ()._state.ToString () == "Combat" && !nearenemys.Contains (enemy))
					nearenemys.Add (enemy);
			

			}

		}

		if (Input.GetKeyDown ("l")) {
			FindClosestEnemy ();
		}


		//transform.LookAt (target.transform.position);

		/*
			if (obj != null) {
				//Find ();
				transform.LookAt(Find());
				lockon = true;
			}

		}
*/

		if(nearenemys.Count!= 0&&!lockon&&target==null)
		FindClosestEnemy ();


		if (nearenemys.Count==0) {
			lockon = false;
			target = null;
			anim.ResetTrigger ("combat");
			anim.SetTrigger("normal");

		}

		/*
		if (lockon == true) {
			//transform.LookAt (lookposition);
			if (Input.GetKeyDown ("k")) {
				lockon = false;
			}
		}
		if (Input.GetKeyDown ("k")) {
			lockon = false;
		}
*/



	}


/*
	public void Find(){

		target = obj [0];
		float MinDistance;
		MinDistance=Vector3.Distance(transform.position,obj[0].transform.position);

		for (int i = 0; i < obj.Count; i++) {
			float distance = Vector3.Distance (transform.position,obj[i].transform.position);
			if (distance < MinDistance){
				
				MinDistance = distance;
				target = obj [i];
				}

		}

		//return target.transform.position;


	}

*/




	public void FindClosestEnemy(){
		if (nearenemys == null)
			return;




		if (nearenemys != null) {
			target = nearenemys[0];
			float MinDistance;
			MinDistance = Vector3.Distance (transform.position,  nearenemys [0].transform.position);

			for (int i = 0; i < nearenemys.Count; i++) {
				if (nearenemys [i].tag == "Enemy") {


					float distance = Vector3.Distance (transform.position,nearenemys[i].transform.position);
					if (distance < MinDistance) {

						MinDistance = distance;
						target = nearenemys [i];
					}

				}

			}

			if (target.GetComponent<ZombieAI> ()._state.ToString () == "Combat") {
				lookposition=new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
				setdefo = true;
				lockon = true;

			}
		}

	










	}



	void FormationCombat(){
		int c = nearenemys.Count;
		if (c > 0) {
			nearnestenemy = target;
			//if(target!=null)

			foreach (GameObject enemy in nearenemys) {
				if (!enemy.GetComponent<ZombieAI> ().isTarget)
					enemy.GetComponent<ZombieAI> ().isTarget = false;
				else
					target.GetComponent<ZombieAI> ().isTarget = true;	
			}
		}

	}


	void LookAtTarget(){
		Vector3 LookDir = target.transform.position - transform.position;
		LookDir.y = 0;
		Quaternion rot = Quaternion.LookRotation (LookDir);
		transform.rotation= Quaternion.Slerp (transform.rotation,rot,Time.deltaTime*1.0f);

	}



	/** Z方向の距離 */
	public float distance = 5.0f;

	/** Y方向の高さ */
	public float height = 2.0f;

	/** 上下高さのスムーズ移動速度 */
	public float heightDamping = 1.0f;

	/** 左右回転のスムーズ移動速度 */
	public float rotationDamping = 1.0f;

	bool setdefo;
	public Transform camerapos;

	void LateUpdate(){

		if (lockon == true) {
			SmootheFollow ();

			combat.TypeChange ("combat");
			FormationCombat ();
			anim.ResetTrigger ("normal");
			anim.SetTrigger ("combat");
			//transform.LookAt (lookposition);
			LookAtTarget();
			//Camera.main.transform.LookAt (lookposition);

		} else {
			combat.TypeChange ("normal");
			target = null;
			anim.ResetTrigger ("combat");
			anim.SetTrigger("normal");
			//if (setdefo) {
				//Camera.main.transform.position = transform.position + default_campos;
				//setdefo = false;
			//}
			//Camera.main.transform.rotation = new Quaternion (0,0,0,0);

			Camera.main.transform.position = camerapos.position;
			Camera.main.transform.rotation = camerapos.rotation;
		}
	}




	void SmootheFollow(){
		if (target == null) {
			return;
		}

		//追従先位置
		float wantedRotationAngle =Camera.main.transform.eulerAngles.y;
		float wantedHeight = transform.position.y + height;

		//現在位置
		float currentRotationAngle = Camera.main.transform.eulerAngles.y;
		float currentHeight = Camera.main.transform.position.y;

		//追従先へのスムーズ移動距離(方向)
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 
			rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		//カメラの移動
		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		Vector3 pos = transform.position - currentRotation * Vector3.forward * distance;
		pos.y = currentHeight;
		Camera.main.transform.position = pos;

		Camera.main.transform.LookAt(transform.position);
	}




	void counternext(){

	}






	//////////////////
	void ControllCombat(){



	}


}
