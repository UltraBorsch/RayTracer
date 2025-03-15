using UnityEngine;
using static Structs;
using static SceneManager;

public class AABB : Geometry<AABBStruct> {
    //assumes variable setting is done through the inspector on the transform
    public override AABBStruct GetStruct() {
        Vector3 center = transform.position;
        Vector3 halfDim = new() {
            x = transform.localScale.x / 2,
            y = transform.localScale.y / 2,
            z = transform.localScale.z / 2
        };

        int mat1 = Mats.Length == 0 ? 0 : GetMatId(Mats[0]),
        mat2 = Mats.Length < 2 ? mat1 : GetMatId(Mats[1]),
        mat3 = Mats.Length < 3 ? mat1 : GetMatId(Mats[2]);

        Vector3Int mats = new(mat1, mat2, mat3);
        return new AABBStruct(center - halfDim, center + halfDim, mats);
    }
}
