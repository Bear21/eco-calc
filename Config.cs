using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EcoPrices
{
   [DataContract(Name = "config")]
   class Config
   {
      [DataMember(Name = "resources")]
      public Resource[] resources;
      [DataMember(Name ="bonuses")]
      public Bonus[] bonuses;
      [DataMember(Name = "price_check")]
      public PriceCheck[] priceCheck;
   }
}
