using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace GamocologistBot.Modules.Translation
{
    /// <summary>
    /// Contains methods for generating bot responses as embedded messages.
    /// </summary>
    public static class EmbedGenerator
    {
        /// <summary>
        /// Generates an embed for when the language is not recognized.
        /// </summary>
        /// <param name="languageInput">The language provided and not recognized.</param>
        /// <returns>The generated embed.</returns>
        internal static Embed GenerateUnknownLanguageEmbed(string languageInput)
        {
            EmbedBuilder unknownLanguageEmbedBuilder = BuildBotEmbedBase();
            unknownLanguageEmbedBuilder.Color = Color.Red;

            EmbedFieldBuilder unknownLanguageField = new EmbedFieldBuilder
            {
                Name = $"Unknown language: {languageInput}",
                IsInline = true
            };
            string possibleLanguages = GeneratePossibleLanguages();
            string finalMessage = "Here are the possible languages.\n" +
                                  "Case is ignored and any spaces, new lines, tabulations, dashes, " +
                                  "underscores and brackets are also ignored:" +
                                  $"\n{possibleLanguages}";
            Console.WriteLine(finalMessage.Length);
            unknownLanguageField.Value = finalMessage;
            unknownLanguageEmbedBuilder.AddField(unknownLanguageField);

            Embed finalEmbed = unknownLanguageEmbedBuilder.Build();
            return finalEmbed;
        }

        /// <summary>
        /// Generates a basic embed builder to be used in other embed generators.
        /// </summary>
        /// <returns>A basic embed builder to be used in other embed generators.</returns>
        private static EmbedBuilder BuildBotEmbedBase()
        {
            EmbedBuilder botEmbedBase = new EmbedBuilder
            {
                Author = GenerateBotAuthor(),
                Color = Color.DarkBlue,
                Timestamp = DateTimeOffset.Now,
                Title = "Translation Results",
                //old link for translator image:
                //"https://cdns.c3dt.com/preview/9825728-com.deepl.translate.alllanguagetranslator.jpg"
                Url = "https://gamocologist.com/bots/gamotranslator"
            };
            return botEmbedBase;
        }

        /// <summary>
        /// Generates the author information for use in embeds.
        /// </summary>
        /// <returns>An embed author builder containing the author information for use in embeds.</returns>
        private static EmbedAuthorBuilder GenerateBotAuthor()
        {
            EmbedAuthorBuilder embedAuthorBuilder = new EmbedAuthorBuilder
            {
                Name = "GamoTranslate",
                IconUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fapitracker.io%2Fa%2Fdeepl&psig=AOvVaw028t84t0CBC87q4q-IuCMc&ust=1650642593447000&source=images&cd=vfe&ved=0CAwQjRxqFwoTCMDqzf3ApfcCFQAAAAAdAAAAABAO",
                Url = "https://gamocologist.com/bots/gamotranslator"
            };
            return embedAuthorBuilder;
        }

        /// <summary>
        /// Generate a text that contains all possible languages for translation.
        /// </summary>
        /// <returns>return a string that contains all possible languages for translation.</returns>
        private static string GeneratePossibleLanguages()
        {
            StringBuilder possibleLanguages = new StringBuilder();
            possibleLanguages.AppendLine();
            possibleLanguages.AppendLine(":arrow_right: Bulgarian");
            possibleLanguages.AppendLine(":arrow_right: Chinese");
            possibleLanguages.AppendLine(":arrow_right: Czech");
            possibleLanguages.AppendLine(":arrow_right: Danish");
            possibleLanguages.AppendLine(":arrow_right: Dutch");
            possibleLanguages.AppendLine(":arrow_right: English");
            possibleLanguages.AppendLine(":arrow_right: English (US) or English (United States)");
            possibleLanguages.AppendLine(":arrow_right: English (GB) or English (Great Britain)");
            possibleLanguages.AppendLine(":arrow_right: Estonian");
            possibleLanguages.AppendLine(":arrow_right: Finnish");
            possibleLanguages.AppendLine(":arrow_right: French");
            possibleLanguages.AppendLine(":arrow_right: German");
            possibleLanguages.AppendLine(":arrow_right: Greek");
            possibleLanguages.AppendLine(":arrow_right: Hungarian");
            possibleLanguages.AppendLine(":arrow_right: Italian");
            possibleLanguages.AppendLine(":arrow_right: Japanese");
            possibleLanguages.AppendLine(":arrow_right: Latvian");
            possibleLanguages.AppendLine(":arrow_right: Lithuanian");
            possibleLanguages.AppendLine(":arrow_right: Polish");
            possibleLanguages.AppendLine(":arrow_right: Portuguese");
            possibleLanguages.AppendLine(":arrow_right: Portuguese Brazil or Portuguese (BZ)");
            possibleLanguages.AppendLine(":arrow_right: Portuguese Portugal or Portuguese (PT)");
            possibleLanguages.AppendLine(":arrow_right: Romanian");
            possibleLanguages.AppendLine(":arrow_right: Russian");
            possibleLanguages.AppendLine(":arrow_right: Slovak");
            possibleLanguages.AppendLine(":arrow_right: Slovenian");
            possibleLanguages.AppendLine(":arrow_right: Spanish");
            possibleLanguages.AppendLine(":arrow_right: Swedish");
            string possibleLanguagesStr = possibleLanguages.ToString();
            return possibleLanguagesStr;
        }

        /// <summary>
        /// Generates a text containing the translation result 
        /// </summary>
        /// <param name="languageCodeSource">The language code of the language to be translated.</param>
        /// <param name="languageCodeDestination">The language code of the language the text is translated to.</param>
        /// <param name="isAutomatic">Indicates whether the <see cref="languageCodeSource"/> was detected automatically
        /// by the translation engine. True if it was. False if it was not.</param>
        /// <param name="translationResult">A tuple containing the result from the translation engine.
        /// The first element of the tuple is a <see cref="string"/> which contains the translated text.
        /// The second element of the tuple is a <see cref="string"/> which contains the language code of the detected
        /// language of the text that was given to be translated. </param>
        /// <returns>Return an <see cref="Embed"/> that contains the translation result.</returns>
        internal static Embed GenerateTranslationResultEmbed(string languageCodeSource, string languageCodeDestination,
            bool isAutomatic, (string translatedText, string detectedSourceLanguageCode) translationResult)
        {
            EmbedBuilder translationResultEmbedBuilder = BuildBotEmbedBase();
            translationResultEmbedBuilder.Color = Color.DarkGreen;
            EmbedFieldBuilder translatedTextField = new EmbedFieldBuilder
            {
                IsInline = false
            };

            string destinationLanguage = LanguageModelConversions.ConvertToLanguageName(languageCodeDestination);
            string languageField;
            if (isAutomatic)
            {
                string detectedLanguage =
                    LanguageModelConversions.ConvertToLanguageName(translationResult.detectedSourceLanguageCode);
                languageField = $"(Automatically Detected) {detectedLanguage}** --> **{destinationLanguage}";
            }
            else
            {
                string sourceLanguage = LanguageModelConversions.ConvertToLanguageName(languageCodeSource);
                languageField = $"{sourceLanguage}** --> **{destinationLanguage}";
            }

            List<string> segmentedText = Utils.DivideUpTextIntoFragmentsNicely(
                translationResult.translatedText);
            int numberOfSegments = segmentedText.Count;
            translatedTextField.Name = $"\nTranslated Text: {languageField}";
            if (numberOfSegments == 0)
                segmentedText.Add(" ");
            translatedTextField.Value = $"{segmentedText[0]}";
            translationResultEmbedBuilder.AddField(translatedTextField);

            for (int i = 1; i < numberOfSegments; i++)
            {
                EmbedFieldBuilder segmentFieldBuilder = new EmbedFieldBuilder
                {
                    Name = "\u200b",
                    IsInline = true,
                    Value = segmentedText[i]
                };
                translationResultEmbedBuilder.AddField(segmentFieldBuilder);
            }

            Embed translationResultEmbed = translationResultEmbedBuilder.Build();
            return translationResultEmbed;
        }

        /// <summary>
        /// Generates a message that indicates that the API's 500000 character/month translation limit has been reached.
        /// </summary>
        /// <returns>an <see cref="Embed"/> that indicates that the API's 500000 character/month translation limit has been reached.</returns>
        internal static Embed GenerateLimitReachedEmbed()
        {
            EmbedBuilder limitReachedEmbedBuilder = BuildBotEmbedBase();
            limitReachedEmbedBuilder.Color = Color.Orange;

            EmbedFieldBuilder limitReachedField = new EmbedFieldBuilder
            {
                Name = "Translation Limit Reached",
                IsInline = false,
                Value = "This bot uses an api which is limited to 500 000 characters per month.\n" +
                        "The limit has been reached and thus no more translations will be performed until the end of the month."
            };
            limitReachedEmbedBuilder.AddField(limitReachedField);

            Embed limitReachedEmbed = limitReachedEmbedBuilder.Build();
            return limitReachedEmbed;
        }
 
        /// <summary>
        /// Generates a response to a translation request where no text was provided for translation.
        /// </summary>
        /// <returns>An <see cref="Embed"/> containing a response to a translation request
        /// where no text was provided for translation.</returns>
        internal static Embed GenerateEmptyTextEmbed()
        {
            EmbedBuilder limitReachedEmbedBuilder = BuildBotEmbedBase();
            limitReachedEmbedBuilder.Color = Color.Red;

            EmbedFieldBuilder emptyTextFieldBuilder = new EmbedFieldBuilder
            {
                Name = "Nothing provided for translation",
                IsInline = false,
                Value = "Okay Dude, look at me in the eyes :eyes: straight in the eyes:\n" +
                        "how do you expect me translate the absence of text, meaning, hopes and dreams?\n" +
                        "I can't translate something which doesn't exist.\n\n"
            };

            limitReachedEmbedBuilder.AddField(emptyTextFieldBuilder);

            Embed generateEmptyTextEmbed = limitReachedEmbedBuilder.Build();
            return generateEmptyTextEmbed;
        }
        
        /// <summary>
        /// Generates a message informing that the API wasn't successfully
        /// </summary>
        /// <returns>An <see cref="Embed"/> containing a message informing that the authentication
        /// key was missing for the API.</returns>
        internal static Embed GenerateApiConnectionErrorEmbed()
        {
            EmbedBuilder apiErrorConnectionErrorEmbedBuilder = BuildBotEmbedBase();
            apiErrorConnectionErrorEmbedBuilder.Color = Color.DarkRed;

            EmbedFieldBuilder apiConnectionErrorFieldBuilder = new EmbedFieldBuilder
            {
                Name = "Authentication for translation API missing",
                IsInline = false,
                Value = "The Gamocolgists uses the DeepL API for translation.\n" +
                        "The link to the API has encountered issues and is not operational."
            };

            apiErrorConnectionErrorEmbedBuilder.AddField(apiConnectionErrorFieldBuilder);

            Embed apiConnectionErrorEmbed = apiErrorConnectionErrorEmbedBuilder.Build();
            return apiConnectionErrorEmbed;
        }
        
        /// <summary>
        /// Generates a message informing that the API wasn't successfully
        /// </summary>
        /// <returns>An <see cref="Embed"/> containing a message informing that the authentication
        /// key was missing for the API.</returns>
        internal static Embed GenerateReconnectionEmbed(bool wasSuccessful)
        {
            EmbedBuilder reconnectionEmbedBuilder = BuildBotEmbedBase();

            EmbedFieldBuilder reconnectionEmbedField = new EmbedFieldBuilder
            {
                Name = wasSuccessful 
                    ? "Reconnection to DeepL API was successful."
                    : "Authentication failure when reconnecting to DeepL API.",
                IsInline = false,
                Value = wasSuccessful 
                    ? "The reconnection to the DeepL API servers was successful."
                    : "The bot failed to reconnect to the DeepL API servers."
            };

            reconnectionEmbedBuilder.Color = wasSuccessful ? Color.DarkGreen : Color.Red;
            reconnectionEmbedBuilder.AddField(reconnectionEmbedField);

            Embed reconnectionEmbed = reconnectionEmbedBuilder.Build();
            return reconnectionEmbed;
        }
    }
}