using UnityEngine;
using System.Collections;

public class WindWakerFreeDeprecated : MonoBehaviour {

	public GameObject target;
	private Camera cam;
	public float turnSensitivity;
	//public float liftSensitivity;
	public float zoomSensitivity;

	//Variables used in calculating the camera curve.
	public float liftMin;
	public float liftMax;
	public float zoomMin;
	public float zoomMax;
	public float xScale;
	public float yTolerance;

	//Previous variables adjusted to the origin.
	private float liftRange;
	private float zoomRange;
	private float yScale;

	private CharacterController lakitu;
	// Use this for initialization
	void Start () {
		lakitu = GetComponent<CharacterController>();
		cam = GetComponent<Camera> ();
		//Precomputation of hyperbolic values
		//Calculate ranges
		liftRange = liftMax - liftMin;
		zoomRange = zoomMax - zoomMin;
		//Calculate yScale from xScale
		yScale = 
			liftRange/(Mathf.Sqrt (1+zoomRange*zoomRange/xScale/xScale)-1);
	}


	//Function that maps a zoom distance to a camere height
	//Currently a hyperbola.
	private float height(float radius){
		float r = radius - zoomMin;
		float y = yScale*(Mathf.Sqrt (1+r*r/xScale/xScale) - 1);
		return y;
	}


	// Update is called once per frame
	void Update () {
		//Get Inputs
		float turnAngle = Input.GetAxis ("HorizontalR") * turnSensitivity;
		float zoomIntensity = Input.GetAxis ("VerticalR") * zoomSensitivity;

		//Invert according to user settings
		if (GlobalVars.invertYAxis)
			zoomIntensity *= -1.0f;

		//See where the camera would turn

		float cosTurn = Mathf.Cos (turnAngle);
		float sinTurn = Mathf.Sin (turnAngle);

		float xPos = transform.position.x - target.transform.position.x;
		float yPos = transform.position.y - target.transform.position.y;
		float zPos = transform.position.z - target.transform.position.z;
		float xPred = xPos * cosTurn - zPos * sinTurn;
		float zPred = xPos * sinTurn + zPos * cosTurn;

		float xDel = xPred - xPos;
		float zDel = zPred - zPos;


		Vector3 predDirection = new Vector3 (xDel, 0, zDel);

		//See where the camera would zoom
		Vector3 zoomDirection = target.transform.position - transform.position;
		zoomDirection = Vector3.Normalize (zoomDirection);
		zoomDirection *= zoomIntensity;


		//Rotate The camera
		lakitu.Move (predDirection);
		//Zoom the camera
		lakitu.Move (zoomDirection);

		Vector3 newPosition = transform.position;

		//Adjust the height
		float xNew = newPosition.x - target.transform.position.x;
		float yNew = newPosition.y - target.transform.position.y;
		float zNew = newPosition.z - target.transform.position.z;

		float rNew = Mathf.Sqrt (xNew * xNew + zNew * zNew);
		yNew = height (rNew) + target.transform.position.y;
		transform.position = new Vector3 (newPosition.x, yNew, newPosition.z);
		float yDel = yNew - (transform.position.y - target.transform.position.y);


		//TODO: Figure out how to implement this code, without jitter.
		//Try implementing the fixed radius; may get two birds with one stone.

		/*
		if(yDel > yTolerance || -yToleran	ce > yDel)
			lakitu.Move (new Vector3(0.0f,yDel,0.0f));

		*/


		//Revert if the camera is compressed against a wall.
		if(rNew > zoomMax)  
			transform.position = 
		    	new Vector3 (xPos, yPos, zPos) + target.transform.position;

		//TODO: Make sure it doesn't get compressed against the character or caught on walls
		
		transform.LookAt(target.transform.position);
	}
}
