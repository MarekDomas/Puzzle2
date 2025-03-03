using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;

namespace Puzzle2;

public partial class MainPage : ContentPage
{
    private static float sideWidth = 146 ;
    private static float sideHeight = 76 ;
    private static List<Image> puzzles = [];
    private static int gridWidth = 5;
    private static int gridHeight = 5;
    public MainPage()
    {
        InitializeComponent();
        initialazePuzzle();
    }
    
    void initialazePuzzle()
    {
        var n = 0;
        for (var i = 0; i < gridWidth; i++)
        {
            for (var j = 0; j < gridHeight; j++)
            {
                DragGestureRecognizer dragGestureRecognizer = new();
                DropGestureRecognizer dropGestureRecognizer = new();

                dropGestureRecognizer.Drop += OnDrop;

                var image = new Image()
                {
                    WidthRequest = sideWidth,
                    HeightRequest = sideHeight,
                    Source = $"image{n}.png"
                };

                image.GestureRecognizers.Add(dragGestureRecognizer);
                image.GestureRecognizers.Add(dropGestureRecognizer);

                puzzles.Add(image);
                MainLayout.Children.Add(image);

                AbsoluteLayout.SetLayoutBounds(image, new (i * sideWidth, j * sideHeight, sideWidth, sideHeight));

                n++;
            }
        }

    }

    private async void OnDrop(object sender, DropEventArgs e)
    {
        var source = await e.Data.GetImageAsync();
        var dropRecognizer = sender as DropGestureRecognizer;
        var dropImage = dropRecognizer.Parent as Image;

        foreach (var puzzle in puzzles)
        {
            if (puzzle.Source == source)
            {
                puzzle.Source = dropImage.Source;
                dropImage.Source = source;
            }
        }
    }
}