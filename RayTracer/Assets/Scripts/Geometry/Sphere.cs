using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;
using static SceneManager;

public class Sphere : Geometry<SphereStruct> {
    public float radius;

    public override void Intersect(Ray ray, Intersection intersection) {
        float b = 2 * Vector3.Dot(ray.direction, ray.origin - transform.position);
        float c = Vector3.SqrMagnitude(ray.origin - transform.position) - radius * radius;

        float discriminant = b * b - 4 * c;

        if (discriminant >= 0) {
            float sqrt = Mathf.Sqrt(discriminant);
            //float t1 = (-b + sqrt) / 2;
            float t2 = (-b - sqrt) / 2;

            if (!(t2 >= intersection.t || t2 <= Constants.EPSILON)) {
                Vector3 n = Vector3.Normalize(ray.Extend(t2) - transform.position);
                intersection.t = t2;
                intersection.normal = n;
                intersection.position = ray.Extend(t2);
                intersection.mat = Mats[0];
            }
        }
    }

    public override SphereStruct GetStruct() {
        return new(radius, transform.position, GetMatId(Mats[0]));
    }
}
