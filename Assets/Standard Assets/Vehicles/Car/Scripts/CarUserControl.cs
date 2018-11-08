using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;



namespace UnityStandardAssets.Vehicles.Car
{

    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
		public EvolutiveManager evolutiveManager;

        private CarController m_Car; // the car controller we want to use

		public float distance = 0.0f;

		public Genes genes;

		public GameObject left45;
		public GameObject front;
		public GameObject right45;
		public GameObject left;
		public GameObject right;

		/*
		private bool isLeft45Green = true;
		private bool isFrontGreen = true;
		private bool isRight45Green = true;
		private bool isLeftGreen = true;
		private bool isRightGreen=  true;
		*/

		private Transform defaultPosition;

		private float defaultPositionX = 0f;
		private float defaultPositionY = 0f;
		private float defaultPositionZ = 3.37f;

		public bool isActive = false;

		public Vector3 lastPosition;

		private float h = 0, v = 0;

		private float timer = 0f;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();

        }

		void Start(){
			lastPosition = transform.position;

			GenerateRandomCar ();

			defaultPosition = gameObject.transform;
		}

		public void GenerateRandomCar(){
			genes.Left45Green = RandomAction();
			genes.Left45Red = RandomAction();

			genes.FrontGreen = RandomAction();
			genes.FrontRed = RandomAction();

			genes.Right45Green = RandomAction();
			genes.Right45Red = RandomAction();

			genes.LeftGreen = RandomAction();
			genes.LeftRed = RandomAction();

			genes.RightGreen = RandomAction();
			genes.RightRed = RandomAction();
		}

		private Action RandomAction(){
			int random = (int)Random.Range (0, 5);
			switch (random) {
			case 0:
				return Action.PressLeft;
				break;
			case 1:
				return Action.PressRight;
				break;
			case 2:
				return Action.Accelerate;
				break;
			case 3:
				return Action.Brake;
				break;
			}

			return Action.DoNothing;
		}

		private Action RandomGene(Action actionA, Action actionB){
			int random = Random.Range (0, 2);

			if (random == 0)
				return actionA;
			return actionB;
		}
			
		public void CombineGenes(Genes geneA){
			//Genes newGene = new Genes();

			this.genes.Left45Green = RandomGene (geneA.Left45Green, this.genes.Left45Green);
			this.genes.Left45Red = RandomGene (geneA.Left45Red, this.genes.Left45Red);

			this.genes.FrontGreen = RandomGene (geneA.FrontGreen, this.genes.FrontGreen);
			this.genes.FrontRed = RandomGene (geneA.FrontRed, this.genes.FrontRed);

			this.genes.Right45Green = RandomGene (geneA.Right45Green, this.genes.Right45Green);
			this.genes.Right45Red = RandomGene (geneA.Right45Red, this.genes.Right45Red);

			this.genes.LeftGreen = RandomGene (geneA.LeftGreen, this.genes.LeftGreen);
			this.genes.LeftRed = RandomGene (geneA.LeftRed, this.genes.LeftRed);

			this.genes.RightGreen = RandomGene (geneA.Right45Green, this.genes.RightGreen);
			this.genes.RightRed = RandomGene (geneA.RightRed, this.genes.RightRed);

		}

		public void FinishLife(){
			isActive = false;
			transform.position = new Vector3 (defaultPositionX, defaultPositionY, defaultPositionZ);
			transform.rotation = Quaternion.identity;

			gameObject.SetActive (false);

			evolutiveManager.ChangeActiveCar ();
		}

		public void Mutation(float prob){
			MutationToAGene ("Left45Green", prob);
			MutationToAGene ("Left45Red", prob);
			MutationToAGene ("FrontGreen", prob);
			MutationToAGene ("FrontRed", prob);
			MutationToAGene ("Right45Green", prob);
			MutationToAGene ("Right45Red", prob);
			MutationToAGene ("LeftGreen", prob);
			MutationToAGene ("LeftRed", prob);
			MutationToAGene ("RightGreen", prob);
			MutationToAGene ("RightRed", prob);
		}

		private void MutationToAGene(string gene, float prob){
			float random = Random.Range (0f, 1f);

			if (random <= prob) {
				if (gene == "Left45Green")
					genes.Left45Green = RandomAction ();
				else if (gene == "Left45Red")
					genes.Left45Red = RandomAction ();
				else if (gene == "FrontGreen")
					genes.FrontGreen = RandomAction ();
				else if (gene == "FrontRed")
					genes.FrontRed = RandomAction ();
				else if (gene == "Right45Green")
					genes.Right45Green = RandomAction ();
				else if (gene == "Right45Red")
					genes.Right45Red = RandomAction ();
				else if (gene == "LeftGreen")
					genes.LeftGreen = RandomAction ();
				else if (gene == "LeftRed")
					genes.LeftRed= RandomAction ();
				else if (gene == "RightGreen")
					genes.RightGreen = RandomAction ();
				else if (gene == "RightRed")
					genes.RightRed = RandomAction ();
			}
		}

        private void FixedUpdate()
        {
			if (isActive) {

				distance += Vector3.Distance (transform.position, lastPosition);
				lastPosition = transform.position;

				// pass the input to the car!
				h = CrossPlatformInputManager.GetAxis("Horizontal");
				//h = 0.5f;
				//Debug.Log ("h = " + h.ToString ());

				v = CrossPlatformInputManager.GetAxis("Vertical");
				//Debug.Log("v = " + v.ToString());

				v = 0f;
				h = 0f;

				DoAction ("Left45");
				DoAction ("Front");
				DoAction ("Right45");
				DoAction ("Left");
				DoAction ("Right");

				// Managing cases in which the car keeps not moving
				Debug.Log("v == " + v + "; h = " + h);
				if (v == 0f) {
					timer += Time.fixedDeltaTime;
					int seconds = (int) timer % 60;
					if (seconds == 3) {
						FinishLife ();
						timer = 0f;
					}
				}
					

				#if !MOBILE_INPUT
				float handbrake = CrossPlatformInputManager.GetAxis("Jump");
				m_Car.Move(h, v, v, handbrake);
				#else
				m_Car.Move(h, v, v, 0f);
				#endif
			}


        }

		private void DoAction(string sensor){
			if (sensor == "Left45") {
				if (left45.GetComponent<SensorBehaviour> ().isGreen)
					SetAction (genes.Left45Green);
				else
					SetAction (genes.Left45Red);
			} else if (sensor == "Front") {
				if (front.GetComponent<SensorBehaviour> ().isGreen)
					SetAction (genes.FrontGreen);
				else
					SetAction (genes.FrontRed);
			} else if (sensor == "Right45") {
				if (right45.GetComponent<SensorBehaviour> ().isGreen)
					SetAction (genes.Right45Green);
				else
					SetAction (genes.Right45Red);
			} else if (sensor == "Left") {
				if (left.GetComponent<SensorBehaviour> ().isGreen)
					SetAction (genes.LeftGreen);
				else
					SetAction (genes.LeftRed);
			} else if (sensor == "Right") {
				if (right.GetComponent<SensorBehaviour> ().isGreen)
					SetAction (genes.RightGreen);
				else
					SetAction (genes.RightRed);
			}

		}

		private void SetAction(Action action){
			if (action == Action.Accelerate)
				v += 0.25f;/*
			else if (action == Action.Brake)
				v -= 0.2f;*/
			else if (action == Action.PressLeft)
				h -= 0.25f;
			else if (action == Action.PressRight)
				h += 0.25f;
		}
    }
}
