using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray {
    public Vector3 origin;
    public Vector3 direction;

    public Ray(Vector3 origin, Vector3 direction) {
        this.origin = origin;
        this.direction = direction;
    }

    public Vector3 Extend(float t) {
        return origin + t * direction;
    }
}
