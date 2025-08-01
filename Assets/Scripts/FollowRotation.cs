using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private Transform _tr;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _tr.rotation = _target.rotation;    
    }
}
