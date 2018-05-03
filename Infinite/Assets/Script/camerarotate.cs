using UnityEngine;
using System.Collections;

public class camerarotate : MonoBehaviour {





	//public GameObject player; 
	Quaternion camerarot;
	GameObject playerdirection;
	public float rotSpeed = 10.0f; 
	float maxAngle = 20; // 最大回転角度
	float minAngle = -30; // 最小回転角度
	float speed = 0.5f; // 回転スピード(お好みで調整してください)
	void Start () {
		camerarot = transform.rotation;
		playerdirection = GameObject.Find ("playerdirection");
	}

	void Update () {
		// 入力情報
		/*
		float turn = Input.GetAxis("Vertical2")*rotSpeed;
		// 現在の回転角度を0～360から-180～180に変換
		float rotateY = (transform.eulerAngles.x> 180) ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
		// 現在の回転角度に入力(turn)を加味した回転角度をMathf.Clamp()を使いminAngleからMaxAngle内に収まるようにする
		float angleY = Mathf.Clamp(rotateY + turn * speed, minAngle, maxAngle);
		// 回転角度を-180～180から0～360に変換
		angleY = (angleY < 0) ? angleY + 360 : angleY;
		// 回転角度をオブジェクトに適用
		//transform.rotation = Quaternion.Euler(angleY, transform.eulerAngles.y, transform.eulerAngles.z);

*/



		float axisH = Input.GetAxis( "Horizontal2" ); 
		transform.Rotate (0,axisH*5.0f,0,Space.World);
		playerdirection.transform.Rotate (0,axisH*5.0f,0,Space.World);

		float axisV = Input.GetAxis("Vertical2")*rotSpeed;
		/////


	//	transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Repeat(transform.eulerAngles.y + axisH, 360f), transform.eulerAngles.z);

		//　カメラの角度が限界角度を超えてなければカメラの角度を更新する

		float rot = Mathf.Repeat(transform.eulerAngles.x + axisV, 360f);

		if(40 > Quaternion.Angle(camerarot, Quaternion.Euler(rot, 0, 0))) {
			transform.rotation = Quaternion.Euler(rot, transform.eulerAngles.y, transform.eulerAngles.z);
		}




	





	}











}
