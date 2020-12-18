using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Prezent
{
    [Serializable()]
    public class Klasa : ISerializable
    {
        public string EmailAdd;
        public string EmailPass;
        public Klasa()
        {
            
        }
        public Klasa(SerializationInfo info, StreamingContext context)
        {
            EmailAdd = info.GetString("Adres");
            EmailPass = info.GetString("Pass");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Adres", EmailAdd);
            info.AddValue("Pass", EmailPass);
        }
    }
    static class Class
    {
        public static Klasa klasa;
    }
}
