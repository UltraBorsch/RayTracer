using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection {
    public float t = float.PositiveInfinity;
    public Vector3 normal = new();
    public Vector3 position = new();
    public MatInfo mat;
}
