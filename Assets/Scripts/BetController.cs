using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Pixelplacement;
using UnityEngine;
using System.Collections;

[Serializable]
public struct Spins
{
    public GameObject spin;
    public string name;
}

public class BetController : Singleton<BetController>
{
    [SerializeField] private TextMeshProUGUI _betNumber;
    [SerializeField] private TextMeshProUGUI _balanceNumber;
    [SerializeField] private TextMeshProUGUI _winningNumber;
    [SerializeField] private List<GameObject> parentObject = new List<GameObject>();
    [SerializeField] private List<Animator> _animators = new List<Animator>();
    [SerializeField] private List<Spins> spins;

    [Header("Chances")]
    private int spawnChance;
    [SerializeField] private Vector2 _nothing;
    [SerializeField] private Vector2 _firstAndSecondLine;
    [SerializeField] private Vector2 _secondAndThirdLine;
    [SerializeField] private Vector2 _tripleLine;

    private Combinations _combo;

    public bool animStart;
    private bool _freeSpinActivated;
    private bool _animEnded;
    
    private int _bet;
    private int _balance;
    private int _normalBet;
    private string balanceKey = "Balance";
    private List<string> _combosNames = new List<string>();
    private List<GameObject> _spawnedSpins = new List<GameObject>();

    private void Start()
    {
        _combo = GetComponent<Combinations>();

        _balance = PlayerPrefs.GetInt(balanceKey, 30);
        _betNumber.text = $"{_bet}";
        _balanceNumber.text = $"{_balance}";
        _winningNumber.text = "-";
    }

    #region Buttons
    public void StartSpin()
    {
        if (_balance < _bet || _bet <= 0 || animStart)
            return;

        if (_balance > 0)
        {
            StartCoroutine(GameStarted());
        }
    }

    private IEnumerator GameStarted()
    {
        SoundManager.Instance.ButtonClick();

        yield return new WaitForSeconds(0.5f);
        SpawnSpinObjects();
        animStart = true;

        SoundManager.Instance.StartSpin();

        yield return new WaitForSeconds(2.5f);
        _balance -= _bet;
        _balanceNumber.text = $"{_balance}";

        PlayerPrefs.SetInt(balanceKey, _balance);

        animStart=false;
    }

    public void Plus()
    {
        if (_bet < 5 && !animStart)
        {
            _bet++;
            _betNumber.text = $"{_bet}";
        }
    }

    public void Minus()
    {
        if (_bet > 1 && !animStart)
        {
            _bet--;
            _betNumber.text = $"{_bet}";
        }
    }
    #endregion

    #region Spawning
    public void SpawnSpinObjects()
    {
        spawnChance = UnityEngine.Random.Range(1, 100);

        DestroySpinObjects();

        switch (spawnChance)
        {
            case int chance when chance >= _nothing.x && chance <= _nothing.y:
                SpawnDifferentAllPoints();
                break;

            case int chance when chance >= _firstAndSecondLine.x && chance <= _firstAndSecondLine.y:
                SpawnSameObjectsOnPoints1And2(0, 1, 2, PrizeMultiplier.Instance.firstAndSecondPoint);
                break;

            case int chance when chance >= _secondAndThirdLine.x && chance <= _secondAndThirdLine.y:
                SpawnSameObjectsOnPoints1And2(1, 2, 0, PrizeMultiplier.Instance.secondAndThirdPoint);
                break;

            case int chance when chance >= _tripleLine.x && chance <= _tripleLine.y:
                SpawnAllPoint();
                break;
        }

        StartCoroutine(PlayAnimation());
    }

