using UnityEngine;
using System.Collections;

public class PressureButton : MonoBehaviour
{
    public Transform buttonTop; // The top part of the button that moves
    public float activationThreshold = 0.1f; // The distance at which the button activates
    public bool isActivated = false;

    public GameObject wall; // Reference to the wall GameObject
    public float deactivateDuration = 2f; // Time in seconds to keep the wall deactivated

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = buttonTop.localPosition;
    }

    void Update()
    {
        // Calculate how much the button has been pressed
        float pressedDistance = initialPosition.y - buttonTop.localPosition.y;

        // Check if the button is pressed enough to activate
        if (pressedDistance >= activationThreshold && !isActivated)
        {
            ActivateButton();
        }
        else if (pressedDistance < activationThreshold && isActivated)
        {
            DeactivateButton();
        }
    }

    void ActivateButton()
    {
        isActivated = true;
        Debug.Log("Button Pressed!");

        if (wall != null)
        {
            StartCoroutine(DeactivateWallCoroutine());
        }
    }

    void DeactivateButton()
    {
        isActivated = false;
        Debug.Log("Button Released!");
    }

    IEnumerator DeactivateWallCoroutine()
    {
        // Deactivate the wall
        wall.SetActive(false);

        // Wait for the specified duration
        yield return new WaitForSeconds(deactivateDuration);

        // Reactivate the wall
        wall.SetActive(true);
    }
}
