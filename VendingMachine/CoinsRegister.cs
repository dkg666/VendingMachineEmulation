using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Models;

namespace VendingMachine
{
    public static class CoinsRegister
    {
        private static Dictionary<CoinDenomination, int> CoinsRegisterDictionary { get; set; }

        public static void InsertCoinToRegister(CoinDenomination denomination)
        {
            if (!CoinsRegisterDictionary.TryGetValue(denomination, out var numberOfCoins)) 
                return;

            CoinsRegisterDictionary[denomination] = ++numberOfCoins;
        }

        public static void TakeCoinsFromRegister(CoinDenomination denomination, int numberOfCoinsToTake)
        {
            if (!CoinsRegisterDictionary.TryGetValue(denomination, out var numberOfCoins)) 
                return;

            CoinsRegisterDictionary[denomination] = numberOfCoins - numberOfCoinsToTake;
        }

        public static Dictionary<CoinDenomination, int> CalculateChange(int currentBalanceInCents, int productPriceInCents)
        {
            var changeInCents = currentBalanceInCents - productPriceInCents;
            if (changeInCents == 0) // Change calculation is not required
                return null;

            var availableSumInCents = CoinsRegisterDictionary.Where(c => (byte)c.Key <= changeInCents)
                .Sum(cd => (byte)cd.Key * cd.Value); //we can't give change in coin denominations bigger than change amount

            if (changeInCents > availableSumInCents) // Change amount is more than we can give, return money for product
            {
                var allDenominations = ((byte[])Enum.GetValues(typeof(CoinDenomination))).OrderByDescending(cd => cd);
                var returnedSetOfCoins = GetSetOfCoins(allDenominations, currentBalanceInCents);
                return returnedSetOfCoins;
            }
            
            var smallerDenominations = ((byte[])Enum.GetValues(typeof(CoinDenomination)))
                .Where(cd => cd <= changeInCents).OrderByDescending(cd => cd); // we want to give change in biggest possible denominations
            var setOfCoinsForChange = GetSetOfCoins(smallerDenominations, changeInCents);

            return setOfCoinsForChange;
        }

        private static Dictionary<CoinDenomination, int> GetSetOfCoins(IEnumerable<byte> coinDenominations, int remainingAmountInCents)
        {
            var changeCoins = new Dictionary<CoinDenomination, int>();
            foreach (var coinDenomination in coinDenominations)
            {
                var numberOfChangeCoins = remainingAmountInCents / coinDenomination;
                if (numberOfChangeCoins == 0)
                    continue;

                if (!CoinsRegisterDictionary.TryGetValue((CoinDenomination)coinDenomination, out var numberOfCoinsInRegistry) ||
                    numberOfCoinsInRegistry < 1)
                    continue;

                if (numberOfCoinsInRegistry < numberOfChangeCoins) //if amount of available coins is smaller we'll take all that's available
                {
                    numberOfChangeCoins = numberOfCoinsInRegistry;
                }

                changeCoins.TryAdd((CoinDenomination)coinDenomination, numberOfChangeCoins);
                TakeCoinsFromRegister((CoinDenomination)coinDenomination, numberOfChangeCoins);

                remainingAmountInCents -= numberOfChangeCoins * coinDenomination;
                if (remainingAmountInCents == 0) break;
            }

            return changeCoins;
        }

        public static void LoadCoinsRegister(Dictionary<CoinDenomination, int> initialCoins)
        {
            CoinsRegisterDictionary = initialCoins;
        }
    }
}