    private void SpawnRandomSpinsItems()
    {
        for (int i = 0; i < parentObject.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var index = UnityEngine.Random.Range(0, spins.Count);
                var spinItem = Instantiate(spins[index].spin, parentObject[i].transform.position, Quaternion.identity, parentObject[i].transform);
                _spawnedSpins.Add(spinItem);
            }
        }
    }

    public void SpawnDifferentAllPoints()
    {
        List<GameObject> obj = new List<GameObject>();
        List<string> str = new List<string>();

        SpawnRandomSpinsItems();

        foreach (var spin in spins)
        {
            obj.Add(spin.spin);
            str.Add(spin.name);
        }

        for (int i = 0; i < parentObject.Count; i++)
        {
            int index = UnityEngine.Random.Range(0, obj.Count);
            GameObject spinItem = Instantiate(obj[index], parentObject[i].transform.position, Quaternion.identity, parentObject[i].transform);
            _combosNames.Add(str[index]);

            _spawnedSpins.Add(spinItem);

            obj.RemoveAt(index);
            str.RemoveAt(index);
        }

        var freeSpin = FindObjectOfType<UIController>();

        if (_combo.IsCombinationMatched(_combosNames.Select(point => point).ToList()))
        {
            freeSpin.ActiveFreeGame();
            _freeSpinActivated = true;
            Debug.Log("Combination matched!");
        }

        StartCoroutine(DelayForMusicAndText(0, SoundManager.Instance.loseGame));

        _combosNames.Clear();
    }

    void SpawnSameObjectsOnPoints1And2(int point1, int point2, int different, int prize)
    {
        SpawnRandomSpinsItems();

        int index1 = UnityEngine.Random.Range(0, spins.Count);
        int index2 = GetDifferentIndex(index1, spins.Count);

        GameObject spinItem1 = Instantiate(spins[index1].spin, parentObject[point1].transform.position, Quaternion.identity, parentObject[point1].transform);
        GameObject spinItem2 = Instantiate(spins[index1].spin, parentObject[point2].transform.position, Quaternion.identity, parentObject[point2].transform);
        GameObject sspinItemdDifferent = Instantiate(spins[index2].spin, parentObject[different].transform.position, Quaternion.identity, parentObject[different].transform);

        _spawnedSpins.Add(spinItem1);
        _spawnedSpins.Add(spinItem2);
        _spawnedSpins.Add(sspinItemdDifferent);

        StartCoroutine(DelayForMusicAndText(prize, SoundManager.Instance.smallWin));
    }

    void SpawnAllPoint()
    {
        SpawnRandomSpinsItems();

        var index = UnityEngine.Random.Range(0, spins.Count);

        foreach (var point in parentObject)
        {
            GameObject spinItem = Instantiate(spins[index].spin, point.transform.position, Quaternion.identity, point.transform);
            _spawnedSpins.Add(spinItem);
        }

        StartCoroutine(DelayForMusicAndText(PrizeMultiplier.Instance.triplePoint, SoundManager.Instance.winner));
    }

    int GetDifferentIndex(int index, int maxIndex)
    {
        int differentIndex = index;
        while (differentIndex == index)
        {
            differentIndex = UnityEngine.Random.Range(0, maxIndex);
        }
        return differentIndex;
    }
    public void DestroySpinObjects()
    {
        foreach (GameObject go in _spawnedSpins)
        {
            Destroy(go);
        }

        _spawnedSpins.Clear();
    }
    #endregion

    public void FreeSpin()
    {
        _normalBet = _bet;
        _bet = PrizeMultiplier.Instance.freeSpinPrize;
        StartSpin();
        _bet = _normalBet;
        _freeSpinActivated = false;
    }
    private IEnumerator DelayForMusicAndText(int multiple, AudioClip clip)
    {
        yield return new WaitForSeconds(2.5f);

        _balance += _bet * multiple;
        _winningNumber.text = $"{_bet * multiple}";
        SoundManager.Instance.AudioPlay(clip);
    }
    public IEnumerator PlayAnimation()
    {
        foreach (Animator anim in _animators)
            anim.SetBool("SpinStarted", true);

        yield return new WaitForSeconds(0.5f);

        foreach (Animator anim in _animators)
            anim.SetBool("SpinStarted", false);
    }
}