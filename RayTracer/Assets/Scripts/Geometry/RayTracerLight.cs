using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class RayTracerLight : MonoBehaviour , IStructable<LightInfo> {
    [SerializeField] new private Light light;
    [SerializeField] private Vector3 attenuation;

    public LightInfo GetStruct() {
        return new(light.intensity, light.type == LightType.Directional, light.color, transform.forward, transform.position, attenuation);
    }
}
