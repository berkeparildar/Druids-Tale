using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Image _cooldownOneBearImage;
    private Image _cooldownTwoBearImage;

    public Sprite humanAbilityOneIcon;
    public Sprite humanAbilityTwoIcon;
    public Sprite humanNextForm;
    public Sprite humanPreviousForm;

    public Sprite catAbilityOneIcon;
    public Sprite catAbilityTwoIcon;
    public Sprite catNextForm;
    public Sprite catPreviousForm;

    public Sprite bearAbilityOneIcon;
    public Sprite bearAbilityTwoIcon;
    public Sprite bearNextForm;
    public Sprite bearPreviousForm;

    private Cat _cat;
    private Human _human;
    private Bear _bear;

    private readonly Color _humanResourceColor = new Color(0, 0, 1, 0.59f);
    private readonly Color _catResourceColor = new Color(1, 1, 0, 0.45f);
    private readonly Color _bearResourceColor = new Color(1, 0, 0, 0.45f);

    private const float MaxManaValue = 360;
    private const float MaxEnergyValue = 100;
    private const float MaxRageValue = 100;

    private float _currentResource;
    private float _currentMaxResource;

    public TextMeshProUGUI questText;
    public Image questFillImage;
    public float questFillAmount;
    public bool questFinished;

    private void Start()
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
        _cooldownOneBearImage = _abilityOneSlot.transform.GetChild(2).GetComponent<Image>();
        _cooldownTwoBearImage = _abilityTwoSlot.transform.GetChild(2).GetComponent<Image>();
        _cat = GetComponent<Cat>();
        _human = GetComponent<Human>();
        _bear = GetComponent<Bear>();
    }

    // Update is called once per frame
    private void Update()
    {
        _healthImage.fillAmount = (Player.Health / 100);
        UpdateQuest();
        switch (Player.CurrentForm)
        {
            case Form.Human:
                _resourceImage.fillAmount = (_human.Mana / MaxManaValue);
                break;
            case Form.Cat:
                _resourceImage.fillAmount = (_cat.Energy / MaxEnergyValue);
                break;
            case Form.Bear:
                _resourceImage.fillAmount = (_bear.Rage / MaxRageValue);
                break;
        }
    }

    private void UpdateQuest()
    {
        if (!questFinished)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                questText.text = "Defeat the invaders! " + GameManager.DeadEnemyCount + "/" + GameManager.EnemyCount;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                questText.text = "Defeat the Destroyer!";
                questFillImage.color = Color.red;
            }
            questFillImage.fillAmount = questFillAmount;
        }
        else
        {
            questText.enabled = false;
            questFillImage.enabled = false;
        }
    }

    public void SetQuestFillAmount(float amount)
    {
        questFillAmount = amount;
    }

    public void QuestFinished()
    {
        questFinished = true;
    }

    public void UpdateUIAccordingToForm()
    {
        switch (Player.CurrentForm)
        {
            case Form.Human:
                _resourceImage.color = _humanResourceColor;
                _abilityOneSlot.sprite = humanAbilityOneIcon;
                _abilityTwoSlot.sprite = humanAbilityTwoIcon;
                _previousFormSlot.sprite = humanPreviousForm;
                _nextFormSlot.sprite = humanNextForm;
                _cooldownOneHumanImage.gameObject.SetActive(true);
                _cooldownTwoHumanImage.gameObject.SetActive(true);
                _cooldownOneCatImage.gameObject.SetActive(false);
                _cooldownTwoCatImage.gameObject.SetActive(false);
                break;
            case Form.Cat:
                _resourceImage.color = _catResourceColor;
                _abilityOneSlot.sprite = catAbilityOneIcon;
                _abilityTwoSlot.sprite = catAbilityTwoIcon;
                _previousFormSlot.sprite = catPreviousForm;
                _nextFormSlot.sprite = catNextForm;
                _cooldownOneCatImage.gameObject.SetActive(true);
                _cooldownTwoCatImage.gameObject.SetActive(true);
                _cooldownOneHumanImage.gameObject.SetActive(false);
                _cooldownTwoHumanImage.gameObject.SetActive(false);
                _cooldownOneBearImage.gameObject.SetActive(false);
                _cooldownTwoBearImage.gameObject.SetActive(false);
                break;
            case Form.Bear:
                _resourceImage.color = _bearResourceColor;
                _abilityOneSlot.sprite = bearAbilityOneIcon;
                _abilityTwoSlot.sprite = bearAbilityTwoIcon;
                _previousFormSlot.sprite = bearPreviousForm;
                _nextFormSlot.sprite = bearNextForm;
                _cooldownOneCatImage.gameObject.SetActive(false);
                _cooldownTwoCatImage.gameObject.SetActive(false);
                _cooldownOneHumanImage.gameObject.SetActive(false);
                _cooldownTwoHumanImage.gameObject.SetActive(false);
                _cooldownOneBearImage.gameObject.SetActive(true);
                _cooldownTwoBearImage.gameObject.SetActive(true);
                break;
        }
    }

    public void ShowCooldownInActionBar(int abilityNo, float cooldown)
    {
        switch (abilityNo)
        {
            case 1:
                switch (Player.CurrentForm)
                {
                    case Form.Human:

                        _cooldownOneHumanImage.gameObject.SetActive(true);
                        _cooldownOneHumanImage.fillAmount = 1;
                        _cooldownOneHumanImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownOneHumanImage.gameObject.SetActive(false);
                        });

                        break;
                    case Form.Cat:

                        _cooldownOneCatImage.gameObject.SetActive(true);
                        _cooldownOneCatImage.fillAmount = 1;
                        _cooldownOneCatImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownOneCatImage
                                .gameObject.SetActive(false);
                        });

                        break;
                    case Form.Bear:

                        _cooldownOneBearImage.gameObject.SetActive(true);
                        _cooldownOneBearImage.fillAmount = 1;
                        _cooldownOneBearImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownOneBearImage
                                .gameObject.SetActive(false);
                        });

                        break;
                }

                break;
            case 2:
                switch (Player.CurrentForm)
                {
                    case Form.Human:

                        _cooldownTwoHumanImage.gameObject.SetActive(true);
                        _cooldownTwoHumanImage.fillAmount = 1;
                        _cooldownTwoHumanImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownTwoHumanImage.gameObject.SetActive(false);
                        });

                        break;
                    case Form.Cat:

                        _cooldownTwoCatImage.gameObject.SetActive(true);
                        _cooldownTwoCatImage.fillAmount = 1;
                        _cooldownTwoCatImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownTwoCatImage
                                .gameObject.SetActive(false);
                        });

                        break;
                    case Form.Bear:

                        _cooldownTwoBearImage.gameObject.SetActive(true);
                        _cooldownTwoBearImage.fillAmount = 1;
                        _cooldownTwoBearImage.DOFillAmount(0, cooldown).OnComplete(() =>
                        {
                            _cooldownTwoBearImage
                                .gameObject.SetActive(false);
                        });

                        break;
                }

                break;
        }
    }
}