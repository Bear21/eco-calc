using System;
using System.Collections.Generic;
using System.Text;

namespace EcoPrices
{
   class Bonus
   {
      public string name;
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
         bonuses = new List<Bonus>();

      }

      public static List<Bonus> bonuses;
      public static double ApplyBonus(Resource res, double count)
      {
         if (bonuses.Contains(res.bonus))
         {
            return count * res.bonus.bonusSize;
         }
         return count;
      }
   }
}
