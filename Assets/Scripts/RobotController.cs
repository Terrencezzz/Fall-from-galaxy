using UnityEngine;

public class RobotController : CharacterControllerBase
{
    protected override void MoveCharacter()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = (cameraTransform.right * moveX + cameraTransform.forward * moveZ).normalized * speed;

        if (controller.isGrounded)
        {
            yVelocity = -0.5f;
            if (Input.GetButtonDown("Jump"))
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;
        controller.Move(move * Time.deltaTime);
    }
}
