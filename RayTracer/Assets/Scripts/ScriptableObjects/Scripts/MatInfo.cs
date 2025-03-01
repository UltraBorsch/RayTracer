using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

[CreateAssetMenu(menuName = "ScriptableObjects/Material")]
public class MatInfo : ScriptableObject, IStructable<Mat> {
    public uint hardness;
    public Color diffuseColour;
    public Color specularColour;

    public Mat Struct { get; private set; }

    public void SetupStruct() {
        Struct = new(hardness, diffuseColour, specularColour);
    }
}
