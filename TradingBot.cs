using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace SimpleMovingAverageCrossStrategy
{
    // Definição da classe OrderType
    public enum OrderType
    {
        Buy,
        Sell
    }

    // Definição da classe ExchangeClient
    public class ExchangeClient
    {
        private string apiKey;
        private string apiSecret;
        private HttpClient httpClient;

        // URL da API do Mercado Bitcoin para obter o preço atual do Bitcoin em reais (BRL)
        private const string MarketDataUrl = "https://www.mercadobitcoin.net/api/PEPE/ticker/";

        // Construtor
        public ExchangeClient(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
            this.httpClient = new HttpClient();
        }

        // Método para obter o preço atual de um ativo
        public async Task<decimal> GetPriceAsync(string symbol)
        {
            try
            {
                // Faz uma solicitação HTTP GET para obter os dados do mercado
                HttpResponseMessage response = await httpClient.GetAsync(MarketDataUrl);

                // Verifica se a solicitação foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Lê o conteúdo da resposta como uma string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Converte a string JSON em um objeto dynamic
                    dynamic marketData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                    // Obtém o preço de compra do Bitcoin em reais (BRL)
                    decimal price = marketData.ticker.last;

                    return price;
                }
                else
                {
                    Console.WriteLine("Erro ao obter preço do ativo. Código de status: " + response.StatusCode);
                    return 0; // Retorna 0 em caso de erro
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter preço do ativo: " + ex.Message);
                return 0; // Retorna 0 em caso de exceção
            }
        }

        // Método para emitir uma ordem de compra ou venda
        public void PlaceOrder(string symbol, OrderType orderType, decimal quantity)
        {
            // Implementação para enviar uma ordem para a exchange ou API
            if (orderType == OrderType.Buy)
            {
                Console.WriteLine($"Ordem de COMPRA emitida para o ativo: {symbol}, Quantidade: {quantity}");
            }
            else if (orderType == OrderType.Sell)
            {
                Console.WriteLine($"Ordem de VENDA emitida para o ativo: {symbol}, Quantidade: {quantity}");
            }
        }

        // Método para emitir uma ordem de stop loss
        public void PlaceStopLossOrder(string symbol, decimal stopPrice)
        {
            // Implementação para enviar uma ordem de stop loss para a exchange ou API
            Console.WriteLine("Ordem de STOP LOSS emitida para o ativo: " + symbol + ", Preço: " + stopPrice);
        }

        // Método para emitir uma ordem de take profit
        public void PlaceTakeProfitOrder(string symbol, decimal takeProfitPrice)
        {
            // Implementação para enviar uma ordem de take profit para a exchange ou API
            Console.WriteLine("Ordem de TAKE PROFIT emitida para o ativo: " + symbol + ", Preço: " + takeProfitPrice);
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Defina as suas chaves de API e outros parâmetros aqui
            string apiKey = "96a0a33dc8cf63826285f8ba958bbf4e375bf5b4548fcda58eb36ed90a3a6e9d";
            string apiSecret = "fc9aec6fbebee31bafbb5aba11698952fb737b27908f722a6e67989a558f48e2";
            string symbol = "FLOKIBRL"; // Símbolo da criptomoeda que deseja negociar
            decimal quantity = 0.01m; // Quantidade de criptomoeda que deseja comprar/vender

            // Inicializa a conexão com a exchange ou API da sua escolha
            ExchangeClient client = new ExchangeClient(apiKey, apiSecret);

            // Mantém o robô em execução infinita
            while (true)
            {
                // Gera um preço aleatório para a ordem de compra (apenas para ilustrar)
                decimal price = await GenerateRandomPriceAsync();
                Console.WriteLine($"Preço atual da criptomoeda: {price}");

                // Emite uma ordem de compra com o preço aleatório
                client.PlaceOrder(symbol, OrderType.Buy, quantity);
                Console.WriteLine($"Ordem de compra emitida para {symbol}.");

                // Calcula o preço de stop loss e take profit
                decimal stopLossPrice = price * 0.99m; // Stop loss de 1% abaixo do preço atual
                decimal takeProfitPrice = price * 1.03m; // Take profit de 3% acima do preço atual
                Console.WriteLine($"Preço de stop loss: {stopLossPrice}");
                Console.WriteLine($"Preço de take profit: {takeProfitPrice}");

                // Emite a ordem de stop loss
                client.PlaceStopLossOrder(symbol, stopLossPrice);
                Console.WriteLine($"Ordem de stop loss emitida para {symbol}.");

                // Emite a ordem de take profit
                client.PlaceTakeProfitOrder(symbol, takeProfitPrice);
                Console.WriteLine($"Ordem de take profit emitida para {symbol}.");

                // Exibe informações sobre a ordem emitida
                Console.WriteLine($"Ordem de compra emitida para {symbol}.");
                Console.WriteLine($"Quantidade: {quantity}");
                Console.WriteLine($"Preço atual: {price}");
                Console.WriteLine($"Stop Loss: {stopLossPrice}");
                Console.WriteLine($"Take Profit: {takeProfitPrice}");

                // Aguarda um intervalo de tempo antes de verificar novamente
                Thread.Sleep(60000); // 1 minuto
            }
        }

        // Método para gerar um preço aleatório apenas para ilustrar
        static async Task<decimal> GenerateRandomPriceAsync()
        {
            Random rand = new Random();
            decimal minPrice = 50000m; // Preço mínimo fictício
            decimal maxPrice = 60000m; // Preço máximo fictício

            // Simula um atraso de 1 segundo (apenas para ilustração)
            await Task.Delay(1000);

            return minPrice + (decimal)rand.NextDouble() * (maxPrice - minPrice);
        }
    }
}
