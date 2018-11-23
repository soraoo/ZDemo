using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace ZXC
{
    public class Test : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            Application.targetFrameRate = 60;
        }
        async void Start()
        {
            ZLog.Debug("Test--------------------");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //await ResMgr.Instance.Init();
            //await ResMgr.Instance.LoadAssetBundle("com");
            // ZLog.Debug(bundle.name);
            //var cube3 = await ResMgr.Instance.LoadAsset<GameObject>(new AssetId("cube3", "Cube3"));
            //var cube2 = await ResMgr.Instance.LoadAsset<GameObject>(new AssetId("cube2", "Cube2"));
            //GameObject.Instantiate(cube3, Vector3.zero, Quaternion.identity);
            //GameObject.Instantiate(cube2, Vector3.up, Quaternion.identity);
            //await new AsyncWaitFrame(1);
            //StartCoroutine(TestIE());
            // ZLog.Debug(Time.deltaTime*30);
            await new AsyncWaitFrame(30);
            stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            await new AsyncWaitFrame(30);
            stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            await new AsyncWaitFrame(30);
            stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            await new AsyncWaitFrame(30);
            stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
        }


        private IEnumerator TestIE()
        {
             Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for(int i =0; i < 30; i++)
                yield return null;
             stopwatch.Stop();
            ZLog.Debug(stopwatch.ElapsedMilliseconds);
        }

        // Update is called once per frame
        // void Update()
        // {
        //     //ZLog.Debug(Time.deltaTime);
        // }
    }
}

