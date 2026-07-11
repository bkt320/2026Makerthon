using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private float pollutionDelay = 10f;
    [SerializeField] private float pollutionAmount = 5f;

    private PhaseManager phaseManager;
    private PollutionManager pollutionManager;

    private float aliveTime;
    private bool pollutionAdded;

    private void OnEnable(){
        aliveTime = 0f;
        pollutionAdded = false;

        phaseManager =
            FindFirstObjectByType<PhaseManager>();

        pollutionManager =
            FindFirstObjectByType<PollutionManager>();
    }

    private void Update(){
        aliveTime += Time.deltaTime;

        if (!pollutionAdded &&
            aliveTime >= pollutionDelay){
            pollutionAdded = true;

            if (pollutionManager != null){
                pollutionManager.AddPollution(
                    pollutionAmount
                );
            }

            if (phaseManager != null){
                phaseManager.RemoveTrash();
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")){
            return;
        }

        if (phaseManager != null){
            phaseManager.CollectTrash();
            phaseManager.RemoveTrash();
        }

        Destroy(gameObject);
    }
}