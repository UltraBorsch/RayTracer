using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

public class RayTracerLight : MonoBehaviour {
    public LightingInfo info;
    [SerializeField] new private Light light;

    private LightInfo lightStruct;

    public void Start() {
        lightStruct = new(info.intensity, info.directional, info.lightColour, transform.forward, transform.position);
    }
}
