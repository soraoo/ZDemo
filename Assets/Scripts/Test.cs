using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace ZXC
{
    public class Test : MonoBehaviour
    {
        // Use this for initialization
        async void Start()
        {
            ZLog.Debug("Test--------------------");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await ResMgr.Instance.Init();
            await ResMgr.Instance.LoadAssetBundle("com");
            // ZLog.Debug(bundle.name);
            var cube1 = await ResMgr.Instance.LoadAsset<GameObject>(new AssetId("cube1", "Cube1"));
            var cube2 = await ResMgr.Instance.LoadAsset<GameObject>(new AssetId("cube2", "Cube2"));
            GameObject.Instantiate(cube1, Vector3.zero, Quaternion.identity);
            GameObject.Instantiate(cube2, Vector3.up, Quaternion.identity);
            stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

