using UnityEngine;
using static Structs;
using static SceneManager;

public class Plane : Geometry<PlaneStruct> {
    public override PlaneStruct GetStruct() {  
        int mat1 = Mats.Length == 0 ? 0 : GetMatId(Mats[0]);
        int mat2 = Mats.Length < 2 ? mat1 : GetMatId(Mats[1]);
        return new PlaneStruct(transform.position, transform.up, new(mat1, mat2));
    }
}
