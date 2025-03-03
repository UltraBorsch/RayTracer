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
                float3 normal = normalize(ray.Extend(t2) - center);
                intersection = GenerateIntersection(t2, normal, ray.Extend(t2), matId);
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
    int2 matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float denom = dot(normal, ray.direction);
        
        if (denom >= 0)
            return intersection;
        
        float t = dot(coord - ray.origin, normal) / denom;
        if (t >= intersection.t || t <= EPSILON)
            return intersection;
        
        float3 position = ray.origin + t * ray.direction;
        int total = floor(position.x) + floor(position.z);
        
        return GenerateIntersection(t, normal, position, matId[total & 1]);
    }
};

struct Quadric {
    float4x4 params;
    int matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float3 d = ray.direction;
        
        float a = mul(float4(ray.direction, 1), mul(params, float4(ray.direction, 1)));
        float b = mul(float4(ray.origin, 1), mul(params, float4(ray.direction, 1))) + mul(float4(ray.direction, 1), mul(params, float4(ray.origin, 1)));
        float c = mul(float4(ray.origin, 1), mul(params, float4(ray.origin, 1)));
        
        float discriminant = b * b - 4 * a * c;
        
        if (a == 0) {
            float t = -c / b;
            float3 normal = float3(0, 0, 0); //??????
            float3 position = ray.Extend(t);
            intersection = GenerateIntersection(t, normal, position, matId);
        } else if (discriminant >= 0) {
            float root = sqrt(discriminant);
            float sol1 = (-b + root) / (2 * a), sol2 = (-b - root) / (2 * a);
            float closest = sol1 < 0 || sol1 > sol2 ? sol2 : sol1;
            
            if (closest >= 0 && !(closest >= intersection.t || closest <= EPSILON)) {
                float3 position = ray.Extend(closest);
                float nx = mul(float4(2, 0, 0, 0), mul(params, float4(position, 1)));
                float ny = mul(float4(0, 2, 0, 0), mul(params, float4(position, 1)));
                float nz = mul(float4(0, 0, 2, 0), mul(params, float4(position, 1)));
                //float nx = 2 * (dot(position, params._11_12_23) + params._14);
                //float ny = 2 * (dot(position, params._21_22_23) + params._24);
                //float nz = 2 * (dot(position, params._31_32_33) + params._34);
                float3 normal = normalize(float3(nx, ny, nz));
                intersection = GenerateIntersection(closest, normal, position, matId);
            }
        }

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