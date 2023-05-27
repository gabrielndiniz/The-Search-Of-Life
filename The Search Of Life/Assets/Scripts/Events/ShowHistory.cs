using UnityEngine;

namespace BeatEmUp.Events
{
    public class ShowHistory : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Combat.Combat combat;
        [SerializeField] private float delay = 2f;
        [SerializeField] private GoToTarget goToTarget;
        [SerializeField] private Fader fader;

        [SerializeField] private GameObject enemy;
        [SerializeField] private AudioSource music;

        private bool bFirstTimePressingAttack = true;
        private float timeAfterPressing = 0;
        private bool bStartedMoving = false;

        // Update is called once per frame
        void Update()
        {
            if (!bFirstTimePressingAttack)
            {
                timeAfterPressing += Time.deltaTime;
            }

            if (Input.GetButtonDown("Fire1") && bFirstTimePressingAttack)
            {
                // Player input fire command once and bFirstTimePressingAttack is true
                canvasGroup.alpha = 1;
                bFirstTimePressingAttack = false;
                timeAfterPressing = 0;
            }

            if (Input.GetButtonDown("Fire1") && !bFirstTimePressingAttack && timeAfterPressing > delay && !bStartedMoving)
            {
                // bFirstTimePressingAttack is false and enough time has passed after pressing attack
                combat.enabled = true;
                enemy.SetActive(true);
                goToTarget.StartMove();
                bStartedMoving = true;
                timeAfterPressing = 0;
                fader.FadeIn();
                canvasGroup.alpha = 0;
                music.Stop();
            }

            if (bStartedMoving && timeAfterPressing > delay)
            {
                // bStartedMoving is true and enough time has passed after starting movement
                Destroy(gameObject);
            }
        }
    }
}