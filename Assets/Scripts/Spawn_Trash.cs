using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject trashObject;
    [SerializeField] private Sprite[] trashSprites;

    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [SerializeField] private float spawnZ = -1f;

    private void Awake(){
        trashObject.SetActive(false);
    }

    public void SpawnTrash(int spawnCount){
        for (int i = 0; i < spawnCount; i++){
            SpawnOneTrash();
        }
    }

    private void SpawnOneTrash(){
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 randomPosition = new Vector3(
            randomX,
            randomY,
            spawnZ
        );

        GameObject newTrash = Instantiate(
            trashObject,
            randomPosition,
            Quaternion.identity
        );

        SpriteRenderer spriteRenderer =
            newTrash.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null &&
            trashSprites.Length > 0){
            int randomIndex = Random.Range(
                0,
                trashSprites.Length
            );

            spriteRenderer.sprite =
                trashSprites[randomIndex];
        }

        newTrash.SetActive(true);

        PhaseManager phaseManager =
            FindFirstObjectByType<PhaseManager>();

        if (phaseManager != null){
            phaseManager.AddTrash();
        }
    }
}