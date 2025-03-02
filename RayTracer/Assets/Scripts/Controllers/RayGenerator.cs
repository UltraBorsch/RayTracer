using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static ComputeHelper;
using static Structs;
using static SceneManager;

public class RayGenerator : MonoBehaviour {
    [SerializeField] private SceneInfo sceneInfo;

    [SerializeField] private Camera cam;
    [SerializeField] private CameraController camController;
    [SerializeField] private Vector2Int resolution;
    [SerializeField] private float aspect;
    [SerializeField] private RenderTexture image; //make rendertexture for to use in shader?
    [SerializeField] private RayTracerLight[] lights;
    [SerializeField] private Vector3? prevPos = null, prevFor = null;
    [SerializeField] private bool useRayTracing = false;
    [SerializeField] private Color ambientColour;
    [SerializeField] private float ambientIntensity;

    [SerializeField] private ComputeShader rayTracer;
    [SerializeField] private Sphere[] spheres;

    private RenderTexture TextureFactory() {
        RenderTexture temp = new(resolution.x, resolution.y, 0) {
            enableRandomWrite = true,
            graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat,
            autoGenerateMips = false,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        temp.Create();

        return temp;
    }

    private void Awake() {
        SetupScene(sceneInfo);
    }

    void Start() {
        resolution.x = Screen.width;
        resolution.y = Screen.height;
        image = TextureFactory();
        aspect = resolution.x / resolution.y;

        camController.useRayTracing = useRayTracing;
        camController.image = image;

        SetRenderVars();
    }

    // Update is called once per frame
    void Update() {
        if (useRayTracing && (prevPos != cam.transform.position || prevFor != cam.transform.forward)) {
            Render();
        }
        prevPos = cam.transform.position;
        prevFor = cam.transform.forward;
    }

    private void Render() {
        //only change if our viewing angle has changed since the last call
        if (prevFor != cam.transform.forward) {
            //d is (i think) focal length. near/far planes not considered yet
            Vector3 camDirection = -cam.transform.forward;
            float d = 1f;
            float top = d * Mathf.Tan(0.5f * Mathf.PI * cam.fieldOfView / 180f);
            float left = cam.aspect * top;

            Vector3 u = Vector3.Cross(cam.transform.up, camDirection).normalized;
            Vector3 v = Vector3.Cross(camDirection, u);

            SetParam(rayTracer, camDirection, "camDirection");
            SetParam(rayTracer, d, "d");
            SetParam(rayTracer, top, "top");
            SetParam(rayTracer, -left, "right");
            SetParam(rayTracer, left, "left");
            SetParam(rayTracer, -top, "bottom");
            SetParam(rayTracer, u, "u");
            SetParam(rayTracer, v, "v");
        }

        //only change if the cams position has changed since the last call
        if (prevPos != cam.transform.position)
            SetParam(rayTracer, cam.transform.position, "camPosition");

        Run(rayTracer, "TraceRays", resolution.x, resolution.y);
    }
    
    private void SetRenderVars() {
        //static variables that shouldnt change during runtime (at least for the purpose of this application).
        //  resolution, the geometry buffers, the output image, ambient vars, etc
        SetParam(rayTracer, new[] { resolution.x, resolution.y }, "resolution");
        rayTracer.SetTexture(0, "result", image);
        CreateAndSetBuffer(rayTracer, "TraceRays", "spheres", spheres);
        CreateAndSetBuffer(rayTracer, "TraceRays", "mats", Mats);
        CreateAndSetBuffer(rayTracer, "TraceRays", "lights", lights);
        SetParam(rayTracer, ambientIntensity, "ambientIntensity");
        SetParam(rayTracer, ambientColour, "ambientColour");

        SetParam(rayTracer, lights.Length, "lightCount");
        SetParam(rayTracer, Mats.Length, "matCount");
        SetParam(rayTracer, spheres.Length, "sphereCount");
    }
}
