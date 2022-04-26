using System.Collections.Generic;
using System.Text;

namespace Template.Modules.Translation
{
    /// <summary>
    ///     The Utils class provides many utility functions for other functions in the program.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     The function removes all unwanted symbols from a string and returns the result.
        /// </summary>
        /// <param name="input">The string to remove the unwanted symbols from.</param>
        /// <param name="undesiredSymbols">
        ///     The unwanted symbols.
        ///     By default, this parameter contains the following list of unwanted symbols:
        ///     '\n' (new line character), '\t' (tabulation character), ' ', '-' and '_'.
        /// </param>
        /// <returns>
        ///     A string based on the <see cref="input" /> string without the undesired
        ///     symbols specified in <see cref="undesiredSymbols" />
        /// </returns>
        internal static string RemoveUndesiredSymbols(string input, char[] undesiredSymbols = null)
        {
            undesiredSymbols ??= new [] { '\n', '\t', ' ', '-', '_', '(', ')' };
            HashSet<char> undesiredSymbolsSet = BuildUndesiredSymbolsHashSet(undesiredSymbols);
            StringBuilder purgedStringBuilder = new StringBuilder();

            foreach (char c in input)
                if (!undesiredSymbolsSet.Contains(c))
                    purgedStringBuilder.Append(c);

            string purgedString = purgedStringBuilder.ToString();
            return purgedString;
        }

        /// <summary>
        ///     Builds the undesired symbols hash set used by the <see cref="RemoveUndesiredSymbols" /> function.
        /// </summary>
        /// <param name="undesiredSymbols">The undesired symbols passed into the <see cref="RemoveUndesiredSymbols" /> function.</param>
        /// <returns>A hashset containing the undesired symbols passed into the <see cref="undesiredSymbols" /></returns>
        private static HashSet<char> BuildUndesiredSymbolsHashSet(char[] undesiredSymbols)
        {
            HashSet<char> undesiredSymbolsSet = new HashSet<char>();

            foreach (char undesiredSymbol in undesiredSymbols) undesiredSymbolsSet.Add(undesiredSymbol);

            return undesiredSymbolsSet;
        }

        /// <summary>
        /// Divide a piece of text into segments that aren't longer than a specified length.
        /// The function attempts to put the most sentences it can in the specified block sizes.
        /// Though it only puts whole sentences inside such blocks.
        /// It will cut off early if it can't fit the sentence in.
        /// Also after starting a new block, any whitespace in front of the first sentence is cleared.
        /// </summary>
        /// <param name="text">The text to divide up.</param>
        /// <param name="maxLength">The length of the blocks. By default, this value is set to 1024.</param>
        /// <param name="initialText">An optional initialText that will be inserted at the beginning
        /// and counted as part of the first block.</param>
        /// <returns>A <see cref="List{T}"/> containing strings which are the divided up blocks.</returns>
        internal static List<string> DivideUpTextIntoFragmentsNicely(string text, int maxLength = 1024,
            string initialText = "")
        {
            List<string> segments = new List<string>();
            int initialTextLength = initialText.Length;
            StringBuilder segmentBuilder = new StringBuilder(initialText);
            int textSize = text.Length;
            int lastPeriod = -1;
            int localIndex = initialTextLength;
            int index = initialTextLength;
            int target = textSize + initialTextLength;
            int textIndex = 0;
            while (index < target)
            {
                if (localIndex % maxLength == 0 && localIndex != 0)
                {
                    localIndex = SplitTextUpNicely(maxLength, segmentBuilder, segments, ref lastPeriod);
                }

                char charToPlace = text[textIndex];
                if (charToPlace == '.')
                    lastPeriod = localIndex;
                segmentBuilder.Append(charToPlace);
                index += 1;
                localIndex += 1;
                textIndex += 1;
            }
            
            string segment = segmentBuilder.ToString();
            segments.Add(segment);

            return segments;
        }

        /// <summary>
        /// A helper function for <see cref="DivideUpTextIntoFragmentsNicely"/>. The role of this sub function
        /// is to divide up the text nicely as described in the <see cref="DivideUpTextIntoFragmentsNicely"/> function.
        /// It also updates all necessary objects.
        /// </summary>
        /// <param name="maxLength">The maximum size of the blocks</param>
        /// <param name="segmentBuilder">The <see cref="StringBuilder"/> used to build the current block.</param>
        /// <param name="segments">The current <see cref="List{T}"/> of blocks.</param>
        /// <param name="lastPeriod">The index in the text of the last period.</param>
        /// <returns>The new value for the local index which is the current index inside the current block.</returns>
        private static int SplitTextUpNicely(int maxLength, StringBuilder segmentBuilder, List<string> segments, ref int lastPeriod)
        {
            string removedChars = "";
            int numberOfRemovedChars = 0;
            if (lastPeriod != -1)
            {
                StringBuilder removedCharsBuilder = new StringBuilder();
                int charsLeftToMove = maxLength - lastPeriod - 1;
                int target = lastPeriod + 1 + charsLeftToMove;
                for (int i = lastPeriod + 1; i < target; i++)
                {
                    char removedChar = segmentBuilder[i];
                    removedCharsBuilder.Append(removedChar);
                }

                removedChars = removedCharsBuilder.ToString();
                removedChars = removedChars.Trim();
                numberOfRemovedChars = removedChars.Length;
                segmentBuilder.Remove(lastPeriod + 1, charsLeftToMove);
            }

            string segment = segmentBuilder.ToString();
            segments.Add(segment);
            segmentBuilder.Clear();
            segmentBuilder.Append(removedChars);
            lastPeriod = -1;
            int localIndex = numberOfRemovedChars;
            return localIndex;
        }
    }
}