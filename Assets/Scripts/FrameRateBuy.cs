using UnityEngine;

public class FrameRateBuy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
        Application.targetFrameRate = 24;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBoughtEvent(BuyAction action)
    {
        if (action.Category == UpgradeCategory.Mesh)
        {
            Application.targetFrameRate = 999;
        }
    }
}
