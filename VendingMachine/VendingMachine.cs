using System;
using System.Collections.Generic;
using VendingMachine.Models;

namespace VendingMachine
{
    public static class VendingMachine
    {
        public static int BalanceInCents { get; set; }
        public static int ProductPriceInCents { get; set; }

        public static void Init(Dictionary<CoinDenomination, int> initialCoins)
        {
            Console.WriteLine("Welcome to Vending Machine emulation!");

            CoinsRegister.LoadCoinsRegister(initialCoins);
        }

        public static void PutCoins()
        {
            while (BalanceInCents < ProductPriceInCents)
            {
                PutCoin();
            }
        }

        public static void ListAvailableDenominations()
        {
            Console.WriteLine("Allowed coin denominations : ");

            foreach (byte val in Enum.GetValues(typeof(CoinDenomination)))
            {
                Console.WriteLine(val);
            }
        }

        public static void GetProductPrice()
        {
            Console.WriteLine("Enter desired product price [ex, 1.30] : ");
            var enteredPrice = Console.ReadLine();
            if (IsPriceValid(enteredPrice, out decimal price))
                ProductPriceInCents = (int)(price * 100);
            else
                Console.WriteLine("Incorrect price, should be positive decimal and the format should be for example 1.25");
        }

        public static void ListChangeCoins()
        {
            var changeCoins = CoinsRegister.CalculateChange(BalanceInCents, ProductPriceInCents);

            if (changeCoins == null)
                Console.WriteLine("Change : 0 ");
            else
            {
                var sumOfCoins = changeCoins.Sum(c => (byte)c.Key * c.Value);
                if (sumOfCoins == BalanceInCents)
                    Console.WriteLine("We don't have enough coins to give a change. Returning money.");
                else
                    Console.WriteLine("Product and Change : ");

                foreach (var (key, value) in changeCoins)
                {
                    Console.WriteLine($"{key} coin(s) : {value}");
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void PutCoin()
        {
            Console.Write("Enter the coin denomination to put :");
            var enteredDenomination = Console.ReadLine();

            if (!IsDenominationValid(enteredDenomination, out var denomination))
            {
                Console.Write("Invalid coin denomination!");
                return;
            }

            CoinsRegister.InsertCoinToRegister((CoinDenomination)denomination);
            BalanceInCents += (byte)denomination;
            Console.WriteLine($"Balance (cents) : {BalanceInCents}");
        }

        private static bool IsPriceValid(string enteredPrice, out decimal price)
        {
            return decimal.TryParse(enteredPrice.Replace(',', '.'), out price)
                   && price > 0;
        }

        private static bool IsDenominationValid(string enteredDenomination, out object denomination)
        {
            return Enum.TryParse(typeof(CoinDenomination), enteredDenomination, true, out denomination)
                   && Enum.IsDefined(typeof(CoinDenomination), denomination);
        }

        
    }
}
