using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EcoPrices
{
   [DataContract(Name = "requires")]
   class Requires
   {
      [DataMember(Name ="name")]
      public string name;
      [DataMember(Name = "count")]
      public double count;
      [DataMember(Name = "bonus")]
      public bool bonus;
   }
   [DataContract(Name = "resource")]
   class Resource
   {
      [DataMember(Name = "name")]
      public string name;
      [DataMember(Name = "bonus")]
      public Bonus bonus;
      public (string, double, bool)[] requires;

      public Resource(string name, Bonus bonus, (string, double, bool)[] requires = default)
      {
         this.name = name ?? throw new ArgumentNullException(nameof(name));
         this.bonus = bonus ?? throw new ArgumentNullException(nameof(bonus));
         this.requires = requires;
      }

      public (string, double, bool)[] GetResources()
      {
         return requires;
      }


      //public string PrintRequires()
      //{
      //   var builder = new StringBuilder();
      //   builder.AppendLine($"{name} requires:");
      //   if (requires != null)
      //   {
      //      foreach (var res in requires)
      //      {
      //         builder.AppendLine($"  {res.Item1.name}: {Bonuses.ApplyBonus(res.Item1, res.Item2)}");
      //      }
      //   }
      //   return builder.ToString();
      //}

      //public (string, (Resource, int)[]) GetResourcesRecusive()
      //{

      //}
   }
}
