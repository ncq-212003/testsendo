using Serilog;

namespace Erp.Social.Sendo.Services
{
    public class MyLogger
    {
        public static void LogError(string msgerror,
            [System.Runtime.CompilerServices.CallerMemberName] string functionError = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            string classerror = Path.GetFileNameWithoutExtension(filePath);
            Log.Error($"\nclass: {classerror} \nfunc: {functionError} \nmsg: {msgerror} \n");
        }
    }
}
