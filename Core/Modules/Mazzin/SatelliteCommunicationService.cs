using System.Drawing;
using System.Threading.Tasks;
using Template.Services;
using TranslatorBot;

namespace Template.Modules.Mazzin;

public static class SatelliteCommunicationService
{
    internal static EmailService BotMail => Startup.Email;
    
    internal static async Task<RequestResult> SetColorForeground(Color color)
    {
        string colorFormatting = FormatColorString(color);
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-FG-{colorFormatting}");
        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetColorForeground(int r, int g, int b, int a = 255)
    {
        Color color = Color.FromArgb(r, g, b, a);
        return await SetColorForeground(color);
    }

    internal static async Task<RequestResult> SetColorBackground(Color color)
    {
        string colorFormatting = FormatColorString(color);
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-BG-{colorFormatting}");

        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetColorBackground(int r, int g, int b, int a = 255)
    {
        Color color = Color.FromArgb(r, g, b, a);
        return await SetColorBackground(color);
    }
    
    internal static async Task<RequestResult> SetColorForegroundContour(Color color)
    {
        string colorFormatting = FormatColorString(color);
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-FGC-{colorFormatting}");

        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetColorForegroundContour(int r, int g, int b, int a = 255)
    {
        Color color = Color.FromArgb(r, g, b, a);
        return await SetColorForegroundContour(color);
    }

    internal static async Task<RequestResult> SetColorBackgroundContour(Color color)
    {
        string colorFormatting = FormatColorString(color);
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-BGC-{colorFormatting}");

        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetColorBackgroundContour(int r, int g, int b, int a = 255)
    {
        Color color = Color.FromArgb(r, g, b, a);
        return await SetColorBackgroundContour(color);
    }

    internal static async Task<RequestResult> SetValue(int newValue)
    {
        string valueStr = newValue.ToString();
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-VAL-{valueStr}");

        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetOffline()
    {
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-STS-OFF");
        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }

    internal static async Task<RequestResult> SetOnline()
    {
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-CLR-STS-ON");
        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }
    
    internal static async Task<RequestResult> SetXPosition(uint value)
    {
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-POS-X-{value}");
        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }
    
    internal static async Task<RequestResult> SetYPosition(uint value)
    {
        string responseRaw = await BotMail.Communicate("MAZZIN", $"OBS-PC-POS-Y-{value}");
        RequestResult response = AnalyseRequestResult(responseRaw);
        return response;
    }
    
    private static string FormatColorString(Color color)
    {
        return $"{color.R}-{color.G}-{color.B}-{color.A}";
    }
    
    private static RequestResult AnalyseRequestResult(string response)
    {
        return response switch
        {
            "TIMEOUT" => RequestResult.TIMED_OUT,
            "SUCCESS" => RequestResult.SUCCESSFUL,
            _ => RequestResult.FAILED
        };
    }
}