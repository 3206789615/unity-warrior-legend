using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    private bool canPress;
    private IInteractable targetItem;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        Debug.Log("Æô¶¯");
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            Debug.Log("111");
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    private void OnActionChange(object obj, InputActionChange actionchange)
    {
        if (actionchange==InputActionChange.ActionStarted)
        {
            Debug.Log(((InputAction)obj).activeControl.device);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("Box");
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
