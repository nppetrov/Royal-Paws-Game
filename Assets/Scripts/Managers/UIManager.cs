using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

using static NinjaChompek.GameManager;
using System.Collections.Generic;

namespace NinjaChompek
{
	[DefaultExecutionOrder(200)]
	public class UIManager : Manager<UIManager>
	{
		[SerializeField] private TextMeshProUGUI _statAuthorityNumber;
		[SerializeField] private TextMeshProUGUI _statMoneyNumber;
		[SerializeField] private TextMeshProUGUI _statArmyNumber;
		[SerializeField] private TextMeshProUGUI _statPeopleNumber;

		[SerializeField] private Image _statAuthorityBackground;
		[SerializeField] private Image _statAuthorityFill;
		[SerializeField] private Image _statMoneyBackground;
		[SerializeField] private Image _statMoneyFill;
		[SerializeField] private Image _statArmyBackground;
		[SerializeField] private Image _statArmyFill;
		[SerializeField] private Image _statPeopleBackground;
		[SerializeField] private Image _statPeopleFill;

		[SerializeField] private TextMeshProUGUI _cardTitle;
		[SerializeField] private TextMeshProUGUI _cardDescription;
		[SerializeField] private Image _cardImage;
		[SerializeField] private Button _buttonLeft;
		[SerializeField] private TextMeshProUGUI _buttonLeftText;
		[SerializeField] private Button _buttonRight;
		[SerializeField] private TextMeshProUGUI _buttonRightText;

		[SerializeField] private CanvasGroup _introPopup;
		[SerializeField] private TMP_Dropdown _playerGender;
		[SerializeField] private TMP_InputField _playerName;
		[SerializeField] private Button _buttonRandomizeName;
		[SerializeField] private Button _buttonBegin;
		[SerializeField] private List<string> _kingNames;
		[SerializeField] private List<string> _queenNames;

		[SerializeField] private Image _mainBackground;

		private TextMeshProUGUI _playerNamePlaceholder;

		private void Start()
		{
			_playerNamePlaceholder = _playerName.placeholder.GetComponent<TextMeshProUGUI>();

			// Randomize the player name placeholder at the beginning of the game,
			// when the gender dropdown changes, or when the randomize button is clicked
			RandomizePlayerNamePlaceholder(false);
			_playerGender.onValueChanged.AddListener(_ => RandomizePlayerNamePlaceholder());
			_buttonRandomizeName.onClick.AddListener(() => RandomizePlayerNamePlaceholder(true, true));

			_buttonBegin.onClick.AddListener(StartGame);
			_buttonLeft.onClick.AddListener(() => GameManager.Instance.ChooseLeft());
			_buttonRight.onClick.AddListener(() => GameManager.Instance.ChooseRight());
		}

		private void StartGame()
		{
			string playerName = _playerName.text;

			if (playerName == "")
			{
				playerName = _playerNamePlaceholder.text;
			}

			_introPopup.DOFade(0, 0.5f).OnComplete(() => _introPopup.gameObject.SetActive(false));
			GameManager.Instance.PlayerName = playerName;
			GameManager.Instance.PlayerGender = _playerGender.options[_playerGender.value].text;

			GameManager.Instance.StartNewGame();
		}

