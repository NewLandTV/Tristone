using UnityEditor;
using UnityEngine;

public class QuickService : Editor
{
    private const string MENU_NAME = "Quick Service/";

    [MenuItem(MENU_NAME + "Github")]
    private static void Github()
    {
        Application.OpenURL("https://github.com/NewLandTV");
    }
}
