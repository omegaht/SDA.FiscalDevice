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
        public S5PrinterData MyS5PrinterData;
        public string comPort;
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
            // Get the configuration port
            comPort = configuration.CommunicationChannel;
            // Test Communication Port status 
            if (Bixolon.OpenFpCtrl(comPort))
            {
                WriteToTrace("Install :: Communication port opened before installation, closing port: " + comPort);
                Bixolon.CloseFpCtrl();
                return new Result(false);
            }
            else
            {
                WriteToTrace("Install :: Opening communication port " + comPort);
                if (Bixolon.OpenFpCtrl(comPort))
                {
                    if (Bixolon.CheckFPrinter())
                    {
                        WriteToTrace("Install :: Printer Connected");
                        // TODO 
                        // Get printer status
                        // Get Z report
                        return new Result(true);
                    }
                    else
                    {
                        WriteToTrace("Install :: Printer Not Connected");
                        // TODO
                        // Get printer status object
                        PStatus = Bixolon.GetPrinterStatus();
                        WriteToTrace("Install :: Error, printer status error code: " + PStatus.PrinterErrorCode);
                        WriteToTrace("Install :: Error, printer status error description " + PStatus.PrinterErrorDescription);
                        // Close com port
                        Bixolon.CloseFpCtrl();
                        return new Result(false);
                    }
                }
                else
                {
                    //TODO : Get printer status, to detect error.
                    // Close com port
                    return new Result(false);
                }
            }
        }

        public void Notify(int notificationId)
        {
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
        /// Verifies the Bixolon printer status, returning true/false depending on it.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeviceStatus()
        {

            return true;
        }
        #endregion
    }
}
