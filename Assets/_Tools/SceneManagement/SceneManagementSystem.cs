using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementSystem : MonoBehaviour
{
    [SerializeField] private Canvas _fadeToBlackObject;
    [Header("Settings")]
    [SerializeField] private bool _isTransitionOn = true;

    [SerializeField]
    [Range(0, 500)]
    [Tooltip("Time in milliseconds to fade to black.To be based on the animation time.")]
    private float _timeToBlack = 100;

    private static SceneManagementSystem _instance;

    public static SceneManagementSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("SceneManagementSystem");
                _instance = obj.AddComponent<SceneManagementSystem>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (_fadeToBlackObject == null)
        {
            Debug.LogError("Fade to black object is not assigned in the inspector. Turning transition off.");
            _isTransitionOn = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        if (_isTransitionOn)
        {
            Instantiate(_fadeToBlackObject, Vector3.zero, Quaternion.identity);
            StartCoroutine(LoadSceneAfterTransition(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ReloadCurrentScene()
    {
        if (_isTransitionOn)
        {
            Instantiate(_fadeToBlackObject, Vector3.zero, Quaternion.identity);
            StartCoroutine(LoadSceneAfterTransition(SceneManager.GetActiveScene().name));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator LoadSceneAfterTransition(string sceneName)
    {
        yield return new WaitForSeconds(_timeToBlack * 2 / 1000f);
        SceneManager.LoadScene(sceneName);
    }
}
