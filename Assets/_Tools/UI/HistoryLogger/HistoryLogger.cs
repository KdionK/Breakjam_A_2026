using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HistoryLogger : MonoBehaviour
{
    private static HistoryLogger _instance;
    public static HistoryLogger Instance { get { return _instance; } }

    [SerializeField] private GameObject LogEntry;
    [SerializeField] private RectTransform contentParent;

    void Awake()
    {
        _instance = this;
        GameObject logEntry = Instantiate(LogEntry, contentParent);
        logEntry.GetComponentInChildren<TextMeshProUGUI>().text =
        $"<color=grey>May this dungeon take your life slowly...";
    }


    public void LogDamage(string source, string dealtTo, string cause, string abilityName)
    {
        GameObject logEntry = Instantiate(LogEntry, contentParent);
        logEntry.GetComponentInChildren<TextMeshProUGUI>().text =
        $"<color=#{Color.red}>{source}</color>{cause}<color=#{Color.green}>{dealtTo.ToLower()}</color> with a <color=#{Color.yellow}>{abilityName}</color>\n";
    }

    public void LogVictory(string monsterName, int amountRewarded)
    {
        GameObject logEntry = Instantiate(LogEntry, contentParent);
        logEntry.GetComponentInChildren<TextMeshProUGUI>().text =
        $"The <color=#{Color.red}>{monsterName} </color> has been vanquished!\n {amountRewarded} turns has been added to your life";
    }

}
