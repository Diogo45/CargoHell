﻿using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using DesamplingRate = StaticBluredScreen.DesamplingRate;

/// <summary>
/// StaticBluredScreen editor.
/// </summary>
[CustomEditor(typeof(StaticBluredScreen))]
[CanEditMultipleObjects]
public class StaticBluredScreenEditor : RawImageEditor
{
	//################################
	// Constant or Static Members.
	//################################

	public enum QualityMode : int
	{
		Fast = (DesamplingRate.x2 << 0) + (DesamplingRate.x2 << 4) + (FilterMode.Bilinear << 8) + (1 << 10),
		Medium = (DesamplingRate.x1 << 0) + (DesamplingRate.x1 << 4) + (FilterMode.Bilinear << 8) + (1 << 10),
		Detail = (DesamplingRate.None << 0) + (DesamplingRate.x1 << 4) + (FilterMode.Bilinear << 8) + (1 << 10),
		Custom = -1,
	}


	//################################
	// Public/Protected Members.
	//################################
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	protected override void OnEnable()
	{
		base.OnEnable();
		_spTexture = serializedObject.FindProperty("m_Texture");
		_spColor = serializedObject.FindProperty("m_Color");
		_spRaycastTarget = serializedObject.FindProperty("m_RaycastTarget");
		_spDesamplingRate = serializedObject.FindProperty("m_DesamplingRate");
		_spReductionRate = serializedObject.FindProperty("m_ReductionRate");
		_spFilterMode = serializedObject.FindProperty("m_FilterMode");
		_spIterations = serializedObject.FindProperty("m_Iterations");
		_spKeepSizeToRootCanvas = serializedObject.FindProperty("m_KeepCanvasSize");
			

		_customAdvancedOption = (qualityMode == QualityMode.Custom);
	}

	/// <summary>
	/// Implement this function to make a custom inspector.
	/// </summary>
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		//================
		// Basic properties.
		//================
		EditorGUILayout.PropertyField(_spTexture);
		EditorGUILayout.PropertyField(_spColor);
		EditorGUILayout.PropertyField(_spRaycastTarget);

		//================
		// Capture effect.
		//================
		GUILayout.Space(10);
		EditorGUILayout.LabelField("Capture Effect", EditorStyles.boldLabel);
		DrawEffectProperties(StaticBluredScreen.shaderName, serializedObject);

		//================
		// Advanced option.
		//================
		GUILayout.Space(10);
		EditorGUILayout.LabelField("Advanced Option", EditorStyles.boldLabel);

		EditorGUI.BeginChangeCheck();
		QualityMode quality = qualityMode;
		quality = (QualityMode)EditorGUILayout.EnumPopup("Quality Mode", quality);
		if (EditorGUI.EndChangeCheck())
		{
			_customAdvancedOption = (quality == QualityMode.Custom);
			qualityMode = quality;
		}

		// When qualityMode is `Custom`, show advanced option.
		if (_customAdvancedOption)
		{
			DrawDesamplingRate(_spDesamplingRate);// Desampling rate.
			DrawDesamplingRate(_spReductionRate);// Reduction rate.
			EditorGUILayout.PropertyField(_spFilterMode);// Filter Mode.
			EditorGUILayout.PropertyField(_spIterations);// Iterations.
		}
		EditorGUILayout.PropertyField(_spKeepSizeToRootCanvas);// Iterations.

		serializedObject.ApplyModifiedProperties();

		// Debug.
		using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
		{
			GUILayout.Label("Debug");

			if (GUILayout.Button("Capture", "ButtonLeft"))
				UpdateTexture(true);

			EditorGUI.BeginDisabledGroup(!(target as StaticBluredScreen).capturedTexture);
			if (GUILayout.Button("Release", "ButtonRight"))
				UpdateTexture(false);
			EditorGUI.EndDisabledGroup();
		}
	}

	//################################
	// Private Members.
	//################################
	const int Bits4 = (1 << 4) - 1;
	const int Bits2 = (1 << 2) - 1;
	bool _customAdvancedOption = false;
	SerializedProperty _spTexture;
	SerializedProperty _spColor;
	SerializedProperty _spRaycastTarget;
	SerializedProperty _spDesamplingRate;
	SerializedProperty _spReductionRate;
	SerializedProperty _spFilterMode;
	SerializedProperty _spIterations;
	SerializedProperty _spKeepSizeToRootCanvas;

	QualityMode qualityMode
	{
		get
		{
			if (_customAdvancedOption)
				return QualityMode.Custom;

			int qualityValue = (_spDesamplingRate.intValue << 0)
			                    + (_spReductionRate.intValue << 4)
			                    + (_spFilterMode.intValue << 8)
			                    + (_spIterations.intValue << 10);

			return System.Enum.IsDefined(typeof(QualityMode), qualityValue) ? (QualityMode)qualityValue : QualityMode.Custom;
		}
		set
		{
			if (value != QualityMode.Custom)
			{
				int qualityValue = (int)value;
				_spDesamplingRate.intValue = (qualityValue >> 0) & Bits4;
				_spReductionRate.intValue = (qualityValue >> 4) & Bits4;
				_spFilterMode.intValue = (qualityValue >> 8) & Bits2;
				_spIterations.intValue = (qualityValue >> 10) & Bits4;
			}
		}
	}


	/// <summary>
	/// Draw effect properties.
	/// </summary>
	public static void DrawEffectProperties(string shaderName, SerializedObject serializedObject)
	{
		bool changed = false;

		//================
		// Effect material.
		//================
		var spMaterial = serializedObject.FindProperty("m_EffectMaterial");
		EditorGUI.BeginDisabledGroup(true);
		EditorGUILayout.PropertyField(spMaterial);
		EditorGUI.EndDisabledGroup();

		//================
		// Blur setting.
		//================
		var spBlurMode = serializedObject.FindProperty("m_BlurMode");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(spBlurMode);
		changed |= EditorGUI.EndChangeCheck();

		// When blur is enable, show parameters.
		if (spBlurMode.intValue != (int)StaticBluredScreen.BlurMode.None)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Blur"));
			EditorGUI.indentLevel--;
		}

		// Set effect material.
		if (!serializedObject.isEditingMultipleObjects && spBlurMode.intValue == 0)
		{
			spMaterial.objectReferenceValue = null;
		}
		else if (changed || !serializedObject.isEditingMultipleObjects)
		{
			spMaterial.objectReferenceValue = StaticBluredScreen.GetOrGenerateMaterialVariant(Shader.Find(shaderName),
				(StaticBluredScreen.BlurMode)spBlurMode.intValue
			);
		}
	}

	/// <summary>
	/// Draws the desampling rate.
	/// </summary>
	void DrawDesamplingRate(SerializedProperty sp)
	{
		using (new EditorGUILayout.HorizontalScope())
		{
			EditorGUILayout.PropertyField(sp);
			int w, h;
			(target as StaticBluredScreen).GetDesamplingSize((DesamplingRate)sp.intValue, out w, out h);
			GUILayout.Label(string.Format("{0}x{1}", w, h), EditorStyles.miniLabel);
		}
	}

	/// <summary>
	/// Updates the texture.
	/// </summary>
	void UpdateTexture(bool capture)
	{
		var current = target as StaticBluredScreen;
		bool enable = current.enabled;
		current.enabled = false;
		current.Release();
		if (capture)
			current.Capture();
			
		EditorApplication.delayCall += () => current.enabled = enable;
	}
}
