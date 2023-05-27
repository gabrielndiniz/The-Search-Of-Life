using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeatEmUp.Combat
{
    public class HealthDisplays : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private Health enemyHealth;
        [SerializeField] private Image enemyHealthBar;

        // Update is called once per frame
        void Update()
        {
            if (playerHealth)
            {
                playerHealthBar.fillAmount = playerHealth.GetHealthPoints() / playerHealth.GetMaxHealthPoints();
            }
            
            
            if (enemyHealth)
            {
                enemyHealthBar.fillAmount = enemyHealth.GetHealthPoints() / enemyHealth.GetMaxHealthPoints();
            }
        }
    }
}