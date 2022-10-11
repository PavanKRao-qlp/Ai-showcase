using UnityEngine.UIElements;

public class EmptyUINode : VisualElement
{
    public new class UxmlFactory : UxmlFactory<EmptyUINode, VisualElement.UxmlTraits> { }
    public EmptyUINode()
    {

    }
}