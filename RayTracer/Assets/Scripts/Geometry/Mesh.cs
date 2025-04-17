using UnityEngine;
using static Structs;
using static SceneManager;
using static ComputeStructs;

public class Mesh : Geometry<MeshStruct> {
    [SerializeField] private MeshFilter meshFilter;
    private Vector2Int indices; //x is vertex start, y is tristart

    public void Setup() {
        indices = AddMesh(meshFilter.sharedMesh, transform);
    }

    public override MeshStruct GetStruct() {
        Vector3 min = meshFilter.mesh.vertices[0], max = meshFilter.mesh.vertices[0];
        for (int i = 0; i < meshFilter.mesh.vertexCount; i++) {
            Vector3 vert = meshFilter.mesh.vertices[i];
            min.x = vert.x < min.x ? vert.x : min.x;
            min.y = vert.y < min.y ? vert.y : min.y;
            min.z = vert.z < min.z ? vert.z : min.z;

            max.x = vert.x > max.x ? vert.x : max.x;
            max.y = vert.y > max.y ? vert.y : max.y;
            max.z = vert.z > max.z ? vert.z : max.z;
        }

        min += transform.position;
        max += transform.position;

        Matrix2x3 corners = new(min, max);
        return new MeshStruct(indices.x, indices.y, meshFilter.mesh.vertexCount, meshFilter.mesh.triangles.Length, Mats.Length == 0 ? 0 : GetMatId(Mats[0]), corners);
    }
}
