using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing;
using bot_discord;
using System.Windows.Forms.DataVisualization.Charting;

namespace DiscordMessageSender
{
    public partial class MainForm : Form
    {
        private volatile bool stopFlag = false;
        private volatile bool stopFlag1 = false;
        private volatile bool stopFlag2 = false;
        private volatile bool stopFlag3 = false;


        private Dictionary<string, int> channelUsageCount = new Dictionary<string, int>();
        public MainForm()
        {
            InitializeComponent();
            LoadData();
            LoadData1();
            LoadData2();
            LoadData3();
            InitializeChart();
            System.Threading.Timer timer = new System.Threading.Timer(UpdateChart, null, 0, 360);
        }
        private void InitializeChart()
        {
            chart1.Titles.Add("Channel Usage Statistics");
            chart1.ChartAreas.Add("Default");
            chart1.Series.Add("ChannelUsage");
            chart1.Series["ChannelUsage"].ChartType = SeriesChartType.Spline;
            chart1.Series["ChannelUsage"].MarkerColor = Color.Red;
            chart1.Series["ChannelUsage"].MarkerStyle = MarkerStyle.Circle;
        }
        private void UpdateChart(object state)
        {
            Invoke(new Action(() => UpdateChart()));
        }
        private void UpdateChart()
        {
            chart1.Series["ChannelUsage"].Points.Clear();

            foreach (var channel in channelUsageCount)
            {
                chart1.Series["ChannelUsage"].Points.AddXY(channel.Key, channel.Value);
            }
        }
        private void SendMessage(string channel)
        {
            // Вызывается при отправке сообщения
            if (channelUsageCount.ContainsKey(channel))
            {
                channelUsageCount[channel]++;
            }
            else
            {
                channelUsageCount[channel] = 1;
            }
            UpdateChart();
        }
        
