struct Intersection {
    float t = 1/0;
    float3 normal;
    float3 position;
    int matID;
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

struct Ray {
    float3 origin;
    float3 direction;

    float3 Extend(float t) {
        return origin + t * direction;
    }
};