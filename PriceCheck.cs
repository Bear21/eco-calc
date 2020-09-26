using System.Runtime.Serialization;
namespace EcoPrices
{
   
   [DataContract(Name ="check_price")]
   public class PriceCheck
   {
      [DataMember(Name ="item")]
      public string name;
      [DataMember(Name ="explain")]
      public bool explain;
      [DataMember(Name = "parts")]
      public bool parts;
      [DataMember(Name = "count")]
      public int count;
   }
}