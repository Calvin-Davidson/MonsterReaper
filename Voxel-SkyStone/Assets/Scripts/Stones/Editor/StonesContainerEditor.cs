using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(StonesContainer))]
public class StonesContainerEditor : EditorWindow
{
    private const string WindowUxmlPath = "Assets/Scripts/Stones/Editor/StonesWindow/Menu.uxml";
    
    [MenuItem("Window/Stones")]
    public static void OpenTilesMenu()
    {
        StonesContainerEditor window = GetWindow<StonesContainerEditor>();
        window.titleContent = new GUIContent("Stones");
    }

    public void CreateGUI()
    {
        var root = rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath); VisualElement labelFromUxml = visualTree.CloneTree();
        MakeFullWindow(labelFromUxml);
        root.Add(labelFromUxml);
        
        RenderScrollView();
        root.Q<Button>("CreateStoneButton").clicked += () =>
        {
            GetData().CreateStone();
            RenderScrollView();
        };

        root.Q<TextField>().RegisterValueChangedCallback(evt => RenderScrollView());
    }

    private void RenderScrollView()
    {
        string filter = rootVisualElement.Q<TextField>("StonesFilter").value;
        var scrollView = rootVisualElement.Q<ScrollView>("StonesScrollView");
        scrollView.Clear();

        Debug.Log("rendering new stone  count:  " + GetData().GetStoneNames().Length);

        String[] names = GetData().GetStoneNames();
        for (var i = 0; i < names.Length; i++)
        {
            var stoneName = names[i];
            if (!string.IsNullOrEmpty(filter) && !stoneName.StartsWith(filter)) return;
            
            RenderStoneItem(scrollView, stoneName);
        }
    }


    private void RenderStoneItem(ScrollView view, string stoneName)
    {
        var label = new Label(stoneName);
        view.Add(label);
        label.AddManipulator(new ContextualMenuManipulator(evt =>
        {
            evt.menu.AppendAction("Remove", action =>
            {
                GetData().DeleteStone(stoneName);
                RenderScrollView();
            });
        }));
    }
    private StonesContainer GetData()
    {
        var assets = AssetDatabase.FindAssets("t:" + nameof(StonesContainer));
        var path = AssetDatabase.GUIDToAssetPath(assets[0]);
        var data = AssetDatabase.LoadAssetAtPath<StonesContainer>(path);
        return data;
    }

    private void MakeFullWindow(VisualElement element)
    {
        element.style.position = Position.Absolute;
        element.style.left = 0;
        element.style.right = 0;
        element.style.top = 0;
        element.style.bottom = 0;
    }
}
