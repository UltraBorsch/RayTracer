using UnityEngine;
using static Structs;
using static SceneManager;

public class Quadric : Geometry<QuadricStruct> {
    [SerializeField] private Vector3 quadraticParams, bilinearParams, linearParams;
    [SerializeField] private float constant;

    public override QuadricStruct GetStruct() {
        return new QuadricStruct(GetMatrix(), GetMatId(Mats[0]));
    }

    private Matrix4x4 GetMatrix() {
        Vector4 col1 = new(quadraticParams.x, bilinearParams.x, bilinearParams.z, linearParams.x);
        Vector4 col2 = new(bilinearParams.x, quadraticParams.y, bilinearParams.y, linearParams.y);
        Vector4 col3 = new(bilinearParams.z, bilinearParams.y, quadraticParams.z, linearParams.z);
        Vector4 col4 = new(linearParams.x, linearParams.y, linearParams.z, constant);

        return new(col1, col2, col3, col4);
    }
}