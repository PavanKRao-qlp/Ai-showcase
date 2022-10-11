using UnityEditor;
using UnityEngine;

public class BTNodeSO : ScriptableObject
{
    public string Name;
    [ContextMenu("delete!")]
    private void Delete()
    {
        UnityEditor.Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
}
