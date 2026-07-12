using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PollutionManager : MonoBehaviour
{
    [SerializeField] private float pollution = 0f;
    [SerializeField] private float maxPollution = 100f;

    [Header("오염도 UI")]
    [SerializeField] private Slider pollutionSlider;
    [SerializeField] private TMP_Text pollutionText;

    [Header("게임 오버")]
    [SerializeField] private TrashSpawner trashSpawner;
    [SerializeField] private int gameOverTrashCount = 100;
    [SerializeField] private float gameOverDelay = 1f;

    [Header("씬")]
    [SerializeField] private string gameOverSceneName = "GameOver";

    private bool gameOver;

    private void Start(){
        Time.timeScale = 1f;

        if (trashSpawner == null){
            trashSpawner =
                FindFirstObjectByType<TrashSpawner>();
        }

        if (pollutionSlider != null){
            pollutionSlider.minValue = 0f;
            pollutionSlider.maxValue =
                maxPollution;
        }

        UpdatePollutionUI();
    }

    public void AddPollution(
        float amount
    ){
        if (gameOver){
            return;
        }

        pollution += amount;

        pollution = Mathf.Clamp(
            pollution,
            0f,
            maxPollution
        );

        UpdatePollutionUI();

        if (pollution >= maxPollution){
            StartCoroutine(
                GameOverSequence()
            );
        }
    }

    private void UpdatePollutionUI(){
        if (pollutionSlider != null){
            pollutionSlider.value =
                pollution;
        }

        if (pollutionText != null){
            pollutionText.text =
                "오염도 : " +
                pollution.ToString("0") +
                " %";
        }
    }

    private IEnumerator GameOverSequence(){
        if (gameOver){
            yield break;
        }

        gameOver = true;

        if (trashSpawner != null){
            trashSpawner.FillScreenWithTrash(
                gameOverTrashCount
            );
        }

        yield return new WaitForSeconds(
            gameOverDelay
        );

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            gameOverSceneName
        );
    }
}
