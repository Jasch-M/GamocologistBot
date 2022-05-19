using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using Discord;

namespace Template.Modules.Mazzin;

public static class Utils
{
    internal static List<EmbedBuilder> DivideUpFields(List<EmbedFieldBuilder> fieldBuilders)
    {
        List<EmbedBuilder> embedBuilders = new();
        EmbedBuilder embedBuilder = new ();
        int numberOfFields = 0;
        foreach (EmbedFieldBuilder fieldBuilder in fieldBuilders)
        {
            if (numberOfFields == 25)
            {
                embedBuilders.Add(embedBuilder);
                embedBuilder = new EmbedBuilder();
                numberOfFields = 0;
            }

            embedBuilder.AddField(fieldBuilder);
            numberOfFields += 1;
        }

        return embedBuilders;
    }
    
    private static readonly Regex HtmlColorRegex = new Regex(
        @"^#((?'R'[0-9a-f]{2})(?'G'[0-9a-f]{2})(?'B'[0-9a-f]{2}))"
        + @"|((?'R'[0-9a-f])(?'G'[0-9a-f])(?'B'[0-9a-f]))$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static System.Drawing.Color? FromHtmlHexadecimal(string colorString)
    {
        if (colorString == null)
        {
            throw new ArgumentNullException("colorString");
        }

        Match match = HtmlColorRegex.Match(colorString);
        if (match.Success)
        {
            GroupCollection matchGroups = match.Groups;

            Group redMatchGroup = matchGroups["R"];
            Group greenMatchGroup = matchGroups["B"];
            Group blueMatchGroup = matchGroups["G"];

            string redMatchValue = redMatchGroup.Value;
            string greenMatchValue = greenMatchGroup.Value;
            string blueMatchValue = blueMatchGroup.Value;

            int redValue = ColorComponentToValue(redMatchValue);
            int greenValue = ColorComponentToValue(greenMatchValue);
            int blueValue = ColorComponentToValue(blueMatchValue);

            System.Drawing.Color analysedColor = System.Drawing.Color.FromArgb(redValue, greenValue, blueValue);

            return analysedColor;
        }

        return null;
    }

    private static int ColorComponentToValue(string component)
    {
        Debug.Assert(component != null);
        Debug.Assert(component.Length > 0);
        Debug.Assert(component.Length <= 2);

        if (component.Length == 1)
        {
            component += component;
        }

        int parsedValue = int.Parse(component,
            System.Globalization.NumberStyles.HexNumber);
        return parsedValue;
    }
}