using System;

namespace TranslatorBot.Modules.Translation
{
    
    public static class LanguageModelConversions
    {
        /// <summary>
        /// Converts a <see cref="string"/> representing a language into a <see cref="Language"/>.
        /// Case is ignored. '\n' (new line character), '\t' (tabulation character), ' ', '-' and '_'
        /// characters present in the string are not taken into account when identifying the language.
        /// Both language names and language codes are parsable.
        /// </summary>
        /// <param name="str">The <see cref="string"/> which represents the language to parse into a <see cref="Language"/>
        /// </param>
        /// <returns>The parsed <see cref="Language"/>.
        /// If the language string is "detect", "auto" or "autodetect" in its basic <see cref="string"/>,
        /// the function will return <see cref="Language.AUTO_DETECT"/> meaning this isn't a language specifically
        /// but one which has to be detected by the translation engine.
        /// If the language can't be identified, the function will return <see cref="Language.UNKNOWN"/></returns>
        public static Language ConvertToLanguage(string str)
        {
            string strPure = Utils.RemoveUndesiredSymbols(str);
            string strLower = strPure.ToLower();
            Language convertedLanguage = strLower switch
            {
                "detect" => Language.AUTO_DETECT,
                "auto" => Language.AUTO_DETECT,
                "autodetect" => Language.AUTO_DETECT,
                "bulgarian" => Language.BULGARIAN,
                "bg" => Language.BULGARIAN,
                "chinese" => Language.CHINESE,
                "zh" => Language.CHINESE,
                "czech" => Language.CZECH,
                "cs" => Language.CZECH,
                "danish" => Language.DANISH,
                "da" => Language.DANISH,
                "dutch" => Language.DUTCH,
                "nl" => Language.DUTCH,
                "english" => Language.ENGLISH_UNITED_STATES,
                "englishus" => Language.ENGLISH_UNITED_STATES,
                "englishunitedstates" => Language.ENGLISH_UNITED_STATES,
                "englishusa" => Language.ENGLISH_UNITED_STATES,
                "englishunitedstatesofamerica" => Language.ENGLISH_UNITED_STATES,
                "en-us" => Language.ENGLISH_UNITED_STATES,
                "englishgb" => Language.ENGLISH_UNITED_KINGDOM,
                "englishgreatbritain" => Language.ENGLISH_UNITED_KINGDOM,
                "en-gb" => Language.ENGLISH_UNITED_KINGDOM,
                "estonian" => Language.ESTONIAN,
                "et" => Language.ESTONIAN,
                "finish" => Language.FINNISH,
                "fi" => Language.FINNISH,
                "french" => Language.FRENCH,
                "fr" => Language.FRENCH,
                "german" => Language.GERMAN,
                "de" => Language.GERMAN,
                "greek" => Language.GREEK,
                "el" => Language.GREEK,
                "hungarian" => Language.HUNGARIAN,
                "hu" => Language.HUNGARIAN,
                "italian" => Language.ITALIAN,
                "it" => Language.ITALIAN,
                "japanese" => Language.JAPANESE,
                "ja" => Language.JAPANESE,
                "latvian" => Language.LATVIAN,
                "lv" => Language.LATVIAN,
                "lithunian" => Language.LITHUANIAN,
                "lt" => Language.LITHUANIAN,
                "polish" => Language.POLISH,
                "pl" => Language.POLISH,
                "portugueuse" => Language.PORTUGUEUSE_BRAZIL,
                "portugueusebz" => Language.PORTUGUEUSE_BRAZIL,
                "portugueusebrazil" => Language.PORTUGUEUSE_BRAZIL,
                "pt-br" => Language.PORTUGUEUSE_BRAZIL,
                "portugueusept" => Language.PORTUGUEUSE_EUROPEAN,
                "portugueuseportugal" => Language.PORTUGUEUSE_EUROPEAN,
                "portugueuseeurope" => Language.PORTUGUEUSE_EUROPEAN,
                "portgueuseeu" => Language.PORTUGUEUSE_EUROPEAN,
                "pt-pt" => Language.PORTUGUEUSE_EUROPEAN,
                "romanian" => Language.ROMANIAN,
                "ro" => Language.ROMANIAN,
                "russian" => Language.RUSSIAN,
                "ru" => Language.RUSSIAN,
                "slovak" => Language.SLOVAK,
                "sk" => Language.SLOVAK,
                "slovenian" => Language.SLOVENIAN,
                "sl" => Language.SLOVENIAN,
                "spanish" => Language.SPANISH,
                "es" => Language.SPANISH,
                "swedish" => Language.SWEDISH,
                "sv" => Language.SWEDISH,
                _ => Language.UNKNOWN
            };

            return convertedLanguage;
        }

