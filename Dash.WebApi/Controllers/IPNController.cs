﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Http;
using anyhelp.Data.DataContext;
using System.Net;
using System.Security.Cryptography;

namespace anyhelp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPNController : BaseController
    {
        private readonly anyhelpContext _context;
        // private readonly TokenManager _tokenManger;
        //private readonly IProjectRepository _projectRepo;
        private class IPNContext
        {
            public HttpRequest IPNRequest { get; set; }

            public string RequestBody { get; set; }

            public string Verification { get; set; } = String.Empty;
        }

        public IPNController(anyhelpContext context)
        {
            _context = context;
            
        }

        [HttpPost("Receive")]
        public IActionResult Receive()
        {

            try
            {
                AppConfiguration appC= new AppConfiguration ();

                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = appC.hashSequence; //"key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                if (Request.Form["status"] == "success")
                {

                    merc_hash_vars_seq = hash_seq.Split('|');
                    //Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = "";// appC.SALT+ " |" + Request.Form["status"];
                    foreach (string hash_var in merc_hash_vars_seq)
                    {
                        if (hash_var == "key")
                        {
                            merc_hash_string = merc_hash_string + appC.MERCHANT_KEY;
                            merc_hash_string = merc_hash_string + '|';
                        }
                        else if (hash_var == "txnid")
                        {
                            merc_hash_string = merc_hash_string + Request.Form[hash_var];
                            merc_hash_string = merc_hash_string + '|';
                        }
                        else if (hash_var == "amount")
                        {
                            merc_hash_string = merc_hash_string + Convert.ToDecimal(Request.Form[hash_var]).ToString("g29");
                            merc_hash_string = merc_hash_string + '|';
                        }
                        else
                        {

                            merc_hash_string = merc_hash_string + (Request.Form[hash_var] !=  (string)null ? Convert.ToString( Request.Form[hash_var]).Trim() : "");// isset if else
                            merc_hash_string = merc_hash_string + '|';
                        }
                    }

                    merc_hash_string += appC.SALT;// appending SALT

                    //Check for presence of additionalCharges and include in hash
                    //if (!string.IsNullOrEmpty( Request.Form["additionalCharges"] ))
                    //{
                    //    merc_hash_string = Request.Form["additionalCharges"] + "|" + appC.SALT + "|" + Request.Form["status"];
                    //}
                    //foreach (string merc_hash_var in merc_hash_vars_seq)
                    //{
                    //    merc_hash_string += "|";
                    //    if (!string.IsNullOrEmpty(Request.Form[merc_hash_var]))
                    //    {
                    //        merc_hash_string = merc_hash_string + Request.Form[merc_hash_var];
                    //    }
                    //    else
                    //    {
                    //        merc_hash_string = merc_hash_string + "";
                    //    }

                    // }


                    //Calculate response hash to verify	
                    ExceptionLogging.SendToText(merc_hash_string);
                    merc_hash = Generatehash512(merc_hash_string).ToLower();

                    ExceptionLogging.SendToText("1 "+ merc_hash);
                    ExceptionLogging.SendToText("2 "+ Request.Form["hash"]);
                    //Comapre status and hash. Hash verification is mandatory.
                    if (merc_hash != Request.Form["hash"])
                    {
                        ExceptionLogging.SendToText("Hash value did not match");
                        

                    }
                    else
                    {
                        order_id = Request.Form["txnid"];
                        ExceptionLogging.SendToText("Payment Response");
                      
                        foreach (var strKey in Request.Form)
                        {
                            ExceptionLogging.SendToText(strKey.Key+"="+ strKey.Value.ToString());
                           
                        }
                        ExceptionLogging.SendToText("Hash Verified...");
                      

                        if (VerifyPayment(order_id, Request.Form["mihpayid"].ToString()))
                        {
                            ExceptionLogging.SendToText("Payment Verified...");
                            //Response.Write("<h2>Payment Verified...</h2><br />");
                        }
                        else
                        {
                            ExceptionLogging.SendToText("Payment Verification Failed...");
                            //Response.Write("<h2>Payment Verification Failed...</h2><br />");
                        }
                        //Hash value did not matched
                    }

                }

                else
                {
                    ExceptionLogging.SendToText("Payment failed or cancelled");
                    //Response.Write("<h2>Payment failed or cancelled</h2>");
                    // osc_redirect(osc_href_link(FILENAME_CHECKOUT, 'payment' , 'SSL', null, null,true));

                }
            }

            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                //Response.Write("<span style='color:red'>" + ex.Message + "</span>");

            }

            //  IPNContext ipnContext = new IPNContext()
            //  {
            //      IPNRequest = Request

            //  };



            //  using (StreamReader reader = new StreamReader(ipnContext.IPNRequest.Body, Encoding.ASCII))
            //  {
            //      ipnContext.RequestBody = reader.ReadToEnd();
            //  }
            //  ExceptionLogging.SendToText("----------Receive---------------" + Environment.NewLine);
            //  ExceptionLogging.SendToText(ipnContext.RequestBody);
            //  ExceptionLogging.SendToText("-------------------------" + Environment.NewLine);

            //  //Store the IPN received from PayPal
            // // LogRequest(ipnContext);

            //  //Fire and forget verification task
            //  //Task.Run(() => VerifyTask(ipnContext));

            //  ////Reply back a 200 code
            //  //return Ok();
            ////  VerifyTask(ipnContext);
            return Ok();
        }

        //This function is used to double check payment
        [NonAction]
        public Boolean VerifyPayment(string txnid, string mihpayid)
        {
            AppConfiguration appC = new AppConfiguration();
            string command = "verify_payment";
            string hashstr = appC.MERCHANT_KEY + " |" + command + "|" + txnid + "|" + appC.SALT;

            string hash = Generatehash512(hashstr);

            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            var request = (HttpWebRequest)WebRequest.Create(appC.PAYU_VERIFY_URL);

            var postData = "key=" + Uri.EscapeDataString(appC.MERCHANT_KEY);
            postData += "&hash=" + Uri.EscapeDataString(hash);
            postData += "&var1=" + Uri.EscapeDataString(txnid);
            postData += "&command=" + Uri.EscapeDataString(command);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (responseString.Contains("\"mihpayid\":\"" + mihpayid + "\"") && responseString.Contains("\"status\":\"success\""))
                return true;
            else
                return false;
            /*
            Here is json response example -

            {"status":1,
            "msg":"1 out of 1 Transactions Fetched Successfully",
            "transaction_details":</strong>
            {	
                "Txn72738624":
                {
                    "mihpayid":"403993715519726325",
                    "request_id":"",
                    "bank_ref_num":"670272",
                    "amt":"6.17",
                    "transaction_amount":"6.00",
                    "txnid":"Txn72738624",
                    "additional_charges":"0.17",
                    "productinfo":"P01 P02",
                    "firstname":"Viatechs",
                    "bankcode":"CC",
                    "udf1":null,
                    "udf3":null,
                    "udf4":null,
                    "udf5":"PayUBiz_PHP7_Kit",
                    "field2":"179782",
                    "field9":" Verification of Secure Hash Failed: E700 -- Approved -- Transaction Successful -- Unable to be determined--E000",
                    "error_code":"E000",
                    "addedon":"2019-08-09 14:07:25",
                    "payment_source":"payu",
                    "card_type":"MAST",
                    "error_Message":"NO ERROR",
                    "net_amount_debit":6.17,
                    "disc":"0.00",
                    "mode":"CC",
                    "PG_TYPE":"AXISPG",
                    "card_no":"512345XXXXXX2346",
                    "name_on_card":"Test Owenr",
                    "udf2":null,
                    "status":"success",
                    "unmappedstatus":"captured",
                    "Merchant_UTR":null,
                    "Settled_At":"0000-00-00 00:00:00"
                }
            }
            }

            Decode the Json response and retrieve "transaction_details" 
            Then retrieve {txnid} part. This is dynamic as per txnid sent in var1.
            Then check for mihpayid and status.

            */
        }
        [NonAction]      
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
    }
}