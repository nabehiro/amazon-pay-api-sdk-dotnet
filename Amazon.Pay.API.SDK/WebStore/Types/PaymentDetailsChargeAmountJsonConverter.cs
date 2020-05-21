using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Amazon.Pay.API.WebStore.Types
{

    public class PaymentDetailsChargeAmountJsonConverter : JsonConverter<Price>
    {
        public override bool CanRead => false;

        public override Price ReadJson(JsonReader reader, Type objectType, Price existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Price value, JsonSerializer serializer)
        {
            // if there wasn't provided anything, write null
            if (value.Amount == 0 && value.CurrencyCode == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();

                writer.WritePropertyName("currencyCode");
                serializer.Serialize(writer, value.CurrencyCode);

                // If CurrencyCode is 'JPY', Amount passed to the UpdateCheckoutSession API
                // must be an integer, othewise the API returns error that
                // "The value 'xxx' provoided for 'Amount' is invalid".
                // 
                // Json.NET serialize decimal value that is actually integer to "XXX.0",
                // so adjust it.
                writer.WritePropertyName("amount");
                writer.WriteRawValue(value.Amount.ToString(null, CultureInfo.InvariantCulture));

                writer.WriteEndObject();
            }
        }
    }

}
