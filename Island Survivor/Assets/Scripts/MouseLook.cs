using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float mouseSensitivity = 1000f;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // read input
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // accumulate rotation values
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // apply pitch to the camera 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // apply yaw to the player capsule 
        transform.parent.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}