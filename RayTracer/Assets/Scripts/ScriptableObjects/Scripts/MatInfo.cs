using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Structs;

[CreateAssetMenu(menuName = "ScriptableObjects/Material")]
public class MatInfo : ScriptableObject {
    public uint hardness;
    public Color diffuseColour;
    public Color specularColour;

    private Mat mat;

    public void OnEnable() {
        mat = new(hardness, diffuseColour, specularColour);
    }
}
