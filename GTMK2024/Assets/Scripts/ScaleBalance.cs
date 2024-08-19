using UnityEngine;

public class ScaleMechanism : MonoBehaviour
{
    public Transform plank;
    public Transform[] pans; // Array for multiple pans
    public float panOffset = 0.5f; // Adjust this offset as needed

    void Update()
    {
        // Update pan positions to follow the plank, with an offset if needed
        foreach (Transform pan in pans)
        {
            Vector3 plankPosition = plank.position;
            // Set pan position relative to the plank
            pan.position = new Vector3(plankPosition.x + panOffset, plankPosition.y, plankPosition.z);
        }
    }
}
