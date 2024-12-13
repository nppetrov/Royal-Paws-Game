using UnityEngine;

namespace NinjaChompek
{
	[DefaultExecutionOrder(100)]
	public class AudioManager : Manager<AudioManager>
	{
		[SerializeField] private AudioSource _musicLoop;
		[SerializeField] private AudioSource _musicGameOver;

		[SerializeField] private AudioSource _sfxDice;
		[SerializeField] private AudioSource _sfxButton;

		public void PlayMusicClip(string music)
		{
			switch (music)
			{
				case "MusicLoop":
					if (_musicLoop.isPlaying)
					{
						return;
					}
					_musicGameOver.Stop();
					_musicLoop.Play();
					break;
				case "GameOver":
					if (_musicGameOver.isPlaying)
					{
						return;
					}
					_musicLoop.Stop();
					_musicGameOver.Play();
					break;

				default:
					Debug.LogWarning("Music not found: " + music);
					break;
			}
		}

		public void PlaySound(string sound)
		{
			switch (sound)
			{
				case "DiceRoll":
					_sfxDice.Play();
					break;
				case "Click":
					_sfxButton.Play();
					break;

				default:
					Debug.LogWarning("Sound not found: " + sound);
					break;
			}
		}

		private void Start()
		{
			PlayMusicClip("MusicLoop");
		}

		private void Update()
		{
			// Mouse click SFX
			if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
			{
				PlaySound("Click");
			}
		}
	}
}