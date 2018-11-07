using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBehaviour : MonoBehaviour {

	public bool isGreen = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Wall") {
			isGreen = false;
			Debug.Log ("Red!");
		}
	}

	private void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Wall") {
			isGreen = true;
			Debug.Log ("Green!");
		}
	}
}
