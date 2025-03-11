using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;

namespace Puzzle2;

public partial class MainPage : ContentPage
{
    private static float sideWidth = 146;
    private static float sideHeight = 76;
    private static List<Image> puzzles = [];
    private static int gridWidth = 5;
    private static int gridHeight = 5;
    public MainPage()
    {
        InitializeComponent();
        InitializePuzzle(null,null);
    }
    
    void InitializePuzzle(object sender, EventArgs e)
    {
        MainLayout.Children.Clear();
        puzzles.Clear();

        //Pokud nechcete aby se obrázky generovaly náhodně, můžete použít proměnnou n která se bude zvyšovat v každé iteraci
        Random random = new();

        //Vygeneruje list od 0 do 24
        var numbers = Enumerable.Range(0, gridHeight* gridWidth).ToList();

        for (var i = 0; i < gridHeight; i++) //Řádky
        {
            for (var j = 0; j < gridWidth; j++) //Sloupce
            {
                DragGestureRecognizer dragGestureRecognizer = new();
                DropGestureRecognizer dropGestureRecognizer = new();

                dropGestureRecognizer.Drop += OnDrop;
                
                var n = random.Next(numbers.Count);
                
                var image = new Image
                {
                    WidthRequest = sideWidth,
                    HeightRequest = sideHeight,
                    Source = $"image{numbers[n]}.png",
                    GestureRecognizers =
                    {
                        dragGestureRecognizer, dropGestureRecognizer
                    }
                };
                numbers.RemoveAt(n);

                puzzles.Add(image);
                MainLayout.Children.Add(image);

                AbsoluteLayout.SetLayoutBounds(
                    image, new (i * sideWidth, j * sideHeight,
                    sideWidth, sideHeight));

            }
        }
    }

    private async void OnDrop(object sender, DropEventArgs e)
    {
        //Zdroj taženého obrázku
        var source = await e.Data.GetImageAsync();
        
        var dropRecognizer = sender as DropGestureRecognizer;
        //Cílený obrázek
        var goalImage = dropRecognizer.Parent as Image;

        foreach (var puzzle in puzzles)
        {
            if (puzzle.Source == source)
            {
                puzzle.Source = goalImage.Source;
                goalImage.Source = source;
            }
        }
    }

    //    File: image0.png 0
    //    File: image5.png 1
    //    File: image10.png 2
    //    File: image15.png 3
    //    File: image20.png 4
    //    File: image1.png 5
    //    File: image6.png 6
    //    File: image11.png 7
    //    File: image16.png 8
    //    File: image21.png 9
    //    File: image2.png 10
    //    File: image7.png 11
    //    File: image12.png 12
    //    File: image17.png 13
    //    File: image22.png 14
    //    File: image3.png 15
    //    File: image8.png 16
    //    File: image13.png 17
    //    File: image18.png 18
    //    File: image23.png 19
    //    File: image4.png 20
    //    File: image9.png 21
    //    File: image14.png 22
    //    File: image19.png 23
    //    File: image24.png 24 

    private void ControlBtn_OnClicked(object? sender, EventArgs e)
    {
        var column = -1;
        var offset = 5;

        for (var i = 0; i < puzzles.Count; i++)
        {
            var puzzle = puzzles[i];

            if (i % 5 == 0) //Odehrává se vždy při novém sloupci
            {
                column++;
                offset = 5;

                if (!puzzle.Source.ToString().Contains($"image{column}.png"))
                {
                    DisplayAlert("Špatně", "Puzzle nejsou správně seřazené", "Ok");
                    return;
                }
                continue;
            }

            if (!puzzle.Source.ToString().Contains($"image{column + offset}.png"))
            {
                DisplayAlert("Špatně", "Puzzle nejsou správně seřazené", "Ok");
                return;
            }
            offset += 5;
        }

        DisplayAlert("Správně", "Puzzle jsou správně seřazené", "Ok");
    }
}