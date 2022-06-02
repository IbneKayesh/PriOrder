using Aio.Db.Client.Entrance;
using Aio.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageDownloader
{
    public partial class Form1 : Form
    {
        int downloaded = 0;
        string api_link = "http://sprodev.sihirfms.com/api/";
        string pic_er_folder = @"E:\ASP Net\PriOrder\PriOrder.App\Images\Products";
        string download_pic_er_folder = @"E:\ASP Net\PriOrder\PriOrder.App\Images\Products-Download";
        string static_image_link = @"https://images.sihirbox.com/bdp/Master/item/";
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            List<string> jegula_nai = new List<string>();
            List<string> jegula_ache = new List<string>();
            DirectoryInfo d = new DirectoryInfo(pic_er_folder);
            //FileInfo[] Files =d.GetFiles("*.jpg");
            FileInfo[] Files = d.GetFiles();
            foreach (FileInfo file in Files)
            {
                jegula_ache.Add(file.Name);
            }
            lblExistImages.Text = "Total Pic Paycee: " + jegula_ache.Count;

            string sql = @"select item_id from items where inactive='N' and ITEM_ID='923806'";
            EQResultTable objList = DatabaseOracleClient.GetDataTable(sql);
            if (objList.Result.ROWS > 0)
            {
                foreach (DataRow itemCode in objList.Table.Rows)
                {
                    bool pic_nai = jegula_ache.Any(x => itemCode["item_id"].ToString().Contains(x));
                    if (!pic_nai)
                    {
                        jegula_nai.Add(itemCode["item_id"].ToString());
                    }
                }
            }
            lblNotExistImages.Text = "Total Pic pai naai: " + jegula_nai.Count;
            Picture_download_er_API_call_koro(jegula_nai);



            //foreach (string item_pic_nai_code in jegula_nai)
            //{
            //    string imgLink = static_image_link + item_pic_nai_code + ".jpg";
            //    using (WebClient webClient = new WebClient())
            //    {
            //        if (IS_VALID_URL_IMAGE(imgLink))
            //        {
            //            webClient.DownloadFile(imgLink, download_pic_er_folder + "\\" + item_pic_nai_code + ".jpg");
            //            downloaded++;
            //        }
            //    }
            //}
            lblNewDownloadImages.Text = "Notun download hoyece: " + downloaded;
        }

        private bool IS_VALID_URL_IMAGE(string link)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(link);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }
            return exists;
        }


        private async void Picture_download_er_API_call_koro(List<string> _jegula_nai)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(api_link);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("ApiKey", "f06ff43be382");
            string requestUrl = "v2" + "/getItemPicture";
            
            foreach(string item_nai in _jegula_nai)
            {
                var objItem = new item_code_link { item_code = "79864" };
                string json = JsonConvert.SerializeObject(objItem);
                StringContent sqlListQuery = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUrl, sqlListQuery);
                var content = response.Content.ReadAsStringAsync();
                List<API_ITEM_LINK> item_Link = JsonConvert.DeserializeObject<List<API_ITEM_LINK>>(content.Result);
                Link_Theke_Picture_Download_Koro(item_Link);
            }
        }

        private void Link_Theke_Picture_Download_Koro(List<API_ITEM_LINK> item_Link)
        {
            foreach (API_ITEM_LINK item in item_Link)
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(item.item_picture, download_pic_er_folder + @"\79864.jpg");
                    //webClient.DownloadFileAsync(new Uri(item.item_picture), download_pic_er_folder);
                }
            }
        }
        //public void SaveImage(string imageUrl, string filename, ImageFormat format)
        //{
        //    WebClient client = new WebClient();
        //    Stream stream = client.OpenRead(imageUrl);
        //    Bitmap bitmap; bitmap = new Bitmap(stream);

        //    if (bitmap != null)
        //    {
        //        bitmap.Save(filename, format);
        //    }

        //    stream.Flush();
        //    stream.Close();
        //    client.Dispose();
        //}


    }
}
