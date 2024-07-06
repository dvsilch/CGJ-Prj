using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    Vector3 direction;

    [ContextMenu("Set Position")]
    void SetPosition()
    {
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(transform, "Set Position");
#endif
        direction = LocationCalculator.GetDirection(transform.position);
        transform.SetPositionAndRotation(LocationCalculator.GetPosition(transform.position),
            Quaternion.FromToRotation(Vector3.up, direction));
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            Gizmos.DrawRay(transform.position, direction * 10);
    }
}
