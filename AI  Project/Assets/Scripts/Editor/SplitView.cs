using UnityEngine.UIElements;

public class SplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

    public SplitView()
    {
        var styleSheet = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/GOAP/Editor/GOAP_VisualizerEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
