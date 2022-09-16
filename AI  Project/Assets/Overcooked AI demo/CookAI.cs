using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookAI : AiAgentBase , IInteractor
{
    public Vector2Int CellPos = Vector2Int.zero;
    public void Interact(IInteractable interactable)
    {
        interactable.Interact(this);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, 1f)) {
                Debug.Log(hitInfo.collider.name);
                var interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Interact(interactable);
                }
            }
        }
    }
}
