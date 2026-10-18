using UnityEngine;

public class TestEnemyZach : MonoBehaviour
{
    [SerializeField] private Material[] mats;
    private int textureLevel;
    private int meshLevel;
    private int soundLevel;
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        textureLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Texture);
        meshLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Mesh);
        soundLevel = BuyHistory.Instance.GetLevel(UnitType.CommonEnemy, UpgradeCategory.Sound);

        UpgradeTexture();
        UpgradeMesh();
        UpgradeSound();
        
        BattleStats.Instance.Subscribe<BuyAction>(OnBoughtEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBoughtEvent(BuyAction action)
    {
        Debug.Log($"Bought: {action}");
        if(action.Type != UnitType.CommonEnemy) return;

        switch (action.Category)
        {
            case UpgradeCategory.Texture:
                textureLevel++;
                UpgradeTexture();
                break;
            case UpgradeCategory.Mesh:
                meshLevel++;
                UpgradeMesh();
                break;
            case UpgradeCategory.Sound:
                soundLevel++;
                UpgradeSound();
                break;
        }
    }

    private void UpgradeTexture()
    {
        if (textureLevel <= 0 || mats.Length == 0) return;
        int i = Mathf.Min(textureLevel, mats.Length) - 1;
        rend.material = mats[i];
    }
    private void UpgradeMesh()
    {
        
    }
    private void UpgradeSound()
    {
        
    }
}
