using System;
using UnityEngine;

namespace ZXC
{
    #region CommonHandler

    public delegate void OnFinishedDelegate();

    #endregion

    #region MsgHandler

    public delegate void OnMsgHandler();
    public delegate void OnMsgHandler<T>(T args);
    public delegate void OnMsgHandler<T, U>(T argsT, U argsU);
    public delegate void OnMsgHandler<T, U, V>(T argsT, U argsU, V argsV);

    #endregion

    #region Chain Callback

    public delegate void ChainThenDelegate(Action next);
    public delegate void ChainThenDelegateWithKill(Action next, Action kill);

    #endregion

    #region Loader Callback

    public delegate void LoadDelegate(bool isOk, string err, UnityEngine.Object result);
    public delegate void LoadAssetDelegate<T>(bool isOk, string err, T asset) where T : UnityEngine.Object;

    #endregion
}