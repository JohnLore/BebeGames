using UnityEngine;
using System.Collections;

//A camera script which assumes no rotations or scaling of the player it is attatched to
public class PlayerCamera : MonoBehaviour {
	
	public GameObject player;
	public float xSensitivity;
	public float ySensitivity;
	public float zoomSensitivity;

	//Store data as a unit vector along with a magnitude
	private Vector3 direction;
	private float magnitude;
	
	void Start ()
	{
		Vector3 initial = transform.position - player.transform.position;
		magnitude = Vector3.Magnitude (initial);
		direction = Vector3.Normalize(initial);
	}

	void Update()
	{
		//Treat a horizontal movement as a rotation about y
		//Treat a vertical movement as a rotation in the plane
		//parallel to y, containing the cameras position.
		float moveHorizontal = Input.GetAxis ("HorizontalR");

		//inverted/uninverted y-axis control set by Menu -- default inverted
		float moveVertical;
		if (GlobalVars.invertYAxis) //invertYAxis is global
			moveVertical = 1.0f * Input.GetAxis ("VerticalR");
		else
			moveVertical =  -1.0f * Input.GetAxis ("VerticalR");

		float Theta = moveVertical * ySensitivity;
		float Phi = moveHorizontal* xSensitivity;
		//Calculate trigs
		float sinTheta = Mathf.Sin (Theta);
		float cosTheta = Mathf.Cos (Theta);

		Vector4 row1 = new Vector4 (cosTheta, -sinTheta, 0.0f, 0.0f);
		Vector4 row2 = new Vector4 (sinTheta,  cosTheta, 0.0f, 0.0f);

		float sinPhi = Mathf.Sin (Phi);
		float cosPhi = Mathf.Cos (Phi);

		Vector4 row1Y = new Vector4 (cosPhi, 0.0f, -sinPhi, 0.0f);
		Vector4 row3Y = new Vector4 (sinPhi, 0.0f,  cosPhi, 0.0f);


		//First get the components x=a,y=b,z=c from the direction vector3
		float a = direction.x;
		float b = direction.y;
		float c = direction.z;


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
		if (direction.y < 0.0f)
			direction.y = 0.0f;

		//TODO: fix -- when player holds down arrow (trying to look through ground)
		//             camera creeps towards player. Prevent camera from zooming in here.


		//Zoom/fade
		float deltaZoom = Input.GetAxis ("Mouse ScrollWheel");
		magnitude += deltaZoom * zoomSensitivity;

	}
	void LateUpdate ()
	{
		transform.position = magnitude * direction + player.transform.position;
		transform.LookAt (player.transform.position);
	}
}