using CodeBase.IteractableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildInObject))]
public class BuildInObjectEditor : Editor
{
    private SerializedProperty contactTypeProperty;
    private string[] objectNames;

    void OnEnable()
    {
        contactTypeProperty = serializedObject.FindProperty("contactType");
        Type baseType = typeof(GrabbableObject);
        Assembly assemblyCSharp = Assembly.Load("Assembly-CSharp");
        Type[] allTypes = assemblyCSharp.GetTypes().Where(type => type.IsSubclassOf(baseType)).ToArray();
        objectNames = allTypes.Select(type => type.Name).ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
         
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
            if (iterator.name == "contactType") continue;
            EditorGUILayout.PropertyField(iterator, true);
            enterChildren = false;
        }
         
        int selectedObjectIndex = 0;
        if (!string.IsNullOrEmpty(contactTypeProperty.stringValue))
            selectedObjectIndex = Array.IndexOf(objectNames, contactTypeProperty.stringValue);
        

        selectedObjectIndex = EditorGUILayout.Popup("Contact Type", selectedObjectIndex, objectNames);

        if (selectedObjectIndex >= 0)
            contactTypeProperty.stringValue = objectNames[selectedObjectIndex];

        serializedObject.ApplyModifiedProperties();
    }
}