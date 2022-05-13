using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Template.Modules.Translation
{
    [Name("Translation")]
    [Summary("Automatic, quick and simple translation")]
    public class TranslationModule : ModuleBase<SocketCommandContext>
    {
        [Command("translate", RunMode = RunMode.Async)]
        [Summary("Translates text from the automatically detected language to another language")]
        public async Task Translate(string targetLanguage, params string[] text)
        {
            await TranslateFrom("autodetect", targetLanguage, text);
        }

        [Command("translatefrom", RunMode = RunMode.Async)]
        [Summary("Translates text from a specified language to another language")]
        public async Task TranslateFrom(string inputLanguage, string targetLanguage, params string[] text)
        {
            if (!TranslationService.IsTranslatorOperational)
            {
                Embed failedApiEmbed = EmbedGenerator.GenerateApiConnectionErrorEmbed();
                await ReplyAsync(embed: failedApiEmbed);
                return;
            }
            
            if (text.Length == 0)
            {
                Embed emptyTextEmbed = EmbedGenerator.GenerateEmptyTextEmbed();
                await ReplyAsync(embed: emptyTextEmbed);
                return;
            }
            
            if (await TranslationService.HasReachedCap())
            {
                Embed reachedLimitEmbed = EmbedGenerator.GenerateLimitReachedEmbed();
                await ReplyAsync(embed: reachedLimitEmbed);
                return;
            }

            string languageCodeSource = LanguageModelConversions.ConvertToLanguageCode(inputLanguage);
            string languageCodeDestination = LanguageModelConversions.ConvertToLanguageCode(targetLanguage);
            
            if (languageCodeSource.Length == 5)
                languageCodeSource = languageCodeSource.Remove(2, 3);
            
            if (languageCodeDestination == "UNKNOWN")
            {
                Embed unknownLanguageEmbed = EmbedGenerator.GenerateUnknownLanguageEmbed(targetLanguage);
                await ReplyAsync(embed: unknownLanguageEmbed);
            }
            else
            {
                string joinedText = string.Join(' ', text);
                (string translatedText, string detectedSourceLanguageCode) translationResult =
                    await TranslationService.Translate(joinedText, languageCodeSource, languageCodeDestination);
                Embed translatedTextEmbedBuilder =
                    EmbedGenerator.GenerateTranslationResultEmbed(languageCodeSource, languageCodeDestination,
                        languageCodeSource == "AUTOMATIC",
                        translationResult);
                await ReplyAsync(embed: translatedTextEmbedBuilder);
            }
        }

        [Command("reconnecttodeepl", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Attempts to reconnect to the DeepL Translation API's servers")]
        public async Task ReconnectToTranslationApi()
        {
            bool isReconnectionSuccess = TranslationService.ReconnectToDeepL();
            Embed reconnectionAttemptResultEmbed = EmbedGenerator.GenerateReconnectionEmbed(isReconnectionSuccess);
            await ReplyAsync(embed: reconnectionAttemptResultEmbed);
        }
    }
}