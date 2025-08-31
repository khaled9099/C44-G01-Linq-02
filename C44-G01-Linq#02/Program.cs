using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            #region Element Operators
            // 1. Get first Product out of Stock
            var firstOutOfStock = ListGenerators.Products.FirstOrDefault(p => p.UnitsInStock == 0);
            Console.WriteLine($"First out of stock product: {firstOutOfStock?.ProductName}");

            // 2. Return first product whose Price > 1000
            var firstExpensive = ListGenerators.Products.FirstOrDefault(p => p.UnitPrice > 1000);
            Console.WriteLine($"First product with price > 1000: {firstExpensive?.ProductName ?? "None"}");

            // 3. Retrieve the second number greater than 5
            var secondNum = arr.Where(n => n > 5).Skip(1).FirstOrDefault();
            Console.WriteLine($"Second number > 5: {secondNum}");
            #endregion

            #region Aggregate Operators
            // 1. Count odd numbers
            var oddCount = arr.Count(n => n % 2 != 0);
            Console.WriteLine($"Odd numbers count: {oddCount}");

            // 2. List of customers and how many orders
            var custOrders = ListGenerators.Customers.Select(c => new { c.CustomerName, Orders = c.Orders.Count() });
            foreach (var c in custOrders) Console.WriteLine($"{c.CustomerName}: {c.Orders}");

            // 3. List categories & how many products
            var catProducts = ListGenerators.Products.GroupBy(p => p.Category)
                                .Select(g => new { Category = g.Key, Count = g.Count() });
            foreach (var c in catProducts) Console.WriteLine($"{c.Category}: {c.Count}");

            // 4. Total of numbers
            Console.WriteLine($"Sum: {arr.Sum()}");

            // 5. Total chars in all words
            var totalChars = ListGenerators.Dictionary.Sum(w => w.Length);
            Console.WriteLine($"Total chars: {totalChars}");

            // 6. Shortest word length
            Console.WriteLine($"Shortest word length: {ListGenerators.Dictionary.Min(w => w.Length)}");

            // 7. Longest word length
            Console.WriteLine($"Longest word length: {ListGenerators.Dictionary.Max(w => w.Length)}");

            // 8. Average word length
            Console.WriteLine($"Average word length: {ListGenerators.Dictionary.Average(w => w.Length)}");

            // 9. Total units in stock for each category
            var stockByCat = ListGenerators.Products.GroupBy(p => p.Category)
                            .Select(g => new { Category = g.Key, TotalStock = g.Sum(p => p.UnitsInStock) });
            foreach (var s in stockByCat) Console.WriteLine($"{s.Category}: {s.TotalStock}");

            // 10. Cheapest price per category
            var cheapest = ListGenerators.Products.GroupBy(p => p.Category)
                          .Select(g => new { g.Key, MinPrice = g.Min(p => p.UnitPrice) });
            foreach (var c in cheapest) Console.WriteLine($"{c.Key}: {c.MinPrice}");

            // 11. Products with cheapest price per category
            var cheapProducts = ListGenerators.Products.GroupBy(p => p.Category)
                                .SelectMany(g => g.Where(p => p.UnitPrice == g.Min(p2 => p2.UnitPrice)));
            foreach (var p in cheapProducts) Console.WriteLine($"{p.Category} - {p.ProductName} : {p.UnitPrice}");

            // 12. Most expensive price per category
            var expensive = ListGenerators.Products.GroupBy(p => p.Category)
                          .Select(g => new { g.Key, MaxPrice = g.Max(p => p.UnitPrice) });
            foreach (var c in expensive) Console.WriteLine($"{c.Key}: {c.MaxPrice}");

            // 13. Products with most expensive price per category

            khaled azoz, [8 / 30 / 2025 8:34 PM]
