using UnityEngine;

public class States : MonoBehaviour
{
    private bool _isViewItem = false;
    private DefaultPickupItem _currentItem;
    private DefaultHoldItem _curentHoldItem;

    public bool IsViewItem
    {
        get {  return _isViewItem; }
        set { _isViewItem = value; }
    }

    public DefaultPickupItem CurrentItem
    {
        get { return _currentItem; }
        set { _currentItem = value; }
    }

    public DefaultHoldItem CurentHoldItem
    {
        get { return _curentHoldItem; }
        set { _curentHoldItem = value; }
    }
}
