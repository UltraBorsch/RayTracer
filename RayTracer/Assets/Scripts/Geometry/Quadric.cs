using UnityEngine;
using static Structs;
using static SceneManager;

public class Quadric : Geometry<QuadricStruct> {
    [SerializeField] private Matrix4x4 parameters;
    [SerializeField] private MatInfo mat;

    public override QuadricStruct GetStruct() {
        return new QuadricStruct(parameters, GetMatId(mat));
    }
}