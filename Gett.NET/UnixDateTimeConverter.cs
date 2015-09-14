#region License information
/*

  Copyright (c) 2014 Togocoder (http://www.codeproject.com/Members/Kim-Togo)
 
  This file is part of Gett.NET library that uses the Ge.tt REST API, http://ge.tt/developers

  Gett.NET is a free library: you can redistribute it and/or modify as nessery
  it under the terms of The Code Project Open License (CPOL) as published by
  the The Code Project, either version 1.02 of the License, or (at your option) any later version.

  Gett.NET is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY OF ANY KIND. The software is provided "as-is".
 
  Please read the The Code Project Open License (CPOL) at http://www.codeproject.com/info/cpol10.aspx

  I would appreciate getting an email, if you choose to use this library in your own work.
  Send an email to togocoder(at)live.com with a little description of your work, thanks! :-)

  ---
  This class handles converting an Unix timestamp to DateTime class.
  It is written by Christophe Geers on September 25, 2011. (About http://cgeers.com/about/)
  
  Please see http://cgeers.com/2011/09/25/writing-a-custom-json-net-datetime-converter/
  for the orginal source code.
  
  It is a JsonConverter type for Json.NET.
*/
#endregion

using System;

// ReSharper disable once CheckNamespace
namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Convert Unix time to and from DateTime.
    /// </summary>
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));
            }

            var ticks = (long)reader.Value;
            var date = new DateTime(1970, 1, 1);
            date = date.AddSeconds(ticks);

            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                TimeSpan delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Unix epoc starts January 1st, 1970");
                }

                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("Expected date object value.");
            }

            writer.WriteValue(ticks);
        }
    }
}
