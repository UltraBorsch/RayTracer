using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using static SceneManager;

public class Sphere : Geometry<SphereStruct> {
    [SerializeField] private float radius;

    public override SphereStruct GetStruct() {
        return new(radius, transform.position, Mats.Length == 0 ? 0 : GetMatId(Mats[0]));
    }
}
