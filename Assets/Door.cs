using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class Door : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        _animator.SetBool(Open, true);
        yield return new WaitForEndOfFrame();
        _animator.SetBool(Open, false);
    }
}
