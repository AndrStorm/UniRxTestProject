using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class VfxManager : MonoBehaviour
{

    public List<GameObject> lightAttackVfxs;


    

    public void PlayHeavyAttackVfx(Vector3 position, Quaternion rotation)
    {
        PlayVfx(lightAttackVfxs[Random.Range(0,2)], position, rotation);
    }
    
    public void PlayLightAttackVfx(Vector3 position, Quaternion rotation)
    {
        PlayVfx(lightAttackVfxs[2], position, rotation);
    }

    
    private void PlayVfx(GameObject vfx, Vector3 position, Quaternion rotation)
    {
        var vfxGO = Instantiate(vfx, position, rotation);
        Destroy(vfxGO, 2f);
    }
}
