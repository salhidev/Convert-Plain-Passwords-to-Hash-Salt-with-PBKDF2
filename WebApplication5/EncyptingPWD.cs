using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication5
{

    public class EncyptingPWD
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int Pbkdf2Iterations = 51000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 2;
        public const int Pbkdf2Index = 1;



        public string ConnectionString { get; set; }
        public string ForgotPasswordCodeLink { get; set; }



        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);

        public void Getpassword()
        {

            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT distinct a.User_Name,a.EncPwd,a.SALT, b.Password from dbo.tbl_user_info AS a,dbo.vw_ShowLISUserData AS b WHERE a.User_Name = b.User_Name AND b.Password IS NOT NULL and b.Password <> '' and a.User_Name <> '' ";
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                
                var PLAINTEXTPWD = Convert.ToString(dr["Password"]);
                var EncryptPWD = HashPassword(PLAINTEXTPWD);
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "updateEncryptPWD";
                cmd1.Connection = con1;
                cmd1.CommandType = CommandType.StoredProcedure;
                char[] delimiter = { ':' };
                var split = EncryptPWD.Split(delimiter);

                var iterations = Int32.Parse(split[IterationIndex]);
                var salt = split[SaltIndex];
                var hash = split[0] +':'+ split[Pbkdf2Index];
                cmd1.Parameters.AddWithValue("@EncryptPWD", hash ?? "");
                cmd1.Parameters.AddWithValue("@Salt", salt ?? "");
                cmd1.Parameters.AddWithValue("@PLAINTEXTPWD", PLAINTEXTPWD ?? "");
                //con1.Open();
                SqlDataReader dr1 = cmd1.ExecuteReader();
                dr1.Close();
                int temp = cmd1.ExecuteNonQuery();
           }

            dr.NextResult();
        }


        public static string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);
            //   HttpContext.Current.Response.Write("salt=" + Convert.ToBase64String(salt));
            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(hash) + ":" +
                   Convert.ToBase64String(salt);
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }

}