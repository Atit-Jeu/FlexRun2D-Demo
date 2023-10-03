using UnityEngine;

namespace iBoost
{
    public class Log
    {
        public static void Info(object msg)
        {
            Debug.Log("<color='blue'>Info: </color>" + "<color='#3333ff'>" + msg + "</color>");
        }
        public static void Error(object msg)
        {
            Debug.Log("<color='red'>Error: </color>" + "<color='#ff3333'>" + msg + "</color>");
        }
        public static void Success(object msg)
        {
            Debug.Log("<color='green'>Success: </color>" + "<color='#006600'>" + msg + "</color>");
        }
        public static void Warning(object msg)
        {
            Debug.Log("<color='#885500'>Warning: </color>" + "<color='#662200'>" + msg + "</color>");
        }
        public static void Show(object msg, Color color)
        {
            string c = ColorUtility.ToHtmlStringRGB(color);
            Debug.Log("<color='#" + c + "'>Warning: </color>" + "<color='#" + c  + "'>" + msg + "</color>");
        }
    }

}