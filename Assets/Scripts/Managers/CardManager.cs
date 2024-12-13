using System.Collections.Generic;
using UnityEngine;
using static NinjaChompek.GameManager;

namespace NinjaChompek
{
	[DefaultExecutionOrder(300)]
	public class CardManager : Manager<CardManager>
	{
		[SerializeField] private Card[] _cards;

		private Dictionary<string, Card> _singleCards = new Dictionary<string, Card>();
		private Dictionary<string, List<Card>> _sequenceCards = new Dictionary<string, List<Card>>();
		private Dictionary<string, Card> _specialCards = new Dictionary<string, Card>();

		private List<Card> _discardedCards = new List<Card>();
		private List<Card> _currentSequence = new List<Card>();

		public Card CurrentCard;

		private void Start()
		{
			ResetAll();
		}

		public void ResetAll()
		{
			CurrentCard = null;
			_singleCards.Clear();
			_sequenceCards.Clear();
			_specialCards.Clear();
			_discardedCards.Clear();
			_currentSequence.Clear();

			foreach (Card card in _cards)
			{
				// Special cards
				if (card.SpecialCard)
				{
					_specialCards.Add(card.Name, card);
					continue;
				}

				// Cards in sequences
				if (card.SequenceName != "")
				{
					if (!_sequenceCards.ContainsKey(card.SequenceName))
					{
						_sequenceCards.Add(card.SequenceName, new List<Card>());
					}

					_sequenceCards[card.SequenceName].Add(card);
					continue;
				}

				// Single cards
				_singleCards.Add(card.name, card);
			}

			// Sort the sequence lists to make sure sequence order is correct
			foreach (KeyValuePair<string, List<Card>> kvp in _sequenceCards)
			{
				kvp.Value.Sort((a, b) => a.SequenceOrder.CompareTo(b.SequenceOrder));
			}
		}

		private void DiscardCard(Card card)
		{
			_discardedCards.Add(card);

			_singleCards.Remove(card.name);
			
			if (_sequenceCards.ContainsKey(card.SequenceName))
			{
				_sequenceCards.Remove(card.SequenceName);
			}
		}

		private void LoadSingleCard(string cardName)
		{
			LoadCard(cardName, false, false);
		}

		private void LoadSequenceCard(string sequenceName, int sequenceOrder)
		{
			LoadCard("", true, false, sequenceName, sequenceOrder);
		}

		private void LoadSpecialCard(string cardName)
		{
			LoadCard(cardName, false, true);
		}

		private void LoadCard(string cardName, bool sequenceCard, bool specialCard, string sequenceName = "", int sequenceOrder = 0)
		{
			if (specialCard)
			{
				CurrentCard = _specialCards[cardName];
				if (CurrentCard.GameOverCard)
				{
					GameManager.Instance.GameOver = true;
					AudioManager.Instance.PlayMusicClip("GameOver");
				}
			}

			if (sequenceCard)
			{
				if (sequenceOrder == 0)
				{
					// Cache the current sequence before discarding it from the dictionary
					_currentSequence = _sequenceCards[sequenceName];
					CurrentCard = _sequenceCards[sequenceName][0];
				}
				else
				{
					CurrentCard = _currentSequence[sequenceOrder];
				}
			}

			if (!sequenceCard && !specialCard)
			{
				CurrentCard = _singleCards[cardName];
			}

			UIManager.Instance.LoadCard(CurrentCard);
		}

		public void LoadNextCard(int nextInSequence = 0)
		{
			if (CurrentCard == null)
			{
				LoadSpecialCard("Intro");
				return;
			}

			if (CurrentCard.Name == "Intro")
			{
				LoadSequenceCard("IntroSequence", 0);
				return;
			}

			string currentSequenceName = CurrentCard.SequenceName;

			DiscardCard(CurrentCard);

			if (nextInSequence > 0)
			{
				LoadSequenceCard(currentSequenceName, nextInSequence);
				return;
			}

			DrawRandomCard();
		}

		private void DrawRandomCard()
		{
			if (_singleCards.Count == 0)
			{
				DrawRandomSequenceCard();
			}

			if (_sequenceCards.Count == 0)
			{
				DrawRandomSingleCard();
			}

			// ~30% chance to draw a sequence card
			bool sequenceCard = Random.value < 0.3f;

			if (sequenceCard)
			{
				DrawRandomSequenceCard();
			}
			else
			{
				DrawRandomSingleCard();
			}
		}

		private void DrawRandomSequenceCard()
		{
			if (_sequenceCards.Count == 0)
			{
				LoadSpecialCard("End");
				return;
			}

			List<string> sequenceNames = new List<string>(_sequenceCards.Keys);
			LoadSequenceCard(sequenceNames[Random.Range(0, sequenceNames.Count)], 0);
		}

		private void DrawRandomSingleCard()
		{
			if (_singleCards.Count == 0)
			{
				LoadSpecialCard("End");
				return;
			}

			List<string> cardNames = new List<string>(_singleCards.Keys);
			LoadSingleCard(cardNames[Random.Range(0, cardNames.Count)]);
		}

		public void GameOver(StatType stat)
		{
			LoadSpecialCard("GameOver" + stat.ToString());
		}

		public void LoadThankYouCard()
		{
			LoadSpecialCard("ThankYou");
		}
	}
}