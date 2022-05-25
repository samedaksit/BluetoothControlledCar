#include <SoftwareSerial.h>
int data;
int in1=2;//Right
int in2=3;//Right
int in3=4;//Left
int in4=5;//Left

//for Front
const int trigPinFront = 7;
const int echoPinFront = 6;
long durationFront;
int distanceFront;

//for Right
const int trigPinRight = 11;
const int echoPinRight = 10;
long durationRight;
int distanceRight;

//for Left
const int trigPinLeft = 13;
const int echoPinLeft = 12;
long durationLeft;
int distanceLeft;

//Bluetooth
const int RX_PIN = 8;
const int TX_PIN = 9;
SoftwareSerial serial(RX_PIN, TX_PIN);

void setup() {
  pinMode(trigPinFront, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPinFront, INPUT);// Sets the echoPin as an Input
  pinMode(trigPinRight, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPinRight, INPUT);// Sets the echoPin as an Input
  pinMode(trigPinLeft, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPinLeft, INPUT);// Sets the echoPin as an Input
  serial.begin(9600);
  pinMode(in1,OUTPUT);
  pinMode(in2,OUTPUT);
  pinMode(in3,OUTPUT);
  pinMode(in4,OUTPUT);
}

void loop() 
{
 
  if(serial.available())
  {
    data = serial.read();
    if(data=='*')
    {
      while(data!='1')
      {
       engeldenkac();
       data = serial.read();
      }
    }
      digitalWrite(in1,LOW);
      digitalWrite(in2,LOW);
      digitalWrite(in3,LOW);
      digitalWrite(in4,LOW);
  }
}



void engeldenkac()
{
  distanceFront = calculateDistanceFront();
  distanceRight = calculateDistanceRight();
  distanceLeft = calculateDistanceLeft();
  serial.print(" E");
  if(distanceFront>20 )
  {
      digitalWrite(in1,LOW);
      digitalWrite(in2,HIGH);
      digitalWrite(in3,LOW);
      digitalWrite(in4,HIGH);
      serial.print(",Y");
      serial.print(distanceFront);
      serial.print(",L");
      serial.print(distanceLeft);
      serial.print(",R");
      serial.print(distanceRight);
   } else 
     {
        //ikisi de dolu
        if((distanceRight<20||distanceRight>800)&&(distanceLeft<20||distanceLeft>800))
        {
          //back
            serial.print(",TB");
            //serial.print(distanceFront);
            digitalWrite(in1,LOW);
            digitalWrite(in2,HIGH);
            digitalWrite(in3,HIGH);
            digitalWrite(in4,LOW);
            delay(4350);
        }
          //sağ dolu sol boş
        else if((distanceRight<20||distanceRight>800)&&(distanceLeft>20&&distanceLeft<800)){
            //dur
            digitalWrite(in1,LOW);
            digitalWrite(in2,LOW);
            digitalWrite(in3,LOW);
            digitalWrite(in4,LOW);
            delay(150);
            //sola dön
            serial.print(",Tl");
            digitalWrite(in1,LOW);
            digitalWrite(in2,HIGH);
            digitalWrite(in3,HIGH);
            digitalWrite(in4,LOW)
            delay(2175);
            }
          //sol dolu sağ boş
          else if((distanceLeft<20||distanceLeft>800)&&(distanceRight>20&&distanceRight<800))
          {
            //dur
            digitalWrite(in1,LOW);
            digitalWrite(in2,LOW);
            digitalWrite(in3,LOW);
            digitalWrite(in4,LOW);
            delay(150);
            //sağa dön
            serial.print(",Tr");
           digitalWrite(in1,HIGH);
            digitalWrite(in2,LOW);
            digitalWrite(in3,LOW);
            digitalWrite(in4,HIGH);
            delay(2175);
          }else 
          {
     
           if(distanceLeft<distanceRight){
            //dur
              digitalWrite(in1,LOW);
              digitalWrite(in2,LOW);
              digitalWrite(in3,LOW);
              digitalWrite(in4,LOW);
              delay(150);
              //sağa dön
              serial.print(",Tr");
              digitalWrite(in1,HIGH);
              digitalWrite(in2,LOW);
              digitalWrite(in3,LOW);
              digitalWrite(in4,HIGH);
              delay(2175);
            }else
            {
              //dur 
              digitalWrite(in1,LOW);
              digitalWrite(in2,LOW);
              digitalWrite(in3,LOW);
              digitalWrite(in4,LOW);
              delay(150);
              //sola dön
              serial.print(",Tl");
              digitalWrite(in1,LOW);
              digitalWrite(in2,HIGH);
              digitalWrite(in3,HIGH);
              digitalWrite(in4,LOW);
              delay(2175);
            }     
           }
     }
  }
int calculateDistanceFront(){ 
  digitalWrite(trigPinFront, LOW); 
  delayMicroseconds(2);
  
  digitalWrite(trigPinFront, HIGH); 
  delayMicroseconds(1000);
  digitalWrite(trigPinFront, LOW);
  durationFront = pulseIn(echoPinFront, HIGH); 
  distanceFront= (durationFront / 2) / 28.5;
  return distanceFront;
}

int calculateDistanceRight(){ 
  digitalWrite(trigPinRight, LOW); 
  delayMicroseconds(2);
  
  digitalWrite(trigPinRight, HIGH); 
  delayMicroseconds(1000);
  digitalWrite(trigPinRight, LOW);
  durationRight = pulseIn(echoPinRight, HIGH); 
  distanceRight= (durationRight / 2) / 28.5;
  return distanceRight;
}

int calculateDistanceLeft(){ 
  digitalWrite(trigPinLeft, LOW); 
  delayMicroseconds(2);
  
  digitalWrite(trigPinLeft, HIGH); 
  delayMicroseconds(1000);
  digitalWrite(trigPinLeft, LOW);
  durationLeft = pulseIn(echoPinLeft, HIGH); 
  distanceLeft= (durationLeft / 2) / 28.5;
  return distanceLeft;
}
