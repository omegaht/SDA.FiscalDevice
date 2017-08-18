using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SkiData.FiscalDevices;
using TfhkaNet.IF.VE;
using TfhkaNet.IF;


namespace SDA.FiscalDevice
{
    public class FiscalDevice : IFiscalDevice
    {

        #region Members
        public Tfhka Bixolon = new Tfhka();
        public PrinterStatus PStatus;
        /// <summary>
        /// Raises the S5 status of the Fiscal Printer to the PC and updates its data
        /// </summary>
        public S5PrinterData MyS5PrinterData;
        public string comPort;
        public DateTime dateLastZ;
        #endregion
        public FiscalDeviceCapabilities Capabilities
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public StateInfo DeviceState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string ShortName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<EventArgs> DeviceStateChanged;
        public event EventHandler<ErrorClearedEventArgs> ErrorCleared;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;
        public event EventHandler<IrregularityDetectedEventArgs> IrregularityDetected;
        public event EventHandler<JournalizeEventArgs> Journalize;
        public event EventHandler<TraceEventArgs> Trace;

        public Result AddDiscount(Discount discount)
        {
            throw new NotImplementedException();
        }

        public Result AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        public Result AddPayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Result CloseTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Result EndOfDay()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// first method called from the SKIDATA application after a fiscal device instance has been created. It starts the initialization of the fiscal device. Depending on the result, this method will be called at least once. A return value of trueTruetrue signals that the fiscal device was detected, connected and initialized properly. 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Result Install(FiscalDeviceConfiguration configuration)
        {
            WriteToTrace("#####     Method :: Install    ####");
            comPort = configuration.CommunicationChannel;
            if (Bixolon.CheckFPrinter() == false)
            {
                if (Reinstall())
                {
                    if (DeviceAvaileableMemory())
                    {
                        EmitU0ZReport();
                        return new Result(true);
                    }
                    else
                        return new Result(false);
                }
                else
                    return new Result(false);
            }
            return new Result(false);
        }

        public void Notify(int notificationId)
        {
            WriteToTrace("####      Method :: Notify      ####");
            switch (notificationId)
            {
                // Close notification Dialog
                case 1:
                    // if service dialog activated, close it.
                    // TODO service dialog
                    break; 
                default:
                    // Exit method.
                    break;
            }
            throw new NotImplementedException();
        }

        public Result OpenTransaction(TransactionData transactionData)
        {
            throw new NotImplementedException();
        }

        public void SetDisplayLanguage(CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }

        public void StartServiceDialog(IntPtr windowHandle, ServiceLevel serviceLevel)
        {
            throw new NotImplementedException();
        }

        public Result VoidTransaction()
        {
            throw new NotImplementedException();
        }

        #region SDA Methods
        /// <summary>
        /// Allows writing on the trace log.
        /// </summary>
        /// <param name="message"></param>
        public void WriteToTrace(string message)
        {
            TraceEventArgs traceArgs = new TraceEventArgs(message, System.Diagnostics.TraceLevel.Info);

            if (this.Trace != null)
            {
                this.Trace(this, traceArgs);
            }
        }
        /// <summary>
        /// Verifies the Bixolon printer availeable memory, returning true/false depending on it.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeviceAvaileableMemory()
        {
            WriteToTrace("####  Method :: DeviceAvaileableMemory    ####");
            try
            {
                MyS5PrinterData = Bixolon.GetS5PrinterData();
                if (MyS5PrinterData.AuditMemoryFreeCapacity > 0)
                {
                    WriteToTrace("DeviceAvaileableMemory :: Availeable " + MyS5PrinterData.AuditMemoryFreeCapacity.ToString() + " MB");
                    return true;
                }
                else
                {
                    WriteToTrace("DeviceAvaileableMemory :: Insufficient");
                    return false;
                }
            }
            catch (PrinterException e)
            {
                WriteToTrace("DeviceAvaileableMemory :: PrinterException" + e.Message);
                return false;
            }
        }
        /// <summary>
        /// Reinstalls the device.
        /// </summary>
        /// <returns></returns>
        public Boolean Reinstall()
        {
            WriteToTrace("####      Method :: Reinstall     ####");
            // Close com port
            Bixolon.CloseFpCtrl();
            // Open com port
            if (Bixolon.OpenFpCtrl(comPort))
            {
                WriteToTrace("Reinstall :: Com port " + comPort + " reopened");
                return true;
            }
            else
            {
                WriteToTrace("Reinstall :: Com port" + comPort +" not opened");
                if (!Bixolon.CheckFPrinter())
                {
                    PStatus = Bixolon.GetPrinterStatus();
                    WriteToTrace("Install :: Error, printer status error code: " + PStatus.PrinterErrorCode);
                    WriteToTrace("Install :: Error, printer status error description " + PStatus.PrinterErrorDescription);
                    return false;
                }
                return false;
            }
            
        }

        public Boolean EmitU0ZReport()
        {
            WriteToTrace("####      Method :: EmitU0ZReport     ####");
            
            DateTime today = DateTime.Now;
            TimeSpan diffdate;
            ReportData Report;
            try
            {
                Report = Bixolon.GetZReport();
                dateLastZ = Report.ZReportDate;
                diffdate = today.Subtract(dateLastZ);
                WriteToTrace("EmitU0ZReport ::  Last Z rerport " + dateLastZ.ToString());
                WriteToTrace("EmitU0ZReport ::  Diffdate in days " + diffdate.Days.ToString());
                // Z report with previus day date.
                if (diffdate.Days > 1)
                    return false;
                else
                    return true;
            }
            catch (PrinterException e)
            {
                WriteToTrace("EmitU0ZReport :: PrinterException " + e.Message + " Data" + e.Data + " InnerException" + e.InnerException + " Source" + e.Source + " TargetSite" + e.TargetSite);
                return false;
            }

        }

        #endregion
    }
}
