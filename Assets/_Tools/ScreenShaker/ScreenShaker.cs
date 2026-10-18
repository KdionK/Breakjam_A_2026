using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public static ScreenShaker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayConstantRumble()
    {
        
    }

}
