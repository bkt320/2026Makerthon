using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PollutionManager : MonoBehaviour
{
    [SerializeField] private float pollution = 0f;
    [SerializeField] private float maxPollution = 100f;

    [Header("오염도 UI")]
    [SerializeField] private Slider pollutionSlider;

    [SerializeField] private TMP_Text pollutionText;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pollutionSlider != null)
        {
            pollutionSlider.minValue = 0f;
            pollutionSlider.maxValue =
                maxPollution;

            pollutionSlider.value =
                pollution;
        }
        UpdatePollutionUI();
    }

    public void AddPollution(float amount)
    {
        pollution += amount;

        if (pollution > maxPollution)
        {
            pollution = maxPollution;
        }

        UpdatePollutionUI();

        Debug.Log(
            "현재 오염도: " +
            pollution +
            " / " +
            maxPollution
        );

        if (pollution >= maxPollution)
        {
            GameOver();
        }
    }

    private void UpdatePollutionUI(){
    if (pollutionSlider != null){
        pollutionSlider.value = pollution;
    }

    if (pollutionText != null){
        pollutionText.text =
            "오염도 : " +
            pollution.ToString("0") +
            " % ";
    }
}

    private void GameOver()
    {
        Debug.Log(
            "게임 오버: 오염도가 100이 되었습니다."
        );

        Time.timeScale = 0f;
    }
}