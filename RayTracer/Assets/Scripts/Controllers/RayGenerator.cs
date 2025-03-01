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
    [SerializeField] private Sphere[] geometry;
    [SerializeField] private RayTracerLight[] lights;
    [SerializeField] private Vector3 prevPos = new(), prevFor = new();
    [SerializeField] private bool useRayTracing = false;
    [SerializeField] private Color ambientColour;
    [SerializeField] private float ambientIntensity;
    
    [SerializeField] private ComputeShader rayTracer;
    private SphereStruct[] spheres;
    private Mat[] mats;
    private LightInfo[] lightStructs;
    private ComputeBuffer sphereBuffer, matBuffer, lightBuffer;

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

    private T[] CreateStructArray<T>(IStructable<T>[] arr) where T : struct {
        T[] structs = new T[arr.Length];
        for (int i = 0; i < structs.Length; i++) {
            arr[i].SetupStruct();
            structs[i] = arr[i].Struct;
        }
        return structs;
    }

    private void Awake() {
        SetupScene(sceneInfo);
    }

    void Start() {
        spheres = CreateStructArray(geometry);
        lightStructs = CreateStructArray(lights);
        //Debug.Log(spheres[0].matId);
        mats = CreateStructArray(Mats);

        resolution.x = Screen.width;
        resolution.y = Screen.height;
        image = TextureFactory();
        aspect = resolution.x / resolution.y;

        camController.useRayTracing = useRayTracing;
        camController.image = image;

        SetRendarVars();
    }

    // Update is called once per frame
    void Update() {
        if (useRayTracing && (prevPos != cam.transform.position || prevFor != cam.transform.forward)) {
            prevPos = cam.transform.position;
            prevFor = cam.transform.forward;
            Render();
        }
        
    }

    private void Render() {
        //only change if our viewing angle has changed since the last call
        //if (prevFor != cam.transform.forward) {
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
        //}

        //only change if the cams position has changed since the last call
        //if (prevPos != cam.transform.position)
            SetParam(rayTracer, cam.transform.position, "camPosition");

        Run(rayTracer, "TraceRays", resolution.x, resolution.y);
    }
    
    private void SetRendarVars() {
        //static variables that shouldnt change during runtime (at least for the purpose of this application).
        //  resolution, the geometry buffers, the output image, ambient vars, etc
        SetParam(rayTracer, new[] { resolution.x, resolution.y }, "resolution");
        rayTracer.SetTexture(0, "result", image);
        sphereBuffer = CreateAndSetBuffer(rayTracer, "TraceRays", "geometry", spheres);
        matBuffer = CreateAndSetBuffer(rayTracer, "TraceRays", "mats", mats);
        lightBuffer = CreateAndSetBuffer(rayTracer, "TraceRays", "lights", lightStructs);
        SetParam(rayTracer, ambientIntensity, "ambientIntensity");
        SetParam(rayTracer, ambientColour, "ambientColour");

        SetParam(rayTracer, lights.Length, "lightCount");
        SetParam(rayTracer, mats.Length, "matCount");
        SetParam(rayTracer, spheres.Length, "geometryCount");
    }

    void OnDestroy() {
        Release(sphereBuffer);
        Release(matBuffer);
        Release(lightBuffer);
    }
}
