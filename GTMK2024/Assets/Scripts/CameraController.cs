using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player;

    [Header("Camera Movement Limits")] 
    
    [SerializeField] private float minCameraHeight;
    [SerializeField] private float maxCameraHeight;
    [SerializeField] private float cameraXOffset;
    [SerializeField] private float movementSpeed = 5f;

    [Header("Camera Scaling Variables")] [SerializeField]
    private float currentCameraSize = 5f; // Base size for the camera

    [SerializeField] private float cameraScaleChange = 0.8f; //How much to increase or decrease the camera
    //as the player becomes bigger or smaller


    private Camera cameraComponent;
    private Vector3 cameraTempPos;

    private Vector2 playerPosition;
    private Vector2 currentCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial camera size
        cameraComponent = gameObject.GetComponent<Camera>();
        cameraComponent.orthographicSize = currentCameraSize;

        //Set the camera Z position
        cameraTempPos.z = -10f;

        playerPosition = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.IsUnityNull())
        {
            player = GameObject.FindWithTag("Player");
            minCameraHeight = player.GetComponent<PlayerMovement>().minCameraHeight;
            maxCameraHeight = player.GetComponent<PlayerMovement>().maxCameraHeight;
        }
            
        playerPosition = player.transform.position;
        MoveCamera();
    }

    public void ChangeCameraScale(int sizeMultiplier)
    {
        // Adjust the camera size proportionally to the player's size
        cameraComponent.orthographicSize = currentCameraSize + cameraScaleChange * sizeMultiplier;
    }
    
    public void MoveCamera()
    {
        //follows player in x
        cameraTempPos.x = playerPosition.x + cameraXOffset;

        //follows player in y within the limits given
        if (playerPosition.y + 0.69f >= maxCameraHeight)
            cameraTempPos.y = maxCameraHeight;
        else if (playerPosition.y + 0.69f <= minCameraHeight)
            cameraTempPos.y = minCameraHeight;
        else
            cameraTempPos.y = playerPosition.y + 0.69f;
        

        transform.position = cameraTempPos;
    }
/*
    public void MoveCamera()
    {
        //follows player in x

        if (cameraTempPos.x <= playerPosition.x + cameraXOffset)
            cameraTempPos.x += movementSpeed;
        else 
            cameraTempPos.x -= movementSpeed;

        //follows player in y within the limits given
        if (playerPosition.y >= maxCameraHeight)
            cameraTempPos.y = maxCameraHeight;
        else if (playerPosition.y <= minCameraHeight)
            cameraTempPos.y = minCameraHeight;
        else
            cameraTempPos.y = playerPosition.y + 0.69f;
        

        transform.position = cameraTempPos;
    }*/
}
