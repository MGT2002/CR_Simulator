using System;

class RouletteSimulator
{
    static Random rng = new();
    private static readonly int[] betNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24];

    static void Main()
    {
        Console.Write("Initial balance: ");
        decimal balance = decimal.Parse(Console.ReadLine()!);
        Console.Write("Stake per spin: ");
        decimal stake = decimal.Parse(Console.ReadLine()!);
        Console.Write("Number of spins: ");
        int spins = int.Parse(Console.ReadLine()!);

        for (int i = 1; i <= spins; i++)
        {
            if (balance < stake)
            {
                Console.WriteLine($"Broke at spin {i}. Balance: {balance}");
                break;
            }

            balance -= stake;
            int result = rng.Next(0, 37); // 0-36

            bool win = Array.IndexOf(betNumbers, result) >= 0;
            if (win)
                balance += stake * 3; // 3x stake

            Console.WriteLine($"Spin {i}: {result} | {(win ? "WIN" : "LOSS")} | Balance: {balance}");
        }

        Console.WriteLine($"Final balance: {balance}");
    }
}