#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TurnBased
{
	public class MainMenu : MonoBehaviour
	{
		//[SerializeField] AudioMixer _mixer;

		[SerializeField] GameObject _default;
		[SerializeField] GameObject _options;
		[SerializeField] GameObject _credits;

		public void ShowOptions()
        {
			_default.SetActive(false);
			_options.SetActive(true);
		}

		public void GoBack()
        {
			_default.SetActive(true);
			_options.SetActive(false);
			_credits.SetActive(false);

		}

		public void ShowCredits()
        {
			_credits.SetActive(true);
			_default.SetActive(false);

		}

        public void Quit()
        {
			Application.Quit();
        }

    }
}
