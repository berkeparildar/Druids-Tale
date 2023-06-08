using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameInterface : MonoBehaviour
{
    private Image _resourceImage;
    private Image _nextFormSlot;
    private Image _previousFormSlot;
    private Image _abilityOneSlot;
    private Image _abilityTwoSlot;
    private Image _healthImage;
    private Image _cooldownOneHumanImage;
    private Image _cooldownTwoHumanImage;
    private Image _cooldownOneCatImage;
    private Image _cooldownTwoCatImage;

    public Sprite humanAbilityOneIcon;
    public Sprite humanAbilityTwoIcon;
    public Sprite humanNextForm;
    public Sprite humanPreviousForm;

    public Sprite catAbilityOneIcon;
    public Sprite catAbilityTwoIcon;
    public Sprite catNextForm;
    public Sprite catPreviousForm;

    private Cat _cat;
    private Human _human;

    private readonly Color _humanResourceColor = new Color(0, 0, 1, 0.59f);
    private readonly Color _catResourceColor = new Color(1, 1, 0, 0.45f);

    private const float MaxManaValue = 360;
    private const float MaxEnergyValue = 100;
    private float _maxRageValue = 100;

    private float _energy;
    private float _mana;

    void Start()
    {
        _healthImage = GameObject.Find("Health").GetComponent<Image>();
        _resourceImage = GameObject.Find("Resource").GetComponent<Image>();
        _nextFormSlot = GameObject.Find("NextForm").GetComponent<Image>();
        _previousFormSlot = GameObject.Find("PreviousForm").GetComponent<Image>();
        _abilityOneSlot = GameObject.Find("AbilityOne").GetComponent<Image>();
        _abilityTwoSlot = GameObject.Find("AbilityTwo").GetComponent<Image>();
        _cooldownOneHumanImage = _abilityOneSlot.transform.GetChild(0).GetComponent<Image>();
        _cooldownTwoHumanImage = _abilityTwoSlot.transform.GetChild(0).GetComponent<Image>();
        _cooldownOneCatImage = _abilityOneSlot.transform.GetChild(1).GetComponent<Image>();
        _cooldownTwoCatImage = _abilityTwoSlot.transform.GetChild(1).GetComponent<Image>();
        _cat = GetComponent<Cat>();
        _human = GetComponent<Human>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthImage.DOFillAmount((Player.Health / 100), 0.5f);
        if (_cat.enabled)
        {
            _resourceImage.color = _catResourceColor;
            _resourceImage.fillAmount = (_cat.Energy / MaxEnergyValue);
            _abilityOneSlot.sprite = catAbilityOneIcon;
            _abilityTwoSlot.sprite = catAbilityTwoIcon;
            _previousFormSlot.sprite = catPreviousForm;
            _nextFormSlot.sprite = catNextForm;
            _cooldownOneCatImage.gameObject.SetActive(true);
            _cooldownTwoCatImage.gameObject.SetActive(true);
            _cooldownOneHumanImage.gameObject.SetActive(false);
            _cooldownTwoHumanImage.gameObject.SetActive(false);
        }
        else if (_human.enabled)
        {
            _resourceImage.color = _humanResourceColor;
            _resourceImage.fillAmount = (_human.Mana / MaxManaValue);
            _abilityOneSlot.sprite = humanAbilityOneIcon;
            _abilityTwoSlot.sprite = humanAbilityTwoIcon;
            _previousFormSlot.sprite = humanPreviousForm;
            _nextFormSlot.sprite = humanNextForm;
            _cooldownOneHumanImage.gameObject.SetActive(true);
            _cooldownTwoHumanImage.gameObject.SetActive(true);
            _cooldownOneCatImage.gameObject.SetActive(false);
            _cooldownTwoCatImage.gameObject.SetActive(false);
        }
    }

    public void ShowCooldownInActionBar(int abilityNo, float cooldown, string form)
    {
        switch (abilityNo)
        {
            case 1:
                switch (form)
                {
                    case "human":
                        _cooldownOneHumanImage.gameObject.SetActive(true);
                        _cooldownOneHumanImage.fillAmount = 1;
                        _cooldownOneHumanImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownOneHumanImage.gameObject.SetActive(false);
                        });
                        break;
                    case "cat":
                        _cooldownOneCatImage.gameObject.SetActive(true);
                        _cooldownOneCatImage.fillAmount = 1;
                        _cooldownOneCatImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownOneCatImage
                                .gameObject.SetActive(false);
                        });
                        break;
                }
                break;
            case 2:
                switch (form)
                {
                    case "human":
                        _cooldownTwoHumanImage.gameObject.SetActive(true);
                        _cooldownTwoHumanImage.fillAmount = 1;
                        _cooldownTwoHumanImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownTwoHumanImage.gameObject.SetActive(false);
                        });
                        break;
                    case "cat":
                        _cooldownTwoCatImage.gameObject.SetActive(true);
                        _cooldownTwoCatImage.fillAmount = 1;
                        _cooldownTwoCatImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownTwoCatImage
                                .gameObject.SetActive(false);
                        });
                        break;
                }
                break;
        }
    }
}