        /// <summary>
        /// Converts a string representing a language as its string into its language code.
        /// Case is ignored. '\n' (new line character), '\t' (tabulation character), ' ', '-' and '_'
        /// characters present in the string are not taken into account when identifying the language.
        /// </summary>
        /// <param name="languageStr">A <see cref="string"/> representing a language.
        /// This must be a language name and not the language code.</param>
        /// <returns>The corresponding language code.
        /// If the language string is "detect", "auto" or "autodetect" in its basic <see cref="string"/>,
        /// the function will return "AUTOMATIC" meaning this isn't a language specifically
        /// but one which has to be detected by the translation engine.
        /// If the language can't be identified, the function will return "UNKNOWN".
        /// </returns>
        public static string ConvertToLanguageCode(string languageStr)
        {
            string strPure = Utils.RemoveUndesiredSymbols(languageStr);
            string strLower = strPure.ToLower();
            string convertedLanguageCode = strLower switch
            {
                "detect" => "AUTOMATIC",
                "auto" => "AUTOMATIC",
                "autodetect" => "AUTOMATIC",
                "bulgarian" => "BG",
                "chinese" => "ZH",
                "czech" => "CS",
                "danish" => "DA",
                "dutch" => "NL",
                "english" => "EN-US",
                "englishus" => "EN-US",
                "englishunitedstates" => "EN-US",
                "englishusa" => "EN-US",
                "englishunitedstatesofamerica" => "EN-US",
                "englishgb" => "EN-GB",
                "englishgreatbritain" => "EN-GB",
                "estonian" => "ET",
                "finish" => "FI",
                "french" => "FR",
                "german" => "DE",
                "greek" => "EL",
                "hungarian" => "HU",
                "italian" => "IT",
                "japanese" => "JA",
                "latvian" => "LV",
                "lithunian" => "LT",
                "polish" => "PL",
                "portugueuse" => "PT-BR",
                "portugueusebrazil" => "PR-BR",
                "portugueusebz" => "PR-BR",
                "portugueuseportugal" => "PT-PT",
                "portugueuseeurope" => "PT-PT",
                "portugueuseeu" => "PT-PT",
                "portugueusept" => "PT-PT",
                "romanian" => "RO",
                "russian" => "RU",
                "slovak" => "SK",
                "slovenian" => "SL",
                "spanish" => "ES",
                "swedish" => "SV",
                _ => "UNKNOWN"
            };

            return convertedLanguageCode;
        }

        /// <summary>
        /// Converts a <see cref="Language"/> into its corresponding language code.
        /// </summary>
        /// <param name="language">The <see cref="Language"/> to convert.</param>
        /// <returns>The corresponding language code.
        /// "AUTOMATIC" corresponds to <see cref="Language.AUTO_DETECT"/> and represents a
        /// language which has to be identified by the engine.
        /// "UNKNOWN" corresponds to <see cref="Language.UNKNOWN"/> an
        /// unrecognized language.</returns>
        public static string ConvertToLanguageCode(Language language)
        {
            string convertedLanguageCode = language switch
            {
                Language.BULGARIAN => "BG",
                Language.CHINESE => "ZH",
                Language.CZECH => "CS",
                Language.DANISH => "DA",
                Language.DUTCH => "NL",
                Language.ENGLISH_UNITED_STATES => "EN-US",
                Language.ENGLISH_UNITED_KINGDOM => "EN-GB",
                Language.ESTONIAN => "ET",
                Language.FINNISH => "FI",
                Language.FRENCH => "FR",
                Language.GERMAN => "DE",
                Language.GREEK => "EL",
                Language.HUNGARIAN => "HU",
                Language.ITALIAN => "IT",
                Language.JAPANESE => "JA",
                Language.LATVIAN => "LV",
                Language.LITHUANIAN => "LT",
                Language.POLISH => "PL",
                Language.PORTUGUEUSE_BRAZIL => "PT-BR",
                Language.PORTUGUEUSE_EUROPEAN => "PT-PT",
                Language.ROMANIAN => "RO",
                Language.RUSSIAN => "RU",
                Language.SLOVAK => "SK",
                Language.SLOVENIAN => "SL",
                Language.SPANISH => "ES",
                Language.SWEDISH => "SV",
                Language.UNKNOWN => "UNKNOWN",
                Language.AUTO_DETECT => "AUTO-DETECT",
                _ => "UNKNOWN"
            };

            return convertedLanguageCode;
        }

