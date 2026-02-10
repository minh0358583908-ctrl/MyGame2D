#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace BaseGame
{
    public static class EditorUtils
    {
        public static float RoundToTwoDecimalPlaces(float number)
        {
            return (float)Math.Round(number, 2, MidpointRounding.AwayFromZero);
        }

        public static Texture2D CreateTexture2D(int width, int height, Color color)
        {
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++) pixels[i] = color;
            var texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public static ReorderableList SetupReorderableList<T>(
            string headerText, List<T> elements, ref T currentElement, Action<Rect, T> drawElement,
            Action<T> selectElement, Action createElement, Action<T> removeElement)
        {
            var list = new ReorderableList(elements, typeof(T), true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    GUI.color = Color.cyan;
                    EditorGUI.LabelField(rect, headerText);
                    GUI.color = Color.white;
                },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    if (index % 2 != 0) GUI.color = Color.yellow;
                    drawElement(rect, elements[index]);
                    GUI.color = Color.white;
                }
            };

            list.onSelectCallback = l => { selectElement(elements[list.index]); };

            if (createElement != null) list.onAddDropdownCallback = (buttonRect, l) => { createElement(); };

            list.onRemoveCallback = l =>
            {
                if (!EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No")) return;
                removeElement(elements[l.index]);
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            };

            return list;
        }
    }
}

#endif