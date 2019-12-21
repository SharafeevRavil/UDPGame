using UnityEngine;
using System.Collections;
using System.Reflection;

public class NewBehaviourScript : MonoBehaviour
{
    private string m_MessageString = "Waiting for assembly";


    void OnAssemblyLoaded(WWWAssembly loadedAssembly)
    {
        m_MessageString = "Assembly " + loadedAssembly.URL + "\n";

        System.Type type = loadedAssembly.Assembly.GetType("MyClass");

        FieldInfo field = type.GetField("myString");
        m_MessageString += (field.GetValue(null) as string) + "\n";

        object instance = loadedAssembly.Assembly.CreateInstance("MyClass");
        MethodInfo method = type.GetMethod("LogMyString");
        m_MessageString += "Return value: " + method.Invoke(instance, null).ToString();
    }


    void OnAssemblyLoadFailed(string url)
    {
        m_MessageString = "Failed to load assembly at " + url;
    }


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0.0f, 0.0f, Screen.width, Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box(m_MessageString);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
}