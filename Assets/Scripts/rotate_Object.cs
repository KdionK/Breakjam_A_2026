using UnityEngine;

public class rotate_Object : MonoBehaviour
{
    void Update()
    {
      transform.Rotate(0, 5 * Time.deltaTime, 0);  
    }
}
