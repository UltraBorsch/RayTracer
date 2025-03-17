#include "Constants.cginc"

struct Ray {
    float3 origin;
    float3 direction;
    float3 invDirection;
    bool3 negativeDir;

    float3 Extend (float t) {
        return origin + t * direction;
    }
    
    void Set (float3 o, float3 d) {
        origin = o;
        direction = d;
        invDirection = 1.0 / d;
        negativeDir = d < 0;
    }
};

Ray GenerateRay (float3 o, float3 d) {
    Ray ray;
    ray.origin = o;
    ray.direction = d;
    ray.invDirection = 1.0 / d;
    ray.negativeDir = d < 0;
    return ray;
}

struct Intersection {
    float t;
    float3 normal;
    float3 position;
    int matId;
    
    void Reset () {
        t = INF;
    }
};

Intersection GenerateIntersection (float t, float3 normal, float3 position, int matId) {
    Intersection intersection;
    intersection.t = t;
    intersection.normal = normal;
    intersection.position = position;
    intersection.matId = matId;
    return intersection;
}

Intersection GenerateIntersection () {
    return GenerateIntersection(INF, float3(0, 0, 0), float3(0, 0, 0), 0);
}

struct Mat {
    uint hardness;
    float4 diffuseColour;
    float4 specularColour;
};

struct Light {
    float intensity;
    int directional;
    float4 lightColour;
    float3 direction, position;
    float3 attenuation;
};