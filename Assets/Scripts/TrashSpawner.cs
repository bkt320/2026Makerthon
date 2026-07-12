using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject trashObject;
    [SerializeField] private Sprite[] trashSprites;

    [Header("생성 범위")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float spawnZ = -1f;

    private PhaseManager phaseManager;

    private void Awake(){
        if (trashObject == null){
            Debug.LogError(
                "Trash Object가 비어 있습니다."
            );

            return;
        }

        trashObject.SetActive(false);
    }

    private void Start(){
        phaseManager =
            FindFirstObjectByType<PhaseManager>();
    }

    public void SpawnTrash(
        int spawnCount
    ){
        for (int i = 0;
            i < spawnCount;
            i++){
            SpawnOneTrash(true);
        }
    }

    public void FillScreenWithTrash(
        int spawnCount
    ){
        for (int i = 0;
            i < spawnCount;
            i++){
            SpawnOneTrash(false);
        }
    }

    private void SpawnOneTrash(
        bool addToTrashCount
    ){
        if (trashObject == null){
            return;
        }

        Vector3 spawnPosition =
            new Vector3(
                Random.Range(
                    minX,
                    maxX
                ),
                Random.Range(
                    minY,
                    maxY
                ),
                spawnZ
            );

        GameObject newTrash =
            Instantiate(
                trashObject,
                spawnPosition,
                Quaternion.identity
            );

        SpriteRenderer spriteRenderer =
            newTrash.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null &&
            trashSprites != null &&
            trashSprites.Length > 0){
            spriteRenderer.sprite =
                trashSprites[
                    Random.Range(
                        0,
                        trashSprites.Length
                    )
                ];
        }

        newTrash.SetActive(true);

        if (!addToTrashCount){
            return;
        }

        if (phaseManager == null){
            phaseManager =
                FindFirstObjectByType<PhaseManager>();
        }

        if (phaseManager != null){
            phaseManager.AddTrash();
        }
    }
}
