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
    [SerializeField] private RenderTexture image, empty;
    [SerializeField] private RayTracerLight[] lights;
    [SerializeField] private Vector3? prevPos = null, prevFor = null;
    [SerializeField] private bool useRayTracing, useScreenResolution;
    [SerializeField] private Color ambientColour;
    [SerializeField] private float ambientIntensity;
    [SerializeField] private float samples;

    [SerializeField] private ComputeShader rayTracer;
    [SerializeField] private Sphere[] spheres;
    [SerializeField] private Plane[] planes;
    [SerializeField] private Quadric[] quadrics;
    [SerializeField] private AABB[] boxes;
    [SerializeField] private Mesh[] meshes;

    [SerializeField] private Sphere dummySphere;
    [SerializeField] private Plane dummyPlane;
    [SerializeField] private Quadric dummyQuadric;
    [SerializeField] private AABB dummyAABB;
    [SerializeField] private Mesh dummyMesh;

    private void Awake() {
        SetupScene(sceneInfo);
    }

    void Start() {
        if (useScreenResolution) {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }

        image = TextureFactory(resolution);
        empty = TextureFactory(resolution);
        aspect = resolution.x / resolution.y;

        camController.useRayTracing = useRayTracing;
        camController.image = image;

        foreach (Mesh mesh in meshes) {
            mesh.Setup();
        }

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

        CopyRenderTexture(empty, image);
        Run(rayTracer, "TraceRays", resolution.x, resolution.y, (int)(samples * samples));
        Run(rayTracer, "AAReduction", resolution.x, resolution.y, 64);
    }
    
    private void SetRenderVars() {
        //static variables that shouldnt change during runtime (at least for the purpose of this application).
        //  resolution, the geometry buffers, the output image, ambient vars, etc
        float[] res = new[] { resolution.x, resolution.y, 1.0f / resolution.x, 1.0f / resolution.y };
        SetParam(rayTracer, res, "resolution");
        //rayTracer.SetTexture(0, "result", image);
        //rayTracer.SetTexture(1, "result", image);
        rayTracer.SetTexture(2, "result", image);

        //Set all the buffers/counts. Note that i cannot pass an empty buffer, and i assume trying to access a non-set buffer is undefined in some way,
        //so I add a dummy value that will never be accessed before setting the empty ones.
        SetParam(rayTracer, lights.Length, "lightCount");
        SetParam(rayTracer, Mats.Length, "matCount");
        SetParam(rayTracer, spheres.Length, "sphereCount");
        SetParam(rayTracer, planes.Length, "planeCount");
        SetParam(rayTracer, quadrics.Length, "quadricCount");
        SetParam(rayTracer, boxes.Length, "AABBCount");
        SetParam(rayTracer, meshes.Length, "meshCount");

        spheres = spheres.Length == 0 ? new[] { dummySphere } : spheres;
        planes = planes.Length == 0 ? new[] { dummyPlane } : planes;
        quadrics = quadrics.Length == 0 ? new[] { dummyQuadric } : quadrics;
        boxes = boxes.Length == 0 ? new[] { dummyAABB } : boxes;
        if (meshes.Length == 0) {
            meshes = new[] { dummyMesh };
            vertices.Add(new(0, 0, 0));
            normals.Add(new(0, 0, 0));
            triangles.AddRange(new[] {0, 0, 0});
        }

        CreateAndSetBuffer(rayTracer, "TraceRays", "spheres", spheres);
        CreateAndSetBuffer(rayTracer, "TraceRays", "planes", planes);
        CreateAndSetBuffer(rayTracer, "TraceRays", "quadrics", quadrics);
        CreateAndSetBuffer(rayTracer, "TraceRays", "boxes", boxes);
        CreateAndSetBuffer(rayTracer, "TraceRays", "meshes", meshes);
        CreateAndSetBuffer(rayTracer, "TraceRays", "mats", Mats);
        CreateAndSetBuffer(rayTracer, "TraceRays", "lights", lights);

        //ambient lighting
        SetParam(rayTracer, ambientIntensity, "ambientIntensity");
        SetParam(rayTracer, ambientColour, "ambientColour");

        //SSAA buffer
        int AABufferCount = (int)(resolution.x * resolution.y * samples * samples);
        ComputeBuffer AABuffer = CreateBuffer<Vector4>(AABufferCount);
        SetBuffer(rayTracer, "TraceRays", "AAResults", AABuffer);
        //SetBuffer(rayTracer, "CorrectImage", "AAResults", AABuffer);
        SetBuffer(rayTracer, "AAReduction", "AAResults", AABuffer);

        //SSAA params
        SetParam(rayTracer, samples, "samples");
        SetParam(rayTracer, 1f / samples, "sampleInv");
        SetParam(rayTracer, 1f / (samples + 1), "nextSampleInv");
        SetParam(rayTracer, samples * samples, "sampleSquare");
        SetParam(rayTracer, 1f / (samples * samples), "sampleSquareInv");

        //Set mesh data buffers
        CreateAndSetBuffer(rayTracer, "TraceRays", "vertices", vertices.ToArray());
        CreateAndSetBuffer(rayTracer, "TraceRays", "triangles", triangles.ToArray());
        CreateAndSetBuffer(rayTracer, "TraceRays", "normals", normals.ToArray());
    }
}
