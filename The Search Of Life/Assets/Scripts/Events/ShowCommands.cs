using System;
using UnityEngine;


namespace BeatEmUp.Events
{
    public class ShowCommands : MonoBehaviour
    {
        private Fader fader;
        private CanvasGroup canvasGroup;
        private bool bStartFadeOut = true;


        private void OnEnable()
        {
            fader = GetComponent<Fader>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (fader != null)
            {
                fader.FadeIn();
            }
        }

        private void Update()
        {
            if (fader != null && canvasGroup.alpha >= 1f && bStartFadeOut)
            {
                bStartFadeOut = false;
                fader.FadeOut();
            }

            if (fader != null && canvasGroup.alpha <= 0.0001f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}