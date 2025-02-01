using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    [SerializeField] AllLevelsSO allLevels;
    string folderPath = "Assets/xkhannx/Created Levels/";

    public void CreateEmptyLevel(string _levelName)
    {
        var obj = ScriptableObject.CreateInstance<LevelDataSO>();

        string filePath = folderPath + _levelName + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(obj, filePath);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        allLevels.levels.Add(obj);
        UnityEditor.EditorUtility.SetDirty(allLevels);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }

    public void DeleteLevel(int levelIndex)
    {
        LevelDataSO level = allLevels.levels[levelIndex];

        allLevels.levels.Remove(level);
        UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(level));
        UnityEditor.EditorUtility.SetDirty(allLevels);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }

    public void RenameLevel(int levelIndex, string newName)
    {
        UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(allLevels.levels[levelIndex]), newName);
        UnityEditor.EditorUtility.SetDirty(allLevels.levels[levelIndex]);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
}
