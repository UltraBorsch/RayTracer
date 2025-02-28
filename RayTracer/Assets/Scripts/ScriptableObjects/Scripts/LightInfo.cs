using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

[CreateAssetMenu(menuName = "ScriptableObjects/Light")]
public class LightingInfo : MonoBehaviour {
    public float intensity;
    public bool directional;
    public Color lightColour;
    public Vector3 direction;

    private new LightInfo light;

    void Start() {
        light = new(intensity, directional, lightColour, direction, transform.position);
    }
}
