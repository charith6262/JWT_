using System.Security.Cryptography;

namespace JWT.Model
{
    public static class PasswordHelper
    {
        //This method will convert plane text password into hashkey format
        public static string HashPassword(string password)
        {
            //Step : 1
            //Generate a random salt (16 bytes & it will ensure same password generate with different hash)

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //Step-2
            //We create password based key derivation function(PBKD) to hashing algorithm
            /*Password ->userpassword
            Salt ->Random salt value
            100000 ->No of iteration for slow attackers
            sha256 ->[hashing algorithm]
            */

            using(var pbkdf2 = new Rfc2898DeriveBytes(password , salt , 100000 , HashAlgorithmName.SHA256))
            {
                //Step-3 , Here we genarate the byte 

                byte[] hash = pbkdf2.GetBytes(32);
                //Step-4 , here we combine salt & hash into single byte .
                //Total = 16 bytes + 32 bytes hash = 48 bytes

                byte[] hashbytes = new byte[48];
                Array.Copy(salt, 0, hashbytes, 0, 16);

                Array.Copy(hash, 0, hashbytes, 16, 32);
                //We are convert byte array to 64 string , this value is saved in DB
                return Convert.ToBase64String(hashbytes);
            }
        }

        //Verify password : we compare here entered password comes from swagger with the stored hash password .

        public static bool verfiedPassword(string password , string storedhash)
        {
            //step:1 : We convert stored hash byte64 back into byte  array

            byte[] hashbytes = Convert.FromBase64String(storedhash);

            //step2 : We extract salt from stored hash (first 16 bytes )

            byte[] salt = new byte[16];
            Array.Copy(hashbytes , 0, salt,0 , 16);

            //step3 : We match entered hash password using same salt algorithm .

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                //step4 : we compare newly generated hash with stored hash 

                for(int i=0; i< 32; i++)
                {
                    if (hashbytes[i+16] != hash[i])
                        return false;
                }
                return true;
            }
        }

    }
}
