using System;
using System.Collections;
using UnityEngine;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField]
        private float _dayLength;

        [SerializeField]
        private int _startHour = 6;

        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        private void Start()
        {
            _currentTime = TimeSpan.FromHours(_startHour);
            StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, _currentTime);
            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }

        public TimeSpan GetCurrentTime()
        {
            return _currentTime;
        }

        // ➤ Skip the night and reset time to 06:00 AM
        public void SkipNight()
        {
            if (_currentTime.Hours >= 18 || _currentTime.Hours < 6)
            {
                _currentTime = TimeSpan.FromHours(6);
                WorldTimeChanged?.Invoke(this, _currentTime);
            }
        }
    }
}
