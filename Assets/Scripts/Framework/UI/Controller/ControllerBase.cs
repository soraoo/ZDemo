using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;

namespace ZXC
{
    public delegate void OnCommand(params object[] param);

    public class ControllerBase : IZController
    {
        public IZViewModel ViewModel { get; protected set; }

        protected Dictionary<int, List<OnCommand>> commandDic;

        protected ControllerBase()
        {
            commandDic = new Dictionary<int, List<OnCommand>>();
        }

        protected void RegisteCommand(int command, OnCommand commandHandler)
        {
            List<OnCommand> commandList = null;
            if (commandDic.TryGetValue(command, out commandList))
            {
                commandList.Add(commandHandler);
            }
            else
            {
                commandList = new List<OnCommand>();
                commandList.Add(commandHandler);
                commandDic.Add(command, commandList);
            }
        }

        protected void UnRegisteCommand(int command, OnCommand commandHandler)
        {
            List<OnCommand> commandList = null;
            if (commandDic.TryGetValue(command, out commandList))
            {
                if (commandList.Contains(commandHandler))
                {
                    commandList.Remove(commandHandler);
                }
                else
                {
                    ZLog.Error("no commandHandler with " + command + " named " + commandHandler.Method.Name);
                }
            }
            else
            {
                ZLog.Error("no command in " + ToString() + " named " + command);
            }
        }

        public void Enabled()
        {
            
        }

        public void Disabled()
        {
            commandDic.Clear();
            commandDic = null;
            ViewModel = null;
        }

        public void ExecuteCommand(int command, params object[] param)
        {
            List<OnCommand> commandList = null;
            if (commandDic.TryGetValue(command, out commandList))
            {
                int count = commandList.Count;
                for (int i = 0; i < count; i++)
                {
                    commandList[i](param);
                }
            }
            else
            {
                ZLog.Error("no command in " + ToString() + " named " + command);
            }
        }
    }
}

