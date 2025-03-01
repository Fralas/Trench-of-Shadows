using TMPro;
using UnityEngine;
using System;

namespace WorldTime
{
    [RequireComponent(typeof(TMP_Text))]
    public class DayCounter : MonoBehaviour
    {
        [SerializeField]
        private WorldTime _worldTime;
        private TMP_Text _text;
        private int day = 1;
        private int lastDay = -1; 

        [SerializeField]
        private int DayCountingHour;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
            _text.SetText(day.ToString()); 
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            if (newTime.Hours == DayCountingHour && newTime.Minutes == 0){
                int currentDay = (int)(newTime.TotalHours / 24);
                if (currentDay != lastDay) 
                {
                    lastDay = currentDay;
                    day++;
                    _text.SetText(day.ToString());
                }
            }
        }
    }
}
