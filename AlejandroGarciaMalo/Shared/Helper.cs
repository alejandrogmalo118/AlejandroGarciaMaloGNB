using AlejandroGarciaMalo.Models.Entities;
using AlejandroGarciaMalo.Models.JsonModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Shared
{
    /// <summary>
    /// URLs for obtaining the data.
    /// </summary>
    public static class HelperConfig
    {
        public static string UrlRates, UrlTransactions;
    }

    /// <summary>
    /// Helper class containing a method for checking the conecction to internet
    /// </summary>
    public static class Helper
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// HelperApi to get data giving a certain Type and a Url.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class HelperApi<T> where T : class
    {
       
        public static async Task<IEnumerable<T>> GetFromApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var resultado = await httpClient.GetAsync(url);
                if (!resultado.IsSuccessStatusCode) return null;

                var texto = await resultado.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(texto);
            }
        }

    }

    /// <summary>
    /// Class for converters
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Method for calcutate the best rate route giving a Currency origin and destination.
        /// </summary>
        /// <param name="origin">Currency origin</param>
        /// <param name="output">Currency destination</param>
        /// <param name="listFinalRates">List for all required rates</param>
        /// <param name="listRates">List origin of rates</param>
        /// <returns></returns>
        public static List<Rate> SearchBestRate(string origin, string output, List<Rate> listFinalRates, List<Rate> listRates)
        {
            List<Rate> listOrigin = listRates.Where(m => m.From.Equals(origin)).ToList();
            List<Rate> listFinal = listRates.Where(m => m.To.Equals(output)).ToList();

            var finBusqueda = false;
            int i, k;

            for (i = 0; i < listOrigin.Count && finBusqueda == false; i++)
            {
                for (k = 0; k < listFinal.Count && finBusqueda == false; k++)
                {
                    if (listOrigin[i].To.Equals(listFinal[k].From))
                    {
                        listFinalRates.Add(listOrigin[i]);
                        listFinalRates.Add(listFinal[k]);
                        finBusqueda = true;
                    }
                }
            }

            if (!finBusqueda)
            {
                listFinalRates.Add(listOrigin[0]);
                SearchBestRate(listOrigin[0].To, output, listFinalRates, listRates);
            }

            return listFinalRates;
        }
    }

    
    
}