        /// <summary>
        /// Converts a language code to its corresponding language name. Case doesn't matter.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The language name. "UNKNOWN" if the name wasn't recognized.</returns>
        public static string ConvertToLanguageName(string languageCode)
        {
            string languageCodeUpper = languageCode.ToUpper();
            string convertedLanguageCode = languageCodeUpper switch
            {
                "BG" => "Bulgarian",
                "ZH" => "Chinese",
                "CS" => "Czech",
                "DA" => "Danish",
                "NL" => "Dutch",
                "EN" => "English",
                "EN-US" => "English (US)",
                "EN-GB" => "English (GB)",
                "ET" => "Estonian",
                "FI" => "Finish",
                "FR" => "French",
                "DE" => "German",
                "EL" => "Greek",
                "HU" => "Hungarian",
                "IT" => "Italian",
                "JA" => "Japanese",
                "LV" => "Latvian",
                "LT" => "Lithunian",
                "PL" => "Polish",
                "PT-BZ" => "Portugueuse (Brazil)",
                "PT-PT" => "Portugueuse (Portgual)",
                "RO" => "Romanian",
                "RU" => "Russian",
                "SK" => "Slovak",
                "SL" => "Slovenian",
                "ES" => "Spanish",
                "SV" => "Swedish",
                _ => "UNKNOWN"
            };

            return convertedLanguageCode;
        }

        /// <summary>
        /// Converts a <see cref="Language"/> to its language name.
        /// </summary>
        /// <param name="language">the <see cref="Language"/> to convert.</param>
        /// <returns>The equivalent language name for the <see cref="Language"/>.
        /// If the <see cref="Language"/> is <see cref="Language.UNKNOWN"/> or can't be matched because the enum access index
        /// is too great, the function will return "UNKNOWN" which represents
        /// a language which is not recognized. If the language <see cref="Language.AUTO_DETECT"/>,
        /// the funcion will return "AUTOMATIC" which indicates a language which has to be detected by
        /// the translation engine.</returns>
        public static string ConvertToLanguageName(Language language)
        {
            string convertedLanguageCode = language switch
            {
                Language.BULGARIAN => "Bulgarian",
                Language.CHINESE => "Chinese",
                Language.CZECH => "Czech",
                Language.DANISH => "Danish",
                Language.DUTCH => "Dutch",
                Language.ENGLISH_UNITED_STATES => "English (US)",
                Language.ENGLISH_UNITED_KINGDOM => "English (GB)",
                Language.ESTONIAN => "Estonian",
                Language.FINNISH => "Finish",
                Language.FRENCH => "French",
                Language.GERMAN => "German",
                Language.GREEK => "Greek",
                Language.HUNGARIAN => "Hungarian",
                Language.ITALIAN => "Italian",
                Language.JAPANESE => "Japanese",
                Language.LATVIAN => "Latvian",
                Language.LITHUANIAN => "Lithunian",
                Language.POLISH => "Polish",
                Language.PORTUGUEUSE_BRAZIL => "Portugueuse (Brazil)",
                Language.PORTUGUEUSE_EUROPEAN => "Portugueuse (Portgual)",
                Language.ROMANIAN => "Romanian",
                Language.RUSSIAN => "Russian",
                Language.SLOVAK => "Slovak",
                Language.SLOVENIAN => "Slovenian",
                Language.SPANISH => "Spanish",
                Language.SWEDISH => "Swedish",
                Language.AUTO_DETECT => "AUTOMATIC",
                _ => "UNKNOWN"
            };

            return convertedLanguageCode;
        }
    }
}