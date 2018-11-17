using System;

namespace ZXC
{
    public class MsgCenterException : Exception
    {
        public MsgCenterException(string msg) : base(msg) { }
    }

    public class AssetLoaderException : Exception
    {
        public AssetLoaderException(string msg) : base(msg) { }
    }
}