using System;

class RouletteSimulator
{
    static Random rng = new Random();
    static int[] betNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

    static void Main()
    {
        Console.Write("Initial balance: ");
        decimal balance = decimal.Parse(Console.ReadLine());
        Console.Write("Base stake: ");
        decimal baseStake = decimal.Parse(Console.ReadLine());
        Console.Write("Loss multiplier: ");
        decimal mult = decimal.Parse(Console.ReadLine());
        Console.Write("Number of spins: ");
        int spins = int.Parse(Console.ReadLine());

        decimal currentStake = baseStake;

        for (int i = 1; i <= spins; i++)
        {
            if (balance < currentStake)
            {
                Console.WriteLine($"Broke at spin {i}. Balance: {balance}");
                break;
            }

            balance -= currentStake;
            int result = rng.Next(0, 37);

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
    }
}