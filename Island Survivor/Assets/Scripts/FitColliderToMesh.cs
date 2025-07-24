using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class FitColliderToMesh : MonoBehaviour {
    void Awake() {
        var col   = GetComponent<CapsuleCollider>();
        var rend  = GetComponentInChildren<Renderer>();
        if (rend == null) return;

        var boundsW = rend.bounds;                         
        Vector3 centerL = transform.InverseTransformPoint(boundsW.center);
        Vector3 sizeW   = boundsW.size;

        col.direction = 1;                                 
        col.height    = sizeW.y;
        col.radius    = Mathf.Max(sizeW.x, sizeW.z) * 0.5f;
        col.center    = new Vector3(centerL.x, col.height * 0.5f, centerL.z);
    }
}