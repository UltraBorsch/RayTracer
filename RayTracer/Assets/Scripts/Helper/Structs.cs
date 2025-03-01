using UnityEngine;

public static class Structs {
    public struct Mat {
        public uint hardness;
        public Color diffuseColour;
        public Color specularColour;

        public Mat(uint hardness, Color diffuse, Color specular) {
            this.hardness = hardness;
            diffuseColour = diffuse;
            specularColour = specular;
        }
    }

    public struct LightInfo {
        public float intensity;
        public int directional;
        public Color lightColour;
        public Vector3 direction;
        public Vector3 position;

        public LightInfo(float intensity, bool directional, Color lightColour, Vector3 direction, Vector3 position) {
            this.intensity = intensity;
            this.directional = directional ? 1 : 0;
            this.lightColour = lightColour;
            this.direction = direction;
            this.position = position;
        }
    }

    public struct SphereStruct {
        public float radius;
        public Vector3 center;
        public int matId;

        public SphereStruct(float radius, Vector3 center, int matId) {
            this.radius = radius;
            this.center = center;
            this.matId = matId;
        }
    }
}