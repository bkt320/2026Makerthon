using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class EndingItem : MonoBehaviour
{
    [SerializeField] private float disappearTime = 5f;

    private PhaseManager phaseManager;

    private float aliveTime;
    private bool playerInRange;
    private bool collected;

    private void OnEnable(){
        aliveTime = 0f;
        playerInRange = false;
        collected = false;

        phaseManager =
            FindFirstObjectByType<PhaseManager>();
    }

    private void Update(){
        aliveTime += Time.deltaTime;

        if (aliveTime >= disappearTime){
            Destroy(gameObject);
            return;
        }

        if (!playerInRange || collected){
            return;
        }

        if (WasCollectKeyPressed()){
            CollectItem();
        }
    }

    private bool WasCollectKeyPressed(){
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null){
            return false;
        }

        return Keyboard.current
            .spaceKey
            .wasPressedThisFrame;
#else
        return Input.GetKeyDown(
            KeyCode.Space
        );
#endif
    }

    private void CollectItem(){
        collected = true;

        if (phaseManager != null){
            phaseManager.CollectEndingItem();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(
        Collider2D other
    ){
        if (!other.CompareTag("Player")){
            return;
        }

        playerInRange = true;
    }

    private void OnTriggerExit2D(
        Collider2D other
    ){
        if (!other.CompareTag("Player")){
            return;
        }

        playerInRange = false;
    }
}
