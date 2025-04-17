using UnityEngine;
using static ComputeStructs;

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
        public Vector3 direction, position;
        public Vector3 attenuation;

        public LightInfo(float intensity, bool directional, Color lightColour, Vector3 direction, Vector3 position, Vector3 attenuation) {
            this.intensity = intensity;
            this.directional = directional ? 1 : 0;
            this.lightColour = lightColour;
            this.direction = direction;
            this.position = position;
            this.attenuation = attenuation;
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

    public struct PlaneStruct {
        public Vector3 coord;
        public Vector3 normal;
        public Vector2Int matIds;

        public PlaneStruct(Vector3 coord, Vector3 normal, Vector2Int matIds) {
            this.coord = coord;
            this.normal = normal;
            this.matIds = matIds;
        }
    }

    public struct QuadricStruct {
        public Matrix4x4 parameters;
        public int matId;

        public QuadricStruct(Matrix4x4 parameters, int matId) {
            this.parameters = parameters;
            this.matId = matId;
        }
    }

    public struct AABBStruct {
        public Matrix2x3 corners;
        public Vector3Int matIds;

        //NOTE: it seems unity/hlsl fills matrices by columns. This is why the placement of the corners in the matrix seems odd.
        public AABBStruct(Vector3 minPos, Vector3 maxPos, Vector3Int matIds) {
            corners = new(minPos, maxPos);
            this.matIds = matIds;
        }
    }

    public struct MeshStruct {
        public Matrix2x3 corners;
        public int vertStart, triStart, vertCount, triCount, matId;

        public MeshStruct(int vertStart, int triStart, int vertCount, int triCount, int matId, Matrix2x3 corners) {
            this.vertStart = vertStart;
            this.triStart = triStart;
            this.vertCount = vertCount;
            this.triCount = triCount;
            this.matId = matId;
            this.corners = corners;
        }

        public MeshStruct(int vertStart, int triStart, int vertCount, int triCount, Matrix2x3 corners) {
            this.vertStart = vertStart;
            this.triStart = triStart;
            this.vertCount = vertCount;
            this.triCount = triCount;
            matId = 0;
            this.corners = corners;
        }
    }
}