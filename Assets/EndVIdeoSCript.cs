using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class EndVIdeoSCript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay(26f));
    }

    // Update is called once per frame
    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main Menu");
    }
}
