using System;
using System.Threading.Tasks;
using DeepL;
using DeepL.Model;
using Template.Modules.Utils;

namespace Template.Modules.Translation
{
    /// <summary>
    /// This static class provides interactions with the translation engine.
    /// </summary>
    public static class TranslationService
    {
        /// <summary>
        /// The <see cref="DeepL.Translator"/> which allows interactions and communications with the engine.
        /// </summary>
        private static Translator _translator = TranslatorSetUp();

        /// <summary>
        /// Checks whether the translation API is running correctly.
        /// True if it is working as intended.
        /// False if the API is down.
        /// </summary>
        internal static bool IsTranslatorOperational { get; private set; }
            
        /// <summary>
        /// Check whether the maximum number of free character translations have been used up this month.
        /// </summary>
        /// <returns>True if maximum number of free character translations have been used up.
        /// False if the maximum number of free character translations have not been used up.</returns>
        internal static async Task<bool> HasReachedCap()
        {
            if (!IsTranslatorOperational) return false;
            Usage usage = await _translator.GetUsageAsync();
            return usage.AnyLimitReached;
        }

        /// <summary>
        /// Translates a text from a language into another language using the DeepL engine.
        /// </summary>
        /// <param name="text">The text to be translated.</param>
        /// <param name="inputLanguageCode">The language code of the text to be translated.</param>
        /// <param name="outputLanguageCode">The language code of the desired translated text.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a tuple with two strings.
        /// The first string contains the translated text.
        /// The second string contains the language code, detected by the engine,
        /// of the text to be translated.</returns>
        internal static async Task<(string, string)> Translate(string text, string inputLanguageCode,
            string outputLanguageCode)
        {
            if (!IsTranslatorOperational)
                return ("", "");

            TextResult translatorResponse = inputLanguageCode == "AUTOMATIC"
                ? await _translator.TranslateTextAsync(text, null, outputLanguageCode)
                : await _translator.TranslateTextAsync(text, inputLanguageCode, outputLanguageCode);
            string translatedText = translatorResponse.Text;
            string detectedLanguageCode = translatorResponse.DetectedSourceLanguageCode;
            (string text, string detectedLanguageCode) translationResult = (translatedText, detectedLanguageCode);
            return translationResult;
        }

        /// <summary>
        /// Sets up the <see cref="_translator"/> object.
        /// Tries to read the authentication key from "translator.txt".
        /// Tries to connect to the engine. Updates <see cref="IsTranslatorOperational"/> accordingly.
        /// </summary>
        /// <returns>The set up translator. If the value is null, then the operation was unsuccessful</returns>
        private static Translator TranslatorSetUp()
        {
            DataAssociation dataAssociation = DataAssociation.FromFile("../../../Modules/Translation/translator_data.txt");
            bool wasObtained = dataAssociation.TryGetValue("DeepL authKey", out string authenticationKey);
            if (wasObtained)
            {
                try
                {
                    Translator translator = new Translator(authenticationKey);
                    Task<TextResult> task = translator.TranslateTextAsync("Hello", "EN", "FR");
                    task.Wait();
                    IsTranslatorOperational = true;
                    return translator;
                }
                catch (Exception)
                {
                    IsTranslatorOperational = false;
                    return null;
                }
            }

            IsTranslatorOperational = false;
            return null;
        }

        /// <summary>
        /// Tries to reconnect the translator to the translation engine via the translation API.
        /// </summary>
        /// <returns>True if the reconnection was successful. False if the reconnection was a failure.</returns>
        internal static bool ReconnectToDeepL()
        {
            _translator = TranslatorSetUp();
            return IsTranslatorOperational;
        }
    }
}
