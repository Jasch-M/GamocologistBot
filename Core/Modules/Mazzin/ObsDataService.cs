using System.IO;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin;

public static class ObsDataService
{
    internal static ObsData ObsData = LoadObsData();

    internal static bool SyncOnStartup = LoadSyncOnStartupData();
    
    internal static string ObsDataLocation => "../../../Modules/Mazzin/ObsData.txt";

    internal static string ObsPresetsLocation => "../../../Modules/Mazzin/Presets";

    private static string ObsSyncStartupLocation => "../../../Modules/Mazzin/ObsSyncStartupSetting.txt";

    private static ObsData LoadObsData()
    {
        if (!File.Exists(ObsDataLocation))
        {
            if (!Directory.Exists(ObsPresetsLocation))
                Directory.CreateDirectory(ObsPresetsLocation);

            using StreamWriter writer = new(ObsDataLocation);
            writer.WriteLine($"\"Presets Path\":\"{ObsPresetsLocation}\"");
            writer.Close();
        }
        
        ObsData obsData = new(ObsDataLocation);
        return obsData;
    }

    internal static void ReloadObsData()
    {
        ObsData = LoadObsData();
    }

    private static bool LoadSyncOnStartupData()
    {
        DataAssociation dataAssociation = DataAssociation.FromFile(ObsSyncStartupLocation);

        if (!dataAssociation.ContainsPropertyName("Sync on Startup"))
            return false;

        string syncOnStartupStr = dataAssociation["Sync on Startup"];
        bool wasConverted = bool.TryParse(syncOnStartupStr, out bool syncOnStartup);
        
        if (!wasConverted)
        {
            dataAssociation.SetValue("Sync on Startup", "false");
            dataAssociation.Save();
            return false;
        }
        
        return syncOnStartup;
    }

    internal static void DisableSyncOnStartup()
    {
        SyncOnStartup = false;
    }

    internal static void EnableSyncOnStartup()
    {
        SyncOnStartup = true;
    }
}