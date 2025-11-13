using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Lightweight in-game developer console for entering debug commands.
/// Toggle with the backquote (`) key by default.
/// </summary>
public class DeveloperConsole : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;
    [SerializeField] private float consoleHeightRatio = 0.33f;
    [SerializeField] private string inputPlaceholder = "Enter command...";

    private readonly List<string> outputLog = new List<string>();
    private readonly Dictionary<string, Action<DeveloperConsole, string[]>> commands =
        new Dictionary<string, Action<DeveloperConsole, string[]>>(StringComparer.OrdinalIgnoreCase);

    private bool isOpen;
    private string inputBuffer = string.Empty;
    private Vector2 scrollPosition;
    private GUIStyle placeholderStyle;
    private readonly List<string> commandHistory = new List<string>();
    private int historyIndex = -1;

    public IEnumerable<string> CommandNames => commands.Keys;

    void Awake()
    {
        DeveloperConsoleCommandRegistry.Populate(commands);
        Log("Developer console ready. Type 'help' for a list of commands.");
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleConsole();
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConsole();
        }
    }

    void OnGUI()
    {
        if (!isOpen)
        {
            return;
        }

        var consoleHeight = Screen.height * Mathf.Clamp01(consoleHeightRatio);
        var consoleRect = new Rect(0f, 0f, Screen.width, consoleHeight);
        var logRect = new Rect(10f, 10f, consoleRect.width - 20f, consoleRect.height - 50f);
        var inputRect = new Rect(10f, consoleRect.height - 35f, consoleRect.width - 20f, 25f);

        GUI.Box(consoleRect, GUIContent.none);

        var contentHeight = Mathf.Max(logRect.height, 20f * outputLog.Count);
        scrollPosition = GUI.BeginScrollView(
            logRect,
            scrollPosition,
            new Rect(0f, 0f, logRect.width - 25f, contentHeight));

        var currentEvent = Event.current;
        var submitRequested = currentEvent.type == EventType.KeyDown &&
            (currentEvent.keyCode == KeyCode.Return || currentEvent.keyCode == KeyCode.KeypadEnter);

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == toggleKey)
        {
            CloseConsole();
            GUI.FocusControl(null);
            currentEvent.Use();
            return;
        }

        GUILayout.BeginVertical();
        foreach (var line in outputLog)
        {
            GUILayout.Label(line);
        }
        GUILayout.EndVertical();

        GUI.EndScrollView();

        if (placeholderStyle == null)
        {
            placeholderStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = GUI.skin.textField.fontSize,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.gray }
            };
        }

        GUI.SetNextControlName("DeveloperConsoleInput");
        inputBuffer = GUI.TextField(inputRect, inputBuffer);

        if (GUI.GetNameOfFocusedControl() != "DeveloperConsoleInput")
        {
            GUI.FocusControl("DeveloperConsoleInput");
        }

        if (string.IsNullOrEmpty(inputBuffer))
        {
            var placeholderRect = new Rect(inputRect.x + 6f, inputRect.y + 2f, inputRect.width - 12f, inputRect.height - 4f);
            GUI.Label(placeholderRect, inputPlaceholder, placeholderStyle);
        }

        if (submitRequested)
        {
            SubmitInput();
            currentEvent.Use();
        }

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.UpArrow)
        {
            RecallHistory(-1);
            currentEvent.Use();
        }

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.DownArrow)
        {
            RecallHistory(1);
            currentEvent.Use();
        }
    }

    private void ToggleConsole()
    {
        if (isOpen)
        {
            CloseConsole();
        }
        else
        {
            OpenConsole();
        }
    }

    private void OpenConsole()
    {
        isOpen = true;
        scrollPosition = Vector2.zero;
    }

    private void CloseConsole()
    {
        isOpen = false;
        inputBuffer = string.Empty;
    }

    private void SubmitInput()
    {
        var trimmedInput = inputBuffer.Trim();
        inputBuffer = string.Empty;

        if (string.IsNullOrEmpty(trimmedInput))
        {
            return;
        }

        outputLog.Clear();
        scrollPosition = Vector2.zero;
        Log($"> {trimmedInput}");
        commandHistory.Add(trimmedInput);
        historyIndex = commandHistory.Count;

        var split = trimmedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var commandName = split[0];
        var args = split.Skip(1).ToArray();

        if (!commands.TryGetValue(commandName, out var command))
        {
            Log($"Unknown command '{commandName}'. Type 'help' to list available commands.");
            return;
        }

        try
        {
            command.Invoke(this, args);
        }
        catch (Exception ex)
        {
            Log($"Command error: {ex.Message}");
        }
    }

    private void RecallHistory(int direction)
    {
        if (commandHistory.Count == 0)
        {
            return;
        }

        historyIndex = Mathf.Clamp(historyIndex + direction, 0, commandHistory.Count);

        if (historyIndex >= 0 && historyIndex < commandHistory.Count)
        {
            inputBuffer = commandHistory[historyIndex];
        }
        else
        {
            inputBuffer = string.Empty;
        }
    }

    public void PrintHelp()
    {
        Log("Available commands:");
        foreach (var key in commands.Keys.OrderBy(k => k))
        {
            Log($"- {key}");
        }
    }

    public void Log(string message)
    {
        outputLog.Add(message);
        scrollPosition.y = float.MaxValue;
    }
}

