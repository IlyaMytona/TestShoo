using System.Collections;
using UnityEngine;


public class DestroyExplosive : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
