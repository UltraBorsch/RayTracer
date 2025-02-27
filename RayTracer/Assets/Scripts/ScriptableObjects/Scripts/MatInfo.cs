using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Material")]
public class MatInfo : ScriptableObject {
    public byte hardness;
    public Color diffuseColour;
    public Color specularColour;
}
