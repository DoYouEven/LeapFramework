using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 16;
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Now draw the property as a Slider or an IntSlider based on whether it’s a float or integer.
        if (property.type != "MinMaxRange")
            Debug.LogWarning("Use only with MinMaxRange type");
        else
        {
            var range = attribute as MinMaxRangeAttribute;
            var minValue = property.FindPropertyRelative("rangeStart");
            var maxValue = property.FindPropertyRelative("rangeEnd");
            var newMin = minValue.floatValue;
            var newMax = maxValue.floatValue;

            var xDivision = position.width * 0.33f;
            var xSliderDivision = position.width * 0.35f;
            var yDivision = position.height * 0.5f;
            EditorGUI.LabelField(new Rect(position.x, position.y, xDivision, yDivision)
                                 , label.text + " [ " + range.minLimit.ToString("0.##") + ", " + range.maxLimit.ToString("0.##") + " ]");


            //EditorGUI.LabelField( new Rect( position.x + position.width - 28f, position.y + yDivision, position.width, yDivision )
            // , range.maxLimit.ToString( "0.##" ) );

            EditorGUI.MinMaxSlider(new Rect(position.x + xDivision + 60, position.y, xSliderDivision, yDivision)
                                   , ref newMin, ref newMax, range.minLimit, range.maxLimit);


            newMin = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision, position.y, 50, yDivision)
                                                       , newMin)
                                 , range.minLimit, newMax);

            newMax = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision + xSliderDivision + 70, position.y, 50, yDivision)
                                                       , newMax)
                                 , newMin, range.maxLimit);

            minValue.floatValue = newMin;
            maxValue.floatValue = newMax;
        }
    }
}