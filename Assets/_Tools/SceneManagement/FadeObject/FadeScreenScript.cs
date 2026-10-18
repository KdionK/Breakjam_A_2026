using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScreenScript : MonoBehaviour
{

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        GetComponentInChildren<Animator>().SetTrigger("FadeOut");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetComponentInChildren<Animator>().SetTrigger("FadeIn");
    }

}
