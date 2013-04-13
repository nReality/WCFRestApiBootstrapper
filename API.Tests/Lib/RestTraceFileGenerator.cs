using System;
using System.IO;
using RestSharp;

namespace API.Tests.Lib
{
    public class RestTraceFileGenerator
    {
        private const string ToggleJavaScript =
            @"<script language=""javascript""> 
                function toggle(idtotoggle) {
	                var elementToToggle = document.getElementById(idtotoggle);
	                if(elementToToggle.style.display == ""block"") {
    		                elementToToggle.style.display = ""none"";
  	                }
	                else {
		                elementToToggle.style.display = ""block"";
	                }
                } 
                </script>";

        private const string Heading = "<body><h2>REST calls made during tests (click to view response):</h2>";
        private const string TraceListItemFormat = @"<li><a href=""#"" onclick=""toggle('{1}{5}')"">{0}: {3}   ({4} milliSecs)</a><div id='{1}{5}' style=""display:none;background-color:AliceBlue"">{2}</div></li>";
        private const string FilePathAndName = "StoryQ_Report\\RestCalls.htm";
        private static int _count = 1;

        //File will be recreated for each process
        static RestTraceFileGenerator()
        {
            CreateFile();
        }

        private static void CreateFile()
        {
            File.WriteAllText(FilePathAndName,
                              string.Format("<html>{0}{1}{2}<ul>", ToggleJavaScript, Heading, Environment.NewLine));
        }


        public static void WriteRestTrace<TRestService>(string resource, IRestResponse response,long timeTotalInMilliSeconds)
        {
            File.AppendAllText(FilePathAndName,
                               string.Format(
                                   TraceListItemFormat,
                                   typeof(TRestService).Name,
                                   CreateHtmlId<TRestService>(resource), 
                                   System.Web.HttpUtility.HtmlEncode(response.Content), 
                                   resource,
                                   timeTotalInMilliSeconds,
                                   ++_count));
        }

        private static string CreateHtmlId<TRestService>(string resource)
        {
            return typeof(TRestService).Name + System.Web.HttpUtility.HtmlEncode(resource).Replace('/', '0').Replace('?', 'Q').Replace('=','E').Replace('&', 'A');
        }
    }
}