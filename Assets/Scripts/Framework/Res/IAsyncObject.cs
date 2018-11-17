using System;

namespace ZXC.Res
{
    /// <summary>
    /// 异步操作对象接口
    /// </summary>
    public interface IAsyncObject
    {
        /// <summary>
        /// 异步结果
        /// </summary>
        /// <value></value>
        object ResultObject { get; }
        /// <summary>
        /// 是否完成
        /// </summary>
        /// <value></value>
        bool IsCompleted { get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        bool IsSuccess { get; }
        /// <summary>
        /// 是否错误
        /// </summary>
        /// <value></value>
        bool IsError { get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <value></value>
        string ErrorMsg { get; }
        /// <summary>
        /// 进度
        /// </summary>
        /// <value></value>
        float Progress { get; }
    }
}