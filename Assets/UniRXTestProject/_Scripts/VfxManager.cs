using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VfxManager : MonoBehaviour
{
    public List<GameObject> lightAttackVfxs;
    public List<GameObject> uiOnButtonPressedVfxs;
    public Transform uiButtonPressedVfxOrigin;


    
    public void PlayAttackPressedVfx()
    {
        foreach (var effect in uiOnButtonPressedVfxs)
        {
            var effectGO = Instantiate(effect, uiButtonPressedVfxOrigin.position,
                Quaternion.identity, uiButtonPressedVfxOrigin);
            var effectPs = effectGO.GetComponent<ParticleSystem>();
            Destroy(effectGO, effectPs.main.duration + effectPs.main.startLifetime.constantMax);
            
        }
    }
    
    public void PlayHeavyAttackVfx(Vector3 position, Quaternion rotation)
    {
        PlayAttackVfx(lightAttackVfxs[Random.Range(0,2)], position, rotation);
    }
    
    public void PlayLightAttackVfx(Vector3 position, Quaternion rotation)
    {
        PlayAttackVfx(lightAttackVfxs[2], position, rotation);
    }
    
    

    
    private void PlayAttackVfx(GameObject vfx, Vector3 position, Quaternion rotation)
    {
        var vfxGO = Instantiate(vfx, position, rotation);
        Destroy(vfxGO, 2f);
    }
}
