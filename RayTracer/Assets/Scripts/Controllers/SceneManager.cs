using System.Collections.Generic;
using UnityEngine;
public static class SceneManager {
    [SerializeField] static public MatInfo[] Mats { get; set; }

    static public readonly List<Vector3> vertices =  new();
    static public readonly List<int> triangles = new();
    static public readonly List<Vector3> normals = new();

    public static int GetMatId(MatInfo mat) {
        if (Mats == null || mat == null) return -1;
        for (int i = 0; i < Mats.Length; i++) 
            if (Mats[i] == mat) return i;
        return -1;
    }

    public static void SetupScene(SceneInfo sceneInfo) {
        Mats = sceneInfo.Mats;
    }

    public static Vector2Int AddMesh(UnityEngine.Mesh mesh) {
        Vector2Int indices = new() { x = vertices.Count, y = triangles.Count };
        vertices.AddRange(mesh.vertices);
        triangles.AddRange(mesh.triangles);
        normals.AddRange(mesh.normals);
        return indices;
    }
}
