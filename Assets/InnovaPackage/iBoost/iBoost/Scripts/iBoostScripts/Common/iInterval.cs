using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iBoost
{
    public class IntervalQueue
    {
        public float delay;
        public Action callback;

        public IntervalQueue(float delay, Action callback)
        {
            this.delay = delay;
            this.callback = callback;
        }
    }

    public static class iInterval
    {
        private static Dictionary<string, float> timer = new Dictionary<string, float>();
        private static Dictionary<string, bool> dic_run_every = new Dictionary<string, bool>();

        // Run Loop
        public static bool Every(float seconds, string name)
        {
            if(!timer.ContainsKey(name)) 
            {
                timer[name] = Time.time;
                return true;
            }

            if(Time.time - timer[name] >= seconds)
            {
                timer[name] = Time.time;
                return true;
            }

            return false;
        }

        public static void ResetTime(string name)
        {
            if(timer.ContainsKey(name))
            {
                timer[name] = Time.time;
            }
        }

        public static void ResetTimeToZero(string name)
        {
            if(timer.ContainsKey(name))
            {
                timer[name] = 0f;
            }
        }


        // Run Once
        public static void RunOnce(MonoBehaviour caller, float time, Action func)
        {
            caller.StartCoroutine(RunOnce_Routine(time, func));
        }
        private static IEnumerator RunOnce_Routine(float time, Action func)
        {
            yield return new WaitForSeconds(time);
            func();
        }

        // Run Interval
        public static void RunEvery(MonoBehaviour caller, string name, float time, Action func)
        {
            dic_run_every[name] = true;
            caller.StartCoroutine(RunEvery_Routine(name, time, func));
        }
        public static void StopEvery(string name)
        {
            if(!dic_run_every.ContainsKey(name)) return;
            dic_run_every[name] = false;
        }
        private static IEnumerator RunEvery_Routine(string name, float time, Action func)
        {
            while(dic_run_every[name])
            {
                func();
                yield return new WaitForSeconds(time);
            }

            dic_run_every.Remove(name);
        }

        // Run Queue
        public static void RunQueue(MonoBehaviour caller, params IntervalQueue[] queue)
        {
            caller.StartCoroutine(RunQueue_Routine(queue));
        }

        private static IEnumerator RunQueue_Routine(IntervalQueue[] queue)
        {
            for(int i = 0, j = queue.Length; i < j; ++i)
            {
                yield return new WaitForSeconds(queue[i].delay);
                queue[i].callback();
            }
        }

        // Run Many
        public static void RunMany(MonoBehaviour caller, int count, float time, Action action, Action callback = null)
        {
            caller.StartCoroutine(RunMany_Routine(count, time, action, callback));
        }

        private static IEnumerator RunMany_Routine(int count, float time, Action action, Action callback )
        {
            for(int c = 0; c < count; ++c)
            {
                yield return new WaitForSeconds(time);
                action();
            }

            if(callback != null)
            {
                callback();
            }
        }
    }
}