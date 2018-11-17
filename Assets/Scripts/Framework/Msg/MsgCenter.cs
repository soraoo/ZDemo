using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC
{
    public static class MsgCenter
    {
        private static Dictionary<uint, Delegate> msgHandlerDic = new Dictionary<uint, Delegate>();

        #region Register UnRegster Send Msg API
        public static void RegisterMsg(uint msg, OnMsgHandler handler)
        {
            OnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler)msgHandlerDic[msg] + handler;
        }

        public static void RegisterMsg<T>(uint msg, OnMsgHandler<T> handler)
        {
            OnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T>)msgHandlerDic[msg] + handler;
        }

        public static void RegisterMsg<T, U>(uint msg, OnMsgHandler<T, U> handler)
        {
            OnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T, U>)msgHandlerDic[msg] + handler;
        }

        public static void RegisterMsg<T, U, V>(uint msg, OnMsgHandler<T, U, V> handler)
        {
            OnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T, U, V>)msgHandlerDic[msg] + handler;
        }

        public static void UnRegisterMsg(uint msg, OnMsgHandler handler)
        {
            OnUnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler)msgHandlerDic[msg] - handler;
        }

        public static void UnRegisterMsg<T>(uint msg, OnMsgHandler<T> handler)
        {
            OnUnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T>)msgHandlerDic[msg] - handler;
        }

        public static void UnRegisterMsg<T, U>(uint msg, OnMsgHandler<T, U> handler)
        {
            OnUnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T, U>)msgHandlerDic[msg] - handler;
        }

        public static void UnRegisterMsg<T, U, V>(uint msg, OnMsgHandler<T, U, V> handler)
        {
            OnUnRegisterMsg(msg, handler);
            msgHandlerDic[msg] = (OnMsgHandler<T, U, V>)msgHandlerDic[msg] - handler;
        }

        public static void SendMsg(uint msg)
        {
            OnSendMsg(msg);
            (msgHandlerDic[msg] as OnMsgHandler)();            
        }

        public static void SendMsg<T>(uint msg, T args)
        {
            OnSendMsg(msg);
            (msgHandlerDic[msg] as OnMsgHandler<T>)(args);
        }

        public static void SendMsg<T, U>(uint msg, T argsT, U argsU)
        {
            OnSendMsg(msg);
            (msgHandlerDic[msg] as OnMsgHandler<T, U>)(argsT, argsU);            
        }

        public static void SendMsg<T, U, V>(uint msg, T argsT, U argsU, V argsV)
        {
            OnSendMsg(msg);
            (msgHandlerDic[msg] as OnMsgHandler<T, U, V>)(argsT, argsU, argsV);            
        }
        #endregion

        #region Public API
        public static void Clear()
        {
            List<uint> msgToRemoveList = new List<uint>();

            foreach(KeyValuePair<uint, Delegate> pair in msgHandlerDic) 
            {
                msgToRemoveList.Add(pair.Key);
            }
            foreach(uint msg in msgToRemoveList) 
            {
                msgHandlerDic.Remove(msg);
            }
        }

        public static void PrintMsgDic()
        {
            ZLog.Debug("\t\t\t=== MESSENGER PrintEventTable ===");
            foreach(KeyValuePair<uint, Delegate> pair in msgHandlerDic) 
            {
                ZLog.Debug("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }
            ZLog.Debug("\n");
        }
        #endregion

        private static void OnRegisterMsg(uint msg, Delegate d)
        {
            Delegate del;
            if (msgHandlerDic.TryGetValue(msg, out del))
            {
                if(del != null && del.GetType() != d.GetType())
                {
                    throw new MsgCenterException(string.Format("Attemp to add listener with inconsistent signature for msg {0}. Current listeners have type {1} and listener being added has type {2}", msg, del.GetType().Name, d.GetType().Name));
                }
            }
            else
            {
                msgHandlerDic.Add(msg, null);
            }
        }

        private static void OnUnRegisterMsg(uint msg, Delegate d)
        {
            Delegate del;
            if(msgHandlerDic.TryGetValue(msg, out del))
            {
                if(del == null)
                {
                    throw new MsgCenterException(string.Format("Attempting to remove listener with for msg \"{0}\" but current listener is null.", msg));
                }
                else if(del.GetType() != d.GetType())
                {
                    throw new MsgCenterException(string.Format("Attempting to remove listener with inconsistent signature for msg {0}. Current listeners have type {1} and listener being removed has type {2}", msg, del.GetType().Name, d.GetType().Name));
                }
            }
            else
            {
                throw new MsgCenterException(string.Format("Attempting to remove listener for type \"{0}\" but MsgCenter doesn't know about this event type.", msg));
            }
        }

        private static void OnRemovedMsg(uint msg)
        {
            if(msgHandlerDic[msg] == null)
            {
                msgHandlerDic.Remove(msg);
            }
        }

        private static void OnSendMsg(uint msg)
        {
            Delegate del;
            if(msgHandlerDic.TryGetValue(msg, out del))
            {
                if(del == null)
                {
                    throw new MsgCenterException(string.Format("Broadcasting message \"{0}\" but current listener is null.", msg));
                }
            }
            else
            {
                throw new MsgCenterException(string.Format("Broadcasting message \"{0}\" but MsgCenter doesn't know about this msg.", msg));
            }
        }
    }
}