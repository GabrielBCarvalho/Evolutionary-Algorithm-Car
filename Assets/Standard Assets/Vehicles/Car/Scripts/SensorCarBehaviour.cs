using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCarBehaviour : MonoBehaviour {

	public GameObject carParent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log ("OnCollision");
		if (collision.gameObject.tag == "Wall") { 
			carParent.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().FinishLife ();
			Debug.Log ("Bateu");
		}
	}
}
