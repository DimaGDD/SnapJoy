using UnityEngine;

public class States : MonoBehaviour
{
    private bool _isViewItem = false;

    public bool IsViewItem
    {
        get {  return _isViewItem; }
        set { _isViewItem = value; }
    }
}
