using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Attach to a trigger at the end of each level and set the next scene name.</summary>
public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) SceneManager.LoadScene(nextSceneName);
    }
}
