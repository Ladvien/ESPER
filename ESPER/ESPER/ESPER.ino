#include <ESP8266WiFi.h>
 
const char* ssid = "Honeysuckle_Hardware";
const char* password = "1532ePkF45a12//145e{k";
 
int ledPin = 2; // GPIO2
WiFiServer server(80);
 
void setup() {
  Serial.begin(115200);
  delay(10);
 
  pinMode(ledPin, OUTPUT);
  bool ledToggle = false;
  digitalWrite(ledPin, LOW);
  
  // Connect to WiFi network
  Serial.print("Connecting to ");
  Serial.println(ssid);
   
  WiFi.begin(ssid, password);
   
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    digitalWrite(ledPin, ledToggle ? HIGH : LOW);
    ledToggle = ledToggle ? false : true;
    Serial.print(".");
  }
  digitalWrite(ledPin, LOW);
  Serial.println("");
  Serial.print("Connected to ");
  Serial.println(ssid);
   
  // Start the server
  server.begin();
  Serial.println("Server started");
 
  // Print the IP address
  Serial.print("Use this URL to connect: ");
  Serial.print("http://");
  Serial.print(WiFi.localIP());
  Serial.println("/");
}
 
void loop() {
  // Check if a client has connected
  WiFiClient client = server.available();
  if (!client) {
    return;
  }
   
  // Wait until the client sends some data
  Serial.println("new client");
  while(!client.available()){
    delay(1);
  }
   
  // Read the first line of the request
  String request = client.readString();

  for(int i = 0; i < sizeof(request); i++){
    Serial.write(request[i]);
    Serial.print(" ");
  }
  Serial.println("");
  
  client.flush();

  uint8_t test[] = {45,46,47,48,244,222};
 
  // Return the response
  client.println("HTTP/1.1 200 OK");
  client.println("Content-Type: text/plain; charset=UTF-8");
  client.println(); //  do not forget this one
  client.write((const uint8_t*)test, sizeof(test));
  
  delay(1);
  Serial.println("Client disonnected");
  Serial.println("");
 
}
 
