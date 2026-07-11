using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject trashObject;
    [SerializeField] private Sprite[] trashSprites;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private int startTrashCount = 10;

    private void Start()
    {
        for (int i = 0; i < startTrashCount; i++)
        {
            SpawnTrash();
        }

        trashObject.SetActive(false);
    }

    private void SpawnTrash()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector2 randomPosition = new Vector2(randomX, randomY);

        GameObject newTrash = Instantiate(
            trashObject,
            randomPosition,
            Quaternion.identity
        );

        SpriteRenderer spriteRenderer =
            newTrash.GetComponent<SpriteRenderer>();

        int randomIndex = Random.Range(0, trashSprites.Length);

        spriteRenderer.sprite = trashSprites[randomIndex];

        newTrash.SetActive(true);
    }
}