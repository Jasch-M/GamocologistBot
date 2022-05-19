using System.Collections.Generic;

namespace Template.Modules.Mazzin;

public static class MazzinRemoteRequestHandler
{
    public static string Handle(string message)
    {
        string[] messageComponents = message.Split('-');
        int numberOfComponents = messageComponents.Length;
        string firstComponent = messageComponents[0];
        string response = firstComponent switch
        {
            "OBS" => HandleObsCommands(messageComponents, numberOfComponents),
            "AUTHORIZED" => HandleAuthorized(messageComponents, numberOfComponents),
            "GOAL LIST" => HandleGoalList(messageComponents, numberOfComponents),
            _ => "UNKNOWN COMMAND"
        };

        return response;
    }

    private static string HandleObsCommands(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 2) return "UNKNOWN COMMAND";
        
        string obsCommandComponent = messageComponents[1];
        ObsData obsData = ObsDataService.ObsData;
        string response = HandleObsCommand(messageComponents, numberOfComponents, obsCommandComponent, obsData);

        return response;
    }

    private static string HandleObsCommand(string[] messageComponents, int numberOfComponents, string obsCommandComponent,
        ObsData obsData)
    {
        string response;
        switch (obsCommandComponent)
        {
            case "RELOAD DATA":
                ObsDataService.ReloadObsData();
                response = "SUCCESS";
                break;
            case "RELOAD PRESETS":
                string presetsPath = ObsDataService.ObsPresetsLocation;
                obsData.ReloadPresets(presetsPath);
                response = "SUCCESS";
                break;
            case "SYNC":
                response = obsData.SyncWithCurrentObs();
                break;
            case "SAVE CURRENT":
                response = HandleObsSaveCommand(messageComponents, numberOfComponents);
                break;
            case "SAVE CURRENT OBS":
                response = HandleObsSaveCurrentCommand(messageComponents, numberOfComponents);
                break;
            case "SET DATA":
                response = HandleObsSetData(messageComponents, numberOfComponents);
                break;
            case "SYNC STARTUP ENABLE":
                ObsDataService.EnableSyncOnStartup();
                response = "SUCCESS";
                break;
            case "SYNC STARTUP DISABLE":
                ObsDataService.DisableSyncOnStartup();
                response = "SUCCESS";
                break;
            default:
                response = "UNKNOWN COMMAND";
                break;
        }

        return response;
    }

    private static string HandleObsSaveCommand(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3) return "MISSING PRESET NAME ARGUMENT";

        string presetNameArgument = messageComponents[2];
        ObsData obsData = ObsDataService.ObsData;
        obsData.SaveCurrentAsPreset(presetNameArgument);
        return "SUCCESS";
    }

    private static string HandleObsSaveCurrentCommand(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3) return "MISSING PRESET NAME ARGUMENT";

        string presetNameArgument = messageComponents[2];
        ObsData obsData = ObsDataService.ObsData;
        string wasSuccessful = obsData.SaveCurrentObsAsPreset(presetNameArgument);
        return wasSuccessful;
    }

    private static string HandleObsSetData(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3) return "MISSING PRESET DATA ARGUMENT";

        string obsPresetData = messageComponents[2];
        ObsPreset obsPreset = new ObsPreset("NOT IMPORTANT", obsPresetData);
        ObsData obsData = ObsDataService.ObsData;
        obsData.SetDataToPresetData(obsPreset);
        return "SUCCESS";
    }

    private static string HandleAuthorized(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 2) return "UNKNOWN COMMAND";
        
        string goalListCommandComponent = messageComponents[1];
        string response = goalListCommandComponent switch
        {
            "ADD" => HandleAuthorizedAdd(messageComponents, numberOfComponents),
            "REMOVE" => HandleAuthorizedRemove(messageComponents, numberOfComponents),
            "CLEAR" => HandleAuthorizedClear(messageComponents, numberOfComponents),
            _ => "UNKNOWN COMMAND"
        };

        return response;
    }

    private static string HandleAuthorizedClear(IReadOnlyList<string> messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3)
        {
            AuthenticationService.ClearAllAuthorized();
        }
        
        string authorizedClearCommandComponent = messageComponents[2];
        string response;
        switch (authorizedClearCommandComponent)
        {
            case "USERS":
                AuthenticationService.ClearAuthorizedUsers();
                response = "SUCCESS";
                break;
            case "ROLES":
                AuthenticationService.ClearAuthorizedRoles();
                response = "SUCCESS";
                break;
            case "CHANNELS":
                AuthenticationService.ClearAuthorizedChannels();
                response = "SUCCESS";
                break;
            case "ALL":
                AuthenticationService.ClearAllAuthorized();
                response = "SUCCESS";
                break;
            default:
                response = "UNKNOWN COMMAND";
                break;
        }

        return response;
    }

    private static string HandleAuthorizedRemove(string[] messageComponents, int numberOfComponents)
    {
        switch (numberOfComponents)
        {
            case <= 3:
                return "MISSING GROUP ARGUMENT";
            case <= 4:
                return "MISSING ID ARGUMENT";
        }

        string authorizedAddCommandComponent = messageComponents[2];
        
        string authorizedAddValue = messageComponents[3];
        bool wasSuccessful = ulong.TryParse(authorizedAddValue, out ulong uid);
        if (!wasSuccessful)
            return "NOT AN ID";
        
        AuthorizationResult authorizationResult = authorizedAddCommandComponent switch
        {
            "USERS" => AuthenticationService.RemoveAuthorizedUser(uid),
            "ROLES" => AuthenticationService.RemoveAuthorizedRole(uid),
            "CHANNELS" => AuthenticationService.RemoveAuthorizedChannel(uid),
            _ => AuthorizationResult.FAILED
        };

        if (authorizedAddCommandComponent != "USERS" && authorizedAddCommandComponent != "ROLES" &&
            authorizedAddCommandComponent != "CHANNELS")
        {
            return "UNKNOWN COMMAND";
        }

        string response = authorizationResult switch
        {
            AuthorizationResult.SUCCESS => "SUCCESS",
            AuthorizationResult.ALREADY_PRESENT => "ALREADY PRESENT",
            AuthorizationResult.FAILED => "FAILED",
            _ => "FAILED"
        };

        return response;
    }

    private static string HandleAuthorizedAdd(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 4) return "UNKNOWN COMMAND";

        string authorizedAddCommandComponent = messageComponents[2];
        
        string authorizedAddValue = messageComponents[3];
        bool wasSuccessful = ulong.TryParse(authorizedAddValue, out ulong uid);
        if (!wasSuccessful)
            return "NOT AN ID";
        
        AuthorizationResult authorizationResult = authorizedAddCommandComponent switch
        {
            "USERS" => AuthenticationService.AddAuthorizedUser(uid),
            "ROLES" => AuthenticationService.AddAuthorizedRole(uid),
            "CHANNELS" => AuthenticationService.AddAuthorizedChannel(uid),
            _ => AuthorizationResult.FAILED
        };

        if (authorizedAddCommandComponent != "USERS" && authorizedAddCommandComponent != "ROLES" &&
            authorizedAddCommandComponent != "CHANNELS")
        {
            return "UNKNOWN COMMAND";
        }

        string response = authorizationResult switch
        {
            AuthorizationResult.SUCCESS => "SUCCESS",
            AuthorizationResult.ALREADY_PRESENT => "ALREADY PRESENT",
            AuthorizationResult.FAILED => "FAILED",
            _ => "FAILED"
        };

        return response;
    }

    private static string HandleGoalList(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 2) return "UNKNOWN COMMAND";

        string goalListCommandComponent = messageComponents[1];
        switch (goalListCommandComponent)
        {
            case "ADD":
                return HandleGoalListAdd(messageComponents, numberOfComponents);
            case "REMOVE":
                return HandleGoalListRemove(messageComponents, numberOfComponents);
            case "CLEAR":
                GoalListService.GoalList.Clear();
                GoalListService.GoalList.Save("../../../Modules/Mazzin/GoalListData.txt");
                return "SUCCESS";
            default:
                return "UNKNOWN COMMAND";
        }
    }

    private static string HandleGoalListRemove(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3) return "UNKNOWN COMMAND";

        string goalListRemoveComponent = messageComponents[2];
        string[] removalComponents = goalListRemoveComponent.Split(' ');
        int numberOfRemovalComponents = removalComponents.Length;

        if (numberOfRemovalComponents > 4) return "UNKNOWN COMMAND";

        GoalList goalList = GoalListService.GoalList;
        bool wasSuccessful = goalList.Remove(goalListRemoveComponent);
        return wasSuccessful ? "SUCCESS" : "FAILED";
    }

    private static string HandleGoalListAdd(string[] messageComponents, int numberOfComponents)
    {
        if (numberOfComponents <= 3) return "UNKNOWN COMMAND";

        string goalListAddComponent = messageComponents[2];
        string[] removalComponents = goalListAddComponent.Split(' ');
        int numberOfRemovalComponents = removalComponents.Length;

        if (numberOfRemovalComponents > 4) return "UNKNOWN COMMAND";

        GoalList goalList = GoalListService.GoalList;
        bool wasSuccessful = goalList.Add(goalListAddComponent);
        return wasSuccessful ? "SUCCESS" : "FAILURE";
    }
}