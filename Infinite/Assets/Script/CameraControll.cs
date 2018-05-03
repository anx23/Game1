using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

	private const string MAIN_CAMERA_TAG_NAME = "MainCamera"; 
	//カメラに表示されているか 
	private bool _isRendered = false; 





	public float speed = 10.0f; 

	public Transform target;    // ターゲットへの参照
	public Vector3 offset;     // 相対座標
	GameObject myCamera;
	void Start ()
	{
		//自分自身とtargetとの相対距離を求める
		//

		//offset = GetComponent<Transform>().position - target.position;
		myCamera = GameObject.FindWithTag ("MainCamera");
	}




	void Update ()
	{

		if(Input.GetKeyDown("t")&&_isRendered){
			//transform.LookAt ();

		}





	}







	void LateUpdate ()
	{

		// 自分自身の座標に、targetの座標に相対座標を足した値を設定する
		//	transform.position =Vector3.Lerp(transform.position, target.position + offset,Time.deltaTime);
		transform.position = target.transform.position+offset;

		//WallCloseStop ();



		//	Vector3 axis = transform.TransformDirection (Vector3.up); 
		//	transform.RotateAround (target.position, axis, speed * Input.GetAxis("Horizontal2")); 


	}

	private void OnWillRenderObject(){ 
		//メインカメラに映った時だけ_isRenderedを有効に 
		if(Camera.current.tag == MAIN_CAMERA_TAG_NAME){ 
			_isRendered = true; 
		} 
	} 




	void WallCloseStop(){
		RaycastHit hit;
		if(Physics.Raycast(myCamera.transform.position,myCamera.transform.forward*-1.0f,out hit,2.0f))
		{
			if (hit.collider.gameObject.tag == "wall")
				myCamera.transform.position = hit.point;

		}

	}
}
