using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGamer()
    {
        SceneManager.LoadScene("game");
    }
}
