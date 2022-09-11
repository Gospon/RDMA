
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectLukas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Languages : ContentPage
    {
        List<string> languageRecords = new List<string>();
        bool isLanguageAscending = false;
        public Languages()
        {
            GetUsersData();
            InitializeComponent();
        }
        public async Task GetUsersData()
        {
            var uri = new Uri("https://www.idt.mdh.se/personal/plt01/languide/?get=results");
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                string json = content.ToString();
                var jsonObject = JObject.Parse(json);
                var data = jsonObject["data"];
                var jsonArray = JArray.Parse(data.ToString());
                foreach (var record in jsonArray)
                {
                    languageRecords.Add(record["language"].ToString());
                }
            }
            LanguageTable.ItemsSource = languageRecords.Where(x => x != "").Distinct().ToList();
        }

        private void OnLanguageTapped(object sender, EventArgs e)
        {
            if (isLanguageAscending)
            {
                var orderedUserRecords = languageRecords.Where(x => x != "").Distinct().OrderBy(languageRecord => languageRecord).ToList();
                LanguageTable.ItemsSource = orderedUserRecords;
                isLanguageAscending = false;
            }
            else
            {
                var orderedUserRecords = languageRecords.Where(x => x != "").Distinct().OrderByDescending(languageRecord => languageRecord).ToList();
                LanguageTable.ItemsSource = orderedUserRecords;
                isLanguageAscending = true;

            }
        }
    }
}