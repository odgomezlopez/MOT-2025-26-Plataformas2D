using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Small script to demonstrate a simple interaction system
public class InteractorSimple : MonoBehaviour
{
    [SerializeField] InputActionReference interaction;
    [SerializeField] bool playerInArea = false;

    #region Unity Lifecycle
    private void OnEnable()
    {
        interaction.action.performed += OnInteractionPerformed;
    }

    private void OnDisable()
    {
        interaction.action.performed -= OnInteractionPerformed;
    }
    #endregion

    #region Trigger Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }
    #endregion

    #region Interaction Handling
    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        if (!playerInArea) return;
        throw new NotImplementedException();
    }
    #endregion
}
