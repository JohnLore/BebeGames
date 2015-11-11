using UnityEngine;
using System.Collections;



public class WindWakerRay : MonoBehaviour {
	public GameObject target;
	

	private Collider targetCollider;

	public static float turnSensitivity = 0.08f;
	public static float liftSensitivity = 0.3f;
		
	private static float minViewDistance = 3.0f;
	private static float maxViewDistance = 25.0f;
	private static float minHeight = 0.0f;
	private static float maxHeight = 15.0f;
	private float presumedRadius;
	private Vector2 wallDirection;

	private float turnDiminish = 10.0f;
	private float turn;
	private float lift;

	private static float a = 9.0f;
	private static float deltaHeight = maxHeight - minHeight;
	private static float deltaView = (maxViewDistance-minViewDistance);
	private static float b = (maxHeight-minHeight)/(Mathf.Sqrt(1+(deltaView*deltaView)/(a*a))-1);



	/*
	 * 1 = (y/b)^2 -(x/a)^2 
	 */
	private float height(float radius)
	{

		float r = radius - minViewDistance;
		float y = b*(Mathf.Sqrt (1+(r*r)/(a*a)) - 1);

		return y;


	}

	// Use this for initialization
	void Start () 
	{
		targetCollider = GetComponentInParent<Collider> ();
		presumedRadius = Mathf.Sqrt (transform.position.x*transform.position.x+transform.position.z*transform.position.z);
	}

	private Vector3 detectWall()
	{
		Vector3 start = target.transform.position;
		Vector3 direction = transform.position - target.transform.position;
		Ray ray = new Ray(start, direction);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, presumedRadius))
		{
			return hit.point;
		}
		return Vector3.zero;
	}

	private void rotateSide(float angle)
	{
		float cosTurn = Mathf.Cos (angle);
		float sinTurn = Mathf.Sin (angle);
		
		float xPos = transform.position.x - target.transform.position.x;

		float zPos = transform.position.z - target.transform.position.z;

		float xPred = xPos * cosTurn - zPos * sinTurn + target.transform.position.x;
		float zPred = xPos * sinTurn + zPos * cosTurn + target.transform.position.z;
		Vector3 newPosition = new Vector3 (xPred, transform.position.y, zPred);
		transform.position = newPosition;
	}

	private void zoom(float intensity)
	{
		//Reset to presumedRadius
		float curx = transform.position.x - target.transform.position.x;
		float curz = transform.position.z - target.transform.position.z;

		float currentRadius = Mathf.Sqrt (curx*curx + curz*curz);
		float scaleFactor = presumedRadius / currentRadius;
		float xPred = curx * scaleFactor;
		float zPred = curz * scaleFactor;



		//Attempt zoom
		if ((presumedRadius > maxViewDistance && intensity > 0) ||
			(presumedRadius < minViewDistance && intensity < 0)) 
		{
			intensity = 0.0f; 
		}
	
		Vector3 newPosition = new Vector3 (xPred, 0.0f, zPred);
		newPosition += intensity * Vector3.Normalize (newPosition); 

		presumedRadius = Vector3.Magnitude (newPosition);

		newPosition += target.transform.position;

		transform.position = newPosition;
	}

	private void adjustHeight()
	{
		float xPred = transform.position.x;
		float yPred = minHeight + height(presumedRadius)+target.transform.position.y;
		float zPred = transform.position.z;
		Vector3 newPosition = new Vector3 (xPred, yPred, zPred);
		transform.position = newPosition;
	}

	public void getInputs()
	{
		turn = turnSensitivity * Input.GetAxis ("HorizontalR");
		lift = liftSensitivity * Input.GetAxis ("VerticalR");

		if (GlobalVars.invertYAxis)
			turn *= -1.0f;

		if (GlobalVars.invertXAxis)
			lift *= -1.0f;
	

	}

	// Update is called once per frame
	void Update () 
	{
		getInputs ();
		rotateSide (turn);
		zoom (lift);
		adjustHeight ();
		Vector3 collisionPoint = detectWall();
		if (collisionPoint != Vector3.zero) 
		{
		
			transform.position = collisionPoint;
		}


		transform.LookAt (target.transform.position);
	}
}
