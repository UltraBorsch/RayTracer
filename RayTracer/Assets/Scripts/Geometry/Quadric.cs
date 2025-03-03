using UnityEngine;
using static Structs;
using static SceneManager;

public class Quadric : Geometry<QuadricStruct> {
    [SerializeField] private Vector3 quadraticParams, bilinearParams, linearParams;
    [SerializeField] private float constant;

    public override QuadricStruct GetStruct() {
        return new QuadricStruct(GetMat(), GetMatId(Mats[0]));
    }

    private Matrix4x4 GetMat() {
        Vector4 r1 = new(quadraticParams.x, bilinearParams.x, bilinearParams.z, linearParams.x);
        Vector4 r2 = new(bilinearParams.x, quadraticParams.y, bilinearParams.y, linearParams.y);
        Vector4 r3 = new(bilinearParams.z, bilinearParams.y, quadraticParams.z, linearParams.z);
        Vector4 r4 = new(linearParams.x, linearParams.y, linearParams.z, constant);

        return new(r1, r2, r3, r4); ;
    }
}