var expensiveProducts = ListGenerators.Products.GroupBy(p => p.Category)
                                .SelectMany(g => g.Where(p => p.UnitPrice == g.Max(p2 => p2.UnitPrice)));
            foreach (var p in expensiveProducts) Console.WriteLine($"{p.Category} - {p.ProductName} : {p.UnitPrice}");

            // 14. Average price of each category
            var avgPrice = ListGenerators.Products.GroupBy(p => p.Category)
                          .Select(g => new { g.Key, AvgPrice = g.Average(p => p.UnitPrice) });
            foreach (var c in avgPrice) Console.WriteLine($"{c.Key}: {c.AvgPrice}");
            #endregion

            #region Set Operators
            // 1. Unique category names
            var uniqueCats = ListGenerators.Products.Select(p => p.Category).Distinct();
            foreach (var c in uniqueCats) Console.WriteLine(c);

            // 2. Unique first letters (products + customers)
            var firstLetters = ListGenerators.Products.Select(p => p.ProductName[0])
                                .Union(ListGenerators.Customers.Select(c => c.CustomerName[0]));
            foreach (var f in firstLetters) Console.WriteLine(f);

            // 3. Common first letters
            var commonFirstLetters = ListGenerators.Products.Select(p => p.ProductName[0])
                                     .Intersect(ListGenerators.Customers.Select(c => c.CustomerName[0]));
            foreach (var c in commonFirstLetters) Console.WriteLine(c);

            // 4. First letters in products but not in customers
            var exceptLetters = ListGenerators.Products.Select(p => p.ProductName[0])
                                   .Except(ListGenerators.Customers.Select(c => c.CustomerName[0]));
            foreach (var e in exceptLetters) Console.WriteLine(e);

            // 5. Concatenate customer & product first letters, no duplicates
            var concatLetters = ListGenerators.Products.Select(p => p.ProductName[0])
                                   .Concat(ListGenerators.Customers.Select(c => c.CustomerName[0]))
                                   .Distinct();
            foreach (var c in concatLetters) Console.WriteLine(c);
            #endregion

            #region Partitioning Operators
            // 1. First 3 orders in Washington
            var first3Orders = ListGenerators.Customers
                                .Where(c => c.City == "Washington")
                                .SelectMany(c => c.Orders)
                                .Take(3);
            foreach (var o in first3Orders) Console.WriteLine(o.OrderID);

            // 2. Skip first 2 orders
            var skip2Orders = ListGenerators.Customers
                                .Where(c => c.City == "Washington")
                                .SelectMany(c => c.Orders)
                                .Skip(2);
            foreach (var o in skip2Orders) Console.WriteLine(o.OrderID);

            // 3. Take until number < its index
            var takeWhile = arr.TakeWhile((n, i) => n >= i);
            Console.WriteLine(string.Join(", ", takeWhile));

            // 4. Skip until divisible by 3
            var skipUntilDiv3 = arr.SkipWhile(n => n % 3 != 0);
            Console.WriteLine(string.Join(", ", skipUntilDiv3));

            // 5. Take until number < first element
            var firstElem = arr.First();
            var takeUntil = arr.TakeWhile(n => n >= firstElem);
            Console.WriteLine(string.Join(", ", takeUntil));
            #endregion

            #region Quantifiers
            // 1. Any word contains "ei"
            bool hasEi = ListGenerators.Dictionary.Any(w => w.Contains("ei"));
            Console.WriteLine($"Any word contains 'ei': {hasEi}");

            // 2. Categories with at least one out of stock
            var outOfStockCats = ListGenerators.Products.GroupBy(p => p.Category)
                                  .Where(g => g.Any(p => p.UnitsInStock == 0));
            foreach (var g in outOfStockCats) Console.WriteLine(g.Key);

            khaled azoz, [8 / 30 / 2025 8:34 PM]
// 3. Categories where all in stock
            var allInStockCats = ListGenerators.Products.GroupBy(p => p.Category)
                                   .Where(g => g.All(p => p.UnitsInStock > 0));
            foreach (var g in allInStockCats) Console.WriteLine(g.Key);
            #endregion

            #region Grouping Operators
            // 1. Numbers grouped by remainder mod 5
            var nums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var groups = nums.GroupBy(n => n % 5);
            foreach (var g in groups)
            {
                Console.WriteLine($"Remainder {g.Key}: {string.Join(", ", g)}");
            }

            // 2. Words grouped by first letter
            var wordGroups = ListGenerators.Dictionary.GroupBy(w => w[0]);
            foreach (var g in wordGroups)
            {
                Console.WriteLine($"{g.Key}: {string.Join(", ", g)}");
            }

            // 3. Group words by custom comparer (anagrams)
            string[] arrWords = { "from", "salt", "earn", "last", "near", "form" };
            var anagramGroups = arrWords.GroupBy(w => String.Concat(w.OrderBy(c => c)));
            foreach (var g in anagramGroups)
            {
                Console.WriteLine(string.Join(", ", g));
            }
            #endregion
        }
    }
}