using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static string targetHash;
    static int passwordLength = 5;
    static int numThreads = 1;

    static void Main(string[] args)
    {
        Console.WriteLine("SHA-256:");
        targetHash = Console.ReadLine();

        Console.WriteLine("1 - Single Thread, 2 - Multi-Thread");
        int mode = int.Parse(Console.ReadLine());

        if (mode == 2)
        {
            Console.WriteLine("Enter the number of threads:");
            numThreads = int.Parse(Console.ReadLine());
        }

        Console.WriteLine("Starting");
        DateTime startTime = DateTime.Now;

        Parallel.For(0, 26 * 26 * 26 * 26 * 26, new ParallelOptions { MaxDegreeOfParallelism = numThreads },
            (i, state) =>
            {
                string password = GeneratePassword(i);
                string hash = CalculateSHA256(password);

                if (hash == targetHash)
                {
                    Console.WriteLine($"Password found: {password}, Hash: {hash}");
                    state.Break();
                }
            });

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;
        Console.WriteLine($"Time elapsed: {duration.TotalSeconds} seconds");
    }

    static string GeneratePassword(int number)
    {
        char[] chars = new char[5];
        for (int i = 0; i < 5; i++)
        {
            chars[i] = (char)((number % 26) + 'a');
            number /= 26;
        }
        Array.Reverse(chars);
        return new string(chars);
    }

    static string CalculateSHA256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
