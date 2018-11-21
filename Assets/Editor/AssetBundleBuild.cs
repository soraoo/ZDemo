using UnityEditor;
using UnityEngine;
using System.IO;

public class AssetBundleBuild
{
    [MenuItem("Tools/Build AssetBundle")]
    public static void Build()
    {
        var outputPath = Application.streamingAssetsPath + "/AssetBundles";
        if(!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
        Debug.Log("Done");
    }
}