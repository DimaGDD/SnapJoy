using UnityEngine;

public class States : MonoBehaviour
{
    private bool _isViewItem = false;
    private DefaultItem _currentItem;

    public bool IsViewItem
    {
        get {  return _isViewItem; }
        set { _isViewItem = value; }
    }

    public DefaultItem CurrentItem
    {
        get { return _currentItem; }
        set { _currentItem = value; }
    }
}