        private async void SendLogToTelegram(string message)
        {
            try
            {
                var botToken = telegramBotTokenTextBox.Text;
                var chatID = telegramChatIDTextBox.Text;

                using (var client = new HttpClient())
                {
                    var sendMessageEndpoint = $"https://api.telegram.org/bot{botToken}/sendMessage";

                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("chat_id", chatID),
                        new KeyValuePair<string, string>("text", message)
                    });

                    var response = await client.PostAsync(sendMessageEndpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        LogMessage($"Failed to send log to Telegram: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error sending log to Telegram: {ex.Message}");
            }
        }
        private async void SendLogToTelegram1(string message1)
        {
            try
            {
                var botToken1 = telegramBotTokenTextBox1.Text;
                var chatID1 = telegramChatIDTextBox1.Text;

                using (var client1 = new HttpClient())
                {
                    var sendMessageEndpoint1 = $"https://api.telegram.org/bot{botToken1}/sendMessage";

                    var content1 = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("chat_id", chatID1),
                        new KeyValuePair<string, string>("text", message1)
                    });

                    var response1 = await client1.PostAsync(sendMessageEndpoint1, content1);
                    var responseContent1 = await response1.Content.ReadAsStringAsync();

                    if (!response1.IsSuccessStatusCode)
                    {
                        LogMessage($"Failed to send log to Telegram: {responseContent1}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage1($"Error sending log to Telegram: {ex.Message}");
            }
        }
        private async void SendLogToTelegram2(string message2)
        {
            try
            {
                var botToken2 = telegramBotTokenTextBox2.Text;
                var chatID2 = telegramChatIDTextBox2.Text;

                using (var client2 = new HttpClient())
                {
                    var sendMessageEndpoint2 = $"https://api.telegram.org/bot{botToken2}/sendMessage";

                    var content2 = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("chat_id", chatID2),
                        new KeyValuePair<string, string>("text", message2)
                    });

                    var response2 = await client2.PostAsync(sendMessageEndpoint2, content2);
                    var responseContent2 = await response2.Content.ReadAsStringAsync();

                    if (!response2.IsSuccessStatusCode)
                    {
                        LogMessage2($"Failed to send log to Telegram: {responseContent2}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage2($"Error sending log to Telegram: {ex.Message}");
            }
        }
        private async void SendLogToTelegram3(string message3)
        {
            try
            {
                var botToken3 = telegramBotTokenTextBox3.Text;
                var chatID3 = telegramChatIDTextBox3.Text;

                using (var client3 = new HttpClient())
                {
                    var sendMessageEndpoint3 = $"https://api.telegram.org/bot{botToken3}/sendMessage";

                    var content3 = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("chat_id", chatID3),
                        new KeyValuePair<string, string>("text", message3)
                    });

                    var response3 = await client3.PostAsync(sendMessageEndpoint3, content3);
                    var responseContent3 = await response3.Content.ReadAsStringAsync();

                    if (!response3.IsSuccessStatusCode)
                    {
                        LogMessage3($"Failed to send log to Telegram: {responseContent3}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage3($"Error sending log to Telegram: {ex.Message}");
            }
        }


        private async Task SendMessageAsync()
        {
            var channelID = channelTextBox.Text;
            var token = headerTextBox.Text;
            var text = payloadTextBox.Text;
            var cooldownTime = int.Parse(cooldownTextBox.Text) * 1000;
            var gifPath = gifTextBox.Text;

            while (!stopFlag)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", token);

                        var content = new MultipartFormDataContent();

                        content.Add(new StringContent(text), "content");

                        if (!string.IsNullOrEmpty(gifPath) && File.Exists(gifPath))
                        {
                            var imageContent = new ByteArrayContent(File.ReadAllBytes(gifPath));
                            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
                            content.Add(imageContent, "file", Path.GetFileName(gifPath));
                        }

                        var response = await client.PostAsync($"https://discord.com/api/v9/channels/{channelID}/messages", content);
                        var responseString = await response.Content.ReadAsStringAsync();

                        var status = responseString.Contains("False");
                       

                        if (status)
                        {
                            LogMessage($"Failed to send to {channelID}");
                            SendLogToTelegram($"Error occurred: {channelID}");
                        }
                        else
                        {
                            if (channelID == "713466163585351791")
                            {
                                LogMessage($"MSG sent to {channelID} - 'реклама'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "713466197458550866")
                            {
                                LogMessage($"MSG sent to {channelID} - 'транспорт'");
                                SendMessage(channelTextBox.Text);
                            }
                            else if (channelID == "713466225220386846")
                            {
                                LogMessage($"MSG sent to {channelID} - 'недвижка'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "713466268560392222")
                            {
                                LogMessage($"MSG sent to {channelID} - 'бизнес'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "713466300860465192")
                            {
                                LogMessage($"MSG sent to {channelID} - 'оказание услуг'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "713466362202292285")
                            {
                                LogMessage($"MSG sent to {channelID} - 'барахолка'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "720689842593464400")
                            {
                                LogMessage($"MSG sent to {channelID} - 'муж.одежда'");
                                SendMessage(channelTextBox.Text);

                            }
                            else if (channelID == "713466334343725106")
                            {
                                LogMessage($"MSG sent to {channelID} - 'черный рынок'");
                                SendMessage(channelTextBox.Text);

                            }
                            else
                            {
                                LogMessage($"MSG sent to {channelID}");
                                SendMessage(channelTextBox.Text);

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Error occurred: {ex.Message}");
                    SendLogToTelegram($"Error occurred: {ex.Message}");
                }
                await Task.Delay(cooldownTime);
            }

        }
        private async Task SendMessageAsync1()
        {
            var channelID1 = channelTextBox1.Text;
            var token1 = headerTextBox1.Text;
            var text1 = payloadTextBox1.Text;
            var cooldownTime1 = int.Parse(cooldownTextBox1.Text) * 1000;
            var gifPath1 = gifTextBox1.Text;

            while (!stopFlag1)
            {
                try
                {
                    using (var client1 = new HttpClient())
                    {
                        client1.DefaultRequestHeaders.Add("Authorization", token1);

                        var content1 = new MultipartFormDataContent();

                        content1.Add(new StringContent(text1), "content");

                        if (!string.IsNullOrEmpty(gifPath1) && File.Exists(gifPath1))
                        {
                            var imageContent1 = new ByteArrayContent(File.ReadAllBytes(gifPath1));
                            imageContent1.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
                            content1.Add(imageContent1, "file", Path.GetFileName(gifPath1));
                        }

                        var response1 = await client1.PostAsync($"https://discord.com/api/v9/channels/{channelID1}/messages", content1);
                        var responseString1 = await response1.Content.ReadAsStringAsync();

                        var status1 = responseString1.Contains("False");


                        if (status1)
                        {
                            LogMessage1($"Failed to send to {channelID1}");
                            SendLogToTelegram1($"Error occurred: {channelID1}");
                        }
                        else if (channelID1 == "713466163585351791")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'реклама'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466197458550866")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'транспорт'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466225220386846")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'недвижка'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466268560392222")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'бизнес'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466300860465192")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'оказание услуг'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466362202292285")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'барахолка'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "720689842593464400")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'муж.одежда'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else if (channelID1 == "713466334343725106")
                        {
                            LogMessage1($"MSG sent to {channelID1} - 'черный рынок'");
                            SendMessage(channelTextBox1.Text);
                        }
                        else
                        {
                            LogMessage1($"MSG sent to {channelID1}");
                            SendMessage(channelTextBox1.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage1($"Error occurred: {ex.Message}");
                    SendLogToTelegram1($"Error occurred: {ex.Message}");
                }
                await Task.Delay(cooldownTime1);
            }

        }
        private async Task SendMessageAsync2()
        {
            var channelID2 = channelTextBox2.Text;
            var token2 = headerTextBox2.Text;
            var text2 = payloadTextBox2.Text;
            var cooldownTime2 = int.Parse(cooldownTextBox2.Text) * 1000;
            var gifPath2 = gifTextBox2.Text;

            while (!stopFlag2)
            {
                try
                {
                    using (var client2 = new HttpClient())
                    {
                        client2.DefaultRequestHeaders.Add("Authorization", token2);

                        var content2 = new MultipartFormDataContent();

                        content2.Add(new StringContent(text2), "content");

                        if (!string.IsNullOrEmpty(gifPath2) && File.Exists(gifPath2))
                        {
                            var imageContent2 = new ByteArrayContent(File.ReadAllBytes(gifPath2));
                            imageContent2.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
                            content2.Add(imageContent2, "file", Path.GetFileName(gifPath2));
                        }

                        var response2 = await client2.PostAsync($"https://discord.com/api/v9/channels/{channelID2}/messages", content2);
                        var responseString2 = await response2.Content.ReadAsStringAsync();

                        var status2 = responseString2.Contains("False");


                        if (status2)
                        {
                            LogMessage2($"Failed to send to {channelID2}");
                            SendLogToTelegram2($"Error occurred: {channelID2}");
                        }
                        else if (channelID2 == "713466163585351791")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'реклама'");
                        }
                        else if (channelID2 == "713466197458550866")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'транспорт'");
                        }
                        else if (channelID2 == "713466225220386846")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'недвижка'");
                        }
                        else if (channelID2 == "713466268560392222")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'бизнес'");
                        }
                        else if (channelID2 == "713466300860465192")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'оказание услуг'");
                        }
                        else if (channelID2 == "713466362202292285")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'барахолка'");
                        }
                        else if (channelID2 == "720689842593464400")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'муж.одежда'");
                        }
                        else if (channelID2 == "713466334343725106")
                        {
                            LogMessage2($"MSG sent to {channelID2} - 'черный рынок'");
                        }
                        else
                        {
                            LogMessage2($"MSG sent to {channelID2}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage2($"Error occurred: {ex.Message}");
                    SendLogToTelegram2($"Error occurred: {ex.Message}");
                }
                await Task.Delay(cooldownTime2);
            }

        }
        private async Task SendMessageAsync3()
        {
            var channelID3 = channelTextBox3.Text;
            var token3 = headerTextBox3.Text;
            var text3 = payloadTextBox3.Text;
            var cooldownTime3 = int.Parse(cooldownTextBox3.Text) * 1000;
            var gifPath3 = gifTextBox3.Text;

            while (!stopFlag3)
            {
                try
                {
                    using (var client3 = new HttpClient())
                    {
                        client3.DefaultRequestHeaders.Add("Authorization", token3);

                        var content3 = new MultipartFormDataContent();

                        content3.Add(new StringContent(text3), "content");

                        if (!string.IsNullOrEmpty(gifPath3) && File.Exists(gifPath3))
                        {
                            var imageContent3 = new ByteArrayContent(File.ReadAllBytes(gifPath3));
                            imageContent3.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
                            content3.Add(imageContent3, "file", Path.GetFileName(gifPath3));
                        }

                        var response3 = await client3.PostAsync($"https://discord.com/api/v9/channels/{channelID3}/messages", content3);
                        var responseString3 = await response3.Content.ReadAsStringAsync();

                        var status3 = responseString3.Contains("False");


                        if (status3)
                        {
                            LogMessage3($"Failed to send to {channelID3}");
                            SendLogToTelegram3($"Error occurred: {channelID3}");
                        }
                        else if (channelID3 == "713466163585351791")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'реклама'");
                        }
                        else if (channelID3 == "713466197458550866")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'транспорт'");
                        }
                        else if (channelID3 == "713466225220386846")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'недвижка'");
                        }
                        else if (channelID3 == "713466268560392222")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'бизнес'");
                        }
                        else if (channelID3 == "713466300860465192")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'оказание услуг'");
                        }
                        else if (channelID3 == "713466362202292285")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'барахолка'");
                        }
                        else if (channelID3 == "720689842593464400")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'муж.одежда'");
                        }
                        else if (channelID3 == "713466334343725106")
                        {
                            LogMessage3($"MSG sent to {channelID3} - 'черный рынок'");
                        }
                        else
                        {
                            LogMessage3($"MSG sent to {channelID3}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage3($"Error occurred: {ex.Message}");
                    SendLogToTelegram3($"Error occurred: {ex.Message}");
                }
                await Task.Delay(cooldownTime3);
            }

        }


        private void LogMessage(string message)
        {
            var currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logText = $"Bot#1 Date: {currentDateTime}  |  ID: {message}\r\n";
            logTextBox.Invoke(new Action(() =>
            {
                logTextBox.AppendText(logText);
                logTextBox.ScrollToCaret();
            }));
            if (debugCheckBox.Checked && !string.IsNullOrEmpty(telegramBotTokenTextBox.Text) && !string.IsNullOrEmpty(telegramChatIDTextBox.Text))
            {
                SendLogToTelegram(logText);
            }

        }
        private void LogMessage1(string message1)
        {
            var currentDateTime1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logText1 = $"Bot#2 Date: {currentDateTime1}  |  ID: {message1}\r\n";
            logTextBox1.Invoke(new Action(() =>
            {
                logTextBox1.AppendText(logText1);
                logTextBox1.ScrollToCaret();
            }));
            if (debugCheckBox1.Checked && !string.IsNullOrEmpty(telegramBotTokenTextBox1.Text) && !string.IsNullOrEmpty(telegramChatIDTextBox1.Text))
            {
                SendLogToTelegram1(logText1);
            }

        }
        private void LogMessage2(string message2)
        {
            var currentDateTime2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logText2 = $"Bot#3 Date: {currentDateTime2}  |  ID: {message2}\r\n";
            logTextBox2.Invoke(new Action(() =>
            {
                logTextBox2.AppendText(logText2);
                logTextBox2.ScrollToCaret();
            }));
            if (debugCheckBox2.Checked && !string.IsNullOrEmpty(telegramBotTokenTextBox2.Text) && !string.IsNullOrEmpty(telegramChatIDTextBox2.Text))
            {
                SendLogToTelegram2(logText2);
            }

        }
        private void LogMessage3(string message3)
        {
            var currentDateTime3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logText3 = $"Bot#4 Date: {currentDateTime3}  |  ID: {message3}\r\n";
            logTextBox2.Invoke(new Action(() =>
            {
                logTextBox3.AppendText(logText3);
                logTextBox3.ScrollToCaret();
            }));
            if (debugCheckBox3.Checked && !string.IsNullOrEmpty(telegramBotTokenTextBox3.Text) && !string.IsNullOrEmpty(telegramChatIDTextBox3.Text))
            {
                SendLogToTelegram3(logText3);
            }

        }

        private void LoadData()
        {
            try
            {
                if (File.Exists("data.json"))
                {
                    var data = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data.json"));
                    channelTextBox.Text = data.channel_id;
                    headerTextBox.Text = data.header;
                    cooldownTextBox.Text = data.cooldown;
                    payloadTextBox.Text = data.payload;
                    gifTextBox.Text = data.gif_path;
                    debugCheckBox.Checked = data.debugging_enabled;
                    telegramBotTokenTextBox.Text = data.telegram_bot_token;
                    telegramChatIDTextBox.Text = data.telegram_chat_id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadData1()
        {
            try
            {
                if (File.Exists("data1.json"))
                {
                    var data1 = JsonConvert.DeserializeObject<Config1>(File.ReadAllText("data1.json"));
                    channelTextBox1.Text = data1.channel_id1;
                    headerTextBox1.Text = data1.header1;
                    cooldownTextBox1.Text = data1.cooldown1;
                    payloadTextBox1.Text = data1.payload1;
                    gifTextBox1.Text = data1.gif_path1;
                    debugCheckBox1.Checked = data1.debugging_enabled1;
                    telegramBotTokenTextBox1.Text = data1.telegram_bot_token1;
                    telegramChatIDTextBox1.Text = data1.telegram_chat_id1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadData2()
        {
            try
            {
                if (File.Exists("data2.json"))
                {
                    var data2 = JsonConvert.DeserializeObject<Config2>(File.ReadAllText("data2.json"));
                    channelTextBox2.Text = data2.channel_id2;
                    headerTextBox2.Text = data2.header2;
                    cooldownTextBox2.Text = data2.cooldown2;
                    payloadTextBox2.Text = data2.payload2;
                    gifTextBox2.Text = data2.gif_path2;
                    debugCheckBox2.Checked = data2.debugging_enabled2;
                    telegramBotTokenTextBox2.Text = data2.telegram_bot_token2;
                    telegramChatIDTextBox2.Text = data2.telegram_chat_id2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadData3()
        {
            try
            {
                if (File.Exists("data3.json"))
                {
                    var data3 = JsonConvert.DeserializeObject<Config3>(File.ReadAllText("data3.json"));
                    channelTextBox3.Text = data3.channel_id3;
                    headerTextBox3.Text = data3.header3;
                    cooldownTextBox3.Text = data3.cooldown3;
                    payloadTextBox3.Text = data3.payload3;
                    gifTextBox3.Text = data3.gif_path3;
                    debugCheckBox3.Checked = data3.debugging_enabled3;
                    telegramBotTokenTextBox3.Text = data3.telegram_bot_token3;
                    telegramChatIDTextBox3.Text = data3.telegram_chat_id3;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SaveData()
        {
            try
            {
                var data = new Config
                {
                    channel_id = channelTextBox.Text,
                    header = headerTextBox.Text,
                    cooldown = cooldownTextBox.Text,
                    payload = payloadTextBox.Text,
                    gif_path = gifTextBox.Text,
                    debugging_enabled = debugCheckBox.Checked,
                    telegram_bot_token = telegramBotTokenTextBox.Text,
                    telegram_chat_id = telegramChatIDTextBox.Text
                };
                File.WriteAllText("data.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveData1()
        {
            try
            {
                var data1 = new Config1
                {
                    channel_id1 = channelTextBox1.Text,
                    header1 = headerTextBox1.Text,
                    cooldown1 = cooldownTextBox1.Text,
                    payload1 = payloadTextBox1.Text,
                    gif_path1 = gifTextBox1.Text,
                    debugging_enabled1 = debugCheckBox1.Checked,
                    telegram_bot_token1 = telegramBotTokenTextBox1.Text,
                    telegram_chat_id1 = telegramChatIDTextBox1.Text
                };
                File.WriteAllText("data1.json", JsonConvert.SerializeObject(data1, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveData2()
        {
            try
            {
                var data2 = new Config2
                {
                    channel_id2 = channelTextBox2.Text,
                    header2 = headerTextBox2.Text,
                    cooldown2 = cooldownTextBox2.Text,
                    payload2 = payloadTextBox2.Text,
                    gif_path2 = gifTextBox2.Text,
                    debugging_enabled2 = debugCheckBox2.Checked,
                    telegram_bot_token2 = telegramBotTokenTextBox2.Text,
                    telegram_chat_id2 = telegramChatIDTextBox2.Text
                };
                File.WriteAllText("data2.json", JsonConvert.SerializeObject(data2, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveData3()
        {
            try
            {
                var data3 = new Config3
                {
                    channel_id3 = channelTextBox3.Text,
                    header3 = headerTextBox3.Text,
                    cooldown3 = cooldownTextBox3.Text,
                    payload3 = payloadTextBox3.Text,
                    gif_path3 = gifTextBox3.Text,
                    debugging_enabled3 = debugCheckBox3.Checked,
                    telegram_bot_token3 = telegramBotTokenTextBox3.Text,
                    telegram_chat_id3 = telegramChatIDTextBox3.Text
                };
                File.WriteAllText("data3.json", JsonConvert.SerializeObject(data3, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckEmptyFields())
            {
                if (!stopFlag) // Если отправка не активна
                {
                    stopFlag = true; // Установка флага остановки
                    UpdateButtonAndSaveData("Start Sending"); // Обновить кнопку и сохранить данные
                }
                else // Если отправка активна
                {
                    stopFlag = false; // Сброс флага остановки
                    UpdateButtonAndSaveData("Stop Sending"); // Обновить кнопку и сохранить данные
                    _ = SendMessageAsync(); // Начать отправку сообщений
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (CheckEmptyFields1())
            {
                if (!stopFlag1) // Если отправка не активна
                {
                    stopFlag1 = true; // Установка флага остановки
                    UpdateButtonAndSaveData1("Start Sending"); // Обновить кнопку и сохранить данные

                }
                else // Если отправка активна
                {
                    stopFlag1 = false; // Сброс флага остановки
                    UpdateButtonAndSaveData1("Stop Sending"); // Обновить кнопку и сохранить данные
                    _ = SendMessageAsync1(); // Начать отправку сообщений
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (CheckEmptyFields2())
            {
                if (!stopFlag2) // Если отправка не активна
                {
                    stopFlag2 = true; // Установка флага остановки
                    UpdateButtonAndSaveData2("Start Sending"); // Обновить кнопку и сохранить данные
                }
                else // Если отправка активна
                {
                    stopFlag2 = false; // Сброс флага остановки
                    UpdateButtonAndSaveData2("Stop Sending"); // Обновить кнопку и сохранить данные
                    _ = SendMessageAsync2(); // Начать отправку сообщений
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (CheckEmptyFields3())
            {
                if (!stopFlag3) // Если отправка не активна
                {
                    stopFlag3 = true; // Установка флага остановки
                    UpdateButtonAndSaveData3("Start Sending"); // Обновить кнопку и сохранить данные
                }
                else // Если отправка активна
                {
                    stopFlag3 = false; // Сброс флага остановки
                    UpdateButtonAndSaveData3("Stop Sending"); // Обновить кнопку и сохранить данные
                    _ = SendMessageAsync3(); // Начать отправку сообщений
                }
            }
        }

        private void UpdateButtonAndSaveData(string buttonText)
        {
            button1.Text = buttonText;
            SaveData();
        }
        private void UpdateButtonAndSaveData1(string buttonText1)
        {
           button4.Text = buttonText1;
           SaveData1();
        }
        private void UpdateButtonAndSaveData2(string buttonText6)
        {
            button6.Text = buttonText6;
            SaveData2();
        }
        private void UpdateButtonAndSaveData3(string buttonText8)
        {
            button8.Text = buttonText8;
            SaveData3();
        }

        private bool CheckEmptyFields()
        {
            if (string.IsNullOrWhiteSpace(channelTextBox.Text) ||
                string.IsNullOrWhiteSpace(headerTextBox.Text) ||
                string.IsNullOrWhiteSpace(cooldownTextBox.Text) ||
                string.IsNullOrWhiteSpace(payloadTextBox.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckEmptyFields1()
        {
            if (string.IsNullOrWhiteSpace(channelTextBox1.Text) ||
                string.IsNullOrWhiteSpace(headerTextBox1.Text) ||
                string.IsNullOrWhiteSpace(cooldownTextBox1.Text) ||
                string.IsNullOrWhiteSpace(payloadTextBox1.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckEmptyFields2()
        {
            if (string.IsNullOrWhiteSpace(channelTextBox2.Text) ||
                string.IsNullOrWhiteSpace(headerTextBox2.Text) ||
                string.IsNullOrWhiteSpace(cooldownTextBox2.Text) ||
                string.IsNullOrWhiteSpace(payloadTextBox2.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckEmptyFields3()
        {
            if (string.IsNullOrWhiteSpace(channelTextBox3.Text) ||
                string.IsNullOrWhiteSpace(headerTextBox.Text) ||
                string.IsNullOrWhiteSpace(cooldownTextBox3.Text) ||
                string.IsNullOrWhiteSpace(payloadTextBox3.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.gif; *.jpg; *.png)|*.gif;*.jpg;*.png",
                Title = "Select an Image File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                gifTextBox.Text = selectedFilePath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Image Files (*.gif; *.jpg; *.png)|*.gif;*.jpg;*.png",
                Title = "Select an Image File"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath1 = openFileDialog1.FileName;
                gifTextBox1.Text = selectedFilePath1;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            var openFileDialog2 = new OpenFileDialog
            {
                Filter = "Image Files (*.gif; *.jpg; *.png)|*.gif;*.jpg;*.png",
                Title = "Select an Image File"
            };

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath2 = openFileDialog2.FileName;
                gifTextBox2.Text = selectedFilePath2;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            var openFileDialog3 = new OpenFileDialog
            {
                Filter = "Image Files (*.gif; *.jpg; *.png)|*.gif;*.jpg;*.png",
                Title = "Select an Image File"
            };

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath3 = openFileDialog3.FileName;
                gifTextBox3.Text = selectedFilePath3;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Discord Auto-Poster";
            notifyIcon1.BalloonTipText = "Приложение свёрнуто";
            notifyIcon1.Text = "Discord Auto-Poster";

            panel3.Visible = false;
            chart1.Visible = false;

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            { notifyIcon1.Visible = false; }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            PnlNav.Height = BtnDashboard.Height;
            PnlNav.Top = BtnDashboard.Top;
            PnlNav.Left = BtnDashboard.Left;
            BtnDashboard.BackColor = Color.FromArgb(46, 51, 73);

            tabControl1.Visible = true;
            panel3.Visible = false;
            chart1.Visible = false;

        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            PnlNav.Height = btnAnalytics.Height;
            PnlNav.Top = btnAnalytics.Top;
            btnAnalytics.BackColor = Color.FromArgb(46, 51, 73);

            tabControl1.Visible = false;
            panel3.Visible = true;
            chart1.Visible = true;

        }

        private void btrnsettings_Click(object sender, EventArgs e)
        {
            PnlNav.Height = btrnsettings.Height;
            PnlNav.Top = btrnsettings.Top;
            btrnsettings.BackColor = Color.FromArgb(46, 51, 73);


            tabControl1.Visible = false;


        }

        private void BtnDashboard_Leave(object sender, EventArgs e)
        {
            BtnDashboard.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void btnAnalytics_Leave(object sender, EventArgs e)
        {
            btnAnalytics.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void btrnsettings_Leave(object sender, EventArgs e)
        {
            btrnsettings.BackColor = Color.FromArgb(24, 30, 54);

        }

        private void label17_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        Point lastpoint;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastpoint = new Point(e.X, e.Y);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastpoint.X;
                this.Top += e.Y - lastpoint.Y;
            }
        }

        private void label17_MouseEnter(object sender, EventArgs e)
        {
            label17.ForeColor = Color.RoyalBlue;
        }

        private void label17_MouseLeave(object sender, EventArgs e)
        {
            label17.ForeColor = Color.Black;
        }

        private void label18_MouseEnter(object sender, EventArgs e)
        {
            label18.ForeColor = Color.RoyalBlue;

        }

        private void label18_MouseLeave(object sender, EventArgs e)
        {
            label18.ForeColor = Color.Black;

        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

       
    }
}
