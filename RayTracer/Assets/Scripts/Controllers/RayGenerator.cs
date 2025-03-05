using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ComputeHelper;
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
    [SerializeField] private bool useRayTracing, useScreenResolution;
    [SerializeField] private Color ambientColour;
    [SerializeField] private float ambientIntensity;

    [SerializeField] private ComputeShader rayTracer;
    [SerializeField] private Sphere[] spheres;
    [SerializeField] private Plane[] planes;
    [SerializeField] private Quadric[] quadrics;
    [SerializeField] private AABB[] boxes;

    [SerializeField] private Sphere dummySphere;
    [SerializeField] private Plane dummyPlane;
    [SerializeField] private Quadric dummyQuadric;
    [SerializeField] private AABB dummyAABB;

    private void Awake() {
        SetupScene(sceneInfo);
    }

    void Start() {
        if (useScreenResolution) {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }

        image = TextureFactory(resolution);
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
        SetParam(rayTracer, resolution, "resolution");
        rayTracer.SetTexture(0, "result", image);

        SetParam(rayTracer, lights.Length, "lightCount");
        SetParam(rayTracer, Mats.Length, "matCount");
        SetParam(rayTracer, spheres.Length, "sphereCount");
        SetParam(rayTracer, planes.Length, "planeCount");
        SetParam(rayTracer, quadrics.Length, "quadricCount");
        SetParam(rayTracer, boxes.Length, "AABBCount");

        spheres = spheres.Length == 0 ? new[] { dummySphere } : spheres;
        planes = planes.Length == 0 ? new[] { dummyPlane } : planes;
        quadrics = quadrics.Length == 0 ? new[] { dummyQuadric } : quadrics;
        boxes = boxes.Length == 0 ? new[] { dummyAABB } : boxes;

        CreateAndSetBuffer(rayTracer, "TraceRays", "spheres", spheres);
        CreateAndSetBuffer(rayTracer, "TraceRays", "planes", planes);
        CreateAndSetBuffer(rayTracer, "TraceRays", "quadrics", quadrics);
        CreateAndSetBuffer(rayTracer, "TraceRays", "boxes", boxes);
        CreateAndSetBuffer(rayTracer, "TraceRays", "mats", Mats);
        CreateAndSetBuffer(rayTracer, "TraceRays", "lights", lights);
        
        SetParam(rayTracer, ambientIntensity, "ambientIntensity");
        SetParam(rayTracer, ambientColour, "ambientColour");
    }
}
