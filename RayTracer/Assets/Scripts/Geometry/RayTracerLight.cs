using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class RayTracerLight : MonoBehaviour , IStructable<LightInfo> {
    public LightingInfo info;
    [SerializeField] new private Light light;

    public LightInfo Struct { get; private set; }

    public void Start() {
        Struct = new(info.intensity, info.directional, info.lightColour, transform.forward, transform.position);
    }
}
