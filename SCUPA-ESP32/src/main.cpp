#include <Arduino.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <LoRa.h>


#define BAUD 9600
#define ss 5
#define rst 4
#define dio0 2

// Bluetooth
BLEServer *pServer = NULL;
BLECharacteristic *pCharacteristic = NULL;
BLEDescriptor *pDescr;

bool deviceConnected = false;
bool oldDeviceConnected = false;

#define SERVICE_UUID "6bf6566b-b277-4573-ba8b-abd8a2378f71"
#define CHARACTERISTIC_UUID "658d3bc7-5ff3-44a7-8910-0fb0ef0a7b28"

// Message  variables 
String BluetoothMessage = "";
String LoraMessage = "";
bool Loraflag = false; 

// Timer variables
unsigned long previousMillis = millis();
#define interval 1000

// Function for receiving the LoRa packet
void receive_lora_packet() {
    int packetSize = LoRa.parsePacket();
    if(packetSize){
        String msg = LoRa.readString();
        Serial.println("Received: " + msg);
        BluetoothMessage = "$" + msg;
    }
}

// Bluetooth callback functions
class MyServerCallbacks : public BLEServerCallbacks {
    void onConnect(BLEServer *pServer) {
        deviceConnected = true;
    };

    void onDisconnect(BLEServer *pServer) {
        deviceConnected = false;
    }
};

// Bluetooth callback function that handles incoming messages
class MyCharacteristicCallbacks : public BLECharacteristicCallbacks {
    void onWrite(BLECharacteristic *pCharacteristic) {
        uint8_t *retrieved = pCharacteristic->getData();
        const char *retrieved_message = (const char *)retrieved;
        LoraMessage = retrieved_message;
        // Setting the LoRa flag to true so the received BLE message  will be send over via LoRa
        Loraflag = true; 
    }
};

// Bluetooth initilizing
void ble_init() {
    BLEDevice::init("ESP32");
    pServer = BLEDevice::createServer();
    pServer->setCallbacks(new MyServerCallbacks());
    BLEService *pService = pServer->createService(SERVICE_UUID);
    pCharacteristic = pService->createCharacteristic(
        CHARACTERISTIC_UUID,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY);

    BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
    pAdvertising->addServiceUUID(SERVICE_UUID);

    pCharacteristic->setCallbacks(new MyCharacteristicCallbacks()); // Assign the callbacks

    pCharacteristic->setReadProperty(true);
    pCharacteristic->setNotifyProperty(true);
    pServer->getAdvertising()->start();
    pService->start();
}

void setup() {
    Serial.begin(BAUD);
    ble_init();

    while (!Serial);
    Serial.println("LoRa Transceiver");
    LoRa.setPins(ss, rst, dio0);
    if (!LoRa.begin(433E6)) {
        Serial.println("Starting LoRa failed!");
        while (1);
    }

    Serial.println("Startup finished");
}

void loop() {
    receive_lora_packet();

    if (deviceConnected) {
        // Send the message over bluetooth if the device is connected
        
        uint8_t *message_bytes = (uint8_t *)BluetoothMessage.c_str();
        size_t length = BluetoothMessage.length();

        unsigned long currentMillis = millis();
        if (currentMillis - previousMillis >= interval) {
            previousMillis = currentMillis;
            if(BluetoothMessage.length() > 0){
                pCharacteristic->setValue(message_bytes, length);
            }
        }
        if(Loraflag){
            // Sending a LoRa message has to be in the main loop 
            LoRa.beginPacket();
            LoRa.print(LoraMessage);
            LoRa.endPacket();
            Loraflag = false; 
        }
    }
    // disconnecting
    if (!deviceConnected && oldDeviceConnected) {
        delay(500);                  // give the bluetooth stack the chance to get things ready
        pServer->startAdvertising(); // restart advertising
        oldDeviceConnected = deviceConnected;
    }
    // connecting
    if (deviceConnected && !oldDeviceConnected) {
        oldDeviceConnected = deviceConnected;
    }
}

