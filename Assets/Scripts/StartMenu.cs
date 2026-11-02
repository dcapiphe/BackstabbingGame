using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelScene = "Level1";
    public void StartGame() => SceneManager.LoadScene(firstLevelScene);
    public void QuitGame() => Application.Quit();
}
