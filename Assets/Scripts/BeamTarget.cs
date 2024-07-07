using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTarget : MonoBehaviour
{
    [Header("目标")]
    public Transform Target;

	[Header("光线宽度")]
    public float BeamWidth = 1.0f;

    LineRenderer _lr;

    [SerializeField, Header("光线用的材质")]
    Material _lineMat;

    private void Start() {
        _lr = new GameObject("beam").AddComponent<LineRenderer>();
        _lr.positionCount = 2;
        _lr.useWorldSpace = true;
        _lr.gameObject.layer = LayerMask.NameToLayer("FX");
        _lr.sharedMaterial = _lineMat;
        _lr.startWidth = _lr.endWidth = BeamWidth;
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, transform.position);
    }

    public void Shoot(Transform t)
    {
        Target = t;
        _lr.enabled = true;
    }

    public void Stop()
    {
        _lr.enabled = false;
    }

    private void Update() {
        if(_lr == null || Target == null)
            return;
        
        if(BeamWidth != _lr.endWidth)
            _lr.startWidth = _lr.endWidth = BeamWidth;

        _lr.SetPosition(1, Target.position);
    }

    private void OnDestroy() {
        if(_lr)
            Destroy(_lr.gameObject);
    }
}
