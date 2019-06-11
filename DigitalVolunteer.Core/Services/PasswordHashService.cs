using System;
using System.Security.Cryptography;

namespace DigitalVolunteer.Core.Services
{
    public class PasswordHashService
    {
        public string GetHash( string password )
        {
            byte[] salt = new byte[16];
            byte[] hash;

            using( var rng = new RNGCryptoServiceProvider() )
            {
                rng.GetBytes( salt );
            }

            using( var pbkdf2 = new Rfc2898DeriveBytes( password, salt, 1000 ) )
            {
                hash = pbkdf2.GetBytes( 20 );
            }

            var bytes = new byte[36];
            Array.Copy( salt, 0, bytes, 0, 16 );
            Array.Copy( hash, 0, bytes, 16, 20 );

            return Convert.ToBase64String( bytes );
        }

        public bool ValidateHash( string password, string hashed )
        {
            var correctHash = Convert.FromBase64String( hashed );

            byte[] hash;
            byte[] salt = new byte[16];
            Array.Copy( correctHash, 0, salt, 0, 16 );

            using( var pbkdf2 = new Rfc2898DeriveBytes( password, salt, 1000 ) )
            {
                hash = pbkdf2.GetBytes( 20 );
            }

            for( int i = 0; i < 20; i++ )
            {
                if( hash[i] != correctHash[i + 16] )
                    return false;
            }

            return true;
        }
    }
}
