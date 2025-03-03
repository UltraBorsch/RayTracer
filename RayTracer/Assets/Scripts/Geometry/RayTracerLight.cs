using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class RayTracerLight : MonoBehaviour , IStructable<LightInfo> {
    [SerializeField] new private Light light;

    public LightInfo GetStruct() {
        return new(light.intensity, light.type == LightType.Directional, light.color, transform.forward, transform.position);
    }
}
