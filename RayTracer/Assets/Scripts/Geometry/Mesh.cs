using UnityEngine;
using static Structs;
using static SceneManager;

public class Mesh : Geometry<MeshStruct> {
    [SerializeField] private MeshFilter meshFilter;
    private Vector2Int indices; //x is vertex start, y is tristart

    public void Setup() {
        indices = AddMesh(meshFilter.sharedMesh);
    }

    public override MeshStruct GetStruct() {
        return new MeshStruct(indices.x, indices.y, meshFilter.mesh.vertexCount, meshFilter.mesh.triangles.Length, Mats.Length == 0 ? 0 : GetMatId(Mats[0]));
    }
}
