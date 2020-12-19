using System.Text; // Required for StringBuilder
using System.Web;
using Microsoft.AspNetCore.Html; // Required for IHtmlString

namespace BioMad_backend.Areas.Admin.Helpers
{
    /// <summary>
    ///  A class to help you output JavaScript code in a Razor view.
    /// </summary>
    public static class JsStringHelpers
    {
        /// <summary>
        /// Encodes a c# string to a string which can be used in JavaScript code escaping the relevant characters
        /// </summary>
        /// <param name="input">the string to escape</param>
        /// <returns>a string which can be used in JavaScript code</returns>
        public static IHtmlContent EncodeJavaScriptString(string input)
        {
            StringBuilder builder = new StringBuilder();
            // Open the double quotes
            builder.Append("\"");
            // Then add each character properly escaping them
            foreach (char c in input)
            {
                switch (c)
                {
                    //First check whether it's one of the defined escape sequences
                    case '\'': //single quote
                        builder.Append("\\\'");
                        break;
                    case '\"': //double quote
                        builder.Append("\\\"");
                        break;
                    case '\\': //backslash
                        builder.Append("\\\\");
                        break;
                    case '\0': //Unicode character 0
                        builder.Append("\\0");
                        break;
                    case '\a': //Alert (character 7)
                        builder.Append("\\a");
                        break;
                    case '\b': //Backspace (character 8)
                        builder.Append("\\b");
                        break;
                    case '\f': //Form feed (character 12)
                        builder.Append("\\f");
                        break;
                    case '\n': //New line (character 10)
                        builder.Append("\\n");
                        break;
                    case '\r': //Carriage return (character 13)
                        builder.Append("\\r");
                        break;
                    case '\t': //Horizontal tab (character 9)
                        builder.Append("\\t");
                        break;
                    case '\v': //Vertical quote (character 11)
                        builder.Append("\\v");
                        break;
                    default:
                        // If it's none of the defined escape sequences, convert the character to an int and check the code
                        int i = (int) c;
                        if (i >= 32 && i <= 127)
                        {
                            // if it's a displayable ASCII character, just write the character
                            builder.Append(c);
                        }
                        else
                        {
                            // otherwise write the Unicode escape sequence for the character with hex value
                            builder.AppendFormat("\\u{0:X04}", i);
                        }

                        break;
                }
            }

            // Close the double quotes
            builder.Append("\"");
            // You have to return an IHtmlString otherwise an HTML escape will be performed e.g. < will be replaced by &lt;
            return new HtmlString(builder.ToString());
        }
    }
}