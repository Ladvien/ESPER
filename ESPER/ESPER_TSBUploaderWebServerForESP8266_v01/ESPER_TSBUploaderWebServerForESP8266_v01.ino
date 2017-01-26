
/*
 * This code has been adapted from:
 *    "SDWebServer - Example WebServer with SD Card backend for esp8266
 *    Copyright (c) 2015 Hristo Gochkov. All rights reserved.
 *    This file is part of the ESP8266WebServer library for Arduino environment."
 * 
*/

const char* ssid = "Honeysuckle_Hardware";
const char* password = "1532ePkF45a12//145e{k";

//const char* ssid = "TCHC-Guest";
//const char* password = "housingfirst";

// Gross.  Global variables.  These are used for collecting Serial Data.
String inputBuffer = "";         
 
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>

const int ledPin = 2;
const char* host = "esp8266sd";

ESP8266WebServer server(80);

void returnOK() {
  server.send(200, "text/plain", "");
}

void returnFail(String msg) {
  server.send(500, "text/plain", msg + "\r\n");
}

void debugWebRequest(){
  String message = "";
  message += "URI: ";
  message += server.uri();
  message += "\nMethod: ";
  message += (server.method() == HTTP_GET)?"GET":"POST";
  message += "\nArguments: ";
  message += server.args();
  message += "\n";
  for (uint8_t i=0; i<server.args(); i++){
    message += " NAME:"+server.argName(i) + "\n VALUE:" + server.arg(i) + "\n";
  }
  server.send(404, "text/plain", message);
  Serial.print(message);
}

void getSerialBuffer(){
  Serial.print("Got data: ");
  Serial.println(inputBuffer);
  server.send(200, "text/plain", inputBuffer);
  inputBuffer = "";
}

void handleUnknownPost(){
  returnOK();
  debugWebRequest();  
  Serial.print("Unknown POST.");
}

void handleStringPost(){
  returnOK();
  debugWebRequest();
  Serial.print("String POST.");
}

void handleDataPost(){
  returnOK();
  debugWebRequest();
  Serial.print("Data POST.");
}

void handleNotFound(){
  returnOK();
  debugWebRequest();
  Serial.print("Resource not found POST.");
}

void setup(void){
  pinMode(ledPin, OUTPUT);
  
  Serial.begin(115200);
  Serial.setDebugOutput(true);
  Serial.print("\n");
  WiFi.begin(ssid, password);
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  digitalWrite(ledPin, HIGH);
  bool isLedPinOn = true;
  
  // Wait for connection
  uint8_t i = 0;
  while (WiFi.status() != WL_CONNECTED && i++ < 20) {//wait 10 seconds
    delay(500);
    Serial.print(".");
    isLedPinOn = !isLedPinOn;  
    
  }
  Serial.println("");
  if(i == 21){
    digitalWrite(ledPin, HIGH);
    Serial.print("Could not connect to");
    Serial.println(ssid);
    while(1) { 
      digitalWrite(ledPin, isLedPinOn ? HIGH : LOW);
      delay(200); 
    }
  }
  
  digitalWrite(ledPin, LOW);  
  
  Serial.print("Connected! IP address: ");
  Serial.println(WiFi.localIP());
  
  if (MDNS.begin(host)) {
    MDNS.addService("http", "tcp", 80);
    Serial.println("MDNS responder started");
    Serial.print("You can now connect to http://");
    Serial.print(host);
    Serial.println(".local");
  }

  server.on("/", HTTP_POST, handleUnknownPost);
  server.on("/string", HTTP_POST, handleStringPost);
  server.on("/data", HTTP_POST, handleDataPost);
  server.on("/buffer", HTTP_POST, getSerialBuffer);
  server.onNotFound(handleNotFound);

  server.begin();
  Serial.println("HTTP server started");
}

void loop(void){
  server.handleClient();
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    inputBuffer += inChar;
  }
}
