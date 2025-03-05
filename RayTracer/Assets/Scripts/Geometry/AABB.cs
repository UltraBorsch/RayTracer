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
        Vector3Int mats = new() {
            x = GetMatId(Mats[0]),
            y = GetMatId(Mats[1]),
            z = GetMatId(Mats[2])
        };
        return new AABBStruct(center - halfDim, center + halfDim, mats);
    }
}
