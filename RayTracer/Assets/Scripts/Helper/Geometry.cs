using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Geometry<T> : MonoBehaviour, IStructable<T> where T : struct {
    public virtual MatInfo[] Mats { get; set; }
    public abstract void Intersect(Ray ray, Intersection intersection);

    public T Struct { get; protected set; }
}
