using UnityEngine;

public class GizmosTest : MonoBehaviour {
    private float _gizmoValue = 0f;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f );
        Gizmos.color = Color.white; //reset
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        _gizmoValue += 0.05f;
        Vector3 pos = transform.position;
        pos.x += Mathf.Sin( _gizmoValue );
        pos.y += Mathf.Cos( _gizmoValue );
        pos.z += Mathf.Cos( _gizmoValue ) * Mathf.Sin( _gizmoValue );
        Gizmos.DrawSphere( pos, 0.1f );

        //draw a line
        Gizmos.DrawLine( transform.position, Vector3.zero );
    }
}
