using UnityEngine;

public class TransitionRemoval : MonoBehaviour
{


    public void RemoveTransitionObject()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
