using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Customers : MonoBehaviour
{
    [SerializeField] private List<GameObject> _customerList;
    [SerializeField] private Animator[] _animators;
    [SerializeField] private GameObject example;
    [SerializeField] private GameObject example2;

    public void CustomerAway()
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("isMove", true);
        }

        StartCoroutine(Delay());

        for (int i = 1; i < _customerList.Count; i++)
        {
            _customerList[0].SetActive(false);
            _customerList[i].transform.DOMove(_customerList[i - 1].transform.position, 1f);
        }
        _customerList.Add(_customerList[0].gameObject);
        _customerList.RemoveAt(0);
        _customerList[4].transform.position = example2.transform.position;
        _customerList[4].SetActive(true);
        _customerList[4].transform.DOMove(example.transform.position, 1f);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.8f);
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("isMove", false);
        }
    }
}