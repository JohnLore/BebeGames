using UnityEngine;
using System.Collections;

public class WindWakerFree : MonoBehaviour {

	public GameObject player;
	public float turnSensitivity;
	public float liftSensitivity;
	public float maxSin;
	public float minSin;
	public float baseDistance;

	//For use in f;
	public float lineWeight;
	public float parabWeight;
	
	//Store data as a unit vector along with a magnitude
	private Vector3 direction;
	private float magnitude;
	private Collider playerCollider;
	
	void Start ()
	{
		Vector3 initial = transform.position - player.transform.position;
		magnitude = Vector3.Magnitude (initial);
		direction = Vector3.Normalize(initial);
		playerCollider = GetComponentInParent<Collider> ();
	}

	float f(float psi)
	{
		return lineWeight * psi + parabWeight * psi * psi;
	}
	
	void Update()
	{
		//Treat a horizontal movement as a rotation about y
		//Treat a vertical movement as a rotation in the plane
		//parallel to y, containing the cameras position.
		//Upward rotation will also zoom out the camera
		//According to a function f, where r = f(psi), and
		//psi is  the current viewing angle.
		float turn = Input.GetAxis ("HorizontalR");
		
		//inverted/uninverted y-axis control set by Menu -- default inverted
		float lift = Input.GetAxis ("VerticalR");
		if (GlobalVars.invertYAxis) //invertYAxis is global
			lift *= 1.0f;
		else
			lift *=  -1.0f;
		
		float Theta = lift * liftSensitivity;
		float Phi = turn * turnSensitivity;


		//First get the components x=a,y=b,z=c from the direction vector3
		float a = direction.x;
		float b = direction.y;
		float c = direction.z;


		if( Theta > 0.0f && b > maxSin)
		{
			Theta = 0.0f;
		}

		if (Theta < 0.0f && b < minSin) {
			Theta = 0.0f;
		}
		//Check if trying to increase past boundary angle

		//Calculate trigs
		float sinTheta = Mathf.Sin (Theta);
		float cosTheta = Mathf.Cos (Theta);
		
		Vector4 row1 = new Vector4 (cosTheta, -sinTheta, 0.0f, 0.0f);
		Vector4 row2 = new Vector4 (sinTheta,  cosTheta, 0.0f, 0.0f);
		
		float sinPhi = Mathf.Sin (Phi);
		float cosPhi = Mathf.Cos (Phi);
		
		Vector4 row1Y = new Vector4 (cosPhi, 0.0f, -sinPhi, 0.0f);
		Vector4 row3Y = new Vector4 (sinPhi, 0.0f,  cosPhi, 0.0f);
		
		
		
		//Construct basis uyvw from xyzw
		
		//[a,0,b,0]
		Vector4 u = Vector3.Normalize(new Vector4 (a, 0.0f, c, 0.0f));
		//[0,1,0,0]
		Vector4 y = new Vector4 (0.0f, 1.0f, 0.0f, 0.0f);
		//[c,0,-a,0]
		Vector4 v = Vector3.Normalize (new Vector4 (c, 0.0f, -a, 0.0f));
		
		Vector4 w = new Vector4 (0.0f, 0.0f, 0.0f, 1.0f);
		
		Vector4 zer0 = Vector4.zero;
		
		float aPrime = Vector3.Dot (u, Vector3.right);
		float cPrime = Vector3.Dot (u, Vector3.forward);
		
		float det = aPrime * aPrime + cPrime * cPrime;
		//Calculate Matrix Transformations
		Matrix4x4 toBasis = Matrix4x4.zero;
		toBasis.SetRow (0, u);
		toBasis.SetRow (1, y);
		toBasis.SetRow (2, v);
		toBasis.SetRow (3, w);
		
		Matrix4x4 rotateV = Matrix4x4.zero;
		rotateV.SetRow (0,row1);
		rotateV.SetRow (1, row2);
		rotateV.SetRow (2, new Vector4(0.0f,0.0f,1.0f,0.0f));
		rotateV.SetRow (3, w);
		
		Matrix4x4 rotateY = Matrix4x4.zero;
		rotateY.SetRow (0, row1Y);
		rotateY.SetRow (1, y);
		rotateY.SetRow (2, row3Y);
		rotateY.SetRow (3, w);
		
		Matrix4x4 frBasis = Matrix4x4.zero;
		frBasis.SetRow (0, u / det);
		frBasis.SetRow (1, y);
		frBasis.SetRow (2, v / det);
		frBasis.SetRow (3, w);
		
		
		//Compact matrix transformations
		Matrix4x4 rotate;
		rotate = toBasis * rotateY * rotateV * frBasis;
		
		direction = rotate * direction;
		
		//don't let player look below ground

		
		
		//TODO: fix -- when player holds down arrow (trying to look through ground)
		//             camera creeps towards player. Prevent camera from zooming in here.
		
		//TODO: fix -- when reset game from menu, the light gets weird
		
		//TODO: fix -- when player looks straight up (holds down up) its spazzes
		
		//Zoom/fade
		float xDir = direction.x;
		float yDir = direction.y;
		float zDir = direction.z;
		float rDir = Mathf.Sqrt(xDir*xDir+zDir*zDir);
		float sinDir = yDir / Mathf.Sqrt (rDir*rDir+yDir*yDir);

		magnitude = f (yDir) + baseDistance;

		//A function call to make sure the camera isn't in a wall.
		correctMagnitude ();
		
	}

	void correctMagnitude()
	{
		RaycastHit hit;
		Vector3 start = transform.position;
		Vector3 end = player.transform.position;
		Vector3 diff = start - end;
		Ray ray = new Ray(start, diff);
		if (Physics.Raycast (ray, out hit))
			if (hit.collider != playerCollider) {
				magnitude = magnitude - hit.distance;
			}

	}
	void LateUpdate ()
	{
		transform.position = magnitude * direction + player.transform.position;
		transform.LookAt (player.transform.position);
	}
}
