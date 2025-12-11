using System.Collections.Generic;
using System.Linq;

public static class KeyToSymbol
{
    /// <summary>
    /// Diccionario: (controlSchemeNormalizado, keyNormalizada) -> símbolo.
    /// </summary>
    private static readonly Dictionary<(string scheme, string key), string> symbols =
        new Dictionary<(string scheme, string key), string>
        {
            // Keyboard & Mouse
            {("keyboard&mouse", "space"),        "␺"},
            {("keyboard&mouse", "leftarrow"),    "←"},
            {("keyboard&mouse", "rightarrow"),   "→"},
            {("keyboard&mouse", "uparrow"),      "↑"},
            {("keyboard&mouse", "downarrow"),    "↓"},
            {("keyboard&mouse", "w"),            "␣"}, // Move

            // Mouse
            {("keyboard&mouse", "leftbutton"),   "⟵"},
            {("keyboard&mouse", "rightbutton"),  "⟶"},

            // Generic gamepad
            {("gamepad", "rt"),                  "↗"},
            {("gamepad", "rb"),                  "↝"},
            {("gamepad", "rs"),                  "⇌"},

            // Xbox
            {("xbox", "a"),                      "🅐"},
            {("xbox", "b"),                      "🅑"},
            {("xbox", "x"),                      "🅧"},
            {("xbox", "y"),                      "🅨"},
            {("xbox", "rt"),                     "↗"},
            {("xbox", "rb"),                     "↝"},
            {("xbox", "rs"),                     "⇌"},

            // PlayStation
            {("playstation", "cross"),           "⇣"},
            {("playstation", "triangle"),        "⇡"},
            {("playstation", "circle"),          "⇢"},
            {("playstation", "square"),          "⇠"},
            {("playstation", "lt"),              "↖"},
            {("playstation", "lb"),              "↜"},
            {("playstation", "ls"),              "⇱"},
            {("playstation", "rt"),              "↗"},
            {("playstation", "rb"),              "↝"},
            {("playstation", "rs"),              "⇲"},

            // Switch
            {("switch", "a"),                    "A"},
            {("switch", "b"),                    "B"},
            {("switch", "x"),                    "X"},
            {("switch", "y"),                    "Y"},
        };

    /// <summary>
    /// Intenta obtener el símbolo para un controlScheme y una key textual.
    /// Devuelve true/false y saca el símbolo por out.
    /// </summary>
    public static bool TryGetSymbol(string controlScheme, string keyDisplay, out string symbol)
    {
        symbol = null;

        if (string.IsNullOrEmpty(controlScheme) || string.IsNullOrEmpty(keyDisplay))
            return false;

        string scheme = controlScheme.ToLowerInvariant();
        string normalizedKey = NormalizeKey(keyDisplay);

        return symbols.TryGetValue((scheme, normalizedKey), out symbol);
    }

    /// <summary>
    /// Devuelve símbolo si existe, si no fallback (o la key original si fallback es null).
    /// </summary>
    public static string GetSymbolOrDefault(string controlScheme, string keyDisplay, string fallback = null)
    {
        if (TryGetSymbol(controlScheme, keyDisplay, out var symbol))
            return symbol;

        return fallback ?? keyDisplay;
    }

    /// <summary>
    /// Normaliza la clave para el diccionario:
    /// - lowercase
    /// - sin espacios
    /// </summary>
    private static string NormalizeKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        return new string(
            key
                .ToLowerInvariant()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
    }
}