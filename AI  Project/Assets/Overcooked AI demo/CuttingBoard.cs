﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : BaseInteractable
{
    public GameObject ItemPrefab;
    public Transform ItemHolder;
    private GameObject ItemOnPlate;

    public override void Interact(IInteractor interactor)
    {
        if (ItemOnPlate == null)
        {
           ItemOnPlate = Instantiate(ItemPrefab, ItemHolder);
        }
    }

}
