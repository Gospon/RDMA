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
    public partial class Results : ContentPage
    {
        List<ResultRecord> resultRecords = new List<ResultRecord>();
        List<int> shows = new List<int>() { 1, 5, 10, 25, 50, 100, 1000 };
        private bool isUserIdAscending = false;
        private bool isEmailAscending = false;
        private bool isExerciseIdAscending = false;
        private bool isResultPercentAscending = false;
        private bool isResultScoreAscending = false;
        private bool isResultMaxAscending = false;
        private bool isSkillAscending = false;
        private bool isLanguageAscending = false;

        public Results()
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
                    ResultRecord resultRecord = new ResultRecord();
                    resultRecord.Id = (int)record["id_user"];
                    resultRecord.Email = record["email"].ToString();
                    resultRecord.CreateTime = (DateTime)record["create_time"];
                    resultRecord.ExerciseId = record["id_exercise"].ToString();
                    resultRecord.ResultPercent = (int)record["result_percent"];
                    resultRecord.ResultScore = (int)record["result_score"];
                    resultRecord.ResultMax = (int)record["result_max"];
                    resultRecord.Skill = record["skill"].ToString();
                    resultRecord.Language = record["language"].ToString();
                    resultRecord.ResultDate = (DateTime)record["result_date"];

                    resultRecords.Add(resultRecord);
                }
            }
            //Cijeli breaka program
            ResultRecordTable.ItemsSource = resultRecords.Where((x, i) => i % 10 == 0);
            MinDate.Date = resultRecords.Select(x => x.CreateTime).Min();
            MaxDate.Date = resultRecords.Select(x => x.CreateTime).Max();
            LanguagePicker.ItemsSource = resultRecords.Where(x => x.Language != "").Select(x => x.Language).Distinct().ToList();
            SkillPicker.ItemsSource = resultRecords.Where(x => x.Skill != "").Select(x => x.Skill).Distinct().ToList();
            ShowPicker.ItemsSource = shows;
        }

        public void OnFilter(object sender, EventArgs args)
        {
            List<ResultRecord> filteredResultRecords = resultRecords;
            if (!String.IsNullOrEmpty(UserID.Text))
            {
                filteredResultRecords = resultRecords.Where(x => UserID.Text == x.Id.ToString()).ToList();
            }

            if (!String.IsNullOrEmpty(UserEmail.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => UserEmail.Text == x.Email).ToList();
            }

            if (!String.IsNullOrEmpty(UserExerciseID.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => UserExerciseID.Text == x.ExerciseId).ToList();
            }
            if (!String.IsNullOrEmpty(ResultMax.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultPercent <= int.Parse(ResultMax.Text)).ToList();
            }
            if (!String.IsNullOrEmpty(ResultMin.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultPercent >= int.Parse(ResultMin.Text)).ToList();
            }
            if (!String.IsNullOrEmpty(ResulScoretMax.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultScore <= int.Parse(ResulScoretMax.Text)).ToList();
            }
            if (!String.IsNullOrEmpty(ResultScoreMin.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultScore >= int.Parse(ResultScoreMin.Text)).ToList();
            }
            if (!String.IsNullOrEmpty(ResultMaxMax.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultMax <= int.Parse(ResultMaxMax.Text)).ToList();
            }
            if (!String.IsNullOrEmpty(ResultMaxMin.Text))
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.ResultMax >= int.Parse(ResultMaxMin.Text)).ToList();
            }
            if (LanguagePicker.SelectedIndex != -1)
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.Language == (string)LanguagePicker.SelectedItem).ToList();
            }
            if (SkillPicker.SelectedIndex != -1)
            {
                filteredResultRecords = filteredResultRecords.Where(x => x.Skill == (string)SkillPicker.SelectedItem).ToList();
            }

            ResultRecordTable.ItemsSource = filteredResultRecords;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowPicker.SelectedIndex != -1 && ResultRecordTable.ItemsSource != null)
            {
                ResultRecordTable.ItemsSource = resultRecords.Take((int)ShowPicker.ItemsSource[ShowPicker.SelectedIndex]).ToList();
            }
        }

        private void OnSearchBarChanged(object sender, EventArgs e)
        {
            ResultRecordTable.ItemsSource = resultRecords
                .Where(x => x.Id.ToString().Contains(TableSearch.Text));
        }

        private void OnClear(object sender, EventArgs e)
        {
            var showPickerList = new List<int>() { 1, 5, 10, 25, 50, 100, 1000 };
            UserID.Text = String.Empty;
            UserEmail.Text = String.Empty;
            UserExerciseID.Text = String.Empty;
            ResultMin.Text = String.Empty;
            ResultMax.Text = String.Empty;
            ResultScoreMin.Text = String.Empty;
            ResulScoretMax.Text = String.Empty;
            ResultMaxMin.Text = String.Empty;
            ResultMaxMax.Text = String.Empty;
            LanguagePicker.ItemsSource = resultRecords.Where((x, i) => i % 10 == 0 && x.Language != "").Select(x => x.Language).Distinct().ToList();
            SkillPicker.ItemsSource = resultRecords.Where((x, i) => i % 10 == 0).Select(x => x.Skill).Distinct().ToList();
            ShowPicker.ItemsSource = showPickerList;
            TableSearch.Text = String.Empty;
            MinDate.Date = resultRecords.Select(x => x.CreateTime).Min();
            MaxDate.Date = resultRecords.Select(x => x.CreateTime).Max();
        }

        private void OnUserId(object sender, EventArgs e)
        {
            if (isUserIdAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.Id).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isUserIdAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.Id).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isUserIdAscending = true;

            }
        }

        private void OnEmail(object sender, EventArgs e)
        {
            if (isEmailAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.Email).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isEmailAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.Email).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isEmailAscending = true;

            }
        }

        private void OnExerciseId(object sender, EventArgs e)
        {
            if (isExerciseIdAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.ExerciseId).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isExerciseIdAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.ExerciseId).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isExerciseIdAscending = true;

            }
        }

        private void OnResultPercent(object sender, EventArgs e)
        {
            if (isResultPercentAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.ResultPercent).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultPercentAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.ResultPercent).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultPercentAscending = true;

            }
        }

        private void OnResultScore(object sender, EventArgs e)
        {
            if (isResultScoreAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.ResultScore).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultScoreAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.ResultScore).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultScoreAscending = true;

            }
        }

        private void OnResultMax(object sender, EventArgs e)
        {
            if (isResultMaxAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.ResultMax).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultMaxAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.ResultMax).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isResultMaxAscending = true;

            }
        }

        private void OnSkill(object sender, EventArgs e)
        {
            if (isSkillAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.Skill).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isSkillAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.Skill).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isSkillAscending = true;

            }
        }

        private void OnLanguage(object sender, EventArgs e)
        {
            if (isLanguageAscending)
            {
                var orderedUserRecords = resultRecords.OrderBy(record => record.Language).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isLanguageAscending = false;
            }
            else
            {
                var orderedUserRecords = resultRecords.OrderByDescending(record => record.Language).ToList();
                ResultRecordTable.ItemsSource = orderedUserRecords;
                isLanguageAscending = true;

            }
        }
    }
}