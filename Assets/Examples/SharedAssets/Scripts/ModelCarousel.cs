using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
// using Unity.InferenceEngine; // 주석 처리
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using UnityEditor;
#if UNITY_EDITOR
// using UnityEditor.Recorder; // 주석 처리
#endif
using UnityEngine;

/**
 *  Usage Notes:
 *
 *  Add onnx models to the list and they will be played sequentially.
 *  Create a recorder and a video of the sequence will be captured automatically if "Auto Record" is selected.
 *  Recording only works in the Editor (not in standalone build)
 *  Create a TextMeshPro Text GameObject and attach it to have the number of training steps of the current model shown.
 *  To manually control transition between models choose a very large time, or "Pause" the system,
 *  then use "Force Next" to advance.
 *  "Reset" will start the sequence from the beginning again, use "Start" to proceed after resetting.
 *  "Time Scale Override" can be set, "Seconds Between Switches" will decrease proportionally if you increase this
 *  (i.e it represents simulated seconds between switches, not real time)
 */

public class ModelCarousel : MonoBehaviour
{
    public bool m_Start = true;
    public bool m_Reset = false;
    public bool m_Pause = true;
    public bool m_ForceNext = false;
    public bool m_Loop = false;
    public bool m_AutoRecord = true;
    public bool m_ResetAgentOnModelChange = false;
    public int m_SecondsBetweenSwitches = 10;
    public float m_TimeScaleOverride = 0.0f;
    // ModelAsset 대신 UnityEngine.Object 사용
    public List<UnityEngine.Object> m_Models = new List<UnityEngine.Object>();
    public bool m_ShowStepNumber = true;
    public int m_StepNumberRounding = 10000;

    private int m_StepsSinceLastSwitch = 0;
    private int m_CurrentModelIndex = 0;
    private int m_CurrentlySetModelIndex = -1;

    // ModelAsset 대신 UnityEngine.Object 사용
    private UnityEngine.Object m_OriginalModel = null;

    private int k_FixedUpdatePerSecond;

    // The attached Agent
    Agent m_Agent;

    public TextMeshProUGUI textMeshComponent;

#if UNITY_EDITOR
    // RecorderWindow 관련 코드 주석 처리하고 대체
    private object GetRecorderWindow()
    {
        // return (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow));
        Debug.LogWarning("Recorder functionality is disabled - RecorderWindow not available");
        return null;
    }
#endif

    private void Reset()
    {
        m_Reset = false;
        m_StepsSinceLastSwitch = 0;
        m_CurrentModelIndex = 0;
        // m_Agent.SetModel(m_OriginalModel.name, m_OriginalModel);
        if (m_OriginalModel != null)
        {
            Debug.Log($"Resetting to original model: {m_OriginalModel.name}");
        }
        textMeshComponent?.SetText("Ready to Start");
    }

    private void OnEnable()
    {
        m_Agent = GetComponent<Agent>();
        var behaviorParams = m_Agent.GetComponent<BehaviorParameters>();
        if (behaviorParams != null)
        {
            m_OriginalModel = behaviorParams.Model;
        }

        Reset();

        k_FixedUpdatePerSecond = (int)(1.0f / Time.fixedDeltaTime);

        if (m_TimeScaleOverride > 0.0f)
        {
            Time.timeScale = m_TimeScaleOverride;
        }
    }

    void StartRecording()
    {
#if UNITY_EDITOR
        if (!m_AutoRecord)
            return;

        Debug.Log("Starting Recording - Recorder functionality disabled");
        // RecorderWindow recorderWindow = GetRecorderWindow();
        // if (!recorderWindow.IsRecording())
        //     recorderWindow.StartRecording();
#endif
    }

    void StopRecording()
    {
#if UNITY_EDITOR
        if (!m_AutoRecord)
            return;

        Debug.Log("Stopping Recording - Recorder functionality disabled");
        // RecorderWindow recorderWindow = GetRecorderWindow();
        // if (recorderWindow.IsRecording())
        //     recorderWindow.StopRecording();
#endif
    }

    void UpdateStepNumberText()
    {
        if (!m_ShowStepNumber)
            return;

        if (m_CurrentModelIndex >= 0 && m_CurrentModelIndex < m_Models.Count && m_Models[m_CurrentModelIndex] != null)
        {
            var result = Regex.Match(m_Models[m_CurrentModelIndex].name, @".*-(\d+)$");

            string newText = "";
            if (result.Success && result.Groups.Count > 0)
            {
                var steps = Int32.Parse(result.Groups[1].Captures[0].Value);

                int round = m_StepNumberRounding;
                steps += round / 2;
                steps /= round;
                steps *= round;

                newText = $"After {steps:n0} steps";
            }

            textMeshComponent?.SetText(newText);
        }
    }

    void SetModel()
    {
        if (m_CurrentModelIndex < 0 || m_CurrentModelIndex >= m_Models.Count)
            return;

        if (m_Models[m_CurrentModelIndex] != null)
        {
            // m_Agent.SetModel(m_Models[m_CurrentModelIndex].name, m_Models[m_CurrentModelIndex]);
            Debug.Log($"Setting model: {m_Models[m_CurrentModelIndex].name}");
        }
        m_CurrentlySetModelIndex = m_CurrentModelIndex;

        UpdateStepNumberText();

        if (m_ResetAgentOnModelChange)
            m_Agent.EndEpisode();
    }

    void FixedUpdate()
    {
        if (m_Start)
        {
            m_Start = false;
            m_Pause = false;
            StartRecording();
        }

        if (m_Reset)
        {
            StopRecording();
            Reset();
            m_Pause = true;
            m_Start = false;
        }

        if (m_Pause && !m_ForceNext)
            return;

        if (m_CurrentlySetModelIndex != m_CurrentModelIndex)
        {
            SetModel();
        }

        m_StepsSinceLastSwitch++;

        if (m_StepsSinceLastSwitch >= m_SecondsBetweenSwitches * k_FixedUpdatePerSecond || m_ForceNext)
        {
            m_ForceNext = false;
            m_StepsSinceLastSwitch = 0;
            m_CurrentModelIndex++;

            if (m_CurrentModelIndex == m_Models.Count)
            {
                if (m_Loop)
                {
                    m_CurrentModelIndex = 0;
                }
                else
                {
                    Application.Quit(0);
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#endif
                    return;
                }
            }

            SetModel();
        }
    }
}