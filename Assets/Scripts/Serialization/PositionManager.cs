using UnityEngine;
using System.Collections;
using UnityEditor;

public class PositionManager : MonoBehaviour
{
    // define a menu option in the editor to create the new asset
    [MenuItem("Assets/Create/PositionManager")]
    public static void CreateAsset()
    {
        // create a new scriptable object
        ScriptingObjects positionManager = ScriptableObject.CreateInstance<ScriptingObjects>();

        // create a .asset file for our new objcet and save it 
        AssetDatabase.CreateAsset(positionManager, "Assets/newPositionManager.asset");
        AssetDatabase.SaveAssets();

        // now switch the inspector to our new object
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = positionManager;
    }
}
