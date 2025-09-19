using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelManager
{
    private static int currentLevel = 0;
    private const string LEVEL_FOLDER = "Levels";

    // Save LevelData vào Application.persistentDataPath
    public static void SaveLevelToFile(LevelData data, string levelName)
    {
        string dir = Path.Combine(Application.persistentDataPath, LEVEL_FOLDER);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, levelName + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"[LevelManager] Saved level to: {path}");
    }

    // Load LevelData từ Application.persistentDataPath
    public static LevelData LoadLevelFromFile(string levelName)
    {
        string dir = Path.Combine(Application.persistentDataPath, LEVEL_FOLDER);
        string path = Path.Combine(dir, levelName +  ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[LevelManager] File not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelData>(json);
    }

#if UNITY_EDITOR
    // Save vào Assets/Resources/Levels (chỉ Editor)
    public static void SaveLevelToResources(LevelData data, string levelName)
    {
        string resourcesDir = Path.Combine(Application.dataPath, "Resources", LEVEL_FOLDER);
        if (!Directory.Exists(resourcesDir)) Directory.CreateDirectory(resourcesDir);

        string path = Path.Combine(resourcesDir, levelName + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log($"[LevelManager] Saved level to Resources: {path}");
    }

    // Load từ Resources (sau khi build)
    public static LevelData LoadLevelFromResources(string levelName)
    {
        var ta = Resources.Load<TextAsset>($"{LEVEL_FOLDER}/{levelName}");
        if (ta == null)
        {
            Debug.LogWarning($"[LevelManager] Resources Levels/{levelName}.json not found");
            return null;
        }

        return JsonUtility.FromJson<LevelData>(ta.text);
    }
#endif
}
