using Shop.Client.ViewModels;

namespace Shop.Client.Views;

public class ConsoleView
{
    private readonly OrderViewModel _viewModel;

    public ConsoleView(OrderViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Write("Podaj ilość do zamówienia (lub ENTER aby zakończyć): ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) break;

            if (int.TryParse(input, out int quantity))
            {
                await _viewModel.SubmitOrderAsync(quantity);
            }
            else
            {
                Console.WriteLine("❌ Niepoprawna liczba.");
            }
        }
    }
}
