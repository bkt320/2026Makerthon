using UnityEngine;

public class EndingItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject endingItemObject;

    [Header("생성 범위")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float spawnZ = -1f;

    private void Awake(){
        if (endingItemObject == null){
            Debug.LogError(
                "Ending Item Object가 비어 있습니다."
            );

            return;
        }

        endingItemObject.SetActive(false);
    }

    public int SpawnEndingItems(
        int spawnCount
    ){
        if (endingItemObject == null){
            return 0;
        }

        int spawnedCount = 0;

        for (int i = 0;
            i < spawnCount;
            i++){
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

            GameObject newEndingItem =
                Instantiate(
                    endingItemObject,
                    spawnPosition,
                    Quaternion.identity
                );

            if (newEndingItem != null){
                newEndingItem.SetActive(true);
                spawnedCount++;
            }
        }

        return spawnedCount;
    }
}
