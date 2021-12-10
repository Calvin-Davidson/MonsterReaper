using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(StonesContainer))]
public class StonesContainerEditor : EditorWindow
{
    private string _selectedStoneName = "";

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

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
        VisualElement labelFromUxml = visualTree.CloneTree();
        MakeFullWindow(labelFromUxml);
        root.Add(labelFromUxml);

        RenderScrollView();
        root.Q<Button>("CreateStoneButton").clicked += () =>
        {
            GetData().CreateStone();
            RenderScrollView();
        };

        root.Q<TextField>().RegisterValueChangedCallback(evt => RenderScrollView());
        SetupStoneSettings();
        RenderStoneSettings();
    }

    private void RenderScrollView()
    {
        string filter = rootVisualElement.Q<TextField>("StonesFilter").value;
        var scrollView = rootVisualElement.Q<ScrollView>("StonesScrollView");
        scrollView.Clear();

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
        if (_selectedStoneName == stoneName) label.style.backgroundColor = Color.red;

        view.Add(label);
        label.AddManipulator(new ContextualMenuManipulator(evt =>
        {
            evt.menu.AppendAction("Remove", action =>
            {
                GetData().DeleteStone(stoneName);
                RenderScrollView();
                RenderStoneSettings();
            });
        }));

        label.AddManipulator(new Clickable(() =>
        {
            _selectedStoneName = label.text;
            RenderScrollView();
            RenderStoneSettings();
        }));
    }

    private void RenderStoneSettings()
    {
        if (String.IsNullOrEmpty(_selectedStoneName) || GetData().GetStoneByName(_selectedStoneName) == null)
        {
            rootVisualElement.Q<VisualElement>("StoneSettings").style.visibility = Visibility.Hidden;
            return;
        }

        rootVisualElement.Q<VisualElement>("StoneSettings").style.visibility = Visibility.Visible;

        if (SelectedStone == null)
        {
            Debug.Log("there is no stone data with the name" + _selectedStoneName);
            return;
        }

        RenderDamageField(rootVisualElement.Q<IntegerField>("StoneTopDamage"), SelectedStone.TopDamage);
        RenderDamageField(rootVisualElement.Q<IntegerField>("StoneRightDamage"), SelectedStone.RightDamage);
        RenderDamageField(rootVisualElement.Q<IntegerField>("StoneBottomDamage"), SelectedStone.BottomDamage);
        RenderDamageField(rootVisualElement.Q<IntegerField>("StoneLeftDamage"), SelectedStone.LeftDamage);

        var nameField = rootVisualElement.Q<TextField>("StoneName");
        nameField.value = _selectedStoneName;

        var imageField = rootVisualElement.Q<ObjectField>("StoneImage");
        imageField.value = SelectedStone.Image;
    }

    private void SetupStoneSettings()
    {
        var nameField = rootVisualElement.Q<TextField>("StoneName");
        nameField.RegisterValueChangedCallback(evt =>
        {
            SelectedStone.SetName(evt.newValue);
            _selectedStoneName = evt.newValue;
            RenderScrollView();
        });
        
        var imageField = rootVisualElement.Q<ObjectField>("StoneImage");
        imageField.objectType = typeof(GameObject);
        imageField.RegisterValueChangedCallback(evt =>  SelectedStone.SetImage(evt.newValue as GameObject));
        
        rootVisualElement.Q<IntegerField>("StoneTopDamage").RegisterValueChangedCallback(evt => SelectedStone.SetTopDamage(evt.newValue));
        rootVisualElement.Q<IntegerField>("StoneRightDamage").RegisterValueChangedCallback(evt => SelectedStone.SetRightDamage(evt.newValue));
        rootVisualElement.Q<IntegerField>("StoneBottomDamage").RegisterValueChangedCallback(evt => SelectedStone.SetBottomDamage(evt.newValue));
        rootVisualElement.Q<IntegerField>("StoneLeftDamage").RegisterValueChangedCallback(evt => SelectedStone.SetLeftDamage(evt.newValue));
    }


    private void RenderDamageField(IntegerField field, int currentValue)
    {
        field.value = currentValue;
    }

    private StonesContainer GetData()
    {
        var assets = AssetDatabase.FindAssets("t:" + nameof(StonesContainer));
        var path = AssetDatabase.GUIDToAssetPath(assets[0]);
        var data = AssetDatabase.LoadAssetAtPath<StonesContainer>(path);
        return data;
    }

    public StoneData SelectedStone => GetData().GetStoneByName(_selectedStoneName);

    private void MakeFullWindow(VisualElement element)
    {
        element.style.position = Position.Absolute;
        element.style.left = 0;
        element.style.right = 0;
        element.style.top = 0;
        element.style.bottom = 0;
    }
}