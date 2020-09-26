using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EcoPrices
{
   [DataContract(Name = "bonus")]
   class Bonus
   {
      [DataMember(Name = "name")]
      public string name;
      [DataMember(Name = "bonus_size")]
      public double bonusSize;
      public Bonus(string name, double size)
      {
         this.name = name;
         bonusSize = 1.0 - size;
      }
   }

   static class Bonuses
   {
      static Bonuses()
      {
         bonuses = new Dictionary<string, double>();

      }

      public static Dictionary<string, double> bonuses;
      public static double ApplyBonus(Resource res, double count)
      {
         if (bonuses.ContainsKey(res.bonus))
         {
            return count * bonuses[res.bonus];
         }
         return count;
      }
   }
}
