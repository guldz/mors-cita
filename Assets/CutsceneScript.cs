using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay(23f));
    }

    public void skip()
    {
        SceneManager.LoadSceneAsync("Map");
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Map");
    }
}