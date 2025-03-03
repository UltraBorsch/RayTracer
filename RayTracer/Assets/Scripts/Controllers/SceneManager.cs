using UnityEngine;
public static class SceneManager {
    [SerializeField] static public MatInfo[] Mats { get; set; }

    public static int GetMatId(MatInfo mat) {
        if (Mats == null || mat == null) return -1;
        for (int i = 0; i < Mats.Length; i++) 
            if (Mats[i] == mat) return i;
        return -1;
    }

    public static void SetupScene(SceneInfo sceneInfo) {
        Mats = sceneInfo.Mats;
    }
}
