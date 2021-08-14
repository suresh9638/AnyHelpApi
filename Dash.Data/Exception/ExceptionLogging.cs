
using System;
using System.IO;


/// <summary>  
/// Summary description for ExceptionLogging  
/// </summary>  
public static class ExceptionLogging
{

    private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

    public static void SendErrorToText(Exception ex)
    {
        var line = Environment.NewLine + Environment.NewLine;

        ErrorlineNo = ex.StackTrace.ToString().Split(':')[ex.StackTrace.ToString().Split(':').Length-1];
        Errormsg = ex.GetType().Name.ToString();
        extype = ex.GetType().ToString();
        exurl = ex.StackTrace.ToString().Split(':')[0]; //context.Request.Scheme +"://"+ context.Request.Host + context.Request.Path;

        ErrorLocation = ex.Message.ToString();

        try
        {
            string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Controller Action:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line+   Environment.NewLine ;
            error+="-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------"+ Environment.NewLine;
            error += "-------------------------------------------------------------------------------------" + Environment.NewLine;
            error += line + Environment.NewLine;
            error += error + Environment.NewLine;
            error += "--------------------------------*End*------------------------------------------"+   Environment.NewLine ;
            error += line + Environment.NewLine;

            var path = AppDomain.CurrentDomain.BaseDirectory + "ErrorLog";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                var pathfile = AppDomain.CurrentDomain.BaseDirectory + "ErrorLog//log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(pathfile))
                { 
                    File.WriteAllText(pathfile, error);
                }
                else
                {
                    File.AppendAllText(pathfile, error);
                }
            }
            catch (Exception ex1)
            {
              //  ExceptionLogging.SendErrorToText(ex1);
            }
          

        }
        catch (Exception ex1)
        {
            //ExceptionLogging.SendErrorToText(ex1);


        }
    }

}