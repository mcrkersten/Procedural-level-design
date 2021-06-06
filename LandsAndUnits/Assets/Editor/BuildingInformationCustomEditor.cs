using UnityEngine;
using UnityEditor;
using UnitsAndFormation;
using UnityEditor.Callbacks;
using System;
using UnityEditorInternal;

[CustomEditor(typeof(InteractableInformation))]
public class BuildingInformationCustomEditor : Editor
{
    InteractableInformation info;

    private SerializedProperty _costsData;
    private ReorderableList costList;

    private SerializedProperty _produceData;
    private ReorderableList productionList;

    private SerializedProperty _productionStorageData;
    private ReorderableList storageList;

    private void OnEnable()
    {
        EditorUtility.SetDirty(target);
        //Find the list in our ScriptableObject script.
        _costsData = serializedObject.FindProperty("_costs");
        _produceData = serializedObject.FindProperty("_thisBuildingProduces");
        _productionStorageData = serializedObject.FindProperty("_storageTypes");

        //Create an instance of our reorderable list.
        costList = new ReorderableList(serializedObject: serializedObject, elements: _costsData, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);
        productionList = new ReorderableList(serializedObject: serializedObject, elements: _produceData, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);

        if(info != null && info._interactableType == InteractableType.Workplace)
        {
            storageList = new ReorderableList(serializedObject: serializedObject, elements: _productionStorageData, draggable: true, displayHeader: true,
                displayAddButton: false, displayRemoveButton: false);
        }
        else
        {
            storageList = new ReorderableList(serializedObject: serializedObject, elements: _productionStorageData, draggable: true, displayHeader: true,
                displayAddButton: true, displayRemoveButton: true);
        }

        //Set up the method callback to draw our list header.
        costList.drawHeaderCallback = DrawHeaderCallbackCost;
        productionList.drawHeaderCallback = DrawHeaderCallbackProduction;
        storageList.drawHeaderCallback = DrawHeaderCallbackStorage;

        //Set up the method callback to draw each element in our reorderable list
        costList.drawElementCallback = DrawElementCallbackCost;
        productionList.drawElementCallback = DrawElementCallbackProduction;
        storageList.drawElementCallback = DrawElementCallbackStorage;

        //Set the height of each element.
        costList.elementHeightCallback += ElementHeightCallbackCost;
        productionList.elementHeightCallback += ElementHeightCallbackProduction;
        storageList.elementHeightCallback = ElementHeightCallbackStorage;

        //Set up the method callback to define what should happen when we add a new object to our list.
        costList.onAddCallback += OnAddCallback;
        productionList.onAddCallback += OnAddCallback;
        storageList.onAddCallback += OnAddCallback;
    }

    private void DrawHeaderCallbackCost(Rect rect)
    {
        EditorGUI.LabelField(rect, "Construction Costs");
    }
    private void DrawHeaderCallbackProduction(Rect rect)
    {
        EditorGUI.LabelField(rect, "Production");
    }
    private void DrawHeaderCallbackStorage(Rect rect)
    {
        EditorGUI.LabelField(rect, "Storage");
    }

