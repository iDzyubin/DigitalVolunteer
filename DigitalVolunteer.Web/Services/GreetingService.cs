using System;

namespace DigitalVolunteer.Web.Services
{
    public class GreetingService
    {
        public string Greeting()
        {
            var currentTime = DateTime.Now.Hour;
            if( currentTime > 7  && currentTime < 11 ) return "Добрый день";
            if( currentTime > 11 && currentTime < 18 ) return "Добрый день";
            return "Добрый вечер";
        }
    }
}