using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using static SceneManager;

public class Sphere : Geometry<SphereStruct> {
    public float radius;

    public override SphereStruct GetStruct() {
        return new(radius, transform.position, GetMatId(Mats[0]));
    }
}
