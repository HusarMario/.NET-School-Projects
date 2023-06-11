namespace CV1
{
    internal class T05
    {
        public static async Task Run(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Chyba - nedostatok argumentov. Program potrebuje 3 vstupne argumenty: spravu riadok farbu");
                return;
            }

            string message = args[0];
            int row = int.Parse(args[1]);
            ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[2]);

            Console.CursorVisible = false;

            int maxWidth = Console.WindowWidth;
            if (message.Length < maxWidth)
            {
                message += new string(' ', maxWidth - message.Length);
            }
            else
                message += " ";

            int i = 0;
            while (true)
            {
                Console.ForegroundColor = color;
                Console.CursorLeft = 0;
                Console.CursorTop = row;

                var rotatingMessage = message[i..];
                if (rotatingMessage.Length < maxWidth)
                    rotatingMessage += message[..i];

                Console.Write(rotatingMessage[..maxWidth]);

                i++;
                i %= message.Length;
                await Task.Delay(100);
            }

        }
    }
}
