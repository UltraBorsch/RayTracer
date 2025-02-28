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
        public bool directional;
        public Color lightColour;
        public Vector3 direction;
        public Vector3 position;

        public LightInfo(float intensity, bool directional, Color lightColour, Vector3 direction, Vector3 Position) {
            this.intensity = intensity;
            this.directional = directional;
            this.lightColour = lightColour;
            this.direction = direction;
            this.position = Position;
        }
    }

    public struct SphereStruct {
        public float radius;
        public Vector3 center;
        public int matID;

        public SphereStruct(float radius, Vector3 center, int matID) {
            this.radius = radius;
            this.center = center;
            this.matID = matID;
        }
    }
}