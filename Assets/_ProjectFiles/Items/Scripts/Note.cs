using UnityEngine;

public class Note : DefaultPickupItem
{
    private Animator _animator;

    private bool _isOpen = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void MoveToTarget(Transform targetPosition, bool isSubActions)
    {
        base.MoveToTarget(targetPosition, isSubActions);

        if (isSubActions && !_isOpen)
        {
            _animator.SetTrigger("Open");
            _isOpen = true;
        }
        else if (!isSubActions && _isOpen)
        {
            _animator.SetTrigger("Close");
            _isOpen = false;
        }
    }   
}
