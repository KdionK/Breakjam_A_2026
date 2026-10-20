using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animatorDoor1;
    [SerializeField] Animator animatorDoor2;


    private bool open1 = false;
    private bool open2 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }
    
    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");

        switch (action.Category)
        {
            case UpgradeCategory.Door1:
                OpenDoor();
                break;
            case UpgradeCategory.Door2:
                // add door2 logic here
                OpenDoor2();
                break;
        }
    }

    private void OpenDoor()
    {
        if (!open1)
        {
            animatorDoor1.SetTrigger("Open");
            open1 = true;
        }
    }
    
    private void OpenDoor2()
    {
        if (!open2)
        {
            animatorDoor2.SetTrigger("Open");
            open2 = true;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
