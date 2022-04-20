using Microsoft.VisualStudio.Web.CodeGeneration.Utils;

namespace MaxAPI.Utils
{
    public static class Base64Url
    {
        public static string Encode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        public static byte[] Decode(string input)
        {
            //input.RequireNotNull(
            //    paramName: "input");
            Requires.NotNull(input, "input");

            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            // Pad with trailing '='s
            switch (output.Length % 4)
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new ArgumentOutOfRangeException("input", "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
