using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string interactAnimation;
    public UnityEvent onInteract;

    private void OnDisable()
    {
        if (GameManager.instance.player.currentInteractable == this)
        {
            GameManager.instance.player.currentInteractable = null;
            GameManager.instance.ShowInteractUI(false);
        }
    }
}

