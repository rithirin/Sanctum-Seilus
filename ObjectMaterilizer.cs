
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectMaterilizer : UdonSharpBehaviour
{
    public Material dissolveMat;
    public Material standardMat;

    private Renderer _rend;
    public float _targetPos;
    public float currentPos;
    public float speed;

    public float _maxPos;


    public bool materlialized;
    public bool loaded;
    
    
    void Start()
    {
        _rend = gameObject.GetComponent<Renderer>();
        materlialized = false;
        _rend.material = dissolveMat;
        _rend.material.SetFloat("_DissolveY", currentPos);
    }

    private void Update()
    {
        if (loaded) return;
        
        if (!Mathf.Approximately(currentPos, _targetPos))
        {
            currentPos = Mathf.Lerp(currentPos, _targetPos, Time.deltaTime * speed);
            _rend.material.SetFloat("_DissolveY", currentPos);
            if (!materlialized)
            {
                materlialized = true;
            }
        }

        if (currentPos > _maxPos -2f && materlialized)
        {
            materlialized = true;
            _rend.material = standardMat;
            loaded = true;
        }
        
    }
}
