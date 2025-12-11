using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public List<GameObject> lightAttackVfxs;
    public List<GameObject> heavyAttackVfxs;
    public List<GameObject> uiOnButtonPressedVfxs;
    public Transform uiButtonPressedVfxOrigin;
    
    public Transform SwordTransform;
    public Transform BowTransform;
    
    
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
    
    
    private int _heavyAttackCounter;
    public void PlayHeavyAttackVfx()
    {
        PlayAttackVfx(heavyAttackVfxs[_heavyAttackCounter++ % heavyAttackVfxs.Count],
            BowTransform.position, Quaternion.identity);
    }

    private int _lightAttackCounter;
    public void PlayLightAttackVfx()
    {
        PlayAttackVfx(lightAttackVfxs[_lightAttackCounter++ % lightAttackVfxs.Count],
            SwordTransform.position, Quaternion.identity);
    }
    
    

    
    private void PlayAttackVfx(GameObject vfx, Vector3 position, Quaternion rotation)
    {
        var vfxGO = Instantiate(vfx, position, rotation);
        Destroy(vfxGO, 2f);
    }
}
