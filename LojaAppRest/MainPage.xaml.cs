using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LojaAppRest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string ip = "http://localhost:49791/";
        public MainPage()
        {
            this.InitializeComponent();
            setFab();
            PopularVD();
        }
        private async void PopularVD()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/veiculo/");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Veiculo> obj = JsonConvert.DeserializeObject<List<Models.Veiculo>>(str);
            LVdisponiveis.ItemsSource = obj.Where(v => v.SituVenda == false).ToList();
            LVvendidos.ItemsSource = obj.Where(v => v.SituVenda == true).ToList();
        }
        private async void setFab()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/fabricante");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Fabricante> obj = JsonConvert.
            DeserializeObject<List<Models.Fabricante>>(str);
            cbFabricante.ItemsSource = null;
            cbFabricante.ItemsSource = obj.OrderBy(f => f.Descricao);
            cbFabricante.DisplayMemberPath = "Descricao";
            cbFabricante.SelectedValuePath = "Id";
        }
        private async void btnInserirFAB_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Fabricante f = new Models.Fabricante
            {

                Id = int.Parse(tbID.Text),
                Descricao = tbDescricao.Text
            };
            List<Models.Fabricante> fl = new List<Models.Fabricante>();
            fl.Add(f);
            string s = "=" + JsonConvert.SerializeObject(fl);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PostAsync("/API/FabPost", content);
            setFab();
        }

        private async void btnListFAB_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/fabricante");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Fabricante> obj = JsonConvert.
            DeserializeObject<List<Models.Fabricante>>(str);
            ListBoxFabricantes.ItemsSource = obj.ToList();
           
        }

        private async void btInsert_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Veiculo f = new Models.Veiculo
            {

                Id = int.Parse(tbIDv.Text),
                Modelo = tbModelo.Text,
                Preco = tbPreco.Text,
                Ano = tbAno.Text,
                IdFabricante = Convert.ToInt32(cbFabricante.SelectedValue.ToString()),
                SituVenda = false
            };
            List<Models.Veiculo> fl = new List<Models.Veiculo>();
            fl.Add(f);
            string s = "=" + JsonConvert.SerializeObject(fl);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PostAsync("API/VeicPost", content);
        }

        private async void btnListV_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            var response = await httpClient.GetAsync("/api/veiculo");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Veiculo> obj = JsonConvert.DeserializeObject<List<Models.Veiculo>>(str);
            lvVeiculo.ItemsSource = obj.ToList();
        }

        private async void BtnVender_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            Models.Veiculo f = (Models.Veiculo)LVdisponiveis.SelectedItem;
            Models.Veiculo ff = new Models.Veiculo
            {
                Id = f.Id,
                Modelo = f.Modelo,
                Ano = f.Ano,
                Preco = f.Preco,
                IdFabricante = f.IdFabricante,
                SituVenda = true
            };
            string s = "=" + JsonConvert.SerializeObject(f);
            var content = new StringContent(s, Encoding.UTF8, "application/x-www-form-urlencoded");
            await httpClient.PutAsync("/api/veiculo/" + f.Id, content);
        }

        
    }
}
