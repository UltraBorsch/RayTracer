#include "Constants.cginc"

struct Ray {
    float3 origin;
    float3 direction;

    float3 Extend (float t) {
        return origin + t * direction;
    }
};

Ray GenerateRay (float3 o, float3 d) {
    Ray ray;
    ray.origin = o;
    ray.direction = d;
    return ray;
}

struct Intersection {
    float t;
    float3 normal;
    float3 position;
    int matId;
};

Intersection GenerateIntersection() {
    Intersection intersection;
    intersection.t = INF;
    return intersection;
}

interface IIntersectable {
    Intersection Intersect (Ray ray, Intersection intersection);
};

struct Mat {
    uint hardness;
    float4 diffuseColour;
    float4 specularColour;
};

struct Light {
    float intensity;
    int directional;
    float4 lightColour;
    float3 direction;
    float3 position;
};