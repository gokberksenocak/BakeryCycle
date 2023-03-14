using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PathCreation.Examples;

public class InputManager : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private Follower _pathFollowerScript;
    [SerializeField] private Animator _animator;
    private bool isTouch;
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        _animator.SetBool("isMove", false);
    }
    private void FixedUpdate()
    {
        if (isTouch)
        {
            _animator.SetBool("isMove", true);
            _pathFollowerScript.PathFollow();
        }
    }
}