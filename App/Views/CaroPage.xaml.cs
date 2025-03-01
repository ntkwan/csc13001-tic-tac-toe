using System;
using System.Threading.Tasks;
using CaroBotAlgorithm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace WinUI_Learn.Views
{
    public sealed partial class CaroPage : Page
    {
        private const int BoardSize = 3;
        private Button[,] buttons = new Button[BoardSize, BoardSize];
        private char[,] board = new char[BoardSize, BoardSize];
        private bool isPlayerXTurn = true;
        private bool isComputerOpponent = false; // Chế độ chơi với máy
        private IAlgorithm computerPlayer; // Plugin chiến lược chơi của máy

        public CaroPage()
        {
            this.InitializeComponent();
            CreateGameBoard();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Nếu có truyền plugin từ MainPage, gán và kích hoạt chế độ chơi với máy
            if (e.Parameter is IAlgorithm plugin)
            {
                computerPlayer = plugin;
                isComputerOpponent = true;
            }
        }

        private void CreateGameBoard()
        {
            GameBoard.Children.Clear();
            GameBoard.RowDefinitions.Clear();
            GameBoard.ColumnDefinitions.Clear();

            for (int i = 0; i < BoardSize; i++)
            {
                GameBoard.RowDefinitions.Add(new RowDefinition());
                GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    int r = row; // lưu lại giá trị cho lambda
                    int c = col; // lưu lại giá trị cho lambda

                    Button btn = new Button
                    {
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Background = new SolidColorBrush(Microsoft.UI.Colors.White)
                    };
                    btn.Click += (s, e) => Button_Click(s, e, r, c);

                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    GameBoard.Children.Add(btn);
                    buttons[row, col] = btn;
                    board[row, col] = ' ';
                }
            }
        }

        // Xử lý sự kiện click của người chơi
        private async void Button_Click(object sender, RoutedEventArgs e, int row, int col)
        {
            // Nếu ô đã được đánh hoặc nếu đến lượt máy (trong chế độ chơi với máy) thì không cho phép click
            if (board[row, col] != ' ' || (!isPlayerXTurn && isComputerOpponent))
                return;

            // Người chơi (giả sử luôn là X)
            MakeMove(row, col, 'X');
            if (CheckWin(row, col))
            {
                StatusText.Text = "Player X thắng!";
                DisableBoard();
                await ShowWinDialog("X");
                return;
            }

            if (IsBoardFull())
            {
                StatusText.Text = "Hòa nhau!";
                await ShowTieDialog();
                return;
            }

            // Chuyển lượt
            isPlayerXTurn = false;
            StatusText.Text = "Đến lượt máy (O)";

            // Nếu đang chơi với máy, gọi nước đi của máy
            if (isComputerOpponent)
            {
                await Task.Delay(500); // thêm độ trễ tùy chọn
                await ComputerMove();
            }
            else
            {
                // Nếu chơi 2 người thì chuyển lượt sang O
                isPlayerXTurn = true;
                StatusText.Text = "Player O's Turn";
            }
        }

        // Thực hiện nước đi của máy
        private async Task ComputerMove()
        {
            if (computerPlayer == null)
            {
                StatusText.Text = "Không load được plugin máy chơi!";
                return;
            }

            // Máy có ký hiệu O
            (int row, int col) move = computerPlayer.GetMove(board, 'O');
            if (move.row == -1 || move.col == -1)
                return;

            MakeMove(move.row, move.col, 'O');
            if (CheckWin(move.row, move.col))
            {
                StatusText.Text = "Máy thắng!";
                DisableBoard();
                await ShowWinDialog("O (Máy)");
                return;
            }

            if (IsBoardFull())
            {
                StatusText.Text = "Hòa nhau!";
                await ShowTieDialog();
                return;
            }

            // Sau lượt máy, chuyển lại lượt người chơi
            isPlayerXTurn = true;
            StatusText.Text = "Player X's Turn";
        }

        // Hàm thực hiện đánh vào ô trên bàn cờ
        private void MakeMove(int row, int col, char symbol)
        {
            board[row, col] = symbol;
            TextBlock tb = new TextBlock
            {
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = symbol.ToString(),
                Foreground = symbol == 'X' ? new SolidColorBrush(Microsoft.UI.Colors.Red) : new SolidColorBrush(Microsoft.UI.Colors.Green)
            };
            buttons[row, col].Content = tb;
            buttons[row, col].IsEnabled = false;
        }

        private bool CheckWin(int row, int col)
        {
            char current = board[row, col];
            return CheckDirection(row, col, 1, 0, current) || // Ngang
                   CheckDirection(row, col, 0, 1, current) || // Dọc
                   CheckDirection(row, col, 1, 1, current) || // Chéo (\)
                   CheckDirection(row, col, 1, -1, current);  // Chéo (/)
        }

        private bool CheckDirection(int row, int col, int dRow, int dCol, char player)
        {
            int count = 1;
            count += CountInDirection(row, col, dRow, dCol, player);
            count += CountInDirection(row, col, -dRow, -dCol, player);
            return count >= 3;
        }

        private int CountInDirection(int row, int col, int dRow, int dCol, char player)
        {
            int count = 0;
            for (int i = 1; i < 3; i++)
            {
                int r = row + i * dRow;
                int c = col + i * dCol;
                if (r < 0 || r >= BoardSize || c < 0 || c >= BoardSize || board[r, c] != player)
                    break;
                count++;
            }
            return count;
        }

        private bool IsBoardFull()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == ' ')
                        return false;
                }
            }
            return true;
        }

        private void DisableBoard()
        {
            foreach (var btn in buttons)
            {
                btn.IsEnabled = false;
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            isPlayerXTurn = true;
            StatusText.Text = "Player X's Turn";
            CreateGameBoard();
        }

        // Hiển thị dialog khi thắng cuộc
        private async Task ShowWinDialog(string winner)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Kết thúc trò chơi",
                Content = $"Player {winner} thắng! Bạn có muốn chơi lại không?",
                PrimaryButtonText = "Có",
                CloseButtonText = "Không"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                NewGame_Click(null, null);
            }
        }

        private async Task ShowTieDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Kết thúc trò chơi",
                Content = "Hòa nhau! Bạn có muốn chơi lại không?",
                PrimaryButtonText = "Có",
                CloseButtonText = "Không"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                NewGame_Click(null, null);
            }
        }
    }
}
