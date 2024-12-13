using UnityEngine;
using UnityEngine.UI;

namespace NinjaChompek
{
	public class MuteAudio : MonoBehaviour
	{
		[SerializeField] private Sprite _audioPlayingSprite;
		[SerializeField] private Sprite _audioMutedSprite;

		private Button _button;
		private Image _image;

		private void Start()
		{
			_button = GetComponent<Button>();
			_image = GetComponent<Image>();
			
			_button.onClick.AddListener(ToggleMute);
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.M))
			{
				ToggleMute();
			}
		}

		private void ToggleMute()
		{
			AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
			_image.sprite = AudioListener.volume == 0 ? _audioMutedSprite : _audioPlayingSprite;
		}
	}
}
