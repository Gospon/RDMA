using Newtonsoft.Json.Linq;
using ProjectLukas.Models;
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
    public partial class Users : ContentPage
    {
        List<UserRecord> userRecords = new List<UserRecord>();
        bool isIdAscending = false;
        bool isEmailAscending = false;
        bool isTimeAscending = false;
        public Users()
        {
            GetUsersData();
            InitializeComponent();
        }

        public async Task GetUsersData()
        {
            var uri = new Uri("https://www.idt.mdh.se/personal/plt01/languide/?get=users");
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
                    UserRecord userRecord = new UserRecord();
                    userRecord.Id = (int)record["id_user"];
                    userRecord.Email = record["email"].ToString();
                    userRecord.CreateTime = (DateTime)record["create_time"];
                    userRecords.Add(userRecord);
                }
            }
            UserRecordTable.ItemsSource = userRecords;

        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ShowPicker.SelectedIndex;
            var selectedIndexValue = ShowPicker.ItemsSource[selectedIndex];
            UserRecordTable.ItemsSource = userRecords.Take((short)selectedIndexValue);
        }

        private void OnSearchBarChanged(object sender, EventArgs e)
        {
            UserRecordTable.ItemsSource = userRecords
                .Where(x => x.Id.ToString().Contains(TableSearch.Text));
        }

        private void OnId(object sender, EventArgs e)
        {
            if (isIdAscending)
            {
                var orderedUserRecords = userRecords.OrderBy(record => record.Id).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isIdAscending = false;
            }
            else
            {
                var orderedUserRecords = userRecords.OrderByDescending(record => record.Id).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isIdAscending = true;

            }

        }

        private void OnEmail(object sender, EventArgs e)
        {
            if (isEmailAscending)
            {
                var orderedUserRecords = userRecords.OrderBy(record => record.Email).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isEmailAscending = false;
            }
            else
            {
                var orderedUserRecords = userRecords.OrderByDescending(record => record.Email).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isEmailAscending = true;

            }
        }

        private void OnTime(object sender, EventArgs e)
        {
            if (isTimeAscending)
            {
                var orderedUserRecords = userRecords.OrderBy(record => record.CreateTime).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isTimeAscending = false;
            }
            else
            {
                var orderedUserRecords = userRecords.OrderByDescending(record => record.CreateTime).ToList();
                UserRecordTable.ItemsSource = orderedUserRecords;
                isTimeAscending = true;

            }

        }
    }
}