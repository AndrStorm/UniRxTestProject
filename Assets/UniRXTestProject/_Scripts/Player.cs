using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public Animator Animator => _animator;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
