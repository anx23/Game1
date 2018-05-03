using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameControll : MonoBehaviour {
	[SerializeField]
	GameObject datapanel;
	[SerializeField]
	Text datatext;


	public  bool isFirst = false;
	public GameObject LoadingUi;
	private AsyncOperation async;
	public Slider Slider;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//最初から
	public void OnFirst(){
		if (File.Exists (Application.dataPath + "/StreamingAssets/inventory.json")) {
			datapanel.SetActive (true);
			datatext.text = "本当に最初からにしますか";
			Reset ();
		}

		isFirst = true;
		//SceneManager.LoadScene ("com");
		LoadingUi.SetActive(true);
		StartCoroutine (LoadScene());
	}

	public void Reset(){
		File.Delete (Application.dataPath + "/StreamingAssets/inventory.json");
		Debug.Log ("Reset");
	}

	//続きから
	public void OnContinue(){
		if (File.Exists (Application.dataPath + "/StreamingAssets/inventory.json")) {
			isFirst = false;
			//SceneManager.LoadScene ("com");
			LoadingUi.SetActive (true);
			StartCoroutine (LoadScene ());
		} else {

			datapanel.SetActive (true);
			datatext.text = "セーブデータが存在しません";
		}



	}


	//読み込み中に表示する
	IEnumerator LoadScene() {
		async = SceneManager.LoadSceneAsync("com");

		while(!async.isDone) {
			Slider.value = async.progress;
			yield return null;
		}

		if (async.isDone)
			LoadingUi.SetActive (true);
	}




}
