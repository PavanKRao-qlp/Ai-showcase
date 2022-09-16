using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public SpriteRenderer Image_;
    public TextMesh Text_;

    public void UpdateUI(Node2D_.TileState tileState)
    {
        if ((tileState & Node2D_.TileState.wall) == Node2D_.TileState.wall) {
            Image_.color = Color.black;
        }
    }
}
