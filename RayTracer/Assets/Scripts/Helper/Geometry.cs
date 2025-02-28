using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Geometry<T> : MonoBehaviour {
    protected T geoStruct;
    public virtual MatInfo[] Mats { get; set; }
    public abstract void Intersect(Ray ray, Intersection intersection);

    public T GetStruct() {
        return geoStruct;
    }
}
