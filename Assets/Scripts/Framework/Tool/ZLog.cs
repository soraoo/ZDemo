using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC
{
    public static class ZLog
    {
        /// <summary>
        /// Log委托
        /// </summary>
        /// <param name="obj"></param>
        public delegate void LogHandler(object obj);

        /// <summary>
        /// 是否开启调试
        /// </summary>
        public static bool IsDebug = true;

        public static LogHandler Error = UnityEngine.Debug.LogError;
#if UNITY_EDITOR
        public static LogHandler Debug = UnityEngine.Debug.Log;
        public static LogHandler Warning = UnityEngine.Debug.LogWarning;
#else
        public static void Debug(object obj)
        {
            if (IsDebug)
                return;
        }

        public static void Warning(object obj)
        {
            if (IsDebug)
                return;
        }
#endif
    }
}