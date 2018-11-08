using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutiveManager : MonoBehaviour {
	private int activeCar = -1;
	private int countGeneration = 1;

	public GameObject[] cars = new GameObject[6];

	private float cameraDefaultPositionX = 0f;
	private float cameraDefaultPositionY = 1.88f;
	private float cameraDefaultPositionZ = -5.48f;

	public Camera cam;

	public Canvas canvas;

	public Text txtGeneration;
	public Text txtCar;
	public Text txtDistance;

	// Use this for initialization
	void Start () {
		ChangeActiveCar ();
		txtGeneration.text = "1";
	}
	
	// Update is called once per frame
	void Update () {
		txtDistance.text = cars [activeCar].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance.ToString ();
	}

	public void ChangeActiveCar(){
		if (activeCar == 5) {	//Finish Generation
			Crossover ();
			Mutation ();
			Genocide ();
			activeCar = 0;
			countGeneration++;
			txtGeneration.text = countGeneration.ToString ();
		} else {	// Change active car
			activeCar++;
			Debug.Log ("Active car = " + activeCar.ToString ());
			Debug.Log ("Generation = " + countGeneration.ToString ());
		}

		// Change active cars and positions in Unity
		for (int i = 0; i < cars.Length; i++) {
			if (i != activeCar) {
				cars [i].SetActive (false);
				//cars [i].GetComponent<Renderer> ().enabled = false;
				cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().isActive = false;
			} else {
				cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().isActive = true;
				cars [i].SetActive (true);
				cam.transform.SetParent (cars [i].transform);
				cam.transform.position = new Vector3 (cameraDefaultPositionX, cameraDefaultPositionY, cameraDefaultPositionZ);
				cam.transform.rotation = Quaternion.identity;
			}
		}
		txtCar.text = activeCar.ToString ();
	}

	private void Crossover(){
		int idBest = 0;

		// Finds the best car
		for (int i = 1; i < cars.Length; i++) {
			if (cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance >
			   cars [idBest].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance)
				idBest = i;
		}

		Debug.Log ("Generation ended. Best car = " + idBest + "; Distance traveled = " + cars [idBest].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance);

		// Combine genes and reset distances
		for (int i = 0; i < cars.Length; i++) {
			if (i != idBest) {
				cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().CombineGenes (cars [idBest].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().genes);
			}
			cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance = 0f;
			cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().lastPosition = cars[i].gameObject.transform.position;
		}
	}

	// Apply mutation to each car
	private void Mutation(){
		for (int i = 0; i < cars.Length; i++)
			cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().Mutation (0.3f);
	}

	// Kills the worse car and generates a new random individual
	private void Genocide(){
		int idLast = 0;
		/*int idLast, idPreLast;
		if (cars [0].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance >
		    cars [1].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance) {
			idPreLast = 0;
			idLast = 1;
		} else {
			idPreLast = 1;
			idLast = 0;
		}
		*/
		// Finds the worst car
		for (int i = 0; i < cars.Length; i++) {
			if (cars [i].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance <
			   cars [idLast].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().distance)
				idLast = i;
		}

		// Generate a new car
		cars [idLast].GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl> ().GenerateRandomCar ();
	}

	/*TODO
	 * Calcular distância percorrida
	 * Ver se o carro bateu
	 * Se bateu
	 * 		Muda carro
	 * 		Voltar pra posição 0
	 * Se acabou o 6
	 * 		Cria a geração
	 */
}
