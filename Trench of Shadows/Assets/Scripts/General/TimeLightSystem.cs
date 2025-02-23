using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace WorldTime
{
    public class TimeLightSystem : MonoBehaviour
    {
        [SerializeField] private WorldTime _worldTime;
        [SerializeField] private Light2D _lampLight;
        [SerializeField] private int turnOnHour = 18;  // Ora di accensione (es. 18:00)
        [SerializeField] private int turnOffHour = 6;  // Ora di spegnimento (es. 06:00)
        [SerializeField] private float transitionDuration = 3f; // Durata transizione (es. 3 secondi)
        [SerializeField] private float flickerIntensity = 0.2f; // Intensità dello sfarfallio
        [SerializeField] private float flickerSpeed = 0.1f; // Velocità dello sfarfallio

        private Coroutine _lightTransitionCoroutine;
        private Coroutine _flickerCoroutine;
        private bool _isOn = false;

        private void Awake()
        {
            if (_worldTime != null)
            {
                _worldTime.WorldTimeChanged += OnWorldTimeChanged;
            }
        }

        private void OnDestroy()
        {
            if (_worldTime != null)
            {
                _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
            }
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            int currentHour = newTime.Hours;
            bool shouldBeOn = IsNightTime(currentHour);

            if (shouldBeOn != _isOn)
            {
                _isOn = shouldBeOn;
                if (_lightTransitionCoroutine != null)
                {
                    StopCoroutine(_lightTransitionCoroutine);
                }
                _lightTransitionCoroutine = StartCoroutine(FadeLight(_isOn));

                if (_isOn)
                {
                    // Avvia lo sfarfallio quando la luce è accesa
                    if (_flickerCoroutine == null)
                        _flickerCoroutine = StartCoroutine(FlickerEffect());
                }
                else
                {
                    // Ferma lo sfarfallio quando la luce si spegne
                    if (_flickerCoroutine != null)
                    {
                        StopCoroutine(_flickerCoroutine);
                        _flickerCoroutine = null;
                    }
                }
            }
        }

        private bool IsNightTime(int currentHour)
        {
            if (turnOnHour < turnOffHour)
            {
                return currentHour >= turnOnHour && currentHour < turnOffHour;
            }
            else
            {
                return currentHour >= turnOnHour || currentHour < turnOffHour;
            }
        }

        private IEnumerator FadeLight(bool turnOn)
        {
            float startIntensity = _lampLight.intensity;
            float targetIntensity = turnOn ? 1f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                _lampLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _lampLight.intensity = targetIntensity;
        }

        private IEnumerator FlickerEffect()
        {
            while (true)
            {
                float randomIntensity = 1f + UnityEngine.Random.Range(-flickerIntensity, flickerIntensity);
                _lampLight.intensity = Mathf.Clamp(randomIntensity, 0.8f, 1.2f);
                yield return new WaitForSeconds(UnityEngine.Random.Range(flickerSpeed, flickerSpeed * 2));
            }
        }
    }
}