    private void DrawElementCallbackCost(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = costList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent("Resource"), includeChildren: true);
    }
    private void DrawElementCallbackProduction(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = productionList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;


         EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent("Resource:"), includeChildren: true);
    }
    private void DrawElementCallbackStorage(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = storageList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;


        EditorGUI.PropertyField(position:
           new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
           element, label: new GUIContent((ResourceType)element.FindPropertyRelative("_resourceType").enumValueIndex + "-storage | size: " + element.FindPropertyRelative("_maxStorage").intValue), includeChildren: true);
    }

    private float ElementHeightCallbackCost(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(costList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }
    private float ElementHeightCallbackProduction(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(productionList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }
    private float ElementHeightCallbackStorage(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(storageList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    private void OnAddCallback(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
    }

    public override void OnInspectorGUI()
    {
        info = (InteractableInformation)target;

        info._interactableID = EditorGUILayout.TextField("interactable ID", info._interactableID);
        info._interactableType = (InteractableType)EditorGUILayout.EnumPopup("Type: ", info._interactableType);

        DrawPrefabSelection();

        EditorGUILayout.Space(10);

        DrawUI_information();

        EditorGUILayout.Space(10);

        DrawConstrucionParamiters();

        //Building Specifics
        DrawBuildingType();

    }

    private  void DrawBuildingType()
    {
        switch (info._interactableType)
        {
            case InteractableType.None:
                break;
            case InteractableType.Construction:
                break;
            case InteractableType.Housing:
                DrawTenantCapacity();
                break;
            case InteractableType.Resource:
                break;
            case InteractableType.Workplace:
                DrawWorkplaceCapacity();
                EditorGUILayout.Space(10);
                DrawProductionParamiters();
                DrawStorageParamiters();
                break;
            case InteractableType.Storage:
                DrawStorageParamiters();
                break;
            case InteractableType.Other:
                break;
            default:
                break;
        }
    }
    private void DrawConstrucionParamiters()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Construction paramiters", myStyle);

        EditorGUI.indentLevel++;
        info._constructionPoints = EditorGUILayout.IntField("Construction HP:", info._constructionPoints);
        EditorGUI.indentLevel--;

        serializedObject.Update();
        costList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProductionParamiters()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Production paramiters", myStyle);

        serializedObject.Update();
        productionList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStorageParamiters()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Storage capacity", myStyle);

        if (info._interactableType == InteractableType.Workplace)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Click to calculate:", GUILayout.MaxWidth(125));
            if (GUILayout.Button("Calculate storage", GUILayout.MaxWidth(140), GUILayout.MaxHeight(25)))
            {
                info.Calculate();
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.Update();
        storageList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTenantCapacity()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Tenant capacity", myStyle);

        EditorGUI.indentLevel++;
        info._occupancyCapacity = EditorGUILayout.IntField("Capacity:", info._occupancyCapacity);
        EditorGUI.indentLevel--;
    }

    private void DrawWorkplaceCapacity()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Workeplace capacity", myStyle);

        EditorGUI.indentLevel++;
        info._occupancyCapacity = EditorGUILayout.IntField("Workers capacity:", info._occupancyCapacity);
        info._visitorCapacity = EditorGUILayout.IntField("Visitors capacity:", info._visitorCapacity);
        EditorGUI.indentLevel--;
    }

    private void DrawUI_information()
    {
        GUIStyle myStyle = GUI.skin.label;
        myStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("User interface description", myStyle);
        EditorGUI.indentLevel++;
        info._name = TextField(info._name, "Building name");
        info._description = TextArea(info._description, "Building description");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size:", GUILayout.MaxWidth(50));
        EditorGUILayout.LabelField("X:", GUILayout.MaxWidth(30));
        info._size.x = EditorGUILayout.IntField(info._size.x);
        EditorGUILayout.LabelField("Y:", GUILayout.MaxWidth(30));
        info._size.y = EditorGUILayout.IntField(info._size.y);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Menu image:", GUILayout.MaxWidth(103));
        info._image = (Sprite)EditorGUILayout.ObjectField(info._image, typeof(Sprite), false);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawPrefabSelection()
    {
        info._constructionPrefab = (GameObject)EditorGUILayout.ObjectField("Construction prefab", info._constructionPrefab, typeof(GameObject), false);
        info._completedPrefab = (GameObject)EditorGUILayout.ObjectField("Completed prefab", info._completedPrefab, typeof(GameObject), false);
    }

    string TextField(string text, string placeholder)
    {
        return TextInput(text, placeholder);
    }

    string TextArea(string text, string placeholder)
    {
        return TextInput(text, placeholder, area: true);
    }

    private string TextInput(string text, string placeholder, bool area = false)
    {
        var newText = area ? EditorGUILayout.TextArea(text) : EditorGUILayout.TextField(text);
        if (text == "")
        {
            const int textMargin = 2;
            var guiColor = GUI.color;
            GUI.color = Color.grey;
            var textRect = GUILayoutUtility.GetLastRect();
            var position = new Rect(textRect.x + textMargin, textRect.y, textRect.width, textRect.height);
            EditorGUI.LabelField(position, placeholder);
            GUI.color = guiColor;
        }
        return newText;
    }
}
