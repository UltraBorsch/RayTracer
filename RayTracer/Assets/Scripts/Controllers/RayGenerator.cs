using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static ComputeHelper;

public class RayGenerator : MonoBehaviour {
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2Int resolution;
    [SerializeField] private float aspect;
    [SerializeField] private Texture2D image; //make rendertexture for to use in shader?
    [SerializeField] private Geometry[] geometry;
    [SerializeField] private RayTracerLight[] lights;
    [SerializeField] private Vector3 prevPos = new(), prevFor = new();
    [SerializeField] private bool useRayTracing = false;
    [SerializeField] private Color ambientColor;
    [SerializeField] private float ambientIntensity;
    [SerializeField] private ComputeShader rayTracer;

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

    void Start() {
        resolution.x = Screen.width;
        resolution.y = Screen.height;
        image = new(resolution.x, resolution.y) {
            wrapMode = TextureWrapMode.Clamp,
            anisoLevel = 0,
            filterMode = FilterMode.Point
        };
        aspect = resolution.x / resolution.y;

    }

    // Update is called once per frame
    void Update() {
        if (useRayTracing && (prevPos != cam.transform.position || prevFor != cam.transform.forward)) {
            prevPos = cam.transform.position;
            prevFor = cam.transform.forward;
            Render();
            image.Apply();
        }
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (useRayTracing) { //if rendering raytracer
            Graphics.Blit(image, destination);
        } else { //else use normal rendering
            Graphics.Blit(source, destination);
        }
    }

    private void Render() {
        //d is (i think) focal length. near/far planes not considered yet
        Vector3 cam_dir = -cam.transform.forward;
        float d = 1f;
        float top = d * Mathf.Tan(0.5f * Mathf.PI * cam.fieldOfView / 180f);
        float right = cam.aspect * top;
        float bottom = -top;
        float left = -right;

        Vector3 w = cam_dir;
        Vector3 u = Vector3.Cross(cam.transform.up, w);
        u = Vector3.Normalize(u);
        Vector3 v = Vector3.Cross(w, u);

        for (int i = 0; i < resolution.x; i++) {
            for (int j = 0; j < resolution.y; j++) {
                float x = i + 0.5f;
                float y = j + 0.5f;

                float su = left + (right - left) * x / resolution.x;
                float sv = bottom + (top - bottom) * y / resolution.y;
                Vector3 dir = Vector3.Normalize(su * u + sv * v - d * w);

                Ray ray = new(cam.transform.position, dir);
                Intersection intersection = new();

                foreach (Geometry geo in geometry) 
                    geo.Intersect(ray, intersection);

                Color colour = Color.black;
                if (intersection.t < float.PositiveInfinity) {
                    foreach (RayTracerLight light in lights) {
                        Vector3 lightDir;
                        if (light.info.directional)
                            lightDir = -light.info.direction;
                        else
                            lightDir = Vector3.Normalize(light.transform.position - intersection.position);
                        //check if point is in shadow
                        //if not, lambertian, phong, attenuation, etc
                        Color lambertian = light.info.intensity * light.info.lightColour * intersection.mat.diffuseColour * Mathf.Max(0f, Vector3.Dot(intersection.normal, lightDir));

                        Vector3 h = Vector3.Normalize(-dir + lightDir);
                        Color bp = light.info.intensity * light.info.lightColour * intersection.mat.specularColour * Mathf.Pow(Mathf.Max(0f, Vector3.Dot(intersection.normal, h)), intersection.mat.hardness);

                        colour += lambertian + bp;
                    }

                    colour += ambientIntensity * ambientColor;
                    
                }
                image.SetPixel(i, j, colour);
                

            }
        }
    }
}
