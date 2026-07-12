using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    [Header("북극곰")]
    [SerializeField] private GameObject polarBear;
    [SerializeField] private float polarBearAppearDelay = 1f;

    [Header("텍스트")]
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private TMP_Text endingText;
    [SerializeField] private float textDuration = 2.5f;

    [Header("아이템 전달 연출")]
    [SerializeField] private GameObject endingItemGiveImage;

    [Header("로비")]
    [SerializeField] private GameObject lobbyButton;
    [SerializeField] private string lobbySceneName = "Lobby";

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
            lobbyButton,
            false
        );

        if (storyText != null){
            storyText.text = "";
        }

        if (endingText != null){
            endingText.text = "";
        }

        StartCoroutine(
            PlayEnding()
        );
    }

    private IEnumerator PlayEnding(){
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

        if (storyText != null){
            storyText.text = "";
        }

        if (endingText != null){
            endingText.text =
                "바다를 지켜냈습니다!";
        }

        SetActive(
            lobbyButton,
            true
        );
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

    public void ReturnToLobby(){
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            lobbySceneName
        );
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
