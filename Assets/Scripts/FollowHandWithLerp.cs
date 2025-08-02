using UnityEngine;

public class FollowHandWithLerp : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    
    private Transform _tr;
    private GameManager _gm;

    [SerializeField] 
    private float _lerpFactor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gm = GameManager.Instance;
        _tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gm.IsMapOut()) return;
        
        _tr.position = Vector3.Lerp(_tr.position, _target.position, _lerpFactor * Time.deltaTime);
    }
}
