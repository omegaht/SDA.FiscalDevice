using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SkiData.FiscalDevices;


namespace SDA.FiscalDevice
{
    public class FiscalDevice : IFiscalDevice
    {
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

        public Result Install(FiscalDeviceConfiguration configuration)
        {
            throw new NotImplementedException();
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
        public void WriteToTrace(string message)
        {
            TraceEventArgs traceArgs = new TraceEventArgs(message, System.Diagnostics.TraceLevel.Info);

            if (this.Trace != null)
            {
                this.Trace(this, traceArgs);
            }
        }
        #endregion
    }
}
