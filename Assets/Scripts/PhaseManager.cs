using System.Collections;
using UnityEngine;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] private TrashSpawner trashSpawner;

    [SerializeField] private TMP_Text trashCountText;

    [Header("페이즈 전환")]
    [SerializeField] private float phaseChangeDelay = 5f;

    [Header("1페이즈")]
    [SerializeField] private int phase1StartCount = 10;
    [SerializeField] private int phase1AddCount = 3;
    [SerializeField] private float phase1SpawnTime = 5f;
    [SerializeField] private int phase1MinTarget = 20;
    [SerializeField] private int phase1MaxTarget = 30;

    [Header("2페이즈")]
    [SerializeField] private int phase2StartCount = 15;
    [SerializeField] private int phase2AddCount = 5;
    [SerializeField] private float phase2SpawnTime = 4f;
    [SerializeField] private int phase2MinTarget = 25;
    [SerializeField] private int phase2MaxTarget = 40;

    [Header("3페이즈")]
    [SerializeField] private int phase3StartCount = 20;
    [SerializeField] private int phase3AddCount = 8;
    [SerializeField] private float phase3SpawnTime = 3f;
    [SerializeField] private int phase3Target = 40;

    private int currentPhase;
    private int collectedTrashCount;
    private int targetTrashCount;

    private int currentTrashCount;

    private bool phaseActive;

    private Coroutine spawnCoroutine;

    private void Start(){
        UpdateTrashUI();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame(){
        yield return StartCoroutine(StartPhase1());

        yield return new WaitForSeconds(phaseChangeDelay);

        yield return StartCoroutine(StartPhase2());

        yield return new WaitForSeconds(phaseChangeDelay);

        yield return StartCoroutine(StartPhase3());

        EndingConditionOne();
    }

    private IEnumerator StartPhase1(){
        currentPhase = 1;

        targetTrashCount = Random.Range(
            phase1MinTarget,
            phase1MaxTarget + 1
        );

        yield return StartCoroutine(
            RunPhase(
                phase1StartCount,
                phase1AddCount,
                phase1SpawnTime
            )
        );
    }

    private IEnumerator StartPhase2(){
        currentPhase = 2;

        targetTrashCount = Random.Range(
            phase2MinTarget,
            phase2MaxTarget + 1
        );

        yield return StartCoroutine(
            RunPhase(
                phase2StartCount,
                phase2AddCount,
                phase2SpawnTime
            )
        );
    }

    private IEnumerator StartPhase3(){
        currentPhase = 3;
        targetTrashCount = phase3Target;

        yield return StartCoroutine(
            RunPhase(
                phase3StartCount,
                phase3AddCount,
                phase3SpawnTime
            )
        );
    }

    private IEnumerator RunPhase(
        int startCount,
        int addCount,
        float spawnTime
    ){
        collectedTrashCount = 0;
        phaseActive = true;

        trashSpawner.SpawnTrash(startCount);

        spawnCoroutine = StartCoroutine(
            SpawnTrashRepeatedly(
                addCount,
                spawnTime
            )
        );

        yield return new WaitUntil(
            () => collectedTrashCount >= targetTrashCount
        );

        phaseActive = false;

        if (spawnCoroutine != null){
            StopCoroutine(spawnCoroutine);
        }
    }

    private IEnumerator SpawnTrashRepeatedly(
        int spawnCount,
        float spawnTime
    ){
        while (phaseActive){
            yield return new WaitForSeconds(
                spawnTime
            );

            if (phaseActive){
                trashSpawner.SpawnTrash(
                    spawnCount
                );
            }
        }
    }

    public void CollectTrash(){
        if (!phaseActive){
            return;
        }

        collectedTrashCount++;
    }

    public void AddTrash(){
        currentTrashCount++;
        UpdateTrashUI();
    }

    public void RemoveTrash(){
        currentTrashCount--;

        if (currentTrashCount < 0){
            currentTrashCount = 0;
        }

        UpdateTrashUI();
    }

    private void UpdateTrashUI(){
        if (trashCountText != null){
            trashCountText.text =
                "남은 쓰레기 : " +
                currentTrashCount +
                "개";
        }
    }

    private void EndingConditionOne(){
        Debug.Log("엔딩 조건 1 달성");
    }
}