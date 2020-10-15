using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace EcoPrices
{
   class Program
   {
      static int IndentCount = 0;
      public static double GetCost(Dictionary<string, Resource> dictionary, string type, bool explain, Dictionary<string, double> parts)
      {
         IndentCount++;
         Resource res = dictionary[type];
         double cost = 0;
         if (explain) 
            Console.WriteLine($"{new string(' ', IndentCount)}GetCost for {type}");
         if (res.requires != null)
         {
            foreach (Requires req in res.requires)
            {
               Dictionary<string, double> subParts = new Dictionary<string, double>();
               double countWithBonus = (req.bonus ? Bonuses.ApplyBonus(res, req.count) : req.count);
               double itemCost = GetCost(dictionary, req.name, explain, subParts) * countWithBonus;
               if (explain)
                  Console.WriteLine($"{new string(' ', IndentCount*2)}Item: {req.name}, Count: {req.count}, BonusApplies: {req.bonus}, Applybonus result: {(req.bonus ? Bonuses.ApplyBonus(res, req.count) : req.count)}\n{new string(' ', IndentCount*2)}AppliedCost: {itemCost}");
               cost += itemCost;

               parts[req.name] = parts.ContainsKey(req.name) ? (parts[req.name] + countWithBonus) : countWithBonus;
               foreach (var part in subParts)
               {
                  parts[part.Key] = parts.ContainsKey(part.Key) ? parts[part.Key] + part.Value*countWithBonus : part.Value * countWithBonus;
               }
            }
         }
         if (cost == 0)
            cost = 1.0;
         if (explain)
            Console.WriteLine($"{new string(' ', IndentCount*2)}GetCost for {type} ended with {cost}");
         IndentCount--;
         return cost;
      }


      static void Main(string[] args)
      {
         if (args.Length != 1)
         {
            Console.WriteLine(@"Click and drag in a json config file.");
         }
         else
         {
            Config baseConfig;
            try
            {
               using (var fs = System.IO.File.OpenRead("base.json"))
               {
                  DataContractJsonSerializer deser = new DataContractJsonSerializer(typeof(Config));
                  baseConfig = deser.ReadObject(fs) as Config;
               }
            }
            catch (System.IO.FileNotFoundException ex)
            {
               baseConfig = null;
            }
            try
            {
               Config sd;
               using (var fs = System.IO.File.OpenRead(args[0]))
               {
                  DataContractJsonSerializer deser = new DataContractJsonSerializer(typeof(Config));
                  sd = deser.ReadObject(fs) as Config;
               }

               foreach (var bonus in sd.bonuses)
               {
                  Bonuses.bonuses.Add(bonus.name, 1.0 - bonus.bonusSize);
               }
               // Add base after override.
               if (baseConfig != null)
               {
                  foreach (var bonus in baseConfig.bonuses)
                  {
                     try
                     {
                        Bonuses.bonuses.Add(bonus.name, 1.0 - bonus.bonusSize);
                     }
                     catch (ArgumentException)
                     {
                     }
                  }
               }
               Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();
               foreach (var resource in sd.resources)
               {
                  dictionary.Add(resource.name, resource);
               }
               // Add base after override.
               if (baseConfig != null)
               {
                  foreach (var resource in baseConfig.resources)
                  {
                     try 
                     {
                        dictionary.Add(resource.name, resource);
                     }
                     catch (ArgumentException)
                     {
                     }
                  }
               }
               foreach (var price in sd.priceCheck)
               {
                  Dictionary<string, double> parts = new Dictionary<string, double>();
                  Console.WriteLine($"\n{price.name} cost: {GetCost(dictionary, price.name, price.explain, parts):f2}");
                  if (price.count > 0)
                  {
                     Console.WriteLine($" count:{price.count}, total cost:{parts["cost"]*price.count:f2}");
                  }
                  int count = price.count == 0 ? 1 : price.count; 
                  if (price.parts)
                  {
                     Console.WriteLine($" Parts:");
                     var sortedParts = parts.Keys.ToList();
                     sortedParts.Sort();
                     bool round = price.count != 0;
                     if (round)
                     {
                        // todo
                     }
                     foreach (var part in sortedParts)
                     {
                        double cost = parts[part] * count;
                        Console.WriteLine($"  {part}: {cost:n2}");
                     }
                  }
                  Console.WriteLine("");
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
         }
         Console.WriteLine("Press enter to exit");
         // wait for 'new line' to exit.
         Console.ReadLine();
      }
   }
}
