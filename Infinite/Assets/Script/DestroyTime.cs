using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour {
	float time;
	public float destroytime=1.0f;
	// Use this for initialization
	void Start () {
		time = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > destroytime)
			Destroy (this.gameObject);
	}
}
