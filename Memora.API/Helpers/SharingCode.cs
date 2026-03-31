namespace SimpleAUTH.Helpers
{
    public class SharingCode
    {
        Random rnd { get; } = new Random();

        public string GenerateCode()
        {
            char randomChar = ' ';
            int randomDigit = 0;
            string code = "";
            for (int i = 0; i < 3; i++)
            {
                randomChar = (char)rnd.Next(65, 90 + 1);       // 65 - 90 ascii uppercase chars
                randomDigit = rnd.Next(0, 9 + 1);               // numbers 0 - 10
                code += randomChar;
                code += randomDigit;
            }

            return code;
        }
    }
}
