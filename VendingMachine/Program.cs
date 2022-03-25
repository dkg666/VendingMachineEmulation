using System.Collections.Generic;
using VendingMachine.Models;

namespace VendingMachine
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                //var initialCoins = new Dictionary<CoinDenomination, int>
                //{
                //    { CoinDenomination.Fifties, 20 },
                //    { CoinDenomination.Twenties, 50 },
                //    { CoinDenomination.Tens, 50 },
                //    { CoinDenomination.Fives, 50 },
                //    { CoinDenomination.Ones, 100 }
                //};
                
                var initialCoins = new Dictionary<CoinDenomination, int>
                {
                    { CoinDenomination.Fifties, 0 },
                    { CoinDenomination.Twenties, 0 },
                    { CoinDenomination.Tens, 1 },
                    { CoinDenomination.Fives, 0 },
                    { CoinDenomination.Ones, 0 }
                };

                VendingMachine.Init(initialCoins);

                VendingMachine.GetProductPrice();
                VendingMachine.ListAvailableDenominations();

                VendingMachine.PutCoins();

                VendingMachine.ListChangeCoins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}