using UnityEngine;
using System.Collections;
public class Status : CoreComponent, IFreezable
{
    
    
        public bool IsFrozen;
        public GameObject iceCubePrefab;


    public void ApplyFreeze(float duration)
    {
        Debug.Log("applying freeze");
        StartCoroutine(RemoveIceCubeAfterDelay(duration));
    }

    private IEnumerator RemoveIceCubeAfterDelay(float delay)
    {
        IsFrozen = true;
        GameObject iceCube = Instantiate(iceCubePrefab, transform);
        Debug.Log("ice cube instantiated");

        yield return new WaitForSeconds(delay);
        if (iceCube != null)
        {
            Destroy(iceCube);
        }
        Debug.Log("ice cube destroyed");
        IsFrozen = false;
    }   
}