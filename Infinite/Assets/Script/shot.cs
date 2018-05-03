using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour {


	public GameObject explosion;
	public float attack = 10;
	Rigidbody ri;
	GameObject muzzle;
	public  GameObject  impacts;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10.0F); 
		ri = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {


	//this.GetComponent<Rigidbody>().AddForce(transform.forward*500);
	//	transform.position += -transform.forward * Time.deltaTime * 10;
		//Vector3 force =transform.position += transform.forward * Time.deltaTime * 10;
	//	ri.AddForce (force);
	}

 void OnTriggerEnter(Collider collider){
		
		//Debug.Log ("hit");
	
		//Destroy (gameObject);

	//	Instantiate (impacts, this.transform.position, this.transform.rotation);

		if (collider.gameObject.tag == "Enemy") {
			Instantiate (explosion, transform.position, transform.rotation);

			Debug.Log ("hitattack");
			//collider.gameObject.SendMessage("Damage",attack);





			Attack(collider.gameObject); //攻撃する
			Destroy (gameObject);
			//Destroy (collider.gameObject);
		}
		//Destroy (gameObject);





	}





	void OnCollisionEnter(Collision collider){

		//Instantiate (impacts, this.transform.position, this.transform.rotation);


		//Debug.Log ("hit");
		if (collider.gameObject.tag == "Enemy") {
			Instantiate (explosion, transform.position, transform.rotation);

			Debug.Log ("hitattack");
			//collider.gameObject.SendMessage("Damage",attack);





			Attack(collider.gameObject); //攻撃する
			Destroy (gameObject);
			//Destroy (collider.gameObject);
		}
		//Destroy (gameObject);


	}



	public void Attack ( GameObject hit ){
		hit.gameObject.SendMessage("Damage", attack);   //相手の"Damage"関数を呼び出す
	}









}

