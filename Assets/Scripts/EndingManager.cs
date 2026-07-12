using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("북극곰")]
    [SerializeField] private GameObject polarBear;
    [SerializeField] private float polarBearAppearDelay = 1f;

    [Header("스토리 텍스트")]
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private float textDuration = 2.5f;

    [Header("아이템 전달 연출")]
    [SerializeField] private GameObject endingItemGiveImage;
    [SerializeField] private TMP_Text endingItemGiveText;

    [Header("엔딩 UI")]
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private TMP_Text endingText;

    [Header("게임 오버 UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;

    [Header("씬")]
    [SerializeField] private string lobbySceneName = "Lobby";

    private bool sequenceStarted;

    private void Start(){
        Time.timeScale = 1f;

        SetActive(
            polarBear,
            false
        );

        SetActive(
            endingItemGiveImage,
            false
        );

        SetActive(
            endingPanel,
            false
        );

        SetActive(
            gameOverPanel,
            false
        );

        if (storyText != null){
            storyText.text = "";
        }
    }

    public void StartEnding(
        int collectedEndingItemCount
    ){
        if (sequenceStarted){
            return;
        }

        sequenceStarted = true;

        StartCoroutine(
            EndingSequence(
                collectedEndingItemCount
            )
        );
    }

    private IEnumerator EndingSequence(
        int collectedEndingItemCount
    ){
        ClearRemainingObjects();

        yield return new WaitForSeconds(
            polarBearAppearDelay
        );

        SetActive(
            polarBear,
            true
        );

        yield return ShowText(
            "북극곰이 나타났다."
        );

        yield return ShowText(
            "바다의 쓰레기를 치워줘서 정말 고마워!"
        );

        yield return ShowText(
            "플레이어는 모은 아이템을 북극곰에게 건넸다."
        );

        SetActive(
            endingItemGiveImage,
            true
        );

        if (endingItemGiveText != null){
            endingItemGiveText.text =
                "엔딩 아이템 " +
                collectedEndingItemCount +
                "개를 건넸다.";
        }

        yield return new WaitForSeconds(
            textDuration
        );

        SetActive(
            endingItemGiveImage,
            false
        );

        yield return ShowText(
            "북극곰은 다시 한번 고맙다고 말했다."
        );

        yield return ShowText(
            "네 덕분에 다시 살아갈 수 있을 것 같아."
        );

        ShowEndingPanel();
    }

    private IEnumerator ShowText(
        string message
    ){
        if (storyText != null){
            storyText.text = message;
        }

        yield return new WaitForSeconds(
            textDuration
        );
    }

    private void ShowEndingPanel(){
        if (storyText != null){
            storyText.text = "";
        }

        SetActive(
            endingPanel,
            true
        );

        if (endingText != null){
            endingText.text =
                "바다를 지켜냈습니다!\n" +
                "북극곰에게 소중한 물건을 전달했습니다.";
        }

        Time.timeScale = 0f;
    }

    public void ShowGameOver(){
        if (sequenceStarted){
            return;
        }

        sequenceStarted = true;

        if (storyText != null){
            storyText.text = "";
        }

        SetActive(
            endingPanel,
            false
        );

        SetActive(
            gameOverPanel,
            true
        );

        if (gameOverText != null){
            gameOverText.text =
                "바다가 쓰레기로 가득 찼습니다.";
        }

        Time.timeScale = 0f;
    }

    public void ReturnToLobby(){
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            lobbySceneName
        );
    }

    private void ClearRemainingObjects(){
        Trash[] trashObjects =
            FindObjectsByType<Trash>(
                FindObjectsSortMode.None
            );

        foreach (Trash trashObject
            in trashObjects){
            if (trashObject != null &&
                trashObject.gameObject
                    .activeInHierarchy){
                Destroy(
                    trashObject.gameObject
                );
            }
        }

        EndingItem[] endingItems =
            FindObjectsByType<EndingItem>(
                FindObjectsSortMode.None
            );

        foreach (EndingItem endingItem
            in endingItems){
            if (endingItem != null &&
                endingItem.gameObject
                    .activeInHierarchy){
                Destroy(
                    endingItem.gameObject
                );
            }
        }
    }

    private void SetActive(
        GameObject target,
        bool active
    ){
        if (target != null){
            target.SetActive(
                active
            );
        }
    }
}
