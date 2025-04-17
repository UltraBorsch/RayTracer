#include "Structs.cginc"
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles

static float3x3 axisNormals = { float3(1, 0, 0), float3(0, 1, 0), float3(0, 0, 1) };

struct Sphere {
    float radius;
    float3 center;
    int matId;
};

struct AABB {
    float2x3 corners;
    int matId[3];
};

struct Plane {
    float3 coord;
    float3 normal;
    int2 matId;
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
};

struct Mesh {
    float2x3 corners;
    int vertStart, triStart, vertCount, triCount, matId;
};

Intersection CheckForIntersections (RWStructuredBuffer<Sphere> objs, int count, Intersection intersection, Ray ray) {
    float b, c, discriminant, root, t2;
    float3 vec, normal;
    for (int i = 0; i < count; i++) {
        Sphere sphere = objs[i];
        b = 2 * dot(ray.direction, ray.origin - sphere.center);
        vec = ray.origin - sphere.center;
        c = dot(vec, vec) - sphere.radius * sphere.radius;

        discriminant = b * b - 4 * c;

        if (discriminant >= EPSILON) {
            root = sqrt(discriminant);
            //float t1 = (-b + root) / 2;
            t2 = (-b - root) * 0.5;
            
            if (!(t2 >= intersection.t || t2 <= EPSILON)) {
                normal = normalize(ray.Extend(t2) - sphere.center);
                intersection = GenerateIntersection(t2, normal, ray.Extend(t2), sphere.matId);
            }
        }
    }
    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<AABB> objs, int count, Intersection intersection, Ray ray) {
    float tmin, tmax, mat, bmin, bmax, dmin, dmax;
    float3 normal;
    
    for (int j = 0; j < count; j++) {
        AABB box = objs[j];
        
        tmin = 0;
        tmax = intersection.t;
        
        for (int i = 0; i < 3; i++) {
            bmin = box.corners[ray.negativeDir[i]][i];
            bmax = box.corners[!ray.negativeDir[i]][i];
            
            dmin = (bmin - ray.origin[i]) * ray.invDirection[i];
            dmax = (bmax - ray.origin[i]) * ray.invDirection[i];
            
            tmin = max(dmin, tmin);
            tmax = min(dmax, tmax);
            if (tmin == dmin) {
                normal = (ray.negativeDir[i] * 2 - 1) * axisNormals[i];
                mat = box.matId[i];
            }
        }
        
        if (!(tmin > tmax || tmin <= EPSILON))
            intersection = GenerateIntersection(tmin, normal, ray.Extend(tmin), mat);
    }
        
    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<Plane> objs, int count, Intersection intersection, Ray ray) {
    float t, denom;
    int total;
    float3 position;
    for (int i = 0; i < count; i++) {
        Plane plane = objs[i];

        denom = dot(plane.normal, ray.direction);
        t = dot(plane.coord - ray.origin, plane.normal) / denom;
        
        if (denom < -EPSILON && !(t > intersection.t || t <= EPSILON)) {
            position = ray.Extend(t);
            total = floor(position.x) + floor(position.z);
        
            intersection = GenerateIntersection(t, plane.normal, position, plane.matId[total & 1]);
        }
    }

    return intersection;
}

Intersection CheckForIntersections (RWStructuredBuffer<Quadric> objs, int count, Intersection intersection, Ray ray) {
    float a, b, c, t, discriminant, root, sol1, sol2, closest;
    float3 position;
    for (int i = 0; i < count; i++) {
        Quadric quad = objs[i];
        a = dot(float4(ray.direction, 1), mul(quad.params, float4(ray.direction, 1)));
        b = dot(float4(ray.origin, 1), mul(quad.params, float4(ray.direction, 1))) + dot(float4(ray.direction, 1), mul(quad.params, float4(ray.origin, 1)));
        c = dot(float4(ray.origin, 1), mul(quad.params, float4(ray.origin, 1)));
        
        discriminant = b * b - 4 * a * c;
        
        if (a <= EPSILON) {
            t = -c / b;
            position = ray.Extend(t);
            intersection = GenerateIntersection(t, quad.GetNormal(position), position, quad.matId);
        } else if (discriminant >= EPSILON) {
            root = sqrt(discriminant);
            sol1 = (-b + root) / (2 * a);
            sol2 = (-b - root) / (2 * a);
            //float closest = sol1 < sol2 ? sol1 : sol2; //sol2 is always the correct solution for the elliptic cones, so assuming that carries for all quadrics
            closest = sol2;
            
            if (closest >= 0 && !(closest >= intersection.t || closest <= EPSILON)) {
                position = ray.Extend(closest);
                intersection = GenerateIntersection(closest, quad.GetNormal(position), position, quad.matId);
            }
        }
    }
    
    return intersection;
}

//mesh intersection located in RayTracer.compute, since it was cumbersome to pass all the vert/tri/normal buffers into the method