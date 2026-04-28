using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Controls()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(6);
    }

    public void menu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}