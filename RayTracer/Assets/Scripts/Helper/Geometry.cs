using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Geometry<T> : MonoBehaviour, IStructable<T> where T : struct {
    [SerializeField] private MatInfo[] mats;
    public virtual MatInfo[] Mats { get { return mats; } protected set { mats = value; } }

    public abstract T GetStruct();
}
