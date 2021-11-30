using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO.Users
{
    public class UserWithSoldProductsDTO
    {
        [JsonProperty("firstName")]
        public string  FirstName{ get; set; }


        [JsonProperty("lastName")]
        public string LastName { get; set; }


        [JsonProperty("soldProducts")]
        public UserSoldProductDTO[] SoldProducts { get; set; }
    }
}
