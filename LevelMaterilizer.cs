
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LevelMaterilizer : UdonSharpBehaviour
{
    public UdonSharpBehaviour[] matObjects;
    public float speed;
    public float maxPos;
    public float startPos;
    
    public bool materlialized;
    public bool loaded;
    
    public float _targetPos;
    public float currentPos;

    public GameObject[] toggleObjects;

    private void Start()
    {
        foreach (var t in matObjects)
        {
            if (t == null) return;
            t.SetProgramVariable("_maxPos", maxPos);
            t.SetProgramVariable("currentPos", startPos);
            t.SetProgramVariable("_targetPos", startPos);
            t.SetProgramVariable("speed", speed);
            _targetPos = startPos;
            currentPos = startPos;
        }
        
        
        foreach (var o in toggleObjects)
        {
            if (o == null) return;
            o.SetActive(false);
        }
    }

    public void Load()
    {
        foreach (var t in matObjects)
        {
            if (t == null) return;
            t.SetProgramVariable("_targetPos", maxPos);
            _targetPos = maxPos;
        }
    }

    private void Update()
        //Loading
    {
        if (loaded) return;
            if (!Mathf.Approximately(currentPos, _targetPos))
            {
                currentPos = Mathf.Lerp(currentPos, _targetPos, Time.deltaTime * speed);
                if (!materlialized)
                {
                    materlialized = true;
                }
            }

            if (currentPos > maxPos - 2f && materlialized)
            {
                materlialized = true;
                loaded = true;
                foreach (var o in toggleObjects)
                {
                    if (o == null) return;    
                    o.SetActive(true);
                }
            }
    }
}
