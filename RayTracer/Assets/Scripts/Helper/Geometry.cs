using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Geometry : MonoBehaviour {
    public MatInfo[] mats;
    abstract public void Intersect(Ray ray, Intersection intersection);
}
