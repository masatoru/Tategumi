using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanako.Models
{
  public class HKHttpUtil
  {
    // baseurl : http://api.geonames.org/
    //suburl : "earthquakesJSON?north=44.1&south=-9.9&east=-22.4&west=55.2&username=bertt"
    static public async Task<string> getXmlTextAsync(string baseUrl,string subUrl)
    {
      var client = new System.Net.Http.HttpClient();
      client.BaseAddress = new Uri(baseUrl);
      var response = await client.GetAsync(subUrl);
      return response.Content.ReadAsStringAsync().Result;
    } 
  }
}
