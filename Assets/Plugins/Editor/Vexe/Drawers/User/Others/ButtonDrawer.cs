﻿using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Drawers
{
    public class ButtonDrawer : CompositeDrawer<object, ButtonAttribute>
    {
        private MethodCaller<object, object> _callback;
        private Layout _buttonLayout;
        private GUIStyle _buttonStyle;
        private static object[] _args = new object[1];

        protected override void Initialize()
        {
            if (attribute.Method.IsNullOrEmpty())
            {
                Debug.Log("Button method is empty!");
                return;
            }

            var method = targetType.GetMethod(attribute.Method, Flags.StaticInstanceAnyVisibility);
            if (method == null)
            {
                Debug.LogError("Button method {0} not found inside {1}".FormatWith(attribute.Method, targetType));
                return;
            }

            _callback = method.DelegateForCall();

            _buttonStyle = attribute.Style;

            float width = GUIStyles.Button.CalcSize(new GUIContent(attribute.DisplayText)).x;
            _buttonLayout = Layout.sWidth(width);
        }

        public override void OnRightGUI()
        {
            if (gui.Button(attribute.DisplayText, _buttonStyle, _buttonLayout) && _callback != null)
            {
                _args[0] = memberValue;
                _callback(rawTarget, _args);
            }
        }
    }
}