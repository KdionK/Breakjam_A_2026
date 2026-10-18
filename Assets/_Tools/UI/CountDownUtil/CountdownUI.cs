using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private NumberPopUpSpawner popUpSpawner;

    /// Le temps d'attente entre chaque mise à jour de l'animation
    [SerializeField][Range(0.001f, 0.5f)] float _secondesEntreUpdateAnimation = 0.005f;

    [SerializeField] private int _countdown;

    private void OnValidate()
    {
        _numberText.text = _countdown.ToString();
    }

    public void ChangeCountUIBy(int amountToLower)
    {
        StartCoroutine(COChangeNumber(amountToLower));
    }

    private IEnumerator COChangeNumber(int amountToChangeBy)
    {
        _countdown -= amountToChangeBy;
        int CountToShow = _countdown - amountToChangeBy;
        popUpSpawner.SpawnNumberPopUp(amountToChangeBy);
        if (amountToChangeBy < 0)
        {
            while (CountToShow > _countdown)
            {
                //ajout d'un point
                CountToShow--;
                _numberText.transform.localScale += 0.1f * Vector3.one;//ajouter à l'échelle

                _numberText.text = CountToShow.ToString();
                yield return new WaitForSeconds(_secondesEntreUpdateAnimation);//attente entre chaque mise à jour
            }
        }
        else if (amountToChangeBy > 0)
        {
            while (CountToShow < _countdown)
            {
                //ajout d'un point
                CountToShow++;
                _numberText.transform.localScale += 0.1f * Vector3.one;//ajouter à l'échelle

                _numberText.text = CountToShow.ToString(); ;//mise à jour du texte
                yield return new WaitForSeconds(_secondesEntreUpdateAnimation);//attente entre chaque mise à jour
            }
        }
        StartCoroutine(COEcraser());
    }

    IEnumerator COEcraser()
    {
        float scaleQuotient = 0.1f;
        while (_numberText.transform.localScale.x >= 1)
        {
            _numberText.transform.localScale -= scaleQuotient * 2 * Vector3.one;
            yield return new WaitForEndOfFrame();
        }
        _numberText.transform.localScale = Vector3.one;
    }
}
