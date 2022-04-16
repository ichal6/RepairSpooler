using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace RepairSpooler
{
    class Program_Główny
    {
        public class WindowsServiceController
        {
            private string NazwaUsługi;

            

            private TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

            public WindowsServiceController(string NazwaUsługi)
            {
                this.NazwaUsługi = NazwaUsługi;
            }

            
            // Ta metoda wychwytuje wyjątek, jeśli usługa nie jest włączona. Stosowanie niezalecane!
            public bool RestartUsługi()
            {
                using (ServiceController Usługa = new ServiceController(NazwaUsługi))
                {
                    try
                    {
                        

                        Usługa.Stop();

                        Usługa.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                        Usługa.Start();
                        Usługa.WaitForStatus(ServiceControllerStatus.Running, timeout);
                        
                        //MessageBox.Show("Reset zakończony sukcesem");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Environment.Exit(0);
                        return false;
                        //throw new Exception("Nie można ponownie uruchomić usługi: {serviceName}", ex);
                    }
                }
            }

            // Ta metoda wychwytuje wyjątek, jeśli usługa nie jest włączona. Stosowanie niezalecane!
            public void ZatrzymajUsługę()
            {
                using (ServiceController Usługa = new ServiceController(NazwaUsługi))
                {
                    try
                    {
                        Usługa.Stop();
                        Usługa.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                        MessageBox.Show("Zatrzymywanie usługi zakończone powodzeniem.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Environment.Exit(0);
                     //   throw new Exception("Can not Stop the Windows Service [{serviceName}]", ex);
                    }
                }
            }

            // Ta metoda wychwytuje wyjątek, jeśli usługa nie jest zatrzymana. Stosowanie niezalecane!
            public bool UruchomUsługę()
            {
                using (ServiceController Usługa = new ServiceController(NazwaUsługi))
                {
                    try
                    {
                        Usługa.Start();
                        Usługa.WaitForStatus(ServiceControllerStatus.Running, timeout);
                        return true;
                      //  MessageBox.Show("Uruchamianie usługi zakończone powodzeniem.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Environment.Exit(0);
                        return false;
                      //  throw new Exception("Can not Start the Windows Service [{serviceName}]", ex);
                    }
                }
            }

            // Ta metoda restartuje usługę, jeśli jest uruchomiona, lub uruchamia ją, jeśli jest wyłączona. Stosowanie zalecane!
            public bool WłączlubWyłącz()
            {
                if (CzyUsługaJestUruchomiona())
                    { 
                        RestartUsługi();
                        return true;
                    }
                else if (CzyUsługaJestZatrzymana())
                    {
                        UruchomUsługę();
                        return true;
                    }
                return false;
            }

            // Ta metoda zatrzymuje usługę, jeśli jest włączona. Jeśli usługa jest już wyłączona nie robi nic. 
            // Ta metoda zwraca wyjątki jeśli nie uda się zatrzymać działającej usługi. Stosowanie niezalecane!
            public bool ZatrzymajUsługęJeśliWłączona()
            {
                using (ServiceController Usługa = new ServiceController(NazwaUsługi))
                {
                    try
                    {
                        if (!CzyUsługaJestUruchomiona())
                        {
                            MessageBox.Show("Usługa już jest zatrzymana.");
                            return false;
                        }
                        Usługa.Stop();
                        Usługa.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                        //MessageBox.Show("Zatrzymywanie usługi zakończone powodzeniem.");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        Environment.Exit(0);
                        return false;
                      //  throw new Exception("Can not Stop the Windows Service [{serviceName}]", ex);
                    }
                    
                }
            }
            // Sprawdza czy usługa jest uruchomiona
            public bool CzyUsługaJestUruchomiona()
            {
                if (Status == ServiceControllerStatus.Running)
                {

            
                    return true;
                }
                else
                {
           
                    return false;
                }
            }
            
            // Sprawdza czy usługa jest zatrzymana
            public bool CzyUsługaJestZatrzymana()
            {
                if (Status == ServiceControllerStatus.Stopped)
                {
                    return true;
                }
                else
                {   
                    return false;
                }
            }

            // Wprowadza Status usługi
            public ServiceControllerStatus Status
            {
                get
                {
                    using (ServiceController service = new ServiceController(NazwaUsługi))
                    {
                        return service.Status;
                    }
                }
            }
        }

        public class UsuńKatalogDrukowania
        {
            
            
            public UsuńKatalogDrukowania()
            {
               
            }
            public void UsuwanieKatalogu()
            {
                try
                {
                    Process.Start("cmd.exe", "/c " + @"del /q /f /s %systemroot%\system32\spool\PRINTERS\*.*");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }
            }
        }
    }
}
