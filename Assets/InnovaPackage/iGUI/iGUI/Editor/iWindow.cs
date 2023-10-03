using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InnovaFramework.iGUI
{
    public class iWindow : EditorWindow 
    {
        public List<iObject>                  defaultsChannel = new List<iObject>();
        public Dictionary<int, List<iObject>> objectChannel   = new Dictionary<int, List<iObject>>();
        protected bool isInitialized                          = false;

        public Rect rect;
        public int  channel = 0;

        private iObject _targetObject = new iObject();
        public iObject targetObject 
        {
            get
            {
                _targetObject.isWindow = true;
                _targetObject.position = rect.position;
                _targetObject.size     = rect.size;
                return _targetObject;
            }
        }



        public void ReReletive()
        {
            foreach(var o in defaultsChannel)
            {
                o.ReRelative();
            }

            foreach(var kv in objectChannel)
            {
                foreach(var o in kv.Value)
                {
                    o.ReRelative();
                }
            }
        }

        public void AddChild(iObject obj, int channel = 0)
        {
            obj.window = this;

            if(channel == -1)
            {
                defaultsChannel.Add(obj);
                return;
            }

            if(!objectChannel.ContainsKey(channel))
            {
                objectChannel[channel] = new List<iObject>();               
            }

            objectChannel[channel].Add(obj);
        }


        public void RemoveChild(iObject obj, int channel = 0) 
        {
            if(channel == -1)
            {
                defaultsChannel.Remove(obj);
                return;
            }

            if(!objectChannel.ContainsKey(channel))
            {
                return;
            }

            objectChannel[channel].Remove(obj);
        }


        public void Render()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                OnInitializeUI();
                OnAfterInitializedUI();
            }

            iEvent.ProcessEvent(Event.current);

            foreach(iObject obj in defaultsChannel)
            {
                obj.Render();
            }

            if(!objectChannel.ContainsKey(channel))
            {
                return;
            }

            for(int i = 0; i < objectChannel[channel].Count; i++)
            {
                objectChannel[channel][i].Render();
            }


            if(GUI.changed)
            {
                Repaint();
            }
        }

        protected virtual void OnInitializeUI() { }
        protected virtual void OnAfterInitializedUI() { }
    }
}
