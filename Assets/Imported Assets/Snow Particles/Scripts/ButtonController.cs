using UnityEngine;

namespace STSnowParticles
{
    public class ButtonController : MonoBehaviour
    {
        [Header("Rain Type")]
        [SerializeField] private GameObject lightSnow;
        [SerializeField] private GameObject mediumSnow;
        [SerializeField] private GameObject heavySnow;

        public void ShowLightSnow()
        {
            DisableSnow();
            lightSnow.SetActive(true);
        }

        public void ShowMediumSnow()
        {
            DisableSnow();
            mediumSnow.SetActive(true);
        }

        public void ShowHeavySnow()
        {
            DisableSnow();
            heavySnow.SetActive(true);
        }

        void DisableSnow()
        {
            lightSnow.SetActive(false);
            mediumSnow.SetActive(false);
            heavySnow.SetActive(false);
        }
    }
}
