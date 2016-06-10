using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {
    private Vector3 _targetPosition = Vector3.zero;
    private float _speed = 10f;
    private float _gravity = 50f;
    private CharacterController _cc;

    // Use this for initialization
	void Start () {
	    _cc = this.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0)) {
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hitInfo;
	        if (Physics.Raycast(ray, out hitInfo)) {
	            if (hitInfo.transform.gameObject.name == "Terrain") {
                    _targetPosition = hitInfo.point;
                }
	        }
	    }

	    if (_targetPosition != Vector3.zero) {
	        if (Vector3.Distance(transform.position, _targetPosition) > 1f) {
	            Vector3 lookAt = _targetPosition;
	            lookAt.y = transform.position.y;
                transform.LookAt(lookAt);

	            float step = _speed*Time.deltaTime;
	            Vector3 moveDirection = transform.forward*step;
	            moveDirection.y -= _gravity*Time.deltaTime;
	            _cc.Move(moveDirection);
	        }
	        else {
                _targetPosition = Vector3.zero;
            }
	    }

	}
}
