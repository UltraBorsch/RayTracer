#include "Structs.cginc"
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles

struct Sphere {
    float radius;
    float3 center;
    int matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float b = 2 * dot(ray.direction, ray.origin - center);
        float3 vec = ray.origin - center;
        float c = dot(vec, vec) - radius * radius;

        float discriminant = b * b - 4 * c;

        if (discriminant >= EPSILON) {
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
    float2x3 corners;
    int matId[3];
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float tmin = 0;
        float tmax = intersection.t;
        float mat;
        float3x3 normals = { float3(1, 0, 0), float3(0, 1, 0), float3(0, 0, 1) };
        float3 normal;
        
        for (int i = 0; i < 3; i++) {
            float bmin = corners[ray.negativeDir[i]][i];
            float bmax = corners[!ray.negativeDir[i]][i];
            
            float dmin = (bmin - ray.origin[i]) * ray.invDirection[i];
            float dmax = (bmax - ray.origin[i]) * ray.invDirection[i];
            
            tmin = max(dmin, tmin);
            tmax = min(dmax, tmax);
            if (tmin == dmin) {
                normal = (ray.negativeDir[i] * 2 - 1) * normals[i];
                mat = matId[i];
            }
        }
        
        if (tmin > tmax || tmin <= EPSILON)
            return intersection;
        return GenerateIntersection(tmin, normal, ray.Extend(tmin), mat);
    }
};

struct Plane {
    float3 coord;
    float3 normal;
    int2 matId;
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float denom = dot(normal, ray.direction);
        
        if (denom >= -EPSILON)
            return intersection;
        
        float t = dot(coord - ray.origin, normal) / denom;
        if (t >= intersection.t || t <= EPSILON)
            return intersection;
        
        float3 position = ray.Extend(t);
        int total = floor(position.x) + floor(position.z);
        
        return GenerateIntersection(t, normal, position, matId[total & 1]);
    }
};

struct Quadric {
    float4x4 params;
    int matId;
    
    float3 GetNormal (float3 position) {
        float4 v2 = mul(params, float4(position, 1));
        float nx = dot(float4(2, 0, 0, 0), v2);
        float ny = dot(float4(0, 2, 0, 0), v2);
        float nz = dot(float4(0, 0, 2, 0), v2);
        return normalize(float3(nx, ny, nz));
    }
    
    Intersection Intersect (Ray ray, Intersection intersection) {
        float a = dot(float4(ray.direction, 1), mul(params, float4(ray.direction, 1)));
        float b = dot(float4(ray.origin, 1), mul(params, float4(ray.direction, 1))) + dot(float4(ray.direction, 1), mul(params, float4(ray.origin, 1)));
        float c = dot(float4(ray.origin, 1), mul(params, float4(ray.origin, 1)));
        
        float discriminant = b * b - 4 * a * c;
        
        if (a <= EPSILON) {
            float t = -c / b;
            float3 position = ray.Extend(t);
            intersection = GenerateIntersection(t, GetNormal(position), position, matId);
        } else if (discriminant >= EPSILON) {
            float root = sqrt(discriminant);
            float sol1 = (-b + root) / (2 * a), sol2 = (-b - root) / (2 * a);
            //float closest = sol1 < sol2 ? sol1 : sol2; //sol2 is always the correct solution for the elliptic cones, so assuming that carries for all quadrics
            float closest = sol2;
            
            if (closest >= 0 && !(closest >= intersection.t || closest <= EPSILON)) {
                float3 position = ray.Extend(closest);
                intersection = GenerateIntersection(closest, GetNormal(position), position, matId);
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