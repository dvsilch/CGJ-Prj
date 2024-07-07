using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteCheck : MonoBehaviour
{
    BeamTarget _beam;

    public void GrabBeamAndRun()
    {
        if(_beam == null)
            _beam = FindObjectOfType<BeamTarget>();
        if(!_beam)
        {
            Debug.LogError("can't find beamTarget component in scene");
            return;
        }
        _beam.Shoot(gameObject.transform);
    }

    public void StopBeam()
    {
        if(_beam == null)
            _beam = FindObjectOfType<BeamTarget>();
        _beam.Stop();
    }
}
