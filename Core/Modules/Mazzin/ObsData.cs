using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Template.Modules.Utils;
using Template.Services;
using Color = System.Drawing.Color;

namespace Template.Modules.Mazzin;

public class ObsData
{
    internal Color MysteryChanceForegroundColor;
    internal Color MysteryChanceBackgroundColor;
    internal Color MysteryChanceForegroundColorContour;
    internal Color MysteryChanceBackgroundColorContour;
    internal int MysteryChancePercentage;
    internal bool MysteryChanceOnline;
    internal int MysteryXPosition;
    internal int MysteryYPosition;
    private Dictionary<string, ObsPreset> _presets;
    internal int NumberOfPresets { get; private set; }

    internal ObsData(string dataLocation)
    {
        DataAssociation dataAssociation = DataAssociation.FromFile(dataLocation);
        string presetsPath = dataAssociation["Presets Path"];
        if (dataAssociation.ContainsPropertyName("Last Preset"))
        {
            string selectedPresetName = dataAssociation["Last Preset"];
            (NumberOfPresets, _presets) = LoadPresets(presetsPath);
            ObsPreset selectedPreset = _presets[selectedPresetName];
            SetDataToPresetData(selectedPreset);
        }
        else
        {
            SetDataToDefaults();
        }
    }

    private void SetDataToDefaults()
    {
        MysteryChanceOnline = true;
        MysteryChancePercentage = 25;
        MysteryChanceForegroundColor = Color.Lime;
        MysteryChanceForegroundColorContour = Color.Gold;
        MysteryChanceBackgroundColor = Color.Transparent;
        MysteryChanceBackgroundColorContour = Color.Transparent;
        MysteryXPosition = 0;
        MysteryYPosition = 0;
    }

    internal void SetDataToPresetData(ObsPreset selectedPreset)
    {
        MysteryChanceOnline = selectedPreset.MysteryChanceOnline;
        MysteryChancePercentage = selectedPreset.MysteryChancePercentage;
        MysteryChanceForegroundColor = selectedPreset.MysteryChanceForegroundColor;
        MysteryChanceForegroundColorContour = selectedPreset.MysteryChanceForegroundColorContour;
        MysteryChanceBackgroundColor = selectedPreset.MysteryChanceBackgroundColor;
        MysteryChanceBackgroundColorContour = selectedPreset.MysteryChanceBackgroundColorContour;
        MysteryXPosition = selectedPreset.MysteryChanceXPosition;
        MysteryYPosition = selectedPreset.MysteryChanceYPosition;
    }

    internal static (int, Dictionary<string, ObsPreset>) LoadPresets(string presetsPath)
    {
        int numberOfPresets = 0;
        Dictionary<string, ObsPreset> presets = new();

        if (Directory.Exists(presetsPath))
        {
            IEnumerable<string> files = Directory.EnumerateFiles(presetsPath);

            foreach (string filePath in files)
            {
                if (!IsPresetFile(filePath)) continue;
                string fileName = GetFileName(filePath);
                ObsPreset obsPreset = new (fileName, filePath);
                numberOfPresets += 1;
                
                presets.Add(fileName, obsPreset);
            }
        }

        return (numberOfPresets, presets);
    }

    private static bool IsPresetFile(string path)
    {
        DataAssociation dataAssociation = DataAssociation.FromFile(path);
        return dataAssociation.ContainsPropertyName("Foreground Color")
               && dataAssociation.ContainsPropertyName("Background Color")
               && dataAssociation.ContainsPropertyName("Foreground Contour Color")
               && dataAssociation.ContainsPropertyName("Background Contour Color")
               && dataAssociation.ContainsPropertyName("Value")
               && dataAssociation.ContainsPropertyName("Is Online")
               && dataAssociation.ContainsPropertyName("X Position")
               && dataAssociation.ContainsPropertyName("Y Position");
    }

    private static string GetFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }

    internal void ReloadPresets(string path)
    {
        (NumberOfPresets, _presets) = LoadPresets(path);
    }

    internal void SaveCurrentAsPreset(string presetName)
    {
        string dataLocation = ObsDataService.ObsPresetsLocation;
        SaveCurrentAsPreset(dataLocation, presetName);
    }

    internal void SaveCurrentAsPreset(string dataLocation, string presetName)
    {
        DataAssociation dataAssociation = DataAssociation.FromFile(dataLocation);
        string presetsPath = dataAssociation["Presets Path"];

        if (!Directory.Exists(presetsPath)) 
            Directory.CreateDirectory(presetsPath);
        
        string fullPath = $"{presetsPath}{Path.PathSeparator.ToString()}{presetName}";
        ObsPreset obsPreset = new(presetName, MysteryChanceForegroundColor, MysteryChanceBackgroundColor, 
            MysteryChanceForegroundColorContour, MysteryChanceBackgroundColorContour, MysteryChancePercentage, 
            MysteryChanceOnline, MysteryXPosition, MysteryYPosition);
        StreamWriter streamWriter = new(fullPath);
        string presetString = obsPreset.ToString();
        streamWriter.WriteLine(presetString);
        streamWriter.Close();
        if (_presets.ContainsKey(presetName))
            _presets[presetName] = obsPreset;
        else
            _presets.Add(presetName, obsPreset);
    }

    internal string SyncWithCurrentObs()
    {
        EmailService emailService = Identification.BotMail;
        Task<string> communicationResponseTask = emailService.Communicate("MAZZIN", "OBS-SYNC-GET");
        communicationResponseTask.Wait();
        string communicationResponse = communicationResponseTask.Result;
        
        if (communicationResponse == "NOT FOUND" || communicationResponse == "TIMEOUT" ||
            communicationResponse == "FAILED")
            return communicationResponse;

        string[] components = communicationResponse.Split('|');
        string nameComponent = components[0];
        string dataComponent = components.Length > 1 ? components[1] : "";
        ObsPreset obsPreset = new(nameComponent, dataComponent);
        
        SetDataToPresetData(obsPreset);
        return "SUCCESS";
    }

    internal string SaveCurrentObsAsPreset(string presetName)
    {
        string dataLocation = ObsDataService.ObsPresetsLocation;
        return SaveCurrentObsAsPreset(dataLocation, presetName);
    }

    internal string SaveCurrentObsAsPreset(string dataLocation, string presetName)
    {
        string wasSuccessful = SyncWithCurrentObs();
        SaveCurrentAsPreset(presetName);
        return wasSuccessful;
    }
}