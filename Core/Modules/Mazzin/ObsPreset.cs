using Discord;
using Template.Modules.Utils;
using Color = System.Drawing.Color;

namespace Template.Modules.Mazzin;

public struct ObsPreset
{
    internal string Name;
    internal Color MysteryChanceForegroundColor;
    internal Color MysteryChanceBackgroundColor;
    internal Color MysteryChanceForegroundColorContour;
    internal Color MysteryChanceBackgroundColorContour;
    internal int MysteryChancePercentage;
    internal bool MysteryChanceOnline;
    internal int MysteryChanceXPosition;
    internal int MysteryChanceYPosition;

    public ObsPreset(string name, string obsPresetPathData)
    {
        DataAssociation dataAssociation = DataAssociation.FromData(obsPresetPathData);
        Name = name;
        
        string mysteryChanceForegroundColorStr = dataAssociation["Foreground Color"];
        string mysteryChanceForegroundContourColorStr = dataAssociation["Foreground Contour Color"];
        string mysteryChanceBackgroundColorStr = dataAssociation["Background Color"];
        string mysteryChanceBackgroundContourColorStr = dataAssociation["Background Contour Color"];
        string mysteryChancePercentageStr = dataAssociation["Value"];
        string mysteryChanceOnlineStr = dataAssociation["Online"];
        string mysteryChanceXPositionStr = dataAssociation["X Position"];
        string mysteryChanceYPositionStr = dataAssociation["Y Position"];

        MysteryChanceOnline = bool.Parse(mysteryChanceOnlineStr);
        MysteryChancePercentage = int.Parse(mysteryChancePercentageStr);
        MysteryChanceForegroundColor = ParseColor(mysteryChanceForegroundColorStr);
        MysteryChanceForegroundColorContour = ParseColor(mysteryChanceForegroundContourColorStr);
        MysteryChanceBackgroundColor = ParseColor(mysteryChanceBackgroundColorStr);
        MysteryChanceBackgroundColorContour = ParseColor(mysteryChanceBackgroundContourColorStr);
        MysteryChanceXPosition = int.Parse(mysteryChanceXPositionStr);
        MysteryChanceYPosition = int.Parse(mysteryChanceYPositionStr);
    }

    internal ObsPreset(string name, Color mysteryChanceForegroundColor, Color mysteryChanceBackgroundColor, 
        Color mysteryChanceForegroundColorContour, Color mysteryChanceBackgroundColorContour, 
        int mysteryChancePercentage, bool mysteryChanceOnline, int mysteryChanceXPosition, int mysteryChanceYPosition)
    {
        Name = name;
        MysteryChanceForegroundColor = mysteryChanceForegroundColor;
        MysteryChanceBackgroundColor = mysteryChanceBackgroundColor;
        MysteryChanceForegroundColorContour = mysteryChanceForegroundColorContour;
        MysteryChanceBackgroundColorContour = mysteryChanceBackgroundColorContour;
        MysteryChancePercentage = mysteryChancePercentage;
        MysteryChanceOnline = mysteryChanceOnline;
        MysteryChanceXPosition = mysteryChanceXPosition;
        MysteryChanceYPosition = mysteryChanceYPosition;
    }

    private static Color ParseColor(string inputString)
    {
        string[] components = inputString.Split(' ');
        int numberOfComponents = components.Length;

        string redComponent = components[0];
        string greenComponent = numberOfComponents > 1 ? components[1] : "0";
        string blueComponent = numberOfComponents > 2 ? components[2] : "0";
        string alphaComponent = numberOfComponents > 3 ? components[3] : "0";

        int redValue = 0;
        int greenValue = 0;
        int blueValue = 0;
        int alphaValue = 0;

        int.TryParse(redComponent, out redValue);
        int.TryParse(greenComponent, out greenValue);
        int.TryParse(blueComponent, out blueValue);
        int.TryParse(alphaComponent, out alphaValue);

        return Color.FromArgb(alphaValue, redValue, greenValue, blueValue);
    }

    public override string ToString()
    {
        return GenerateCurrentPresetText();
    }
    
    private string GenerateCurrentPresetText()
    {
        string foregroundColorStr = GenerateColorRepresentationForPreset(MysteryChanceForegroundColor);
        string foregroundContourColorStr = GenerateColorRepresentationForPreset(MysteryChanceForegroundColorContour);
        string backgroundColorStr = GenerateColorRepresentationForPreset(MysteryChanceBackgroundColor);
        string backgroundContourColorStr = GenerateColorRepresentationForPreset(MysteryChanceBackgroundColorContour);

        string foregroundColorLine = $"\"Foreground Color\": \"{foregroundColorStr}\"";
        string foregroundContourColorLine = $"\"Foreground Contour Color\": \"{foregroundContourColorStr}\"";
        string backgroundColorLine = $"\"Background Color\": \"{backgroundColorStr}\"";
        string backgroundContourColorLine = $"\"Background Contour Color\": \"{backgroundContourColorStr}\"";
        string valueLine = $"\"Value\": \"{MysteryChancePercentage}\"";
        string onlineLine = $"\"Online\": \"{MysteryChanceOnline}\"";
        string xPositionLine = $"\"X Position\": \"{MysteryChanceXPosition}\"";
        string yPositionLine = $"\"Y Position\": \"{MysteryChanceYPosition}\"";

        string finalPresetText = $"{foregroundColorLine}" +
                                 $"\n{foregroundContourColorLine}" +
                                 $"\n{backgroundColorLine}" +
                                 $"\n{backgroundContourColorLine}" +
                                 $"\n{valueLine}" +
                                 $"\n{onlineLine}" +
                                 $"\n{xPositionLine}" +
                                 $"\n{yPositionLine}";

        return finalPresetText;
    }

    private static string GenerateColorRepresentationForPreset(Color color)
    {
        int redValue = color.R;
        int greenValue = color.G;
        int blueValue = color.B;
        int alphaValue = color.A;

        string red = redValue.ToString();
        string green = greenValue.ToString();
        string blue = blueValue.ToString();
        string alpha = alphaValue.ToString();

        string colorRepresentation = $"{red} {green} {blue} {alpha}";
        return colorRepresentation;
    }
}