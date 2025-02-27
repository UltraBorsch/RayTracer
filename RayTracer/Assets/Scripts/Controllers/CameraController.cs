using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // Start is called before the first frame update
    public Quaternion rotation;
    public Vector3 movement;
    public Rigidbody body;
    public float movementSpeed, sensitivity;
    private float pitch, yaw, initialX, initialY;
    public bool lockMotion;
    public bool lockRotation;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked; //cursor settings
        Cursor.visible = false;
        rotation = transform.rotation; //get initial camera rotation. need to track rotation
        Vector3 initialRotation = transform.rotation.eulerAngles;
        initialX = Input.GetAxisRaw("Mouse X");
        initialY = Input.GetAxisRaw("Mouse Y");
        pitch = initialRotation.x; //set initial pitch and yaw
        yaw = initialRotation.y;
    }

    // Update is called once per frame
    void Update() {
        if (!lockMotion) {
            movement.x = Input.GetAxisRaw("Horizontal"); //is the player moving left/right (1 or -1), i.e. A or D
            movement.y = Input.GetAxisRaw("Jump"); //is the player moving up/down (1 or -1), i.e. space or shift
            movement.z = Input.GetAxisRaw("Vertical"); //is the player moving forward/backward (1 or -1), i.e. W or S
        }

        if (!lockRotation) {
            yaw += (Input.GetAxisRaw("Mouse X") - initialX) * sensitivity; //moving mouse to look left/right
            pitch -= (Input.GetAxisRaw("Mouse Y") - initialY) * sensitivity; //moving mouse to look up/down
        }
    }

    void FixedUpdate() {

        body.linearVelocity = movement.z * movementSpeed * transform.forward; //Add velocity to player relative to direction theyre looking
        body.linearVelocity += movement.x * movementSpeed * transform.right;
        body.linearVelocity += movement.y * movementSpeed * transform.up;

        transform.localRotation = Quaternion.Euler(new Vector3(pitch, yaw, 0)); //rotate players body along y axis (left/right), will affect what direction they move in since forward changes
    }
}
