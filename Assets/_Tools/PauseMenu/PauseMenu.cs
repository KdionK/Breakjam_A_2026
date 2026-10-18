using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _SettingsMenuUI;

    public void OpenSettings()
    {
        _SettingsMenuUI.SetActive(true);
    }

    public void CloseSettings()
    {
        _SettingsMenuUI.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        gameObject.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        gameObject.SetActive(false);
        CloseSettings();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

}
