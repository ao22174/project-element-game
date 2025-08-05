using UnityEngine;
using System.Collections;
public class Status : CoreComponent, IFreezable
{
    
    
        public bool IsFrozen;
        public GameObject iceCubePrefab;
        GameObject iceCube;



    public void ApplyFreeze(float duration)
    {
        Debug.Log("applying freeze");
        if(iceCube== null)
        StartCoroutine(RemoveIceCubeAfterDelay(duration));
    }
    public void RemoveFreeze()
    {
        IsFrozen = false;
        if (iceCube != null)
        {
            Destroy(iceCube);
            iceCube = null;
        }
    }

    private IEnumerator RemoveIceCubeAfterDelay(float delay)
    {
        IsFrozen = true;
        iceCube = Instantiate(iceCubePrefab, transform);

Debug.Log($"Instantiated IceCube: {iceCube.name}");
        yield return new WaitForSeconds(delay);
        if (iceCube != null)
        {
            Destroy(iceCube);
                    Debug.Log("ice cube destroyed");

        }
        IsFrozen = false;
    }   
}