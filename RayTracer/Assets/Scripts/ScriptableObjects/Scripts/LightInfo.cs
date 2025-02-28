using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

[CreateAssetMenu(menuName = "ScriptableObjects/Light")]
public class LightingInfo : ScriptableObject {
    public float intensity;
    public bool directional;
    public Color lightColour;
    public Vector3 direction;
}
