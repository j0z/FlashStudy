using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FlashStudy
{
    public sealed partial class MainPage : Page
    {
        List<Card> cards = new List<Card>();

        int currentCard = 1;

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        //int totalCards = 0;

        async void WriteCards()
        {
            StorageFile cardsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("cards", CreationCollisionOption.ReplaceExisting);

            string json = JsonConvert.SerializeObject(cards);

            await FileIO.WriteTextAsync(cardsFile, json);


        }

        async void ReadCards()
        {
            try
            {
                StorageFile cardsFile = await localFolder.GetFileAsync("cards");
                String json = await FileIO.ReadTextAsync(cardsFile);
               
                cards = JsonConvert.DeserializeObject<List<Card>>(json);

                //totalCards = cards.Count();
                cardCounter();
            }
            catch (Exception)
            {
            }
        }


        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ReadCards();

                if (cards.Count() == 0)
                {
                    object sender = new object();
                    RoutedEventArgs x = new RoutedEventArgs();

                    enterAddMode(sender, x);
                }
            }
            catch
            {
            }
        }

        private void cardCounter()
        {
            txtBlck_cardCount.Text = "Total: " + currentCard + "/" + cards.Count;
        }
        private void enterAddMode(object sender, RoutedEventArgs e)
        {
            btn_addCard.Visibility = Visibility.Visible;
            txtBox_cardTextEntry.Visibility = Visibility.Visible;
            txtBox_cardTitle.Visibility = Visibility.Visible;
            btn_doneAdding.Visibility = Visibility.Visible;

            btn_flipCard.Visibility = Visibility.Collapsed;
            txtBlck_cardTitle.Visibility = Visibility.Collapsed;
            txtBlck_cardBody.Visibility = Visibility.Collapsed;
            btn_backCard.Visibility = Visibility.Collapsed;
            btn_nextCard.Visibility = Visibility.Collapsed;

            txtBox_cardTitle.Focus(FocusState.Pointer); 
        }

        private void addCard(object sender, RoutedEventArgs e)
        {
            Card card = new Card();

            card.sideA = txtBox_cardTitle.Text;
            card.sideB = txtBox_cardTextEntry.Text;

            cards.Add(card);

            txtBox_cardTitle.Text = "Enter Term";
            txtBox_cardTextEntry.Text = "Enter Defintion Here";
            txtBox_cardTitle.Focus(FocusState.Pointer);

            //totalCards++;
            cardCounter();
        }

        private void removeCard(object sender, RoutedEventArgs e)
        {
            if (cards.Count > 1)
            {
                cards.RemoveAt(currentCard - 1);
                previousCard(sender, e);

                cardCounter();

                WriteCards();
            }
            if (cards.Count == 1)
            {
                cards.RemoveAt(currentCard - 1);
                cardCounter();
                WriteCards();

                enterAddMode(sender, e);
            }
        }

        private void nextCard(object sender, RoutedEventArgs e)
        {
            if (currentCard < cards.Count && currentCard != 0)
            {
                currentCard++;

                txtBlck_cardBody.Visibility = Visibility.Collapsed;
                btn_redoCard.Visibility = Visibility.Collapsed;
                txtBlck_cardTitle.Text = cards.ElementAt(currentCard - 1).sideA;
                txtBlck_cardBody.Text = cards.ElementAt(currentCard - 1).sideB;
                cardCounter();
            }
            if (currentCard == 0)
                currentCard = 1;
        }
        

        private void previousCard(object sender, RoutedEventArgs e)
        {
            if (currentCard > 1)
            {
                currentCard--;

                txtBlck_cardBody.Visibility = Visibility.Collapsed;
                btn_redoCard.Visibility = Visibility.Collapsed;
                txtBlck_cardTitle.Text = cards.ElementAt(currentCard - 1).sideA;
                txtBlck_cardBody.Text = cards.ElementAt(currentCard - 1).sideB;
                cardCounter();
            }
        }

        private void exitAddMode(object sender, RoutedEventArgs e)
        {
            btn_addCard.Visibility = Visibility.Collapsed;
            txtBox_cardTextEntry.Visibility = Visibility.Collapsed;
            txtBox_cardTitle.Visibility = Visibility.Collapsed;
            btn_doneAdding.Visibility = Visibility.Collapsed;
            txtBlck_cardBody.Visibility = Visibility.Collapsed;

            btn_flipCard.Visibility = Visibility.Visible;
            txtBlck_cardTitle.Visibility = Visibility.Visible;
            btn_nextCard.Visibility = Visibility.Visible;
            btn_backCard.Visibility = Visibility.Visible;

            currentCard = 1;
            WriteCards();
        }

        private void flipCard(object sender, RoutedEventArgs e)
        {
            txtBlck_cardBody.Visibility = Visibility.Visible;
            btn_redoCard.Visibility = Visibility.Visible;
        }

        private void redoCard(object sender, RoutedEventArgs e)
        {
            Card card = cards.ElementAt(currentCard - 1);
            cards.Add(card);
            cardCounter();
        }

        private void tapTitleEdit(object sender, RoutedEventArgs e)
        {
            txtBox_cardTitle.SelectionStart = 0;
            txtBox_cardTitle.SelectionLength = txtBox_cardTitle.Text.Length;
        }

        private void tapTextEntry(object sender, RoutedEventArgs e)
        {
            txtBox_cardTextEntry.SelectionStart = 0;
            txtBox_cardTextEntry.SelectionLength = txtBox_cardTextEntry.Text.Length;
        }
    }
}
