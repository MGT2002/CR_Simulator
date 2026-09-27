using System;

class RouletteSimulator
{
    static readonly Random rnd = new();
    static readonly int[] betNumbers = [19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36];
    static decimal totalBalance = 2000M;

    static bool disableLogs = true;
    static ulong spinCounter = 0;
    static ulong stakeCounter = 0;

    static void Main()
    {
        while (totalBalance > 0 && totalBalance < 2000M * 2)
        {
            if (!disableLogs)
                Console.WriteLine("Press 'q' to quit or any other key to continue...");
            //if (Console.ReadLine() == "q")
            //{
            //    Console.WriteLine("Exiting...");
            //    break;
            //}

            RunGameWithConfig(null);
            Console.WriteLine("Game completed. total balance: {0} | Total Spin count: {1} | Total Stake count: {2}",
                totalBalance, spinCounter, stakeCounter);
        }
    }

    public static void RunGameWithConfig(Config? config)
    {
        if (config == null)
        {
            //default config
            config = new Config
            {
                InitialBalance = 20000m,
                BaseStake = 1m,
                LossMultiplier = 3m,
                NumberOfSpins = 1_000_000,
                SkipFirstLossCount = 15,
            };
            config.BreakGameAtBalance = config.InitialBalance * 2m;
        }
        if (!disableLogs)
            Console.WriteLine(config);

        decimal balance = config.InitialBalance;
        totalBalance -= balance;
        decimal baseStake = config.BaseStake;
        decimal mult = config.LossMultiplier;
        int spins = config.NumberOfSpins;
        int lossCounter = 0;

        decimal currentStake = baseStake;

        for (int i = 1; i <= spins; i++)
        {
            if (lossCounter < config.SkipFirstLossCount)
            {
                currentStake = 0;
                if (!disableLogs)
                    Console.WriteLine("Set stake to 0 | lossCounter: {0}", lossCounter);
            }
            else if (lossCounter == config.SkipFirstLossCount)
            {
                currentStake = baseStake;
                if (!disableLogs)
                    Console.WriteLine("Set stake to baseStake | lossCounter: {0}", lossCounter);
            }
            if (!disableLogs)
                Console.WriteLine($"Spin {i} | Stake: {currentStake} | Balance: {balance}");

            if (balance < currentStake)
            {
                if (!disableLogs)
                    Console.WriteLine($"Broke at spin {i}. Balance: {balance}");
                break;
            }

            if (balance >= config.BreakGameAtBalance)
            {
                if (!disableLogs)
                    Console.WriteLine($"Reached break balance at spin {i}. Balance: {balance}");
                break;
            }

            balance -= currentStake;
            if(currentStake > 0)
                stakeCounter++;
            int result = rnd.Next(0, 37);

            bool win = Array.IndexOf(betNumbers, result) >= 0;
            if (win)
            {
                balance += currentStake * Config.WinMultiplier;
                currentStake = baseStake; // reset
                lossCounter = 0; // reset loss counter
            }
            else
            {
                currentStake *= mult; // multiply on loss
                lossCounter++;
            }

            if (!disableLogs)
                Console.WriteLine($"Result number: {result} | {(win ? "WIN" : "LOSS")} | Balance: {balance}\n");

            spinCounter++;
        }

        if (!disableLogs)
            Console.WriteLine($"Final balance: {balance}");
        totalBalance += balance;
    }
}

public record Config()
{
    public const decimal WinMultiplier = 2m;

    public decimal InitialBalance { get; init; }
    public decimal BaseStake { get; init; }
    public decimal LossMultiplier { get; init; }
    public int NumberOfSpins { get; init; }
    public int SkipFirstLossCount { get; init; }
    public decimal BreakGameAtBalance { get; set; }
}