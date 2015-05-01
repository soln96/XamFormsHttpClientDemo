using System;
using Xamarin.Forms;
using System.Diagnostics;
using ModernHttpClient;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TesteDeLogin
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					Padding = new Thickness(30,30),
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						},
						new Button {
							BackgroundColor = Color.Green,
							Text = "Login with Nuvem",
							TextColor = Color.White,
							Command = new Command (LoginNuvem)
						}
					}
				}
			};


		}

		public class Veiculo {
			public int IdVeiculo { get; set;	}
			public int IdConta { get; set; }
			public string ContaNome { get; set; }
			public string Nome { get; set; }
			public string Placa { get; set; }
			public string Latitude { get; set; }
			public string Longitude { get; set; }
		}
			
		public class Rootobject {
			public Veiculo[] veiculos { get; set; }
		}

		protected async void LoginNuvem()
		{
			var cookies = new CookieContainer ();
			var handler = new HttpClientHandler ();
			handler.CookieContainer = cookies;

			var client = new HttpClient (handler);
			var uri = new Uri ("http://nuvem.rastreamentoweb.com/mobile/login");

			var values = new Dictionary<string, string>();

			values.Add ("username", "xxxx");
			values.Add ("password", "xxxx");
			values.Add ("charset", Encoding.UTF8.ToString());
			values.Add ("Content-Type", "application/x-www-form-urlencoded");

			var content = new FormUrlEncodedContent (values);
			var response = await client.PostAsync (uri, content);

			if (response.IsSuccessStatusCode) {	

				//
				// Montando o cookie para a próxima chamada
				//

				IEnumerable<Cookie> responseCookies = cookies.GetCookies (uri).Cast<Cookie> ();
				foreach (Cookie cookie in responseCookies) {
					await MainPage.DisplayAlert (cookie.Name, cookie.Value, "Sim");
				}

				uri = new Uri ("http://nuvem.rastreamentoweb.com/mobile/veiculos");

				var response2 = await client.GetAsync (uri);

				if (response2.IsSuccessStatusCode) {

//					var veiculos = new List<Veiculo> ();
//
//					veiculos.Add (new Veiculo () { 
//						IdConta = 1,
//						ContaNome = "Marcelo Amorim",
//						IdVeiculo = 1,
//						Nome = "Teste de Serialização",
//						Placa = "XYZ 2345",
//						Latitude = "22.8987872",
//						Longitude = "42.928394"
//					});
//
//					veiculos.Add (new Veiculo () { 
//						IdConta = 2,
//						ContaNome = "Daniel Amorim",
//						IdVeiculo = 1,
//						Nome = "Teste de Serialização",
//						Placa = "KKK 1234",
//						Latitude = "-22.8987872",
//						Longitude = "-42.928394"
//					});
//
//					string output = Newtonsoft.Json.JsonConvert.SerializeObject (veiculos);

					var veiculos = response2.Content.ReadAsStringAsync().Result;
					var resposta = JsonConvert.DeserializeObject<List<Veiculo>> (veiculos);

					await MainPage.DisplayAlert ("GetAsync veiculos", String.Format("Teste com sucesso para {0} veículos.", resposta.Count), "Ok");
				}
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

