using UnityEngine;
using static Structs;
using static SceneManager;

public class Plane : Geometry<PlaneStruct> {
    [SerializeField] private MatInfo mat1;
    [SerializeField] private MatInfo mat2;
    public override PlaneStruct GetStruct() {
        return new PlaneStruct(transform.position, transform.up, new(GetMatId(mat1), GetMatId(mat2)));
    }
}
