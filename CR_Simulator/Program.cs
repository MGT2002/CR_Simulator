using System;

class RouletteSimulator
{
    static readonly Random rnd = new();
    static readonly int[] betNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24];
    static decimal totalBalance = 100m;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Press 'q' to quit or any other key to continue...");
            if (Console.ReadLine() == "q")
            { 
                Console.WriteLine("Exiting...");
                break;
            }

            RunGameWithConfig();
            Console.WriteLine("Game completed. total balance: {0}", totalBalance);
        }
    }

    private static void RunGameWithConfig()
    {
        var config = new Config
        {
            InitialBalance = 10m,
            BaseStake = 2m,
            LossMultiplier = 3m,
            NumberOfSpins = 1000
        };
        Console.WriteLine(config);

        decimal balance = config.InitialBalance;
        totalBalance -= balance;
        decimal baseStake = config.BaseStake;
        decimal mult = config.LossMultiplier;
        int spins = config.NumberOfSpins;

        decimal currentStake = baseStake;

        for (int i = 1; i <= spins; i++)
        {
            if (balance < currentStake)
            {
                Console.WriteLine($"Broke at spin {i}. Balance: {balance}");
                break;
            }

            balance -= currentStake;
            int result = rnd.Next(0, 37);

            bool win = Array.IndexOf(betNumbers, result) >= 0;
            if (win)
            {
                balance += currentStake * 3;
                currentStake = baseStake; // reset
            }
            else
            {
                currentStake *= mult; // multiply on loss
            }

            Console.WriteLine($"Spin {i}: {result} | {(win ? "WIN" : "LOSS")} | Stake: {currentStake} | Balance: {balance}");
        }

        Console.WriteLine($"Final balance: {balance}");
        totalBalance += balance;
    }
}

public record Config()
{
    public decimal InitialBalance { get; init; }
    public decimal BaseStake { get; init; }
    public decimal LossMultiplier { get; init; }
    public int NumberOfSpins { get; init; }
}