# Gamocologist's Discord Bot

This is a bot that is used to help the Gamocologists with their Discord server.
For now, it includes the following functional modules:

### **Translation**

This module is used to translate text from one language to another.
It has two general purpose commands:

-> **§translate** - Translates text from an autodetected language to another specified language.
The syntax is:

```
§translate <language> <text>
```

no quotation marks are required around the language or text. 
The list of supported languages is:
* auto-detect - automatically detect the language of the text
* bulgarian - Bulgarian
* chinese - Chinese
* czech - Czech
* danish - Danish
* dutch - Dutch
* english - English
* finnish - Finnish
* french - French
* german - German
* greek - Greek
* hungarian - Hungarian
* italian - Italian
* japanese - Japanese
* latvian - Latvian
* lithuanian - Lithuanian
* portuguese - Portuguese
* romanian - Romanian
* russian - Russian
* slovak - Slovak
* slovenian - Slovenian
* spanish - Spanish
* swedish - Swedish


-> **§translatefrom** - Translate text from a specified language to another specified language.
The syntax is:

```
§translatefrom <from language> <to language> <text>
```

no quotation marks are required around the languages or text.
The list of supported languages is the same as for §translate.

The result is always returned in an embed.
If the translation text goes over the 1024 character limit, the text will be split into multiple fields within the embed.
This separation is done nicely. Sentences or words are not split.

The bot is impervious to API outages. 
If the API is down, the bot will not break, but will instead return an informative message.
A reconnection attempt can be made by using the §reconnecttodeepl command.
The limit for translation requests is set to 500 000 characters per month.
Should you exceed this limit, an embed will be returned informing you of this.