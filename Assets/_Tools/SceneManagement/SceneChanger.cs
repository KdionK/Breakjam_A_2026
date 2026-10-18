using UnityEngine;
/// <summary>
/// This class is to be used as a component on any object that is used to change scenes.
/// This is mostly thought for buttons, box and square triggers
/// </summary>
public class SceneChanger : MonoBehaviour
{

    public void ChangeScene(string sceneToLoad)
    {
        if (string.IsNullOrEmpty(sceneToLoad) || sceneToLoad == "Default")
        {
            Debug.LogError("Scene to load is not assigned in the inspector.");
            return;
        }
        else if (!Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.LogError("Scene to load is not in the build settings.");
            return;
        }
        else if (sceneToLoad.ToLower() == "this")
        {
            SceneManagementSystem.Instance.ReloadCurrentScene();
            return;
        }
        else
        {
            SceneManagementSystem.Instance.LoadScene(sceneToLoad);
        }
    }
}
