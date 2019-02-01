using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class AssetBuildUtility
{
    [MenuItem("Tools/Build AssetBundle")]
    public static void Build()
    {
        var outputPath = ZXC.ResUtility.GetAssetBundlesPath();
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        var inputPath = $"{Application.dataPath}/Products/";
        var subStartIdx = inputPath.Length;
        var buildList = new List<AssetBundleBuild>();
        BuildAssets(inputPath, buildList, subStartIdx);
        BuildPipeline.BuildAssetBundles(outputPath, buildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
        Debug.Log($"Done------bundle num is {buildList.Count}");
    }

    private static void BuildAssets(string dirPath, ICollection<AssetBundleBuild> buildList, int subStartIdx)
    {
        var dirInfo = new DirectoryInfo(dirPath);
        foreach (var subDirInfo in dirInfo.GetDirectories())
        {
            BuildAssets(subDirInfo.FullName, buildList, subStartIdx);
        }

        var fileInfos = dirInfo.GetFiles();
        if (fileInfos.Length == 0)
            return;
        var build = new AssetBundleBuild();
        var length = fileInfos.Length;
        var assetNames = new string[length];
        var bundleName = dirPath.Substring(subStartIdx, dirPath.Length - 1);
        for (var i = 0; i < length; i++)
        {
            var assetName = fileInfos[i].FullName.Replace(Application.dataPath, "Assets");
            assetNames[i] = assetName;
        }
        build.assetBundleName = bundleName;
        build.assetNames = assetNames;
        buildList.Add(build);
    }
}