using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCalculator : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;

    private static LocationCalculator instance;

    public static LocationCalculator Instance => instance == null ? instance = FindObjectOfType<LocationCalculator>() : instance;

    public static float Radius => Instance.planet.transform.localScale.x * 0.5f;

    public static Vector3 GetDirection(Vector3 worldPosition)
    {
        return worldPosition - Instance.planet.transform.position;
    }

    public static Vector3 GetPosition(Vector3 worldPosition)
    {
        return Instance.planet.transform.position + GetDirection(worldPosition).normalized * Radius;
    }
}
