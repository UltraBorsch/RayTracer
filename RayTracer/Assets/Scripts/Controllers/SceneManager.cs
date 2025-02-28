using UnityEngine;

public class SceneManager : MonoBehaviour {
    [SerializeField] public static MatInfo[] Mats {get; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetMatId(MatInfo mat) {
        for (int i = 0; i < Mats.Length; i++) 
            if (Mats[i] == mat) return i;
        return -1;
    }
}
