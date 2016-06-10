using UnityEngine;
using System.Collections;

public class KeyboardMovement : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if( Input.GetKeyDown( KeyCode.Space ) ) {
            //immeditale after button is pressed
        }

        //while button is pressed
        if( Input.GetKey( KeyCode.RightArrow ) ) {
            this.transform.Rotate(0f, 120f * Time.deltaTime, 0f); //rotates right
        }
        else if( Input.GetKey( KeyCode.LeftArrow ) ) {
            this.transform.Rotate( 0f, -120f * Time.deltaTime, 0f ); //rotates left
        }

	    if (Input.GetKey(KeyCode.UpArrow)) {
	        this.transform.Translate(0f, 0f, 4f * Time.deltaTime, Space.Self); //moves itself forward
	    }
        else if( Input.GetKey( KeyCode.DownArrow ) ) {
            this.transform.Translate( 0f, 0f, -4f * Time.deltaTime, Space.Self ); //moves itself backwards
        }

        if( Input.GetKeyUp( KeyCode.Space ) ) {
            //immediately after button is release
        }
	}
}
