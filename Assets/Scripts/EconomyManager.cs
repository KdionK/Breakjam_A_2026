using System.Collections;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private NumberPopUpSpawner popUpSpawner;

    /// Le temps d'attente entre chaque mise à jour de l'animation
    [SerializeField][Range(0.001f, 0.5f)] float _secondesEntreUpdateAnimation = 0.005f;

    [SerializeField] 
    [Tooltip("The amount of currency the player has. Serialized for test purposes")] 
    private int _currency = 0;

    public int Currency
    {
        get { return _currency; }
    }

    private static EconomyManager instance;
    public static EconomyManager Instance => instance;

    private Coroutine _currentCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        _numberText.text = _currency.ToString();
    }

    private IEnumerator COChangeNumber(int amountToChangeBy)
    {
        _currency += amountToChangeBy;
        Debug.Log("Currency changed by: " + amountToChangeBy + ". New currency: " + _currency);
        int CountToShow = _currency - amountToChangeBy;
        popUpSpawner.SpawnNumberPopUp(amountToChangeBy);
        if (amountToChangeBy < 0)
        {
            while (CountToShow > _currency)
            {
                //ajout d'un point
                CountToShow--;
                _numberText.transform.localScale += 0.1f * Vector3.one;//ajouter à l'échelle

                _numberText.text = CountToShow.ToString();
                yield return new WaitForSeconds(_secondesEntreUpdateAnimation*amountToChangeBy);//attente entre chaque mise à jour
            }
        }
        else if (amountToChangeBy > 0)
        {
            while (CountToShow < _currency)
            {
                //ajout d'un point
                CountToShow++;
                _numberText.transform.localScale += 0.05f * Vector3.one;//ajouter à l'échelle

                _numberText.text = CountToShow.ToString(); ;//mise à jour du texte
                yield return new WaitForSeconds(_secondesEntreUpdateAnimation*amountToChangeBy);//attente entre chaque mise à jour
            }
        }
        _currentCoroutine = StartCoroutine(COEcraser());
    }

    IEnumerator COEcraser()
    {
        float scaleQuotient = 0.2f;
        while (_numberText.transform.localScale.x >= 1)
        {
            _numberText.transform.localScale -= scaleQuotient* Vector3.one;
            yield return new WaitForEndOfFrame();
        }
        _numberText.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Change the amount of currency the player has. If the player doesn't have enough money, the change will not be applied.
    /// </summary>
    /// <param name="amountToChangeBy"></param>
    public void ChangeCurrency(int amountToChangeBy)
    {
        if (amountToChangeBy < 0 && _currency + amountToChangeBy < 0)
        {
            return;
            //TODO: add a pop up to show the player that he doesn't have enough money
            //TODO: add a sound to show the player that he doesn't have enough money
            //Debug.Log("Not enough money");
        }
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        
        _numberText.text = _currency.ToString();
        _currentCoroutine = StartCoroutine(COChangeNumber(amountToChangeBy));
        return;
    }
}
