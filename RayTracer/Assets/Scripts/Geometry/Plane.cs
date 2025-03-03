using UnityEngine;
using static Structs;
using static SceneManager;

public class Plane : Geometry<PlaneStruct> {
    public override PlaneStruct GetStruct() {
        int mat1 = GetMatId(Mats[0]);
        int mat2 = Mats.Length > 1 ? GetMatId(Mats[1]) : mat1;
        return new PlaneStruct(transform.position, transform.up, new(mat1, mat2));
    }
}
