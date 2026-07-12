using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    [SerializeField] private string lobbySceneName = "Lobby";

    public void ReturnToLobby(){
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            lobbySceneName
        );
    }
}
