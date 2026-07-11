using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    private Vector2 moveInput;

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private float playerHalfWidth;
    private float playerHalfHeight;

    private void Start(){
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        CalculatePlayerSize();
    }

    private void Update(){
        ReadMovementInput();
        MovePlayer();
        RotatePlayer();
    }

    private void ReadMovementInput(){
#if ENABLE_INPUT_SYSTEM
        moveInput = Vector2.zero;

        if (Keyboard.current == null){
            return;
        }

        if (Keyboard.current.aKey.isPressed){
            moveInput.x -= 1f;
        }

        if (Keyboard.current.dKey.isPressed){
            moveInput.x += 1f;
        }

        if (Keyboard.current.sKey.isPressed){
            moveInput.y -= 1f;
        }

        if (Keyboard.current.wKey.isPressed){
            moveInput.y += 1f;
        }
#else
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
#endif

        moveInput = moveInput.normalized;
    }

    private void MovePlayer(){
        Vector3 moveDirection = new Vector3(
            moveInput.x,
            moveInput.y,
            0f
        );

        Vector3 nextPosition =
            transform.position +
            moveDirection *
            moveSpeed *
            Time.deltaTime;

        transform.position =
            ClampPositionToCamera(nextPosition);
    }

    private void RotatePlayer(){
        if (moveInput == Vector2.zero){
            return;
        }

        float angle = Mathf.Atan2(
            moveInput.y,
            moveInput.x
        ) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(
            0f,
            0f,
            angle - 90f
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private Vector3 ClampPositionToCamera(
        Vector3 targetPosition
    ){
        if (mainCamera == null){
            return targetPosition;
        }

        float distanceFromCamera =
            Mathf.Abs(
                transform.position.z -
                mainCamera.transform.position.z
            );

        Vector3 bottomLeft =
            mainCamera.ViewportToWorldPoint(
                new Vector3(
                    0f,
                    0f,
                    distanceFromCamera
                )
            );

        Vector3 topRight =
            mainCamera.ViewportToWorldPoint(
                new Vector3(
                    1f,
                    1f,
                    distanceFromCamera
                )
            );

        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            bottomLeft.x + playerHalfWidth,
            topRight.x - playerHalfWidth
        );

        targetPosition.y = Mathf.Clamp(
            targetPosition.y,
            bottomLeft.y + playerHalfHeight,
            topRight.y - playerHalfHeight
        );

        return targetPosition;
    }

    private void CalculatePlayerSize(){
        if (spriteRenderer == null){
            playerHalfWidth = 0f;
            playerHalfHeight = 0f;
            return;
        }

        playerHalfWidth =
            spriteRenderer.bounds.extents.x;

        playerHalfHeight =
            spriteRenderer.bounds.extents.y;
    }
}