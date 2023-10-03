using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InnovaFramework.iGUI
{
    public class iPadding
    {
        public float top;
        public float left;
        public float right;
        public float bottom;
        public float space;

        public iPadding(float top, float left, float right, float bottom, float space)
        {
            this.top = top;
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.space = space;
        }
        public iPadding() { }
    }

    public enum iRelativePosition
    {
        RIGHT_OF,
        LEFT_OF,
        TOP_OF,
        BOTTOM_OF,

        CENTER_Y_OF,
        CENTER_X_OF,

        RIGHT_IN,
        LEFT_IN,
        TOP_IN,
        BOTTOM_IN,

        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }


    public enum iSize
    {
        X,
        Y
    }


    public enum iRelativeSize
    {
        EQUAL,
        PERCENTAG,
        SPLIT,
        FILL,
        BETWEEN
    }


    public enum iInputType
    {
        INT,
        FLOAT,
        STRING,
        OBJECT,
        PASSWORD,
        DELAYED_INT,
        DELAYED_FLOAT,
        DELAYED_STRING
    }

    public enum iSliderType
    {
        INT,
        FLOAT
    }


    public delegate void iObjectEvent(iObject obj, Event evt);


    public class iObject
    {
        protected struct RelativePositionData
        {
            public float             space;
            public iObject           obj;
            public iRelativePosition relative;
        }


        protected struct RelativeSizeData
        {
            public int           splitCount;
            public float         percentage;
            public float         space;
            public iObject       objReference1;
            public iObject       objReference2;
            public iRelativeSize relative;
            public iSize         size;
        }

        // Properties
        public bool    active   = true;
        public bool    enabled  = true;
        public bool    isWindow = false; // Do not set to true
        public string  name;
        public string  tag;
        public string  description;
        public string  text;
        public string  tooltips;
        public object  attechment       = null; // Use for Attech some object to this object
        public Color   color            = new Color(0, 0, 0, 0);
        public Color   contentColor     = new Color(0, 0, 0, 0);
        public Color   backgroundColor  = new Color(0, 0, 0, 0);
        public Texture texture          = null;

        // Transform
        public Vector2 position;
        public Vector2 size;

        // Setting 
        public iWindow window;
        public iObject parent;
        public GUIStyle style;
        public float labelSpace;

        // Get Only
        public float   bottom   { get { return position.y + size.y; } }
        public float   right    { get { return position.x + size.x; } }
        public float   width    { get { return size.x; } }
        public float   height   { get { return size.y; } }
        public float   widthEx  { get { return size.x + iGUIUtility.space; } }
        public float   heightEx { get { return size.y + iGUIUtility.space; } }
        public Rect    rect     { get { return new Rect(position.x, position.y, size.x, size.y );}}
        public Vector2 center   { get { return new Vector2( position.x + (size.x / 2f), position.y + (size.y / 2f) );}}

        // Callback
        private bool isMouseDown = false;
        public event iObjectEvent OnMouseDown;
        public event iObjectEvent OnMouseUp;
        public event iObjectEvent OnMouseClick;

        protected List<RelativeSizeData>     lastRelativeSize     = new List<RelativeSizeData>();
        protected List<RelativePositionData> lastRelativePosition = new List<RelativePositionData>();

        private bool  isInit = false;
        private Color tempColor;
        private Color tempBGColor;
        private Color tempContentColor;
        private float tempLabelSpace;


        #region Method
        public virtual void Start() { }
        public virtual void Render() 
        { 
            if (!isInit)
            {
                Start();
                isInit = true;
            }

            ProcessEvent(Event.current);
        }


        public Vector2 SplitSize(int splitTo)
        {
            Vector2 vec = new Vector2();
            vec.x = (size.x - (iGUIUtility.space * (splitTo + 1))) / splitTo;
            vec.y = (size.y - (iGUIUtility.space * (splitTo + 1))) / splitTo;
            return vec;
        }


        public void BeginProcessProperty()
        {
            tempColor        = GUI.color;
            tempBGColor      = GUI.backgroundColor;
            tempContentColor = GUI.contentColor;
            tempLabelSpace   = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = string.IsNullOrEmpty(text) ? 0 : labelSpace;

            if (color.a > 0)           GUI.color           = color;
            if (contentColor.a > 0)    GUI.contentColor    = contentColor;
            if (backgroundColor.a > 0) GUI.backgroundColor = backgroundColor;

            GUI.enabled = enabled;
        }


        public void CopyProperty(iObject source)
        {
            color           = source.color;
            enabled         = source.enabled;
            labelSpace      = source.labelSpace;
            contentColor    = source.contentColor;
            backgroundColor = source.backgroundColor;
        }


        public void EndProcessProperty()
        {
            GUI.color                   = tempColor;
            GUI.contentColor            = tempContentColor;
            GUI.backgroundColor         = tempBGColor;
            GUI.enabled                 = true;
            EditorGUIUtility.labelWidth = tempLabelSpace;
        }


        public void CopyTransform(iObject source)
        {
            this.position = source.position;
            this.size     = source.size;
        }


        public void ParseGUIContent(GUIContent content)
        {
            text = content.text;
            tooltips = content.tooltip;
            texture = content.image;
        }


        public void LoadBuiltInIcon(string name)
        {
            ParseGUIContent(EditorGUIUtility.IconContent(name));
        }


        public virtual void ReRelative()
        {
            RePositionWithLastTwoRelativePosition();
            ReSizeWithLastTwoRelativeSize();
        }


        public virtual void RePositionWithLastTwoRelativePosition()
        {
            for(int i = 0; i < lastRelativePosition.Count; i++)
            {
                RelativePosition(lastRelativePosition[i].relative, lastRelativePosition[i].obj, lastRelativePosition[i].space, false);
            }
            GUI.changed = true;
        }


        public virtual void ReSizeWithLastTwoRelativeSize()
        {
            for(int i = 0; i < lastRelativeSize.Count; i++)
            {
                RelativeSizeData relative = lastRelativeSize[i];
                switch(relative.relative)
                {
                    case iRelativeSize.FILL     : 
                    case iRelativeSize.EQUAL    : RelativeSize(relative.size, relative.relative, relative.objReference1, relative.space, false);                         break; 
                    case iRelativeSize.BETWEEN  : RelativeSize(relative.size, relative.relative, relative.objReference1, relative.objReference2, relative.space, false); break; 
                    case iRelativeSize.SPLIT    : RelativeSize(relative.size, relative.relative, relative.objReference1, relative.splitCount, relative.space, false);    break; 
                    case iRelativeSize.PERCENTAG: RelativeSize(relative.size, relative.relative, relative.objReference1, relative.percentage, relative.space, false);    break; 
                }
            }
            GUI.changed = true;
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iObject objReference, float percentage, float padding = 8, bool enableCached = true)
        {
            if (enableCached)
            {
                if(lastRelativeSize.Count >= 2) { lastRelativeSize.RemoveAt(0); }
                lastRelativeSize.Add(new RelativeSizeData()
                {
                    size          = size,
                    space         = padding,
                    relative      = relative,
                    percentage    = percentage,
                    objReference1 = objReference
                });
            }

            switch(relative)
            {
                case iRelativeSize.PERCENTAG:
                {
                    if (size == iSize.X)
                    {
                        float width = iGUIUtility.SplitSizeByPercentage(objReference.width - (padding * 2), percentage)[0];
                        this.size.x = width;
                    }
                    else
                    {
                        float height = iGUIUtility.SplitSizeByPercentage(objReference.height - (padding * 2), percentage)[0];
                        this.size.y  = height;
                    }
                    break;
                }
            }
            GUI.changed = true;
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iObject objReference, int splitCount, float padding = 8, bool enableCached = true)
        {
            if (enableCached)
            {
                if(lastRelativeSize.Count >= 2) { lastRelativeSize.RemoveAt(0); }
                lastRelativeSize.Add(new RelativeSizeData()
                {
                    size          = size,
                    space         = padding,
                    relative      = relative,
                    splitCount    = splitCount,
                    objReference1 = objReference
                });
            }

            switch(relative)
            {
                case iRelativeSize.SPLIT:
                {
                    if(size == iSize.X)
                    {
                        float width = objReference.width - ( padding * 2);
                        this.size.x = width.Split(splitCount);
                    }
                    else
                    {
                        float height = objReference.height - (padding * 2);
                        this.size.y = height.Split(splitCount);
                    }
                    break;
                }
            }
            GUI.changed = true;
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iObject objReference, float space = 8, bool enableCached = true)
        {
            if (enableCached)
            {
                if(lastRelativeSize.Count >= 2) { lastRelativeSize.RemoveAt(0); }
                lastRelativeSize.Add(new RelativeSizeData()
                {
                    size          = size,
                    space         = space,
                    relative      = relative,
                    objReference1 = objReference
                });
            }

            switch(relative)
            {
                case iRelativeSize.FILL:
                {
                    if (size == iSize.X)
                    {
                        this.size.x = objReference.width - (space * 2);
                    }
                    else
                    {
                        this.size.y = objReference.height - (space * 2);
                    }
                    break;
                }
                case iRelativeSize.EQUAL:
                {
                    if (size == iSize.X)
                    {
                        this.size.x = objReference.width;
                    }
                    else
                    {
                        this.size.y = objReference.height;
                    }
                    break;
                }
            }
            GUI.changed = true;
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iObject topOrLeft, iObject bottomOrRight, float space = 8, bool enableCached = true)
        {
            if (enableCached)
            {
                if(lastRelativeSize.Count >= 2) { lastRelativeSize.RemoveAt(0); }
                lastRelativeSize.Add(new RelativeSizeData()
                {
                    size          = size,
                    space         = space,
                    relative      = relative,
                    objReference1 = topOrLeft,
                    objReference2 = bottomOrRight
                });
            }
            
            switch(relative)
            {
                case iRelativeSize.BETWEEN:
                {
                    if (size == iSize.X)
                    {
                        float left  = topOrLeft.isWindow     ? 0 : topOrLeft.right;
                        float right = bottomOrRight.isWindow ? bottomOrRight.width : bottomOrRight.position.x;
                        this.size.x = iGUIUtility.WidthBetween2Objects(left, right, space);
                    }
                    else
                    {
                        float top    = topOrLeft.isWindow     ? 0 : topOrLeft.bottom;
                        float bottom = bottomOrRight.isWindow ? bottomOrRight.height : bottomOrRight.position.y;
                        this.size.y  = iGUIUtility.HeightBetween2Objects(top, bottom, space);
                    }
                    break;
                }
            }
            GUI.changed = true;
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iObject topOrLeft, iWindow bottomOrRight, float space = 8, bool enableCached = true)
        {
            RelativeSize(size, relative, topOrLeft, bottomOrRight.targetObject, space);
        }


        public void RelativeSize(iSize size, iRelativeSize relative, iWindow topOrLeft, iObject bottomOrRight, float space = 8, bool enableCached = true)
        {
            RelativeSize(size, relative, topOrLeft.targetObject, bottomOrRight, space);
        }




        public void RelativePosition(iRelativePosition relative, Rect rect, float space = 8, bool enableCached = true)
        {
            iObject obj  = new iObject();
            obj.size     = rect.size;
            obj.position = rect.position;
            RelativePosition(relative, obj, space, enableCached);
        }


        public void RelativePosition(iRelativePosition relative, iWindow mWindow, float space = 8, bool enableCached = true)
        {
            iObject obj = new iObject();
            obj.size    = mWindow.maxSize;
            RelativePosition(relative, obj, space, enableCached);
        }


        public void RelativePosition(iRelativePosition relative, iObject mObject, float space = 8, bool enableCached = true)
        {
            if(enableCached)
            {
                if(lastRelativePosition.Count >= 2) { lastRelativePosition.RemoveAt(0); }
                lastRelativePosition.Add(new RelativePositionData()
                {
                    relative = relative,
                    obj      = mObject,
                    space    = space
                });
            }


            switch (relative)
            {
                case iRelativePosition.RIGHT_OF:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x + mObject.size.x + space,
                            y = position.y
                        };

                        break;
                    }
                case iRelativePosition.LEFT_OF:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x - size.x - space,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.TOP_OF:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y - size.y - space
                        };
                        break;
                    }
                case iRelativePosition.BOTTOM_OF:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y + mObject.size.y + space
                        };
                        break;
                    }
                case iRelativePosition.LEFT_IN:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x + space,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.RIGHT_IN:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x + mObject.size.x - size.x - space,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.TOP_IN:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y + space
                        };
                        break;
                    }
                case iRelativePosition.BOTTOM_IN:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y + mObject.size.y - size.y - space
                        };
                        break;
                    }
                case iRelativePosition.CENTER_Y_OF:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y + mObject.size.y / 2f - size.y / 2f
                        };
                        break;
                    }
                case iRelativePosition.CENTER_X_OF:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x + mObject.size.x / 2f - size.x / 2f,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.LEFT:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.RIGHT:
                    {
                        position = new Vector2()
                        {
                            x = mObject.position.x + mObject.size.x - size.x,
                            y = position.y
                        };
                        break;
                    }
                case iRelativePosition.TOP:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y
                        };
                        break;
                    }
                case iRelativePosition.BOTTOM:
                    {
                        position = new Vector2()
                        {
                            x = position.x,
                            y = mObject.position.y + mObject.size.y - size.y
                        };
                        break;
                    }
            }

            GUI.changed = true;
        }


        private void ProcessEvent(Event evt)
        {
            switch(evt.type)
            {
                case EventType.MouseDown:
                {
                    if (rect.Contains(evt.mousePosition))
                    {
                        isMouseDown = true;
                        OnMouseDown?.Invoke(this, evt);
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (rect.Contains(evt.mousePosition))
                    {
                        OnMouseUp?.Invoke(this, evt);

                        if (isMouseDown)
                        {
                            OnMouseClick?.Invoke(this, evt);
                        }
                        isMouseDown = false;
                    }

                    break;
                }
            }
        }

        #endregion
    }
}