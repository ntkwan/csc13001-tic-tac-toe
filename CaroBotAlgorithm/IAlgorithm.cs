// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CaroBotAlgorithm;

public interface IAlgorithm
{
    (int row, int col) GetMove(char[,] board, char computerSymbol);
}
