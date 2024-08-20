using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public Transform buttonTop; // The top part of the button that moves
    public float activationThreshold = 0.1f; // The distance at which the button activates
    public bool isActivated = false;

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
    }

    void DeactivateButton()
    {
        isActivated = false;
        Debug.Log("Button Released!");
    }
}
