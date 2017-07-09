using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Bonus))]
public class BonusEditor : Editor 
{
	public override void OnInspectorGUI()
    {
        Bonus bonus = target as Bonus;

        if(!bonus) return;

        bonus.Type = (BonusType)(EditorGUILayout.EnumPopup("Bonus Type:", bonus.Type));

        if(bonus.Type == BonusType.Ammo)
        {
            bonus.AmmoType = (AmmoType)(EditorGUILayout.EnumPopup("Ammo Type:", bonus.AmmoType));
        }

        bonus.Value = EditorGUILayout.FloatField("Value", bonus.Value);
    }
}
