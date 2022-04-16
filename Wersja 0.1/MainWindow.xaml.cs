using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Forms;

namespace Repair_Spooler
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TrayClass trayClass;

        private RepairSpooler.Program_Główny.WindowsServiceController p1 = null;

        private RepairSpooler.Program_Główny.UsuńKatalogDrukowania k1 = null;

        public MainWindow()
        {
            InitializeComponent();
            
            PrzygotujWiązanie();

            Closing += PrzyZamykaniuAplikacji;

            StateChanged += ZmianaRozmiaruOkna;

            trayClass = new TrayClass(this, "Repair Spooler 1.0");
        }

        private void PrzygotujWiązanie()
        {
            p1 = new RepairSpooler.Program_Główny.WindowsServiceController("Spooler");

            k1 = new RepairSpooler.Program_Główny.UsuńKatalogDrukowania();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            
            if(p1.WłączlubWyłącz())
            {
                trayClass.ShowTrayInformation("RepairSpooler", "Reset usługi zakończony powodzeniem. ");
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if(p1.ZatrzymajUsługęJeśliWłączona())
            {
                trayClass.ShowTrayInformation("RepairSpooler", "Zatrzymywanie usługi zakończone powodzeniem. ");
            }

        }

        private void Wyczyść_Click(object sender, RoutedEventArgs e)
        {

            k1.UsuwanieKatalogu();
        }

        private void AutoNaprawa_Click(object sender, RoutedEventArgs e)
        {
            p1.ZatrzymajUsługęJeśliWłączona();
            k1.UsuwanieKatalogu();
            if(p1.UruchomUsługę())
            {
                trayClass.ShowTrayInformation("RepairSpooler", "Autonaprawa usługi zakończona powodzeniem. ");
            }
        }

        private void PrzyZamykaniuAplikacji(object sender, CancelEventArgs e)
        {
            trayClass.ZniszczTray();
            
        }

        private void ZmianaRozmiaruOkna(object sender, EventArgs e)
        {

            
            switch (this.WindowState)
            {                
                case WindowState.Minimized:
                    Hide();
                    break;               
            }
        }
    }
}
