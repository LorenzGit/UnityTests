using UnityEngine;
using System.Collections;

public class CameraDragRotateZoom : MonoBehaviour {

	private bool _oneTouch;
	private bool _twoTouch;
	private int _rotation;
	private float _twoTouchDistance;
	private float _twoTouchAngle;
	private Vector3 _v3MousePosition;
	private Plane plane = new Plane(Vector3.up, Vector3.zero);
	private int touchCount = 0;

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.touchSupported) {
			if(touchCount != Input.touchCount){
				touchCount = Input.touchCount;
				if(touchCount != 0){
					_v3MousePosition = getRayPosition();
					_oneTouch = true;
					if(touchCount == 2){
						_twoTouch = true;
						_twoTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
						_twoTouchAngle = getTwoFingerAngle();
					}
					else{
						_twoTouch = false;
					}
				}
				else{
					_oneTouch = false;
					_twoTouch = false;
				}
			}
		}
		else{
			if (Input.GetMouseButtonDown (0)) {
				if(!_oneTouch){
					_oneTouch = true;
				}
				_v3MousePosition = getRayPosition();
			}
			else if (Input.GetMouseButtonUp (0)) {
				if(_oneTouch){
					_oneTouch = false;
				}
			}
		}

		if (_oneTouch) {
			transform.position -= getRayPosition() - _v3MousePosition;
		}
		if (_twoTouch) {
			float newTwoTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
			zoom ((newTwoTouchDistance - _twoTouchDistance) * 0.01f );
			_twoTouchDistance = newTwoTouchDistance;

			float newTwoTouchAngle = getTwoFingerAngle();
			transform.RotateAround (getRayPosition(), new Vector3(0.0f,1.0f,0.0f), newTwoTouchAngle-_twoTouchAngle);
			_twoTouchAngle = newTwoTouchAngle;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			zoom (1);
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			zoom (-1);
		}

		/*if (Input.GetMouseButtonDown (0)) {
            Debug.Log ("Mouse Pressed left click.");
            if(Input.touchSupported){
                Debug.Log("FINGERS "+Input.touchCount);
                Debug.Log("FINGER " +Input.touches[Input.touchCount-1].fingerId);
                Debug.Log("FINGER " +Input.touches[0].fingerId);
                fingerId = Input.touches[Input.touchCount-1].fingerId;
            }
            v3OrgMouse = getRayPosition();
            if(!oneTouchDown){*/


		/*Ray ray = getRay();
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100f)) {
			GameObject objectCollider = hit.collider.gameObject;
			//objectCollider.transform.Translate(1,0,0);
		}
		else{
			Debug.Log ("No Object clicked");
		}*/

		//oneTouchDown = true;
		//}

		/*if(!twoTouchDown && Input.touchCount>=2){
                                                                initialZoomDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                                                                twoTouchDown = true;
                                                }*/
		/*}
		else if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Mouse Up");
			//v3OrgMouse = getRayPosition();
			//if(Input.touchCount<=2){
			if(Input.touchCount<=1){
				oneTouchDown = false;
			}
			//twoTouchDown = false;
			//}
		}
		else if (oneTouchDown) {*/
			/*rotation++;
			float radians = rotation * (Mathf.PI/180);
			Vector3 pos = this.transform.position;
			pos.x = Mathf.Sin(radians)*20;
			pos.z = Mathf.Cos(radians)*20;
			this.transform.position = pos;
			this.transform.LookAt(new Vector3(0,0,0));*/

			//Debug.Log(getRayPosition());
			//Debug.Log(v3OrgMouse);
			//transform.position -= getRayPosition() - v3OrgMouse;
			//v3OrgMouse = getRayPosition();

			/*if (twoTouchDown) {
                zoom ((Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - initialZoomDistance) * 0.1f );
                initialZoomDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }*/
			//}
			//else{

			//}
		}

		float getTwoFingerAngle(){
			Vector2 v2 = Input.GetTouch(0).position - Input.GetTouch(1).position;
			float angle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg;
			return angle;
			//return Vector2.Angle(Input.GetTouch(0).position, Input.GetTouch(1).position);
		}

		void zoom(float delta){
			Vector3 rayPosition = getRayPosition();
			Vector3 initialPosition = Camera.main.transform.position;
			Camera.main.transform.Translate (Vector3.forward * delta * Camera.main.transform.position.y * 0.2f);
			//Debug.Log(Camera.main.transform.position.y);
			if(Camera.main.transform.position.y < 4 || Camera.main.transform.position.y>25){
				Camera.main.transform.position = initialPosition;
			}
			transform.position -= getRayPosition() - rayPosition;
		}

		Vector3 getRayPosition(){
			Ray ray = getRay ();
			// Detect at what distance the ray meets the plane.
			float dist;
			plane.Raycast(ray, out dist);
			// Get The Vector3 of that point.
			Vector3 pos = ray.GetPoint (dist);
			//Set Y to 0 to avoid camera moving on the 0 parameter.
			return pos;
		}

		Ray getRay(){
			// Generate Ray that start from camera focus point to point clicked in space.
			return Camera.main.ScreenPointToRay (getMousePosition());
		}

		Vector2 getMousePosition(){
			return Input.mousePosition;
		}
	}