		private void Update()
		{
			// Left/right keyboard presses
			if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				if (_introPopup.gameObject.activeSelf)
				{
					// If the intro popup is active, start the game
					StartGame();
				}
				else
				{
					// Otherwise, the left arrow key should choose the left button
					GameManager.Instance.ChooseLeft();
				}
			}
			else if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				if (_introPopup.gameObject.activeSelf)
				{
					// If the intro popup is active, start the game
					StartGame();
				}
				else
				{
					// Otherwise, the right arrow key should choose the right button
					GameManager.Instance.ChooseRight();
				}
			}
		}

		private void RandomizePlayerNamePlaceholder(bool playSound = true, bool clearInput = false)
		{
			if (playSound)
			{
				AudioManager.Instance.PlaySound("DiceRoll");
			}

			if (clearInput)
			{
				_playerName.text = "";
			}

			var names = _playerGender.value == 0 ? _kingNames : _queenNames;
			_playerNamePlaceholder.text = names[Random.Range(0, names.Count)];
		}

		public void UpdateStats()
		{

			UpdateNumber(_statAuthorityNumber, GameManager.Instance.Authority);
			UpdateNumber(_statMoneyNumber, GameManager.Instance.Money);
			UpdateNumber(_statArmyNumber, GameManager.Instance.Army);
			UpdateNumber(_statPeopleNumber, GameManager.Instance.People);

			UpdateFillAmount(_statAuthorityFill, _statAuthorityBackground, Mathf.Clamp(GameManager.Instance.Authority / 100f, 0f, 1f));
			UpdateFillAmount(_statMoneyFill, _statMoneyBackground, Mathf.Clamp(GameManager.Instance.Money / 100f, 0f, 1f));
			UpdateFillAmount(_statArmyFill, _statArmyBackground, Mathf.Clamp(GameManager.Instance.Army / 100f, 0f, 1f));
			UpdateFillAmount(_statPeopleFill, _statPeopleBackground, Mathf.Clamp(GameManager.Instance.People / 100f, 0f, 1f));
		}
		
		private void UpdateNumber(TextMeshProUGUI target, int value)
		{
			if (target.text == value.ToString())
			{
				return;
			}

			target.text = value.ToString();
			target.transform.DOScale(1.5f, 0.1f).OnComplete(() => target.transform.DOScale(1f, 0.1f));
		}

		private void UpdateFillAmount(Image target, Image targetBackground, float fillAmount)
		{
			if (target.fillAmount == fillAmount)
			{
				return;
			}

			target.DOFillAmount(fillAmount, 0.5f);
			target.transform.DOScale(1.2f, 0.1f).OnComplete(() => target.transform.DOScale(1f, 0.1f));
			targetBackground.transform.DOScale(1.2f, 0.1f).OnComplete(() => targetBackground.transform.DOScale(1f, 0.1f));
		}

		public void LoadCard(Card card)
		{
			CardVisual cardVisual = card.CardVisual;

			_cardTitle.gameObject.SetActive(true);

			if (card.Character != null)
			{
				_cardTitle.text = card.Character.Name;

				if (cardVisual == null)
				{
					cardVisual = card.Character.CardVisual;
				}
			}

			if (cardVisual != null)
			{
				_cardImage.DOFade(0, 0.25f).OnComplete(() =>
				{
					_cardImage.sprite = cardVisual.CardImage;
					_cardImage.DOFade(1, 0.25f);
				});
				_mainBackground.DOColor(cardVisual.BackgroundColor, 0.5f);

				if (!string.IsNullOrEmpty(cardVisual.CardTitle))
				{
					_cardTitle.text = cardVisual.CardTitle;
				}
				
				if (card.Character == null && string.IsNullOrEmpty(cardVisual.CardTitle))
				{
					_cardTitle.gameObject.SetActive(false);
				}
			}

			_cardDescription.text = HandlePlaceholders(card.Description);
			_buttonLeftText.text = "<margin=1em>" + HandlePlaceholders(card.ButtonLeftText);
			_buttonRightText.text = "<margin=1em>" + HandlePlaceholders(card.ButtonRightText);
		}

		private string HandlePlaceholders(string input)
		{
			string output;

			output = input.Replace("[PlayerName]", GameManager.Instance.PlayerName);
			output = output.Replace("[playerName]", GameManager.Instance.PlayerName);
			output = output.Replace("[KingGender]", GameManager.Instance.PlayerGender);
			output = output.Replace("[kingGender]", GameManager.Instance.PlayerGender.ToLower());

			return output;
		}
	}
}
