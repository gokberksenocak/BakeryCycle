using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class Triggers : MonoBehaviour
{
    [SerializeField] private Customers _customers;
    [SerializeField] private MoneyCount _moneyCount;
    [SerializeField] private TextMeshPro _countText;
    [SerializeField] private List<GameObject> _doughs;
    [SerializeField] private List<GameObject> _myDoughs;
    [SerializeField] private GameObject _stackPoint;
    [SerializeField] private Transform _breadPoint;
    [SerializeField] private Transform _bakingPoint;
    [SerializeField] private Transform _firstDoughPlace;
    [SerializeField] private Transform _bakery;
    [SerializeField] private Transform[] _standPoint;
    [SerializeField] private Transform _stand;
    [SerializeField] private GameObject _bread;
    private int _count;
    private bool _isEnter;
    private int _throwingDoughCount;//firindaki ekmek sayisi dogru yerde dursun diye kullanildi
    private int index_make;//ekmek firindan cikarken sayma islemi icin kullanildi

    [SerializeField] private Image[] _filledBars;
    private int _number;
    private bool _allow;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DoughMachine") && !_isEnter && _count != 5)
        {
            _number = 1;//
            _allow = true;//
            InvokeRepeating(nameof(MakePastry), 1f, 1f);
        }
        else if (other.CompareTag("BakeryEnter") && !_isEnter)
        {
            _number = 2;//
            _allow = true;//
            InvokeRepeating(nameof(MakeBread), 1f, 2f);
        }
        else if (other.CompareTag("Stand") && !_isEnter)
        {
            _number = 3;//
            _allow = true;//
        }
        _isEnter = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _isEnter = false;
        if (other.CompareTag("DoughMachine"))
        {
            CancelInvoke(nameof(MakePastry));
            if (_stackPoint.transform.childCount > 0)
            {
                for (int i = 0; i < _stackPoint.transform.childCount; i++)
                {
                    _myDoughs.Add(_stackPoint.transform.GetChild(i).gameObject);
                }
            }
            _countText.gameObject.SetActive(false);
            _allow = false;
            _filledBars[0].fillAmount = 1;
        }
        else if (other.CompareTag("BakeryEnter") && !_isEnter)
        {
            _allow = false;
            _filledBars[1].fillAmount = 1;
        }
        else if (other.CompareTag("BakeryExit"))
        {
            ExitBakery();
        }
        else if (other.CompareTag("Stand") && !_isEnter)
        {
            _allow = false;
            _filledBars[2].fillAmount = 1;
        }
    }
    void MakePastry()
    {
        _count++;
        _countText.gameObject.SetActive(true);
        _countText.text = _count.ToString() + "/5";
        _doughs[_count - 1].transform.DOMove(_stackPoint.transform.position + new Vector3(0, .1f * (_count - 1), 0), .025f);
        _doughs[_count - 1].transform.DOLocalRotateQuaternion(Quaternion.identity, 0.0125f);
        _doughs[_count - 1].transform.SetParent(_stackPoint.transform);
        if (_count == 5)
        {
            _allow = false;//
            _filledBars[0].fillAmount = 1;//
            CancelInvoke(nameof(MakePastry));
        }
    }
    void EnterBakery()
    {
        for (int i = 0; i < _myDoughs.Count; i++)
        {
            _myDoughs[i].gameObject.transform.DOMove(_bakingPoint.position, .5f);
        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(.6f);
            for (int i = 0; i < _myDoughs.Count; i++)
            {
                _myDoughs[i].gameObject.transform.SetParent(_firstDoughPlace);
                _myDoughs[i].gameObject.transform.position = _firstDoughPlace.position;
            }
        }
        StartCoroutine(Delay());
        _throwingDoughCount = _myDoughs.Count;
        _count = 0;
    }
    void MakeBread()
    {
        if (_throwingDoughCount > 0)
        {
            Instantiate(_bread, _breadPoint.GetChild(0).position + new Vector3(0, .1f * (_breadPoint.childCount-1), 0), _breadPoint.GetChild(0).transform.rotation, _breadPoint);
            index_make++;
            if (index_make >= _throwingDoughCount)
            {
                CancelInvoke(nameof(MakeBread));
                index_make = 0;
                _throwingDoughCount = 0;
            }
        }  
    }
    void ExitBakery()
    {
        _myDoughs.Clear();
        for (int i = 1; i < _breadPoint.transform.childCount ; i++)
        {
            _myDoughs.Add(_breadPoint.transform.GetChild(i).gameObject);
            if (i==5)
            {
                break;
            }
        }
        for (int i = 0; i < _myDoughs.Count; i++)
        {
            _myDoughs[i].transform.position = _stackPoint.transform.position + new Vector3(0, .15f * i, 0);
            _myDoughs[i].transform.SetParent(_stackPoint.transform);
        }
        for (int j = 1; j < _breadPoint.childCount; j++)
        {
            _breadPoint.GetChild(j).position = _breadPoint.GetChild(0).position + new Vector3(0, .1f * (j - 1), 0);
        }
    }
    void ComeStand()
    {
        for (int i = 0; i < _myDoughs.Count; i++)
        {
            _myDoughs[i].transform.transform.DOMove(_standPoint[i].position, .2f);
            _myDoughs[i].transform.rotation = _standPoint[i].rotation;
            _myDoughs[i].transform.SetParent(_standPoint[i]);
        }
        _myDoughs.Clear();
        StopCoroutine(CustomerGetsBread());
        StartCoroutine(CustomerGetsBread());
    }
    IEnumerator CustomerGetsBread()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _stand.childCount + 1; i++)
        {
            if (i==5)
            {
                StopCoroutine(CustomerGetsBread());
                break;
            }
            if (_stand.GetChild(i).childCount !=0)
            {
                Destroy(_stand.GetChild(i).GetChild(0).gameObject);
                _moneyCount.IncreaseMoney();
                _customers.CustomerAway();
                yield return new WaitForSeconds(2f);
            }
        }
    }
    private void LateUpdate()
    {
        if (_number==1 && _allow)
        {
             _filledBars[0].fillAmount -= Time.deltaTime;
            if (_filledBars[0].fillAmount==0)
            {
                _filledBars[0].fillAmount = 1;
            }
        }
        else if (_number == 2 && _allow)
        {
            _filledBars[1].fillAmount -= Time.deltaTime * 1.75f;
            if (_filledBars[1].fillAmount == 0)
            {
                EnterBakery();
                _allow = false;
                _filledBars[1].fillAmount = 1;
            }
        }
        else if (_number == 3 && _allow)
        {
            _filledBars[2].fillAmount -= Time.deltaTime * 5f ;
            if (_filledBars[2].fillAmount==0)
            {
                ComeStand();
                _allow = false;
                _filledBars[2].fillAmount = 1;
            }
        }
    }
}