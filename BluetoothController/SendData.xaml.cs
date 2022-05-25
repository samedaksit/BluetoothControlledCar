
using BluetoothController.ViewModel;
using Plugin.BluetoothClassic.Abstractions;
using SkiaSharp;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace BluetoothController
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendData : ContentPage
    {
        private SKPaint carPaint = new SKPaint();
        private SKPaint obstPaint = new SKPaint();
        private SKPath routePath = new SKPath();
        private SKPath obstLeftroutePath = new SKPath();
        private SKPath obstRightroutePath = new SKPath();
        private SKPath obstFrontroutePath = new SKPath();
        private SKPoint lastPoint = new SKPoint();

        private SKMatrix tmpMatrix = SKMatrix.MakeIdentity();
        

        private float dis = 0.5F;
        private int f, l, r, le = 0, re = 0, fe = 0;
        private int OLx = 0, OLy = 0, ORx = 0, ORy = 0, OFx = 0, OFy = 0;

        private float X = 0.5F;
        private float Y = 0F;

        public EventHandler<String> myMessages;
        public string allMessages = "";
        public SendData()
        {
            carPaint.Color = SKColors.Blue;
            carPaint.Style = SKPaintStyle.Stroke;
            carPaint.StrokeWidth = 8F;
            obstPaint.Color = SKColors.Red;
            obstPaint.Style = SKPaintStyle.Stroke;
            obstPaint.StrokeWidth = 6F;
            OLx = 0;
            OLy = -15;
            ORx = 0;
            ORy = 15;
            OFx = 15;
            OFy = 0;
            lastPoint.X = 40F;
            lastPoint.Y = 40F;
            
            routePath.MoveTo(lastPoint.X,lastPoint.Y);  
            
            InitializeComponent();
            System.Console.WriteLine("samed - Button Clicked!");
            DigitViewModel model = (DigitViewModel)BindingContext;

             myMessages += new EventHandler<string>(GetMessage); 
            


            if (App.CurrentBluetoothConnection != null)
            {
                App.CurrentBluetoothConnection.OnStateChanged += CurrentBluetoothConnection_OnStateChanged;
                App.CurrentBluetoothConnection.OnRecived += CurrentBluetoothConnection_OnRecived;
                App.CurrentBluetoothConnection.OnError += CurrentBluetoothConnection_OnError;
            }

            CarMove.Clicked += delegate
            {
                try
                {
                    App.CurrentBluetoothConnection.Transmit(new Memory<byte>(new byte[1] { 42 })); //sends "*" to arduino
                }
                catch (Exception outPutEX)
                {

                }
            };

            CarStop.Clicked += delegate
            {
                try
                {
                    App.CurrentBluetoothConnection.Transmit(new Memory<byte>(new byte[1] { 49 })); //sends "1" to arduino
                }
                catch (Exception outPutEX)
                {

                }
            };
        }

        ~SendData()
        {
            if (App.CurrentBluetoothConnection != null)
            {
                App.CurrentBluetoothConnection.OnStateChanged -= CurrentBluetoothConnection_OnStateChanged;
                App.CurrentBluetoothConnection.OnRecived -= CurrentBluetoothConnection_OnRecived;
                App.CurrentBluetoothConnection.OnError -= CurrentBluetoothConnection_OnError;
            }
        }

        private void GetMessage(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    allMessages += e;
                    string[] s = allMessages.Split('E');
                    if (s.Length > 2)
                    {
                        string[] route = s[1].Split(',');
                        for (int i = 0; i < route.Length; i++)
                        {
                            string c = route[i];
                            if (c.Contains("Y"))
                            {

                                FrontSensor.Text = route[i].Replace("Y", "");
                                Situation.Text = "---";
                                int.TryParse(FrontSensor.Text, out f);
                                if (f < 30)
                                {
                                    if (fe == 0)
                                    {
                                        obstFrontroutePath.MoveTo(lastPoint.X + OFx, lastPoint.Y + OFy);
                                    }
                                    fe = 1;
                                }
                                else
                                {
                                    fe = 0;
                                }
                            }
                            else if (c.Contains("L"))
                            {
                                LeftSensor.Text = route[i].Replace("L", "");
                                Situation.Text = "---";
                                int.TryParse(LeftSensor.Text, out l);
                                if (l < 20)
                                {
                                    if (le == 0)
                                    {
                                        obstLeftroutePath.MoveTo(lastPoint.X + OLx, lastPoint.Y + OLy);
                                    }
                                    le = 1;
                                }
                                else
                                {
                                    le = 0;
                                }
                            }
                            else if (c.Contains("R"))
                            {
                                
                                RightSensor.Text = route[i].Replace("R", "");
                                Situation.Text = "---";
                                int.TryParse(RightSensor.Text, out r);
                                if (r < 20)
                                {
                                    if (re == 0)
                                    {
                                        obstRightroutePath.MoveTo(lastPoint.X + ORx, lastPoint.Y + ORy);
                                    }
                                    re = 1;
                                }
                                else
                                {
                                    re = 0;
                                }
                            }
                            else if (c.Contains("Tl"))
                            {
                                if (X == dis && Y == 0F)
                                {
                                    X = 0F;
                                    Y = -dis;
                                }
                                else if (X == 0F && Y == -dis)
                                {
                                    X = -dis;
                                    Y = 0F;
                                }
                                else if (X == -dis && Y == 0F)
                                {
                                    X = 0F;
                                    Y = dis;
                                }
                                else if (X == 0F && Y == dis)
                                {
                                    X = dis;
                                    Y = 0F;
                                }
                                //sola döndükten sonra solda engel varsa
                                if (OLx == 0 && OLy == -15)
                                {
                                    OLx = -15;
                                    OLy = 0;
                                }
                                else if (OLx == -15 && OLy == 0)
                                {
                                    OLx = 0;
                                    OLy = 15;
                                }
                                else if (OLx == 0 && OLy == 15)
                                {
                                    OLx = 15;
                                    OLy = 0;
                                }
                                else if (OLx == 15 && OLy == 0)
                                {
                                    OLx = 0;
                                    OLy = -15;
                                }
                                //sola döndükten sonra sağda engel varsa
                                if (ORx == 0 && ORy == 15)
                                {
                                    ORx = 15;
                                    ORy = 0;
                                }
                                else if (ORx == 15 && ORy == 0)
                                {
                                    ORx = 0;
                                    ORy = -15;
                                }
                                else if (ORx == 0 && ORy == -15)
                                {
                                    ORx = -15;
                                    ORy = 0;
                                }
                                else if (ORx == -15 && ORy == 0)
                                {
                                    ORx = 0;
                                    ORy = 15;
                                }
                                //sola döndükten sonra önde engel varsa
                                if (OFx == 15 && OFy == 0)
                                {
                                    OFx = 0;
                                    OFy = -15;
                                }
                                else if (OFx == 0 && OFy == -15)
                                {
                                    OFx = -15;
                                    OFy = 0;
                                }
                                else if (OFx == -15 && OFy == 0)
                                {
                                    OFx = 0;
                                    OFy = 15;
                                }
                                else if (OFx == 0 && OFy == 15)
                                {
                                    OFx = 15;
                                    OFy = 0;
                                }

                                Situation.Text = "Turning Left...";
                            }
                            else if (c.Contains("Tr"))
                            {
                                if (X == dis && Y == 0F)
                                {
                                    X = 0F;
                                    Y = dis;
                                }
                                else if (X == 0F && Y == -dis)
                                {
                                    X = dis;
                                    Y = 0F;
                                }
                                else if (X == -dis && Y == 0F)
                                {
                                    X = 0F;
                                    Y = -dis;
                                }
                                else if (X == 0F && Y == dis)
                                {
                                    X = -dis;
                                    Y = 0F;
                                }
                                //sağa döndükten sonra solda engel varsa
                                if (OLx == 0 && OLy == -15)
                                {
                                    OLx = 15;
                                    OLy = 0;
                                }
                                else if(OLx == 15 && OLy == 0)
                                {
                                    OLx = 0;
                                    OLy = 15;
                                }
                                else if (OLx == 0 && OLy == 15)
                                {
                                    OLx = -15;
                                    OLy = 0;
                                }
                                else if (OLx == -15 && OLy == 0)
                                {
                                    OLx = 0;
                                    OLy = -15;
                                }
                                //sağa döndükten sonra sağda engel varsa
                                if (ORx == 0 && ORy == 15)
                                {
                                    ORx = -15;
                                    ORy = 0;
                                }
                                else if (ORx == -15 && ORy == 0)
                                {
                                    ORx = 0;
                                    ORy = -15;
                                }
                                else if (ORx == 0 && ORy == -15)
                                {
                                    ORx = 15;
                                    ORy = 0;
                                }
                                else if (ORx == 15 && ORy == 0)
                                {
                                    ORx = 0;
                                    ORy = 15;
                                }
                                //sağa döndükten sonra önde engel varsa
                                if (OFx == 15 && OFy == 0)
                                {
                                    OFx = 0;
                                    OFy = 15;
                                }
                                else if (OFx == 0 && OFy == 15)
                                {
                                    OFx = -15;
                                    OFy = 0;
                                }
                                else if (OFx == -15 && OFy == 0)
                                {
                                    OFx = 0;
                                    OFy = -15;
                                }
                                else if (OFx == 0 && OFy == -15)
                                {
                                    OFx = 15;
                                    OFy = 0;
                                }
                                Situation.Text = "Turning Right...";
                            }
                            else if (c.Contains("TB"))
                            {
                                if (X == dis && Y == 0F)
                                {
                                    X = -dis;
                                    Y = 0F;
                                }
                                else if (X == 0F && Y == -dis)
                                {
                                    X = 0F;
                                    Y = dis;
                                }
                                else if (X == -dis && Y == 0F)
                                {
                                    X = dis;
                                    Y = 0F;
                                }
                                else if (X == 0F && Y == dis)
                                {
                                    X = 0F;
                                    Y = -dis;
                                }
                                //solda engel varsa
                                if (OLx == 0 && OLy == -15)
                                {
                                    OLx = 0;
                                    OLy = 15;
                                }
                                else if (OLx == 0 && OLy == 15)
                                {
                                    OLx = 0;
                                    OLy = -15;
                                }
                                else if (OLx == 15 && OLy == 0)
                                {
                                    OLx = -15;
                                    OLy = 0;
                                }
                                else if (OLx == -15 && OLy == 0)
                                {
                                    OLx = 15;
                                    OLy = 0;
                                }
                                //sağda engel varsa
                                if (ORx == 0 && ORy == 15)
                                {
                                    ORx = 0;
                                    ORy = -15;
                                }
                                else if (ORx == 0 && ORy == -15)
                                {
                                    ORx = 0;
                                    ORy = 15;
                                }
                                else if (ORx == -15 && ORy == 0)
                                {
                                    ORx = 15;
                                    ORy = 0;
                                }
                                else if (ORx == 15 && ORy == 0)
                                {
                                    ORx = -15;
                                    ORy = 0;
                                }
                                //önde engel varsa
                                if (OFx == 15 && OFy == 0)
                                {
                                    OFx = -15;
                                    OFy = 0;
                                }
                                else if (OFx == -15 && OFy == 0)
                                {
                                    OFx = 15;
                                    OFy = 0;
                                }
                                else if (OFx == 0 && OFy == -15)
                                {
                                    OFx = 0;
                                    OFy = 15;
                                }
                                else if (OFx == 0 && OFy == 15)
                                {
                                    OFx = 0;
                                    OFy = -15;
                                }
                                Situation.Text = "Turning Back...";
                            }
                            float newX = lastPoint.X + X;
                            float newY = lastPoint.Y + Y;
                            lastPoint.X = newX;
                            lastPoint.Y = newY;
                            if (newX < 0F)
                            {
                                tmpMatrix.TransX = -newX-2F;
                            }
                            if (newY < 0F)
                            {
                                System.Console.WriteLine("samed - canvaswidth " + canvasView.CanvasSize.Width);
                                tmpMatrix.TransY = -newY-2F;
                            }
                            routePath.LineTo(lastPoint.X, lastPoint.Y);
                            if (le == 1)
                            {
                                obstLeftroutePath.LineTo(lastPoint.X + OLx, lastPoint.Y + OLy);
                            }
                            if (re == 1)
                            {
                                obstRightroutePath.LineTo(lastPoint.X + ORx, lastPoint.Y + ORy);
                            }
                            if (fe == 1)
                            {
                                obstFrontroutePath.LineTo(lastPoint.X + OFx, lastPoint.Y + OFy);
                            }
                            canvasView.InvalidateSurface();
                        }

                        allMessages = allMessages.Replace("E" + s[1], "");
                    }
                }
                catch (Exception outPutEX)
                {
                    System.Console.WriteLine( outPutEX.Message);
                }
            });

        }

        private void CurrentBluetoothConnection_OnStateChanged(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            var model = (DigitViewModel)BindingContext;
            if (model != null)
            {
                model.ConnectionState = stateChangedEventArgs.ConnectionState;
            }
        }

        private void CurrentBluetoothConnection_OnRecived(object sender, Plugin.BluetoothClassic.Abstractions.RecivedEventArgs recivedEventArgs)
        {
            DigitViewModel model = (DigitViewModel)BindingContext;
            if (model != null)
            {
                model.SetReciving();
                try
                {
                    String arduinoData = System.Text.Encoding.UTF8.GetString(recivedEventArgs.Buffer.ToArray(), 0, recivedEventArgs.Buffer.Length);
                    System.Console.WriteLine(arduinoData);
                    try
                    {
                        myMessages.Invoke("dataSender", arduinoData);
                    }
                    catch (Exception outPutEX)
                    {
                        System.Console.WriteLine( outPutEX.Message);
                    }
                }
                catch (Exception outPutEX)
                {
                    System.Console.WriteLine( outPutEX.Message);
                }
                model.SetRecived();
            }
        }



        private void CurrentBluetoothConnection_OnError(object sender, System.Threading.ThreadExceptionEventArgs errorEventArgs)
        {
            if (errorEventArgs.Exception is BluetoothDataTransferUnitException)
            {

            }
        }



        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface =  e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.AliceBlue);

            canvas.Concat(ref tmpMatrix);
            canvas.DrawPath(routePath,carPaint);
            canvas.DrawPath(obstLeftroutePath, obstPaint);
            canvas.DrawPath(obstRightroutePath, obstPaint);
            canvas.DrawPath(obstFrontroutePath, obstPaint);
            
        }
    }
}