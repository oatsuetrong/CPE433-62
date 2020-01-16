using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DNWS
{
  class StatPlugin : IPlugin
  {
    protected static Dictionary<String, int> statDictionary = null;
    public StatPlugin()
    {
      if (statDictionary == null)
      {
        statDictionary = new Dictionary<String, int>();

      }
    }

    public void PreProcessing(HTTPRequest request)
    {
      if (statDictionary.ContainsKey(request.Url))
      {
        statDictionary[request.Url] = (int)statDictionary[request.Url] + 1;
      }
      else
      {
        statDictionary[request.Url] = 1;
      }
    }
    public HTTPResponse GetResponse(HTTPRequest request)
    {
      String[] ip = Regex.Split(request.getPropertyByKey("RemoteEndPoint"), ":");
      HTTPResponse response = null;
      StringBuilder sb = new StringBuilder();
      sb.Append("<html><body>");
      foreach (KeyValuePair<String, int> entry in statDictionary)
      // {
      //   sb.Append(entry.Key + ": " + entry.Value.ToString() + "<br />");
      // }
      sb.Append("Client IP: ").Append(ip[0]).Append("<br><br>");
      sb.Append("Client Port: ").Append(ip[1]).Append("<br><br>");
      sb.Append("Browser Information: ").Append(request.getPropertyByKey("User-Agent")).Append("<br><br>");
      sb.Append("Accept Language: ").Append(request.getPropertyByKey("Accept-Language")).Append("<br><br>");
      sb.Append("Accept Encoding: ").Append(request.getPropertyByKey("Accept-Encoding")).Append("<br><br>");
      sb.Append("</body></html>");
      response = new HTTPResponse(200);
      response.body = Encoding.UTF8.GetBytes(sb.ToString());
      return response;
    }

    public HTTPResponse PostProcessing(HTTPResponse response)
    {
      throw new NotImplementedException();
    }
  }
}