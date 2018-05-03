using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EventManager : MonoBehaviour {


	private VideoPlayer moviePlayer;
	public VideoClip vclip;
	[SerializeField]
	private GameObject movieCanvas;

	private AudioSource audioSource;
	private GameObject player;
	private CombatSystem cs;
	public RenderTexture rendertex;
	// Use this for initialization
	void Start () {
		moviePlayer = GetComponent<VideoPlayer> ();
		audioSource = GetComponent <AudioSource> ();

		GameObject go = GameObject.Find ("GameManager");
		player = GameObject.FindWithTag ("Player");
		cs = player.GetComponent<CombatSystem> ();

		rendertex = new RenderTexture ((int)vclip.width,(int)vclip.height,24);
	
		if (go != null) {
			if (go.GetComponent<GameControll> ().isFirst == true)
				movieCanvas.SetActive (true);
		//	cs.enabled = false;


			moviePlayer.Play ();
			Time.timeScale = 0;

		} else {
			movieCanvas.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if ((ulong)moviePlayer.frame == moviePlayer.frameCount)
		{
			//※ここに終了したときの処理など
			Debug.Log("finish");
			Time.timeScale = 1.0f;
			movieCanvas.SetActive (false);
			cs.enabled=true;
			return;
		}
	}
}
