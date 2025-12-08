using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "Scriptable Objects/GameSettingsSO")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Attack Delay Settings")]
    public float AttackDelayLight = 500f;
    public float AttackDelayHeavy = 1000f;
    public float DoubleClickDelay = 250f;
    
    [Header("Attack Vfx Settings")]
    public int LightAttackVfxDelay = 200;
    public int HeavyAttackVfxDelay = 300;
    
    [Header("Highlight Vfx Settings")]
    public int HighlightOnInterval = 20;
    public int HighlightOffInterval = 30;
    public float HighlightOffPressedMul = 0.5f;
    [Range(0, 1)] public float HighlightDeltaPerInterval = 0.05f;
}
