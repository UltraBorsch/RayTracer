#include "Structs.cginc"

struct Sphere {
    float radius;
    float3 center;
    int matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float b = 2 * dot(ray.direction, ray.origin - center);
        float3 vec = ray.origin - center;
        float c = dot(vec, vec) - radius * radius;

        float discriminant = b * b - 4 * c;

        if (discriminant >= 0) {
            float root = sqrt(discriminant);
            //float t1 = (-b + root) / 2;
            float t2 = (-b - root) / 2;

            if (!(t2 >= intersection.t || t2 <= EPSILON)) {
                float3 n = normalize(ray.Extend(t2) - center);
                intersection.t = t2;
                intersection.normal = n;
                intersection.position = ray.Extend(t2);
                intersection.matId = matId;
            }
        }
        return intersection;
    }
};

struct AABB {
    float3 center;
    float width, height, length;
    int matId[3];
    
    Intersection Intersect (Ray ray, Intersection intersection) {

        return intersection;
    }
};

struct Plane {
    float3 coord;
    float3 normal;
    int matId[2];
    
    Intersection Intersect (Ray ray, Intersection intersection) {

        return intersection;
    }
};

struct Quadric {
    float4x4 params;
    int matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {

        return intersection;
    }
};

Intersection CheckForIntersections (RWStructuredBuffer<Sphere> objs, int count, Intersection intersection, Ray ray) {
    for (int i = 0; i < count; i++) 
        intersection = objs[i].Intersect(ray, intersection);
    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<AABB> objs, int count, Intersection intersection, Ray ray) {
    for (int i = 0; i < count; i++) 
        intersection = objs[i].Intersect(ray, intersection);
    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<Plane> objs, int count, Intersection intersection, Ray ray) {
    for (int i = 0; i < count; i++) 
        intersection = objs[i].Intersect(ray, intersection);
    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<Quadric> objs, int count, Intersection intersection, Ray ray) {
    for (int i = 0; i < count; i++) 
        intersection = objs[i].Intersect(ray, intersection);
    return intersection;
}