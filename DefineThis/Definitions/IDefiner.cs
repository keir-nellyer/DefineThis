using System.Collections.Generic;

namespace DefineThis.Definitions
{
    /// <summary>
    /// Gets definitions for a word or phrase.
    /// </summary>
    public interface IDefiner
    {
        /// <summary>
        /// Gets the definitions of a phrase.
        /// </summary>
        /// <param name="phrase">the phrase</param>
        /// <returns>the definitions</returns>
        IEnumerable<string> GetDefinitions(string phrase);
    }
}