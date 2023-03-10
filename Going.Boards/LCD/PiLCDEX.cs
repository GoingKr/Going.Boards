using Devinno.PLC.Ladder;
using Going.Boards.Chips;
using Going.Boards.Interfaces;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;


namespace Going.Boards.LCD
{
    public class PiLCDEX : GoingBoard
    {
        #region Properties
        public override bool[] Input { get; } = new bool[4];
        public override bool[] Output { get; } = new bool[5];        
        public override ushort[] DAOUT { get; } = new ushort[1];        
        #endregion

        #region Member Variable
        IGpioPin[] Outs = new IGpioPin[5];
        IGpioPin[] Ins = new IGpioPin[4];

        public ushort DAOutValue;
        ushort Olddata;
        MCP4725 Dev;

        #endregion

        #region Constructor
        public PiLCDEX()
        {
            Dev = new MCP4725(0x60);

            Ins[0] = Pi.Gpio[P1.Pin38]; Ins[0].PinMode = GpioPinDriveMode.Input;
            Ins[1] = Pi.Gpio[P1.Pin40]; Ins[1].PinMode = GpioPinDriveMode.Input;
            Ins[2] = Pi.Gpio[P1.Pin15]; Ins[2].PinMode = GpioPinDriveMode.Input;
            Ins[3] = Pi.Gpio[P1.Pin16]; Ins[3].PinMode = GpioPinDriveMode.Input;

            Outs[0] = Pi.Gpio[P1.Pin29]; Outs[0].PinMode = GpioPinDriveMode.Output;
            Outs[1] = Pi.Gpio[P1.Pin31]; Outs[1].PinMode = GpioPinDriveMode.Output;
            Outs[2] = Pi.Gpio[P1.Pin32]; Outs[2].PinMode = GpioPinDriveMode.Output;
            Outs[3] = Pi.Gpio[P1.Pin33]; Outs[3].PinMode = GpioPinDriveMode.Output;
            Outs[4] = Pi.Gpio[P1.Pin22]; Outs[4].PinMode = GpioPinDriveMode.Output;
        }

        #endregion

        #region Method
        #region Begin
        public override void Begin(GoingPLC engine)
        {
            Load(engine);
            Out(engine);            
        }

        #endregion

        #region Load
        public override void Load(GoingPLC engine)
        {
            for (int i = 0; i < 4; i++)
                Input[i] = Ins[i].Read();
        }
        #endregion

        #region Out
        public override void Out(GoingPLC engine)
        {
            for (int i = 0; i < 5; i++)
                Outs[i].Write(Output[i]);

            if (Olddata != DAOUT[0])
            {
                Dev.WriteData(engine, DAOUT[0]);
                Olddata = DAOUT[0];
            }

        }
        #endregion

        #endregion
    }
}
