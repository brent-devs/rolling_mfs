using System;
using System.Collections.Generic;
using UnityEngine;

public static class DeveloperConsoleCommandRegistry
{
    public static void Populate(IDictionary<string, Action<DeveloperConsole, string[]>> commands)
    {
        if (commands == null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        commands["help"] = (console, args) =>
        {
            console.PrintHelp();
        };

        commands["liststates"] = (console, args) =>
        {
            var names = Enum.GetNames(typeof(InteractionState));
            console.Log("Interaction states: " + string.Join(", ", names));
        };

        commands["getstate"] = (console, args) =>
        {
            var handler = InteractionHandling.Instance;
            if (handler == null)
            {
                console.Log("InteractionHandling singleton is not available in the scene.");
                return;
            }

            console.Log($"Current interaction state: {handler.CurrState}");
        };

        commands["setstate"] = SetInteractionState;
        commands["state"] = SetInteractionState;
    }

    private static void SetInteractionState(DeveloperConsole console, string[] args)
    {
        if (args.Length == 0)
        {
            console.Log("Usage: setstate <None|Idle|Shopping|MyTurn|Grabbing>");
            return;
        }

        var handler = InteractionHandling.Instance;
        if (handler == null)
        {
            console.Log("InteractionHandling singleton is not available in the scene.");
            return;
        }

        if (!Enum.TryParse(args[0], true, out InteractionState desiredState))
        {
            console.Log($"Unknown interaction state '{args[0]}'.");
            return;
        }

        handler.CurrState = desiredState;
        console.Log($"Interaction state changed to {desiredState}.");
    }
}

