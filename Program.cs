using System;
using System.Collections.Generic;

namespace EcoPrices
{
   class Program
   {
      static int IndentCount = 0;
      public static double GetCost(Dictionary<string, Resource> dictionary, string type)
      {
         IndentCount++;
         Resource res = dictionary[type];
         double cost = 0;
         Console.WriteLine($"{new string(' ', IndentCount)}GetCost for {type}");
         if (res.requires != null)
         {
            foreach ((string, double, bool) req in res.requires)
            {
               
               double itemCost = GetCost(dictionary, req.Item1) * (req.Item3 ? Bonuses.ApplyBonus(res, req.Item2) : req.Item2);
               Console.WriteLine($"{new string(' ', IndentCount*2)}Item: {req.Item1}, Count: {req.Item2}, BonusApplies: {req.Item3}, Applybonus result: {(req.Item3 ? Bonuses.ApplyBonus(res, req.Item2) : req.Item2)}\n{new string(' ', IndentCount*2)}AppliedCost: {itemCost}");
               cost += itemCost;
            }
         }
         if (cost == 0)
            cost = 1.0;
         Console.WriteLine($"{new string(' ', IndentCount*2)}GetCost for {type} ended with {cost}");
         IndentCount--;
         return cost;
      }

      //public static void Print(Dictionary<string, Resource> dictionary, string type)
      //{
      //   Resource res = dictionary[type];
      //   Dictionary<string, Resource> costs = new Dictionary<string, Resource>();
      //   foreach ((string, double, bool) req in res.requires)
      //   {
      //      req.Item1
      //   }
      //}

      static void Main(string[] args)
      {
         Bonus Logging = new Bonus("logging", .5);
         Bonus Smith = new Bonus("smith", .5);
         Bonus Carpentry = new Bonus("carpentry", .5);
         Bonus Tailoring= new Bonus("tailoring", .5);
         Bonus None = new Bonus("none", 0);
         Bonuses.bonuses.Add(Logging);
         Bonuses.bonuses.Add(Carpentry);
         Bonuses.bonuses.Add(Smith);
         Bonuses.bonuses.Add(Tailoring);

         Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();

         Resource cost = new Resource("cost", None);
         dictionary.Add("cost", cost);
         Resource cal = new Resource("cal", None, new (string, double, bool)[] { ("cost", 0.004, false) });
         dictionary.Add("cal", cal);

         Resource log = new Resource("log", Logging, new (string, double, bool)[] { ("cost", 0.1, false) });
         dictionary.Add("log", log);
         Resource hewn = new Resource("hewn", Logging, new (string, double, bool)[] { ("log", 2, true), ("cal", 6, false) });
         dictionary.Add("hewn", hewn);
         Resource iron = new Resource("iron", Smith, new (string, double, bool)[] { ("cost", 0.9, false) });
         dictionary.Add("iron", iron);
         Resource nails = new Resource("nails", Smith, new (string, double, bool)[] { ("cost", 0.07, false) });
         dictionary.Add("nails", nails);

         Resource board = new Resource("board", Carpentry, new (string, double, bool)[] { ("hewn", 0.3333, true), ("cal", 2, false) });
         dictionary.Add("board", board);

         Resource plantFibre = new Resource("plantFibre", None, new (string, double, bool)[] { ("cost", 0.05, false) });
         dictionary.Add("plantFibre", plantFibre);

         Resource cloth = new Resource("cloth", None, new (string, double, bool)[] { ("plantFibre", 3, true), ("cal", 10, false) });
         dictionary.Add("cloth", cloth);

         Resource lumber = new Resource("lumber", Carpentry, new (string, double, bool)[] { ("nails", 4.0, true),
            ("hewn", 4.0, true), ("cal", 12, false) });
         dictionary.Add("lumber", lumber);

         Resource largeLumberSign = new Resource("largeLumberSign", Carpentry, new (string, double, bool)[] { ("board", 10, true),
            ("lumber", 8.0, true), ("cal", 40, false) });
         dictionary.Add("largeLumberSign", largeLumberSign);

         Resource machinistTable = new Resource("machinistTable", Carpentry, new (string, double, bool)[] { 
            ("board", 12, true),
            ("lumber", 12, true),
            ("iron", 12, true),
            ("cal", 100, false) });
         dictionary.Add("machinistTable", machinistTable);


         //Console.WriteLine($"Lumber cost: {GetCost(dictionary, "lumber")}");

         //Console.WriteLine($"nail cost: {GetCost(dictionary, "nails")}");


         //Console.WriteLine($"board cost: {GetCost(dictionary, "board")}");

         Console.WriteLine($"largeLumberSign cost: {GetCost(dictionary, "largeLumberSign")}");

         Console.WriteLine($"cloth cost: {GetCost(dictionary, "cloth")}");

         //Console.WriteLine($"machinistTable cost: {GetCost(dictionary, "machinistTable")}");

      }
   }
}
