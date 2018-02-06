using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication5
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int Pbkdf2Iterations = 1000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;


        public string ConnectionString { get; set; }
        public string ForgotPasswordCodeLink { get; set; }



        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Convert(object sender, EventArgs e)
        {
            // Either directly set your Logic here or call another method
            EncyptingPWD en = new EncyptingPWD();
            en.Getpassword();
        }
    }
}