using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private TrashSpawner trashSpawner;
    [SerializeField] private EndingItemSpawner endingItemSpawner;

    [Header("UI")]
    [SerializeField] private TMP_Text trashCountText;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text spawnTimerText;
    [SerializeField] private TMP_Text endingItemText;

    [Header("씬")]
    [SerializeField] private string endingSceneName = "Ending";

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

    [Header("엔딩 아이템")]
    [SerializeField] private int endingItemCountPerPhase = 3;
    [SerializeField] private float endingItemSpawnDelayMin = 3f;
    [SerializeField] private float endingItemSpawnDelayMax = 8f;
    [SerializeField] private int endingItemTargetMin = 3;
    [SerializeField] private int endingItemTargetMax = 6;

    private int currentPhase;
    private int collectedTrashCount;
    private int targetTrashCount;
    private int currentTrashCount;

    private int collectedEndingItemCount;
    private int endingItemTargetCount;

    private float spawnTimer;

    private bool phaseActive;
    private bool phaseChanging;

    private bool endingConditionOneCompleted;
    private bool endingConditionTwoCompleted;
    private bool endingCompleted;

    private Coroutine trashSpawnCoroutine;
    private Coroutine endingItemSpawnCoroutine;

    private void Start(){
        FindManagers();

        int minTarget = Mathf.Min(
            endingItemTargetMin,
            endingItemTargetMax
        );

        int maxTarget = Mathf.Max(
            endingItemTargetMin,
            endingItemTargetMax
        );

        endingItemTargetCount = Random.Range(
            minTarget,
            maxTarget + 1
        );

        UpdateTrashUI();
        UpdatePhaseUI();
        UpdateEndingItemUI();

        StartCoroutine(StartGame());
    }

    private void Update(){
        UpdateSpawnTimerUI();
    }

    private void FindManagers(){
        if (trashSpawner == null){
            trashSpawner =
                FindFirstObjectByType<TrashSpawner>();
        }

        if (endingItemSpawner == null){
            endingItemSpawner =
                FindFirstObjectByType<EndingItemSpawner>();
        }
    }

    private IEnumerator StartGame(){
        yield return StartCoroutine(
            StartPhase(
                1,
                phase1StartCount,
                phase1AddCount,
                phase1SpawnTime,
                Random.Range(
                    phase1MinTarget,
                    phase1MaxTarget + 1
                )
            )
        );

        yield return StartCoroutine(
            WaitForNextPhase()
        );

        yield return StartCoroutine(
            StartPhase(
                2,
                phase2StartCount,
                phase2AddCount,
                phase2SpawnTime,
                Random.Range(
                    phase2MinTarget,
                    phase2MaxTarget + 1
                )
            )
        );

        yield return StartCoroutine(
            WaitForNextPhase()
        );

        yield return StartCoroutine(
            StartPhase(
                3,
                phase3StartCount,
                phase3AddCount,
                phase3SpawnTime,
                phase3Target
            )
        );

        CompleteEndingConditionOne();
    }

    private IEnumerator StartPhase(
        int phaseNumber,
        int startCount,
        int addCount,
        float spawnTime,
        int targetCount
    ){
        currentPhase = phaseNumber;
        targetTrashCount = targetCount;
        collectedTrashCount = 0;

        phaseActive = true;
        phaseChanging = false;

        UpdatePhaseUI();

        if (trashSpawner != null){
            trashSpawner.SpawnTrash(
                startCount
            );
        }

        trashSpawnCoroutine = StartCoroutine(
            SpawnTrashRepeatedly(
                addCount,
                spawnTime
            )
        );

        endingItemSpawnCoroutine = StartCoroutine(
            SpawnEndingItemsDuringPhase()
        );

        yield return new WaitUntil(
            () => collectedTrashCount >= targetTrashCount
        );

        phaseActive = false;

        StopPhaseCoroutines();
    }

    private IEnumerator SpawnTrashRepeatedly(
        int spawnCount,
        float spawnTime
    ){
        while (phaseActive){
            spawnTimer = spawnTime;

            while (spawnTimer > 0f &&
                phaseActive){
                spawnTimer -= Time.deltaTime;

                if (spawnTimer < 0f){
                    spawnTimer = 0f;
                }

                yield return null;
            }

            if (phaseActive &&
                trashSpawner != null){
                trashSpawner.SpawnTrash(
                    spawnCount
                );
            }
        }
    }

    private IEnumerator SpawnEndingItemsDuringPhase(){
        if (endingItemSpawner == null){
            Debug.LogWarning(
                "EndingItemSpawner가 연결되지 않았습니다."
            );

            yield break;
        }

        int spawnedCount = 0;

        while (phaseActive &&
            spawnedCount < endingItemCountPerPhase){
            float minDelay = Mathf.Min(
                endingItemSpawnDelayMin,
                endingItemSpawnDelayMax
            );

            float maxDelay = Mathf.Max(
                endingItemSpawnDelayMin,
                endingItemSpawnDelayMax
            );

            float remainingDelay = Random.Range(
                minDelay,
                maxDelay
            );

            while (remainingDelay > 0f &&
                phaseActive){
                remainingDelay -= Time.deltaTime;

                yield return null;
            }

            if (!phaseActive){
                yield break;
            }

            int createdCount =
                endingItemSpawner.SpawnEndingItems(1);

            if (createdCount > 0){
                spawnedCount += createdCount;
            }
        }
    }

    private IEnumerator WaitForNextPhase(){
        phaseChanging = true;

        float remainingTime = phaseChangeDelay;

        while (remainingTime > 0f){
            spawnTimer = remainingTime;
            remainingTime -= Time.deltaTime;

            yield return null;
        }

        spawnTimer = 0f;
        phaseChanging = false;
    }

    private void StopPhaseCoroutines(){
        if (trashSpawnCoroutine != null){
            StopCoroutine(
                trashSpawnCoroutine
            );

            trashSpawnCoroutine = null;
        }

        if (endingItemSpawnCoroutine != null){
            StopCoroutine(
                endingItemSpawnCoroutine
            );

            endingItemSpawnCoroutine = null;
        }
    }

    public void CollectTrash(){
        if (!phaseActive){
            return;
        }

        collectedTrashCount++;

        if (collectedTrashCount >
            targetTrashCount){
            collectedTrashCount =
                targetTrashCount;
        }

        UpdatePhaseUI();
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

    public void CollectEndingItem(){
        if (endingConditionTwoCompleted){
            return;
        }

        collectedEndingItemCount++;

        UpdateEndingItemUI();

        if (collectedEndingItemCount >=
            endingItemTargetCount){
            CompleteEndingConditionTwo();
        }
    }

    private void CompleteEndingConditionOne(){
        if (endingConditionOneCompleted){
            return;
        }

        endingConditionOneCompleted = true;

        CheckEnding();
    }

    private void CompleteEndingConditionTwo(){
        if (endingConditionTwoCompleted){
            return;
        }

        endingConditionTwoCompleted = true;

        UpdateEndingItemUI();

        CheckEnding();
    }

    private void CheckEnding(){
        if (endingCompleted){
            return;
        }

        if (endingConditionOneCompleted &&
            endingConditionTwoCompleted){
            endingCompleted = true;
            phaseActive = false;
            phaseChanging = false;

            StopPhaseCoroutines();

            Time.timeScale = 1f;

            SceneManager.LoadScene(
                endingSceneName
            );
        }
    }

    private void UpdateTrashUI(){
        if (trashCountText != null){
            trashCountText.text =
                "남은 쓰레기 : " +
                currentTrashCount +
                "개";
        }
    }

    private void UpdatePhaseUI(){
        if (phaseText == null){
            return;
        }

        if (currentPhase <= 0){
            phaseText.text =
                "게임 준비 중";

            return;
        }

        phaseText.text =
            "Phase " +
            currentPhase +
            "\n수거 : " +
            collectedTrashCount +
            " / " +
            targetTrashCount;
    }

    private void UpdateSpawnTimerUI(){
        if (spawnTimerText == null){
            return;
        }

        int seconds = Mathf.CeilToInt(
            spawnTimer
        );

        if (phaseChanging){
            spawnTimerText.text =
                "다음 페이즈까지 : " +
                seconds +
                "초";

            return;
        }

        if (phaseActive){
            spawnTimerText.text =
                "다음 쓰레기 생성까지 : " +
                seconds +
                "초";

            return;
        }

        spawnTimerText.text = "";
    }

    private void UpdateEndingItemUI(){
        if (endingItemText == null){
            return;
        }

        if (endingConditionTwoCompleted){
            endingItemText.text =
                "엔딩 아이템 수집 완료!";

            return;
        }

        endingItemText.text =
            "엔딩 아이템 : " +
            collectedEndingItemCount +
            " / " +
            endingItemTargetCount;
    }
}
