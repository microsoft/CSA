# Nested Teamplates  
The Nested Templates are built to be generic in nature with the ability to pass in the values needed to build your specific architecture. Below you will find details on how to utilize each of the nested templates in this folder:  

# Templates  

[VNet](#VnetTemplate)  
[Empty NSG](#NSG-EMPTY-ExistingSubnetTemplate)  
[NSG with Rules](#NSG-ExistingSubnetTemplate)  

## <a name="VnetTemplate"></a>VNet Template  
This template will deploy a Virtual Network in Azure. It accepts a dynamic list of Subnets with their IP Ranges.  

### Typical Nested Template used before  
NA  

### Typical Nested Template used after  
NSG-Empty-ExistingSubnet
NSG-ExistingSubnet  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  vNETName | Name of the Virtual Network   | pocVNET    |
| addressRange   | Address Range the the entire subnet | 10.0.0.0/16  |
| subnets   | Array of subnets with their IP Range. The subnet range should be seperated from the IP Range with the \| deliminator | ["subnetA\|10.0.1.0/24","subnetB\|10.0.2.0/24","subnetC\|10.0.3.0/24"]  |  

### Sample Deployment  

    {
      "name": "deployVNET",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployVNETTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "vNETName": {
            "value": "pocVNET"
          },
          "addressRange": {
            "value": "10.0.0.0/16"
          },
          "subnets": {
            "value": [
                "subnetA\|10.0.1.0/24",
                "subnetB\|10.0.2.0/24",
                "subnetC\|10.0.3.0/24"
            ]
          }
        }
      }
    }  

## <a name="NSG-EMPTY-ExistingSubnetTemplate"></a>NSG-EMPTY-ExistingSubnet Template   
This template will deploy an empty NSG and attach it to an exising Subnet  

### Typical Nested Template used before  
VNet  

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine  
APIM  
AppGW  
AzureRedisCache  
PrivateAKSMICluster  
PrivateEndpoint    

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  virtualNetworkName | Name of the existing Virtual Network   | pocVNET    |
| subnetName   | Name of the subnet in the existing VNet to attach the NSG to | subnetA  |
| subnetAddressPrefix   | The IP range of the subnet attaching the NSG to | 10.0.1.0/24  |
| nsgName   | Name of the NSG | subnetA-NSG  |  


### Sample Deployment  

    {
      "name": "deployAKSNSG",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployNSGEmptyTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "virtualNetworkName": {
            "value": "pocVNET"
          },
          "subnetName": {
            "value": "subnetA"
          },
          "subnetAddressPrefix": {
            "value": "[reference(resourceId(resourceGroup().name, 'Microsoft.Network/virtualNetworks/subnets', 'pocVNEt', 'subnetA'), '2018-03-01').addressPrefix]"
          },
          "nsgName": {
            "value": "subnetA-NSG"
          }
        }
      }
    }


## <a name="NSG-ExistingSubnetTemplate"></a>NSG-ExistingSubnet Template   
This template will deploy an NSG and attach it to an exising Subnet. You pass in the NSG rules for the NSG using an array. 

### Typical Nested Template used before  
VNet  

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine  
APIM  
AppGW  
AzureRedisCache  
PrivateAKSMICluster  
PrivateEndpoint    

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  virtualNetworkName | Name of the existing Virtual Network   | pocVNET    |
| subnetName   | Name of the subnet in the existing VNet to attach the NSG to | subnetA  |
| subnetAddressPrefix   | The IP range of the subnet attaching the NSG to | 10.0.1.0/24  |
| nsgName   | Name of the NSG | subnetA-NSG  |  
| securityRules   | Array of security rules with a \| deleminator | RuleName\|Description\|Protocol\|Source Port Range\|Destination Port Range\|Source Address Prefix\|Destination Address Prefix\|Access\|Priority\|Direction  |  


### Sample Deployment  

    {
      "name": "deployAKSNSG",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployNSGTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "virtualNetworkName": {
            "value": "pocVNET"
          },
          "subnetName": {
            "value": "subnetB"
          },
          "subnetAddressPrefix": {
            "value": "[reference(resourceId(resourceGroup().name, 'Microsoft.Network/virtualNetworks/subnets', 'pocVNEt', 'subnetB'), '2018-03-01').addressPrefix]"
          },
          "nsgName": {
            "value": "subnetB-NSG"
          },
          "securityRules": {
            "value": [
              "deny-all|Deny-All-Traffic|Tcp|*|*|*|*|Deny|500|Inbound",
              "allow-443|Allow-SSL|Tcp|*|443|*|*|Allow|100|Inbound",
              "allow-8080|Allow-SSL|Tcp|*|8080|*|*|Allow|110|Inbound",
              "allow-HealthProbe|Allow-AppGWHealth|Tcp|*|65200-65535|*|*|Allow|120|Inbound"
            ]
          }
        }
      }
    }
