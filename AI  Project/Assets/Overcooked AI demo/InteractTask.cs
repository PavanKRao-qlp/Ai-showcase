using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class InteractTask : BaseTask
{
    public IInteractable interactableObject;
    public IInteractor interactor;
    public override async Task<bool> Execute(CancellationToken token)
    {
        interactor.Interact(interactableObject);
        return true;
    }
}
