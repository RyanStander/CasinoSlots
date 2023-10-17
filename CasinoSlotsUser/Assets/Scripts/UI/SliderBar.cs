using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        private void OnValidate()
        {
            if (slider == null)
                slider = GetComponent<Slider>();
        }

        public void SetMaxValue(float maxValue)
        {
            //checks if there is a fill rect set for the slider
            if (slider != null && slider.fillRect != null)
            {
                slider.maxValue = maxValue;
                slider.value = maxValue;
            }
            else
            {
                Debug.LogWarning("No slider fill for bar was found, player value cannot be updated. Please add a slider fill in the slider component");
            }
        }

        //updates the players current value
        public void SetCurrentValue(float currentValue)
        {
            //checks if there is a fill rect set for the slider
            if (slider != null && slider.fillRect != null)
            {
                slider.value = currentValue;
            }
            else
            {
                Debug.LogWarning("No slider fill for value bar was found, player value cannot be updated. Please add a slider fill in the slider component");
            }
        }
    }
}
